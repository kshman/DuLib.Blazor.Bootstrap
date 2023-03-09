namespace Du.Blazor.Bootstrap;

/// <summary>
/// 모달
/// </summary>
public class BsModal : BsComponent, IBsContentHandler, IAsyncDisposable
{
	[Parameter] public bool? CloseButton { get; set; }
	[Parameter] public bool? Scrollable { get; set; }
	[Parameter] public bool? Middle { get; set; }
	[Parameter] public bool? Fade { get; set; }
	[Parameter] public BsBackDrop? BackDrop { get; set; }
	[Parameter] public BsExpand? Expand { get; set; }
	[Parameter] public BsExpand? FullScreen { get; set; }
	[Parameter] public string? DialogClass { get; set; }
	/// <summary>확장 여부</summary>
	[Parameter] public bool Expanded { get; set; }

	/// <summary>확장된 다음 이벤트</summary>
	[Parameter] public EventCallback<BsExpandedEventArgs> OnExpanded { get; set; }
	/// <summary>확장 여부 변경 이벤트</summary>
	[Parameter] public EventCallback<bool> ExpandedChanged { get; set; }

	[Inject] private IJSRuntime JSRuntime { get; set; } = default!;

	private bool ActualCloseButton => CloseButton ?? BsSettings.ModalCloseButton;
	private bool ActualScrollable => Scrollable ?? BsSettings.ModalScrollable;
	private bool ActualMiddle => Middle ?? BsSettings.ModalMiddle;
	private bool ActualFade => Fade ?? BsSettings.ModalFade;
	private BsBackDrop? ActualBackDrop => BackDrop ?? BsSettings.ModalBackDrop;
	private BsExpand? ActualExpand => Expand ?? BsSettings.ModalExpand;
	private BsExpand? ActualFullScreen => FullScreen ?? BsSettings.ModalFullScreen;
	private string? ActualDialogClass => DialogClass ?? BsSettings.ModalDialogClass;

	private ElementReference _self;
	private IJSObjectReference? _js;
	private DotNetObjectReference<BsModal>? _drf;

