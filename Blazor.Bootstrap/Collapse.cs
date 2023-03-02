namespace Du.Blazor.Bootstrap;

/// <summary>
/// 붕괴?! (콜랩스)<br/>
/// 기능은... 뻔하져
/// </summary>
public class Collapse : ComponentFragment, IAsyncDisposable
{
	/// <summary>윗단에 놓이는 나브바</summary>
	[CascadingParameter] public NavBar? NavBar { get; set; }

	/// <summary>부모 아이디</summary>
	[Parameter] public string? ParentId { get; set; }
	/// <summary>처리 방향</summary>
	[Parameter] public BsDirection Direction { get; set; } = BsDirection.Vertical;
	/// <summary>확장 여부</summary>
	[Parameter] public bool Expanded { get; set; }

	/// <summary>확장될 때 이벤트</summary>
	[Parameter] public EventCallback<ExpandedEventArgs> OnExpanding { get; set; }
	/// <summary>확장된 다음 이벤트</summary>
	[Parameter] public EventCallback<ExpandedEventArgs> OnExpanded { get; set; }
	/// <summary>확장 여부 변경 이벤트</summary>
	[Parameter] public EventCallback<bool> ExpandedChanged { get; set; }

	//
	[Inject] private IJSRuntime JSRuntime { get; set; } = default!;

	//
	private ElementReference _self;
	private IJSObjectReference? _js;
	private DotNetObjectReference<Collapse>? _drf;

	private bool _expanded;
	private bool _now_show;
	private bool _now_hide;

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		if (NavBar is not null)
		{
			// 나브바 아래 있을 땐 나브바에서 준 아이디를 쓴다
			Id = NavBar.CollapseId;
		}
	}

	//
	protected override void OnParametersSet() =>
		_expanded = Expanded;

	//
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add(NavBar is not null, "navbar-collapse")
			.Add("collapse")
			.Add(Direction == BsDirection.Horizontal, "collapse-horizontal")
			.Add(_expanded, "show");
	}

	//
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
		_js ??= await JSRuntime.ImportModuleAsync<Collapse>();

	//
	protected override bool ShouldRender() =>
		!_now_show && !_now_hide;

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/* 
		 * <div @ref="_self"
		 *      class="@CssClass"
		 *      id="@Id"
		 *      aria-expanded="@Expanded"
		 *      data-bs-parent="@ParentId"
		 *      @attributes="@UserAttrs">
		 *     @ChildContent
		 * </div>
		 */
		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", CssClass);
		builder.AddAttribute(2, "id", Id);
		builder.AddAttribute(3, "aria-expanded", Expanded);
		builder.AddAttribute(4, "data-bs-parent", ParentId);
		builder.AddMultipleAttributes(5, UserAttrs);
		builder.AddElementReferenceCapture(6, p => _self = p);
		builder.AddContent(7, ChildContent);
		builder.CloseElement(); // div
	}

	/// <summary>보여준다</summary>
	public async Task ExpandAsync()
	{
		if (_expanded)
			return;

		_now_show = true;

		await (await PrepareModule())
			.InvokeVoidAsync("show", _self);
	}

	//
	/// <summary>감춘다</summary>
	public async Task CollapseAsync()
	{
		if (_expanded is false)
			return;

		_now_hide = true;

		await (await PrepareModule())
			.InvokeVoidAsync("hide", _self);
	}

	//
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore().ConfigureAwait(false);
		GC.SuppressFinalize(this);
	}

	//
	protected virtual async ValueTask DisposeAsyncCore()
	{
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

	// 반드시 public
	[JSInvokable("ivk_clps_bs")]
	public async Task InternalHandleShow()
	{
		if (_js is null)
			return;

		_now_show = true;
		await InvokeOnExpanding(Id, true);
	}

	// 반드시 public
	[JSInvokable("ivk_clps_bsn")]
	public async Task InternalHandleShownAsync()
	{
		if (_js is null)
			return;

		var prev = _now_show;

		_expanded = true;
		await InvokeExpandedChanged(true);

		_now_show = false;
		await InvokeOnExpanded(Id, true);

		if (prev)
			StateHasChanged();
	}

	// 반드시 public
	[JSInvokable("ivk_clps_eh")]
	public async Task InternalHandleHide()
	{
		if (_js is null)
			return;

		_now_hide = true;
		await InvokeOnExpanding(Id, false);
	}

	// 반드시 public
	[JSInvokable("ivk_clps_ehn")]
	public async Task InternalHandleHidden()
	{
		if (_js is null)
			return;

		var prev = _now_hide;

		_expanded = false;
		await InvokeExpandedChanged(false);

		_now_hide = false;
		await InvokeOnExpanded(Id, false);

		if (prev)
			StateHasChanged();
	}

	//
	private Task InvokeOnExpanding(string id, bool expanded) =>
		OnExpanding.InvokeAsync(new ExpandedEventArgs(id, expanded));
	private Task InvokeOnExpanded(string id, bool expanded) =>
		OnExpanded.InvokeAsync(new ExpandedEventArgs(id, expanded));
	private Task InvokeExpandedChanged(bool e) => ExpandedChanged.InvokeAsync(e);
}
