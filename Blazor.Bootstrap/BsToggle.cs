namespace Du.Blazor.Bootstrap;

/// <summary>
///   <para>토글러, 보통 붕괴 토글에 쓰임</para>
///   <para>드랍다운의 펼치기/닫기 버튼으로 쓰이며, NavBar 버튼으로도 쓸 수 있음</para>
/// </summary>
/// <seealso cref="DropDown" />
public class BsToggle : BsComponent, IAsyncDisposable
{
	// 우선순위
	//	1. 드랍다운			=> nav-link dropdown-toggle
	//	2. 나브바			=> navbar-toggler
	//  3. 오프캔바스		=> (none)
	//	4. 붕괴(=나브바)	=> (none)
	// 우선 순위를 지키지 않으면 나브바 아래 드랍다운이 동작안한다
	// 한편, 나브바와 붕괴가 같이 있으면... 안된다. 동작이 제대로 안됨
	private enum Mode
	{
		None,
		OffCanvas,
		Collapse,
	}

	/// <summary>드랍다운</summary>
	[CascadingParameter] public BsDropDown? DropDown { get; set; }
	/// <summary>나브바</summary>
	[CascadingParameter] public BsNavBar? NavBar { get; set; }
	/// <summary>붕괴 아이디. 나브바가 붕괴면 나브바에서 가져옴</summary>
	[Parameter] public string? CollapseId { get; set; }
	/// <summary>오프캔바스 아이디. 나브바가 오프면 나브바에서 가져옴</summary>
	[Parameter] public string? OffCanvasId { get; set; }

	/// <summary>표시할 때 사용하는 태그</summary>
	[Parameter] public string? Tag { get; set; }

	/// <summary>표시할 텍스트</summary>
	[Parameter] public string? Text { get; set; }
	/// <summary>ARIA LABEL. 이 값이 없으면 브라우저가 싫어함</summary>
	[Parameter] public string? AriaLabel { get; set; }
	/// <summary>토글 모양 <see cref="BsToggleType"/></summary>
	[Parameter] public BsToggleType Type { get; set; } = BsToggleType.Button;
	/// <summary>드랍다운일 때 자동으로 닫기 방법 <see cref="BsDropAutoClose"/></summary>
	[Parameter] public BsDropAutoClose AutoClose { get; set; } = BsDropAutoClose.True;
	/// <summary>버튼 모양일 때 색깔 <see cref="BsVariant"/></summary>
	[Parameter] public BsVariant? Variant { get; set; }
	/// <summary>버튼 모양일 때 크기 <see cref="BsSize"/></summary>
	[Parameter] public BsSize? Size { get; set; }
	/// <summary>버튼 모양일 때 외곽선 모양인가 여부</summary>
	[Parameter] public bool? Outline { get; set; }
	/// <summary>버튼이 아닐 때 선택 모양을 표시(채워서)하는지 여부</summary>
	[Parameter] public bool Caret { get; set; }
	/// <summary>분할 버튼</summary>
	/// <remarks>※ 구현 안됨</remarks>
	[Parameter] public bool Split { get; set; } // 당분간 안만듬

	/// <summary>확장되면 처리하는 이벤트</summary>
	[Parameter] public EventCallback<BsExpandedEventArgs> OnExpanded { get; set; }
	// OnClick 구현해야하나?
	//[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

	//
	[Inject] private IJSRuntime JSRuntime { get; set; } = default!;
	[Inject] private ILogger<BsToggle> Logger { get; set; } = default!;

	//
	private BsVariant ActualVariant => Variant ?? BsSettings.ButtonVariant;
	private BsSize ActualSize => Size ?? BsSettings.ButtonSize;
	private bool ActualOutline => Outline ?? BsSettings.ButtonOutline;

	//
	private ElementReference _self;
	private IJSObjectReference? _js;
	private DotNetObjectReference<BsToggle>? _drf;
	private Mode _mode;

