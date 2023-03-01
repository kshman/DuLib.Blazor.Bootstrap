namespace Du.Blazor.Bootstrap;

/// <summary>
///   <para>토글러, 보통 붕괴 토글에 쓰임</para>
///   <para>드랍다운의 펼치기/닫기 버튼으로 쓰이며, NavBar 버튼으로도 쓸 수 있음</para>
/// </summary>
/// <seealso cref="DropDown" />
public class Toggle : ComponentFragment, IAsyncDisposable
{
	// 우선순위
	//	1. 드랍다운			=> nav-link dropdown-toggle
	//	2. 나브바			=> navbar-toggler
	//	3. 붕괴(=나브바)	=> (none)
	// 우선 순위를 지키지 않으면 나브바 아래 드랍다운이 동작안한다
	// 한편, 나브바와 붕괴가 같이 있으면... 안된다. 동작이 제대로 안됨

	/// <summary>드랍다운</summary>
	[CascadingParameter] public DropDown? DropDown { get; set; }
	/// <summary>나브바</summary>
	[CascadingParameter] public NavBar? NavBar { get; set; }
	/// <summary>붕괴 아이디. 나브바가 지정될 경우 나브바에서 가져옴</summary>
	[Parameter] public string? CollapseId { get; set; }

	/// <summary>표시할 때 사용하는 태그</summary>
	[Parameter] public string? Tag { get; set; }

	/// <summary>표시할 텍스트</summary>
	[Parameter] public string? Text { get; set; }
	/// <summary>ARIA LABEL. 이 값이 없으면 브라우저가 싫어함</summary>
	[Parameter] public string? AriaLabel { get; set; }
	/// <summary>토글 모양 <see cref="BsToggle"/></summary>
	[Parameter] public BsToggle Layout { get; set; } = BsToggle.Button;
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
	[Parameter] public EventCallback<ExpandedEventArgs> OnExpanded { get; set; }
	// OnClick 구현해야하나?
	//[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

	//
	[Inject] private IJSRuntime JSRuntime { get; set; } = default!;
	[Inject] private ILogger<Toggle> Logger { get; set; } = default!;

	//
	protected BsVariant ActualVariant => Variant ?? Nulo.DefaultSettings.Variant;
	protected BsSize ActualSize => Size ?? Nulo.DefaultSettings.Size;
	protected bool ActualOutline => Outline ?? Nulo.DefaultSettings.Outline;

