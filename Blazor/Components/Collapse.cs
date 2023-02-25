using Microsoft.AspNetCore.Components.Rendering;
using static System.Net.Mime.MediaTypeNames;

namespace Du.Blazor.Components;

/// <summary>
/// 붕괴?! (콜랩스)
/// </summary>
public class Collapse : ComponentContent, IAsyncDisposable
{
	/// <summary>윗단에 놓이는 나브바</summary>
	[CascadingParameter] public NavBar? NavBar { get; set; }

	/// <summary>부모 아이디</summary>
	[Parameter] public string? ParentId { get; set; }
	/// <summary>처리 방향</summary>
	[Parameter] public TagDirection Direction { get; set; } = TagDirection.Vertical;
	/// <summary>확장 여부</summary>
	[Parameter] public bool Expanded { get; set; }

	/// <summary>확장될 때 이벤트</summary>
	[Parameter] public EventCallback<ExpandedEventArgs> OnExpanding { get; set; }
	/// <summary>확장된 다음 이벤트</summary>
	[Parameter] public EventCallback<ExpandedEventArgs> OnExpanded { get; set; }
	/// <summary>확장 여부 변경 이벤트</summary>
	[Parameter] public EventCallback<bool> ExpandedChanged { get; set; }

	//
	[Inject] private IJSRuntime JS { get; set; } = default!;

	//
	private ElementReference _self;
	private DotNetObjectReference<Collapse>? _objref;

	private bool _expanded;
	private bool _now_show;
	private bool _now_hide;

	//
	protected override void OnComponentInitialized()
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
	protected override void OnComponentClass(CssCompose css)
	{
		css
			.AddIf(NavBar is not null, "navbar-collapse")
			.Add("collapse")
			.AddIf(Direction == TagDirection.Horizontal, "collapse-horizontal")
			.AddIf(_expanded, "show");
	}

	//
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
			return;

		_objref ??= DotNetObjectReference.Create(this);
		await JS.InvokeVoidAsync("DUCLPS.init", _self, _objref, Expanded);
	}

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
		await JS.InvokeVoidAsync("DUCLPS.show", _self);
	}

	//
	/// <summary>감춘다</summary>
	public async Task CollapseAsync()
	{
		if (_expanded is false)
			return;

		_now_hide = true;
		await JS.InvokeVoidAsync("DUCLPS.hide", _self);
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
		try
		{
			await JS.InvokeVoidAsync("DUCLPS.disp", _self);
		}
		catch (JSDisconnectedException)
		{
			// 그럴 수도 있음
		}

		_objref?.Dispose();
	}

	// 반드시 public
	[JSInvokable("ivk_clps_bs")]
	public async Task InternalHandleShow()
	{
		_now_show = true;
		await InvokeOnExpanding(Id, true);
	}

	// 반드시 public
	[JSInvokable("ivk_clps_bsn")]
	public async Task InternalHandleShownAsync()
	{
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
		_now_hide = true;
		await InvokeOnExpanding(Id, false);
	}

	// 반드시 public
	[JSInvokable("ivk_clps_ehn")]
	public async Task InternalHandleHidden()
	{
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