	//
	protected override void OnInitialized()
	{
		// 나브바가 있네
		if (NavBar is not null)
		{
			// 드랍이 있음
			if (DropDown is not null)
			{
				// 이럴 때는 링크만사용
				Type = BsToggleType.A;
			}
			// 드랍이 없고, 나브바엔 토글이 없음 
			else if (NavBar.ToggleRef is null)
			{
				// 토글이 여러개면 곤란하다 하나만
				// 아니면, 토글 내 드랍에서 캐치하는 것도 곤란
				NavBar.ToggleRef = this;

				// 나브바에서 아이디 셋팅
				if (NavBar.Type == BsNavBarType.Collapse)
				{
					// 나브바가 컬랩스 모드
					CollapseId ??= '#' + NavBar.TargetId;
				}
				else  /*if (NavBar.Mode == BsNavBarType.OffCanvas)*/
				{
					// 나브바가 오프캔바스 모드
					// 오프캔바스가 기본이니깐 걍.. 검사 안함
					OffCanvasId ??= '#' + NavBar.TargetId;
				}

				AriaLabel ??= "Toggle navigation"; // 이거 지정안하면 브라우저에서 욕함
			}
		}

		if (OffCanvasId.IsHave())
			CheckAndSetMode(Mode.OffCanvas);
		else if (CollapseId.IsHave())
			CheckAndSetMode(Mode.Collapse);
		else
		{
			// 클랩스가 아닌데 나브바면 이상하쟎아. 예컨데 아이디가 잘못됐다든가
			// 나브바 밑에 드랍이면 있다.
			//ThrowIf.ItemNotNull(NavBar);

			// 이 경우엔 반드시 드랍다운이어야 함
			ThrowIf.ContainerIsNull(this, DropDown);
		}

		if (Split && Type is not BsToggleType.Button)
		{
			Logger.LogError(BsSettings.UseLocaleMesg
				? "{name}: 스플릿 모드를 쓰려거든 레이아웃을 반드시 버튼으로 하세요."
				: "{name}: Layout must be button when split mode.",
				nameof(Split));
			Type = BsToggleType.Button;
		}

		Logger.LogTrace(BsSettings.UseLocaleMesg
			? "{name}: 성공적으로 초기화 했어요."
			: "{name}: initialized successfully.",
			nameof(BsToggle));
	}

	//
	private void CheckAndSetMode(Mode mode)
	{
		if (Split)
		{
			Logger.LogCritical(BsSettings.UseLocaleMesg
					? "{name}: 오프캔바스와 분리는 함께 쑬 수 없어요."
					: "{name}: Invalid usage in OffCanvas mode.",
				nameof(Split));
			Split = false;
		}

		if (NavBar is not null) // 나브바 처리
		{
			Variant ??= BsVariant.None;

			if (Type is not BsToggleType.Button) // 버튼만 됨
			{
				Logger.LogCritical(BsSettings.UseLocaleMesg
						? "{name}: 나브바 안에서 쓸 때는 반드시 {type} 이어야 해요."
						: "{name}: Must be {type} when contained within NavBar.",
					nameof(Type), nameof(BsToggleType.Button));
				Type = BsToggleType.Button;
			}
		}

		_mode = mode;
	}

	//
	protected override void OnComponentClass(BsCss cssc)
	{
		if (NavBar is not null)
		{
			if (DropDown is null)
			{
				cssc.Add("navbar-toggler")
					.Add(OffCanvasId.IsHave(), "d-flex");
			}
			else
			{
				cssc.Add("nav-link")
					.Add("dropdown-toggle");
			}
		}

		if (Type == BsToggleType.Button)
		{
			if (NavBar is null)
			{
				cssc.Add(_mode is Mode.None, "dropdown-toggle")
					.Add("btn")
					.Add(ActualVariant.ToButtonCss(ActualOutline))
					.Add(ActualSize.ToCss("btn"));
			}
			else
			{
				cssc.Add(_mode is not Mode.OffCanvas, "btn", NavBar.Expand.ToCss("d", "none"))
					.Add(ActualVariant.ToButtonCss(ActualOutline))
					.Add(ActualSize.ToCss("btn"));
			}
		}
		else
		{
			// 그냥 드롭다운일 경우
			cssc.Add(Caret && _mode is Mode.None, "dropdown-toggle");
		}

		cssc.Add(Split, "dropdown-toggle-split")
			.Register(() => (DropDown?.Expanded ?? false).IfTrue("show"));
	}

	//
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
			return;

		if (_mode is not Mode.None)
			return;