	//
	private ElementReference _self;
	private IJSObjectReference? _js;
	private DotNetObjectReference<Toggle>? _drf;
	private bool _use_collapse;

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
				Layout = BsToggle.A;
			}
			// 드랍이 없고, 나브바엔 토글이 없음 
			else if (NavBar.ToggleId.IsWhiteSpace())
			{
				// 토글이 여러개면 곤란하다 하나만
				// 아니면, 토글 내 드랍에서 캐치하는 것도 곤란
				NavBar.ToggleId = Id;

				// 나브바에서 아이디 셋팅
				CollapseId ??= '#' + NavBar.CollapseId;

				AriaLabel ??= "Toggle navigation"; // 이거 지정안하면 브라우저에서 욕함
			}
		}

		if (CollapseId.IsHave())
		{
			// 콜랩스 모드
			if (Split) // 스플릿 못씀
			{
				Logger.LogCritical(Settings.UseLocaleMesg
						? "{name}: 붕괴?! 컨트롤의 분리 기능과 나브바를 함께 쓰면 안되요."
						: "{name}: Cannot use with Collapse control or NavBar.",
						nameof(Split));
				Split = false;
			}

			if (NavBar is not null) // 나브바 처리
			{
				Variant ??= BsVariant.None;

				if (Layout is not BsToggle.Button) // 버튼만 됨
				{
					Logger.LogCritical(Settings.UseLocaleMesg
						? "{name}: 나브바 안에서 쓸 때는 반드시 {type} 이어야 해요."
						: "{name}: Must be {type} when contained within NavBar.",
						nameof(Layout), nameof(BsToggle.Button));
					Layout = BsToggle.Button;
				}
			}

			_use_collapse = true;
		}
		else
		{
			// 클랩스가 아닌데 나브바면 이상하쟎아. 예컨데 아이디가 잘못됐다든가
			// 나브바 밑에 드랍이면 있다.
			//ThrowIf.ItemNotNull(NavBar);

			// 이 경우엔 반드시 드랍다운이어야 함
			ThrowIf.ContainerIsNull(this, DropDown);
		}

		if (Split && Layout is not BsToggle.Button)
		{
			Logger.LogError(Settings.UseLocaleMesg
				? "{name}: 스플릿 모드를 쓰려거든 레이아웃을 반드시 버튼으로 하세요."
				: "{name}: Layout must be bsButton when split mode.",
				nameof(Split));
			Layout = BsToggle.Button;
		}

		Logger.LogTrace(Settings.UseLocaleMesg
			? "{name}: 성공적으로 초기화 했어요."
			: "{name}: initialized successfully.",
			nameof(Toggle));
	}

	//
	protected override void OnComponentClass(CssCompose cssc)
	{
		if (NavBar is not null)
		{
			if (DropDown is null)
				cssc.Add("navbar-toggler");
			else
			{
				cssc.Add("nav-link")
					.Add("dropdown-toggle"); // 밑에도 나오지만, 그때는 _use_collapse가 false임 여긴 true
			}
		}

		if (Layout == BsToggle.Button)
		{
			cssc.Add(_use_collapse is false, "dropdown-toggle")
				.Add("btn")
				.Add(ActualVariant.ToButtonCss(ActualOutline))
				.Add(ActualSize.ToCss("btn"));
		}
		else
		{
			cssc.Add(Caret && _use_collapse is false, "dropdown-toggle");
		}

		cssc.Add(Split, "dropdown-toggle-split")
			.Register(() => (DropDown?.Expanded ?? false).IfTrue("show"));
	}

	//
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
			return;

		if (_use_collapse)
			return;

		_drf ??= DotNetObjectReference.Create(this);
		await PrepareModule();
		if (_js is not null)
			await _js.InvokeVoidAsync("initialize", _self, _drf);
	}

	//
	private async Task PrepareModule()
	{
		_js ??= await JSRuntime.ImportModuleAsync("toggle");
	}

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

		var tag = Tag ?? Layout.ToTag();

		builder.OpenElement(0, tag);

		builder.AddAttribute(1, Layout == BsToggle.Button ? "type" : "role", "button");
		builder.AddAttribute(2, "class", CssClass);

		if (_use_collapse && DropDown is null)
		{
			// 붕괴 또는 나브바 일 떄
			builder.AddAttribute(3, "data-bs-toggle", "collapse");
			builder.AddAttribute(4, "data-bs-target", CollapseId);
			if (CollapseId![0] == '#')
				builder.AddAttribute(5, "aria-controls", CollapseId[1..]);
		}
		else
		{
			// 드랍다운 일 때
			builder.AddAttribute(6, "data-bs-toggle", "dropdown");
			builder.AddAttribute(7, "data-bs-auto-close", AutoClose.ToCss());
		}

		builder.AddAttribute(8, "aria-expanded", "false");
		builder.AddAttribute(9, "aria-label", AriaLabel);
		builder.AddMultipleAttributes(10, UserAttrs);

		if (_use_collapse is false)
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
	protected virtual async Task DisposeAsyncCore()
	{
		if (_use_collapse)
			return;

		if (_js is not null)
		{
			try
			{
				await _js.InvokeVoidAsync("dispose", _self);
				await _js.DisposeAsync();
			}
			catch (JSDisconnectedException)
			{
				// 그럴 수도 있음
			}
		}

		_drf?.Dispose();
	}

	//
	[JSInvokable("ivk_tgl_show")]
	public async Task InternalHandleShowAsync()
	{
		if (DropDown is not null)
			await DropDown.InternalExpandedChangedAsync(true);

		await InvokeOnExpandedAsync(new ExpandedEventArgs(Id, true));
	}

	//
	[JSInvokable("ivk_tgl_hide")]
	public async Task InternalHandleHideAsync()
	{
		if (DropDown is not null)
			await DropDown.InternalExpandedChangedAsync(false);

		await InvokeOnExpandedAsync(new ExpandedEventArgs(Id, false));
	}

	//
	private Task InvokeOnExpandedAsync(ExpandedEventArgs e) => OnExpanded.InvokeAsync(e);
}
