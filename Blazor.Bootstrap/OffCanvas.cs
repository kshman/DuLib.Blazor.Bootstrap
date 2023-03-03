namespace Du.Blazor.Bootstrap;

/// <summary>
/// 오프캔바스
/// </summary>
public class OffCanvas : ComponentFragment, IAsyncDisposable, ITagContentHandler
{
	#region 기본 설정
	public class Settings
	{
		public bool CloseButton { get; set; }
		public bool Scrollable { get; set; }
		public BsBackDrop? BackDrop { get; set; }
		public BsDimension Responsive { get; set; }
		public BsPlacement Placement { get; set; }
		public string? Class { get; set; }
		public string? HeaderClass { get; set; }
		public string? ContentClass { get; set; }
		public string? FooterClass { get; set; }
	}

	public static Settings DefaultSettings { get; }

	static OffCanvas()
	{
		DefaultSettings = new Settings
		{
			CloseButton = true,
			Scrollable = false,
			BackDrop = null,
			Responsive = BsDimension.None,
			Placement = BsPlacement.Right,
		};
	}
	#endregion

	/// <summary>윗단에 놓이는 나브바</summary>
	[CascadingParameter] public NavBar? NavBar { get; set; }

	[Parameter] public Settings? Set { get; set; }

	[Parameter] public string? Text { get; set; }
	[Parameter] public bool? CloseButton { get; set; }
	[Parameter] public bool? Scrollable { get; set; }
	[Parameter] public BsBackDrop? BackDrop { get; set; }
	[Parameter] public BsDimension? Responsive { get; set; }
	[Parameter] public BsPlacement? Placement { get; set; }

	// 언제나 그린다
	[Parameter] public bool Always { get; set; } //= false;
	[Parameter] public bool Expanded { get; set; }

	[Parameter] public EventCallback<ExpandedEventArgs> OnExpanded { get; set; }
	[Parameter] public EventCallback<bool> ExpandedChanged { get; set; }

	//
	[Inject] private IJSRuntime JSRuntime { get; set; } = default!;

	//
	private bool ActualCloseButton => CloseButton ?? Set?.CloseButton ?? DefaultSettings.CloseButton;
	private bool ActualScrollable => Scrollable ?? Set?.Scrollable ?? DefaultSettings.Scrollable;
	private BsBackDrop? ActualBackDrop => BackDrop ?? Set?.BackDrop ?? DefaultSettings.BackDrop;
	private BsDimension ActualResponsive => Responsive ?? Set?.Responsive ?? DefaultSettings.Responsive;
	private BsPlacement ActualPlacement => Placement ?? Set?.Placement ?? DefaultSettings.Placement;

	//
	private ElementReference _self;
	private IJSObjectReference? _js;
	private DotNetObjectReference<OffCanvas>? _drf;
	private bool _internal_expanding;

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		if (NavBar is not null)
		{
			// 나브바 아래 있을 땐 나브바에서 준 아이디를 쓴다
			Id = NavBar.TargetId;
		}

		_internal_expanding = Expanded;
	}

	/// <inheritdoc />
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc
			.Add(ActualResponsive.ToOffCanvasCss())
			.Add(ActualPlacement.ToOffCanvasCss())
			.Add(Class is null, Set?.Class ?? DefaultSettings.Class)
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
		_js ??= await JSRuntime.ImportModuleAsync<OffCanvas>();

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * <div @ref="_self" class="@CssClass" tabindex="-1" id="@Id" data-bs-backdrop="@ActualBackDrop.ToBootStrap()">
		 * 	@if (_expanded || Always || Responsive != BsDimension.None)
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

		if (NavBar is not null)
			builder.AddAttribute(5, "data-bs-scroll",  "true");

		builder.AddElementReferenceCapture(6, (p) => _self = p);

		if (Expanded || Always || responsive != BsDimension.None)
		{
			builder.OpenComponent<CascadingValue<OffCanvas>>(7);
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
	protected virtual async Task DisposeAsyncCore()
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
					.Add(content.Class is null, Set?.HeaderClass ?? DefaultSettings.HeaderClass);
				break;
			case TagContentRole.Footer:
				cssc.Add("offcanvas-footer")
					.Add(content.Class is null, Set?.FooterClass ?? DefaultSettings.FooterClass);
				break;
			case TagContentRole.Content:
				cssc.Add("offcanvas-body")
					.Add(content.Class is null, Set?.ContentClass ?? DefaultSettings.ContentClass);
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

			if (NavBar is not null)
				builder.AddAttribute(10, "data-bs-target", '#' + Id);

			builder.CloseElement();
		}

		builder.CloseElement(); // div
	}
	#endregion
}