	private readonly BsCss _css_dialog = new();
	private bool _expanded;

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		_expanded = Expanded;
	}

	/// <inheritdoc />
	protected override void OnComponentClass(BsCss cssc)
	{
		cssc.Add("modal")
			.Add(ActualFade, "fade")
			.Add(_expanded, "show")
			.Add(Class is null, BsSettings.ModalClass);

		_css_dialog
			.Add("modal-dialog")
			.Add(ActualExpand?.ToCss("modal"))
			.Add(ActualFullScreen?.ToCss("modal-fullscreen", "down", true))
			.Add(ActualScrollable, "modal-dialog-scrollable")
			.Add(ActualMiddle, "modal-dialog-centered")
			.Add(ActualDialogClass);
	}

	/// <inheritdoc />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
			return;

		_drf ??= DotNetObjectReference.Create(this);

		await (await PrepareModule())
			.InvokeVoidAsync("initialize", _self, _drf, Expanded);
	}

	//
	private async ValueTask<IJSObjectReference> PrepareModule() =>
		_js ??= await JSRuntime.ImportModuleAsync("modal");

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * <div @ref="_self" class="@CssClass" tabindex="-1" data-bs-backdrop="@ActualBackDrop?.ToBootStrap()">
		 * 	<div class="@_css_dialog.Class">
		 * 		<div class="modal-content">
		 * 			@ChildContent
		 * 		</div>
		 * 	</div>
		 * </div>
		 */

		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", CssClass);
		builder.AddAttribute(2, "tabindex", "-1");
		builder.AddAttribute(3, "data-bs-backdrop", ActualBackDrop?.ToBootStrap());
		builder.AddElementReferenceCapture(4, p => _self = p);

		builder.OpenElement(5, "div");
		builder.AddAttribute(6, "class", _css_dialog.Class);

		builder.OpenElement(7, "div");
		builder.AddAttribute(8, "class", "modal-content");

		builder.OpenComponent<CascadingValue<BsModal>>(9);
		builder.AddAttribute(10, "Value", this);
		builder.AddAttribute(11, "IsFixed", true);
		builder.AddAttribute(12, "ChildContent", (RenderFragment)((b) =>
				b.AddContent(13, ChildContent)));
		builder.CloseComponent(); // CascadingValue<BsModal>

		builder.CloseElement(); // div

		builder.CloseElement(); // div

		builder.CloseElement(); // div
	}

	//
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore().ConfigureAwait(false);
		GC.SuppressFinalize(this);
	}

	//
	private async ValueTask DisposeAsyncCore()
	{
		if (_js is not null)
			await _js.DisposeModuleAsync(_self);

		_drf?.Dispose();
	}

	/// <summary>
	/// 모달 열기
	/// </summary>
	/// <returns></returns>
	public async Task ExpandAsync()
	{
		if (_expanded)
			return;

		await (await PrepareModule())
			.InvokeVoidAsync("show", _self);
	}

	/// <summary>
	/// 모달 닫기. 이 메소드는 쓰지 말고 BsButton Close="true"로 만들것
	/// </summary>
	/// <returns></returns>
	public async Task CollapseAsync()
	{
		if (_expanded is false)
			return;

		await (await PrepareModule())
			.InvokeVoidAsync("hide", _self);
	}

	//
	[JSInvokable("ivk_mdl_os")]
	public async Task InternalHandleShownAsync()
	{
		_expanded = true;
		Expanded = true;

		await InvokeExpandedChanged(true);
		await InvokeOnExpanded(Id, true);
	}

	//
	[JSInvokable("ivk_mdl_oh")]
	public async Task InternalHandleHiddenAsync()
	{
		_expanded = false;
		Expanded = true;

		await InvokeExpandedChanged(false);
		await InvokeOnExpanded(Id, false);
	}

	//
	private Task InvokeOnExpanded(string id, bool expanded) =>
		OnExpanded.InvokeAsync(new BsExpandedEventArgs(id, expanded));
	private Task InvokeExpandedChanged(bool e) => ExpandedChanged.InvokeAsync(e);

	#region IBsContentHandler
	/// <inheritdoc />
	void IBsContentHandler.OnClass(BsContentRole role, BsContent content, BsCss cssc)
	{
		switch (role)
		{
			case BsContentRole.Header:
				cssc.Add("modal-header")
					.Add(content.Class is null, BsSettings.ModalHeaderClass);
				break;
			case BsContentRole.Footer:
				cssc.Add("modal-footer")
					.Add(content.Class is null, BsSettings.ModalFooterClass);
				break;
			case BsContentRole.Content:
				cssc.Add("modal-body")
					.Add(content.Class is null, BsSettings.ModalContentClass);
				break;
			default:
				ThrowIf.ArgumentOutOfRange(nameof(role), role);
				break;
		}
	}

	/// <inheritdoc />
	void IBsContentHandler.OnRender(BsContentRole role, BsContent content, RenderTreeBuilder builder)
	{
		switch (role)
		{
			case BsContentRole.Header:
				InternalRenderTreeHeader(content, builder);
				break;
			case BsContentRole.Footer:
			case BsContentRole.Content:
				ComponentRenderer.TagFragment(content, builder);
				break;
			default:
				ThrowIf.ArgumentOutOfRange(nameof(role), role);
				break;
		}
	}

	//
	private void InternalRenderTreeHeader(BsContent content, RenderTreeBuilder builder)
	{
		/*
		 * <div class="modal-header">
		 * 	<ModalTitle/>
		 * 	@HeadChild
		 * 	@if (ActualCloseButton)
		 * 	{
		 * 		<button type="button" class="btn-close text-reset" data-bs-dismiss="modal" aria-label="Close"></button>
		 * 	}
		 * </div>
		 */

		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", content.CssClass);
		builder.AddMultipleAttributes(2, content.UserAttrs);
		builder.AddContent(3, content.ChildContent);

		if (ActualCloseButton)
		{
			builder.OpenElement(10, "button");
			builder.AddAttribute(11, "type", "button");
			builder.AddAttribute(12, "class", "btn-close text-reset");
			builder.AddAttribute(13, "data-bs-dismiss", "modal");
			builder.AddAttribute(14, "aria-label", "Close");
			builder.CloseElement();
		}

		builder.CloseElement();
	}
	#endregion
}
