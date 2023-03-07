namespace Du.Blazor.Bootstrap;

/// <summary>
/// 오프캔바스
/// </summary>
public class BsOffCanvas : ComponentFragment, IAsyncDisposable, ITagContentHandler
{
	/// <summary>윗단에 놓이는 나브바</summary>
	[CascadingParameter] public BsNavBar? NavBar { get; set; }

	/// <summary>닫기 버튼 표시. 기본값: 표시</summary>
	[Parameter] public bool? CloseButton { get; set; }
	/// <summary>스크롤 가능한가. 기본값: 안씀</summary>
	[Parameter] public bool? Scrollable { get; set; }
	/// <summary>백드랍. 기본값: 안씀</summary>
	[Parameter] public BsBackDrop? BackDrop { get; set; }
	/// <summary>Responsive. 기본값: None</summary>
	[Parameter] public BsExpand? Responsive { get; set; }
	/// <summary>표시 위치. 기본값: 오른쪽</summary>
	[Parameter] public BsPlacement? Placement { get; set; }

	/// <summary>언제나 그린다. 기본값: 끔</summary>
	[Parameter] public bool Always { get; set; } //= false;
	/// <summary>열림 상태</summary>
	[Parameter] public bool Expanded { get; set; }

	/// <summary>열림 이벤트</summary>
	[Parameter] public EventCallback<ExpandedEventArgs> OnExpanded { get; set; }
	/// <summary>열림 상태 변경 이벤트</summary>
	[Parameter] public EventCallback<bool> ExpandedChanged { get; set; }

	//
	[Inject] private IJSRuntime JSRuntime { get; set; } = default!;

	//
	private bool ActualCloseButton => CloseButton ?? BsDefaults.OffCanvasCloseButton;
	private bool ActualScrollable => Scrollable ?? BsDefaults.OffCanvasScrollable;
	private BsBackDrop? ActualBackDrop => BackDrop ?? BsDefaults.OffCanvasBackDrop;
	private BsExpand? ActualResponsive => Responsive ?? BsDefaults.OffCanvasResponsive;
	private BsPlacement ActualPlacement => Placement ?? BsDefaults.OffCanvasPlacement;

	//
	private ElementReference _self;
	private IJSObjectReference? _js;
	private DotNetObjectReference<BsOffCanvas>? _drf;
	private bool _internal_expanding;

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		if (NavBar is not null)
		{
			// 나브바 아래 있을 땐 나브바에서 준 아이디를 쓴다
			Id = NavBar.TargetId;
			Responsive ??= NavBar.Expand;

			NavBar.OffCanvasRef ??= this;
		}

		_internal_expanding = Expanded;
	}

	/// <inheritdoc />
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc
			.Add(ActualResponsive is null ? "offcanvas" : ((BsExpand)ActualResponsive).ToCss("offcanvas"))
			.Add(ActualPlacement.ToOffCanvasCss())
			.Add(NavBar?.Type == BsNavBarType.OffCanvas, "flex-grow-1")
			.Add(Class is null, BsDefaults.OffCanvasClass)
			.Register(() => Expanded.IfTrue("show"));
	}

	/// <inheritdoc />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
			_drf ??= DotNetObjectReference.Create(this);

		if (_internal_expanding)
		{
			_internal_expanding = false;
			await (await PrepareModule())
				.InvokeVoidAsync("show", _self, _drf, ActualScrollable, true);
		}
	}

	//
	private async ValueTask<IJSObjectReference> PrepareModule() =>
		_js ??= await JSRuntime.ImportModuleAsync("offcanvas");

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * <div @ref="_self" class="@CssClass" tabindex="-1" id="@Id" data-bs-backdrop="@ActualBackDrop.ToBootStrap()">
		 * 	@if (_expanded || Always || Responsive != BsExpand.None)
		 * 	{
		 * 		<CascadingValue Value="this" IsFixed="true">
		 * 			@ChildContent
		 * 		</CascadingValue>
		 * 	}
		 * </div>
		 */
		var backdrop = ActualBackDrop;
		var responsive = ActualResponsive;

		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", CssClass);
		builder.AddAttribute(2, "tabindex", -1);
		builder.AddAttribute(3, "id", Id);

		if (backdrop is not null)
			builder.AddAttribute(4, "data-bs-backdrop", ((BsBackDrop)backdrop).ToBootStrap());

