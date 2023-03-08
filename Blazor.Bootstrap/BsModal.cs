namespace Du.Blazor.Bootstrap;

/// <summary>
/// 모달
/// </summary>
public class BsModal : ComponentFragment, ITagContentHandler, IAsyncDisposable
{
	[Parameter] public bool? CloseButton { get; set; }
	[Parameter] public bool? Scrollable { get; set; }
	[Parameter] public bool? Middle { get; set; }
	[Parameter] public BsBackDrop? BackDrop { get; set; }
	[Parameter] public BsExpand? Size { get; set; }
	[Parameter] public BsExpand? FullScreen { get; set; }
	[Parameter] public string? DialogClass { get; set; }
	/// <summary>확장 여부</summary>
	[Parameter] public bool Expanded { get; set; }
	
	/// <summary>확장된 다음 이벤트</summary>
	[Parameter] public EventCallback<ExpandedEventArgs> OnExpanded { get; set; }
	/// <summary>확장 여부 변경 이벤트</summary>
	[Parameter] public EventCallback<bool> ExpandedChanged { get; set; }

	[Inject] private IJSRuntime JSRuntime { get; set; } = default!;

	private bool ActualCloseButton => CloseButton ?? BsDefaults.ModalCloseButton;
	private bool ActualScrollable => Scrollable ?? BsDefaults.ModalScrollable;
	private bool ActualMiddle => Middle ?? BsDefaults.ModalMiddle;
	private BsBackDrop? ActualBackDrop => BackDrop ?? BsDefaults.ModalBackDrop;
	private BsExpand? ActualSize => Size ?? BsDefaults.ModalSize;
	private BsExpand? ActualFullScreen => FullScreen ?? BsDefaults.ModalFullScreen;
	private string? ActualDialogClass => DialogClass ?? BsDefaults.ModalDialogClass;

	private ElementReference _self;
	private IJSObjectReference? _js;
	private DotNetObjectReference<BsModal>? _drf;

	private readonly CssCompose _css_dialog = new();
	private bool _expanded;

	/// <inheritdoc />
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add("modal fade")
			.Add(Class is null, BsDefaults.ModalClass);

		_css_dialog
			.Add("modal-dialog")
			.Add(ActualSize?.ToCss("modal"))
			.Add(ActualFullScreen?.ToCss("modal-fullscreen", "down", true))
			.Add(ActualScrollable, "modal-dialog-scrollable")
			.Add(ActualMiddle, "modal-dialog-centered")
			.Add(ActualDialogClass);
	}

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		_expanded = Expanded;
	}

	/// <inheritdoc />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
			return;

		_drf ??= DotNetObjectReference.Create(this);

		if (Expanded)
		{
			await (await PrepareModule())
			.InvokeVoidAsync("show", _self, _drf);
		}
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

		if (!_expanded)
			return;

		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", CssClass);
		builder.AddAttribute(2, "tabindex", "-1");
		builder.AddAttribute(3, "data-bs-backdrop", ActualBackDrop?.ToBootStrap());
		builder.AddElementReferenceCapture(4, p => _self = p);

		builder.OpenElement(5, "div");
		builder.AddAttribute(6, "class", _css_dialog.Class);

		builder.OpenElement(7, "div");
		builder.AddAttribute(8, "class", "modal-content");
		builder.AddContent(9, ChildContent);
		builder.CloseElement(); // div

		builder.CloseElement(); // div

		builder.CloseElement(); // div
	}

	//
	public Task ExpandAsync()
	{
		_expanded = true;
		Expanded = true;
		StateHasChanged();
		return Task.CompletedTask;
	}

	//
	public async Task CollapseAsync()
	{
		await (await PrepareModule())
			.InvokeVoidAsync("hide", _self);
	}

	//
	[JSInvokable("ivk_mdl_os")]
	public async Task InternalHandleShownAsync()
	{
		if (_js is null)
			return;

		_expanded = true;
		await InvokeExpandedChanged(true);
		await InvokeOnExpanded(Id, true);
	}

	//
	[JSInvokable("ivk_mdl_oh")]
	public async Task InternalHandleHiddenAsync()
	{
		if (_js is null)
			return;

		_expanded = false;
		await InvokeExpandedChanged(false);
		await InvokeOnExpanded(Id, false);
		StateHasChanged();
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

	//
	private Task InvokeOnExpanded(string id, bool expanded) =>
		OnExpanded.InvokeAsync(new ExpandedEventArgs(id, expanded));
	private Task InvokeExpandedChanged(bool e) => ExpandedChanged.InvokeAsync(e);

	#region ITagContentHandler
	/// <inheritdoc />
	void ITagContentHandler.OnClass(TagContentRole role, TagContent content, CssCompose cssc)
	{
		switch (role)
		{
			case TagContentRole.Header:
				cssc.Add("modal-header")
					.Add(content.Class is null, BsDefaults.ModalHeaderClass);
				break;
			case TagContentRole.Footer:
				cssc.Add("modal-footer")
					.Add(content.Class is null, BsDefaults.ModalFooterClass);
				break;
			case TagContentRole.Content:
				cssc.Add("modal-body")
					.Add(content.Class is null, BsDefaults.ModalContentClass);
				break;
			default:
				ThrowIf.ArgumentOutOfRange(nameof(role), role);
				break;
		}
	}

	/// <inheritdoc />
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

	//
	private void InternalRenderTreeHeader(TagContent content, RenderTreeBuilder builder)
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