		_drf ??= DotNetObjectReference.Create(this);
		try
		{
			await (await PrepareModule())
				.InvokeVoidAsync("initialize", _self, _drf);
		}
		catch (JSDisconnectedException)
		{
			_js = null;
		}
	}

	//
	private async ValueTask<IJSObjectReference> PrepareModule() =>
		_js ??= await JSRuntime.ImportModuleAsync("toggle");

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * 	<TAG @ref="_self"
		 * 		type="button"	<--- 버튼이면
		 *		role="button"	<--- 버튼이 아니면
		 * 		class="@CssClass"
		 * 		data-bs-toggle="dropdown"
		 * 		data-bs-auto-close="@AutoClose.ToCss()"
		 * 		aria-expanded="false"
		 * 		@attributes="@UserAttrs">
		 * 	@Text
		 * 	@ChildContent
		 * 	</TAG>
		 */

		var tag = Tag ?? Type.ToTag();

		builder.OpenElement(0, tag);

		builder.AddAttribute(1, Type == BsToggleType.Button ? "type" : "role", "button");
		builder.AddAttribute(2, "class", CssClass);

		switch (_mode)
		{
			case Mode.OffCanvas:
			{
				// 오프캔바스 일 때
				builder.AddAttribute(3, "data-bs-toggle", "offcanvas");
				builder.AddAttribute(4, "data-bs-target", OffCanvasId);
				if (OffCanvasId![0] == '#')
					builder.AddAttribute(5, "aria-controls", OffCanvasId[1..]);
				break;
			}
			case Mode.Collapse:
			{
				// 붕괴 일때
				builder.AddAttribute(3, "data-bs-toggle", "collapse");
				builder.AddAttribute(4, "data-bs-target", CollapseId);
				if (CollapseId![0] == '#')
					builder.AddAttribute(5, "aria-controls", CollapseId[1..]);
				builder.AddAttribute(5, "aria-expanded", "false");
				break;
			}
			case Mode.None when DropDown is not null:
			{
				// 드랍다운 일 때
				builder.AddAttribute(3, "data-bs-toggle", "dropdown");
				builder.AddAttribute(4, "data-bs-auto-close", AutoClose.ToCss());
				builder.AddAttribute(5, "aria-expanded", "false");
				break;
			}
			default:
			{
				// 별거 안하고 오류 안냄. 드랍다운 취급
				builder.AddAttribute(3, "data-bs-toggle", "dropdown");
				builder.AddAttribute(5, "aria-expanded", "false");
				break;
			}
		}

		builder.AddAttribute(6, "aria-label", AriaLabel);
		builder.AddMultipleAttributes(7, UserAttrs);

		if (_mode is Mode.None)
		{
			// 리퍼런스는 자바스크립트에서 쓰는데 드랍다운에서만 씀
			builder.AddElementReferenceCapture(11, e => _self = e);
		}

		if (Text.IsHave() || ChildContent is not null)
		{
			if (Text.IsHave())
				builder.AddContent(20, Text);
			builder.AddContent(21, ChildContent);
		}
		else if (NavBar is not null)
		{
			// 나브바일 때 기본 토글러 아이콘 뿌려줌
			builder.AddMarkupContent(22, @"<span class=""navbar-toggler-icon""></span>");
		}

		builder.CloseElement(); // Tag 닫기
	}

	//
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore().ConfigureAwait(false);
		GC.SuppressFinalize(this);
	}

	//
	private async Task DisposeAsyncCore()
	{
		if (_mode is not Mode.None)
			return;

		if (_js is not null)
			await _js.DisposeModuleAsync(_self);

		_drf?.Dispose();
	}

	//
	[JSInvokable("ivk_tgl_show")]
	public async Task InternalHandleShowAsync()
	{
		if (DropDown is not null)
			await DropDown.InternalExpandedChangedAsync(true);

		await InvokeOnExpandedAsync(new BsExpandedEventArgs(Id, true));
	}

	//
	[JSInvokable("ivk_tgl_hide")]
	public async Task InternalHandleHideAsync()
	{
		if (DropDown is not null)
			await DropDown.InternalExpandedChangedAsync(false);

		await InvokeOnExpandedAsync(new BsExpandedEventArgs(Id, false));
	}

	//
	private Task InvokeOnExpandedAsync(BsExpandedEventArgs e) => OnExpanded.InvokeAsync(e);
}