#if false
		if (NavBar is not null) // 나브바 모드일 때는 무조건 스크롤
			builder.AddAttribute(5, "data-bs-scroll", "true");
#endif

		builder.AddElementReferenceCapture(6, (p) => _self = p);

		if (Expanded || Always || responsive is not null)
		{
			builder.OpenComponent<CascadingValue<BsOffCanvas>>(7);
			builder.AddAttribute(8, "Value", this);
			builder.AddAttribute(9, "IsFixed", true);
			builder.AddAttribute(10, "ChildContent", (RenderFragment)((b) =>
				b.AddContent(11, ChildContent)));
			builder.CloseComponent(); // CascadingValue<TType>
		}

		builder.CloseElement();
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
		if (Expanded)
		{
			if (_js is not null)
				await _js.DisposeModuleAsync(_self);
		}

		_drf?.Dispose();
	}

	//
	public Task ExpandAsync()
	{
		if (Expanded is false)
			_internal_expanding = true;

		Expanded = true;

		StateHasChanged();

		return Task.CompletedTask;
	}

	//
	public async Task CollapseAsync()
	{
		if (Expanded)
			return;

		await (await PrepareModule())
			.InvokeVoidAsync("hide", _self);
	}

	//
	[JSInvokable("ivk_ofcs_os")]
	public async Task InternalHandleShown()
	{
		await InvokeExpandedChanged(true);
		await InvokeOnExpanded(new ExpandedEventArgs(Id, true));
	}

	//
	[JSInvokable("ivk_ofcs_oh")]
	public async Task InternalHandleHidden()
	{
		Expanded = false;
		await InvokeExpandedChanged(false);
		await InvokeOnExpanded(new ExpandedEventArgs(Id, false));
		StateHasChanged();
	}

	//
	private Task InvokeOnExpanded(ExpandedEventArgs e) => OnExpanded.InvokeAsync(e);
	private Task InvokeExpandedChanged(bool e) => ExpandedChanged.InvokeAsync(e);

	#region ITagContentHandler
	//
	void ITagContentHandler.OnClass(TagContentRole role, TagContent content, CssCompose cssc)
	{
		switch (role)
		{
			case TagContentRole.Header:
				cssc.Add("offcanvas-header")
					.Add(content.Class is null, BsDefaults.OffCanvasHeaderClass);
				break;
			case TagContentRole.Footer:
				cssc.Add("offcanvas-footer")
					.Add(content.Class is null, BsDefaults.OffCanvasFooterClass);
				break;
			case TagContentRole.Content:
				cssc.Add("offcanvas-body")
					.Add(content.Class is null, BsDefaults.OffCanvasContentClass);
				break;
			default:
				ThrowIf.ArgumentOutOfRange(nameof(role), role);
				break;
		}
	}

	//
	void ITagContentHandler.OnRender(TagContentRole role, TagContent content, RenderTreeBuilder builder)
	{
		switch (role)
		{
			case TagContentRole.Header:
				InternalRenderTreeHeader(content, builder);
				break;
			case TagContentRole.Footer:
			case TagContentRole.Content:
				ComponentRenderer.TagFragment(content, builder);
				break;
			default:
				ThrowIf.ArgumentOutOfRange(nameof(role), role);
				break;
		}
	}

	private void InternalRenderTreeHeader(TagContent content, RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", content.CssClass);
		builder.AddMultipleAttributes(2, content.UserAttrs);
		builder.AddContent(3, content.ChildContent);

		if (ActualCloseButton)
		{
			// <button type="button" class="btn-close" data-bs-dismiss="offcanvas" aria-label="Close"></button>
			builder.OpenElement(4, "button");
			builder.AddAttribute(5, "type", "button");
			builder.AddAttribute(6, "class", "btn-close");
			builder.AddAttribute(7, "data-bs-target", '#' + Id);
			builder.AddAttribute(8, "data-bs-dismiss", "offcanvas");
			builder.AddAttribute(9, "aria-label", "Close");
			builder.CloseElement();
		}

		builder.CloseElement(); // div
	}
	#endregion
}
