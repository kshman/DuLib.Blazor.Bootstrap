using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;

namespace Du.Blazor.Components;

/// <summary>
///   <para>토글러</para>
///   <para>드랍다운의 펼치기/닫기 버튼으로 쓰이며, NavBar 버튼으로도 쓸 수 있음</para>
/// </summary>
/// <seealso cref="DropDown" />
public class Toggle : ComponentContent, IAsyncDisposable
{
    //[CascadingParameter] public Nav? Nav { get; set; }
    [CascadingParameter] public DropDown? DropDown { get; set; }

    [Parameter] public ToggleLayout Layout { get; set; } // ToggleLayout.Button
    [Parameter] public DropAutoClose AutoClose { get; set; }

    [Parameter] public ComponentColor Color { get; set; }
    [Parameter] public ComponentSize Size { get; set; }
    [Parameter] public bool Outline { get; set; }
    [Parameter] public bool Caret { get; set; }

    [Parameter] public bool Split { get; set; } // 당분간 안만듬

    [Parameter] public string? Text { get; set; }

    [Parameter] public string? Tag { get; set; }

    [Parameter] public EventCallback<ExpandedEventArgs> OnExpanded { get; set; }

    //
    [Inject] private IJSRuntime JS { get; set; } = default!;
    [Inject] private ILogger<Toggle> Logger { get; set; } = default!;

    //
    private ElementReference _self;
    private DotNetObjectReference<Toggle>? _drf;

    //
    protected override void OnComponentInitialized()
    {
        if (Split && Layout is not ToggleLayout.Button)
        {
            Logger.LogError($"{nameof(Split)}: Layout must be button when split mode");
            Layout = ToggleLayout.Button;
        }

        ThrowIf.ContainerIsNull(DropDown, this);
        //ThrowIf.NotImplementedWithCondition<ToggleLayout>(Layout == ToggleLayout.Button);
    }

    //
    protected override void OnComponentClass(CssCompose css)
    {
        if (Layout == ToggleLayout.Button)
        {
            css
                .Add("dropdown-toggle")
                .Add("btn")
                .Add(Color.ToButtonCss(Outline))
                .Add(Size.ToCss("btn"));
        }
        else
        {
            css.AddIf(Caret, "dropdown-toggle");
        }

        css
            .AddIf(Split, "dropdown-toggle-split")
            .Register(() => (DropDown?.Expanded ?? false).IfTrue("show"));
    }

    //
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        _drf ??= DotNetObjectReference.Create(this);
        await JS.InvokeVoidAsync("DUDROP.init", _self, _drf);
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

        // 태그 설정은 나중에 Nav, NavBar 해놓고 바꿔야함 안그러면 속성1의 type/role 깨짐
        //var tag = Nav is not null ? "A" : Tag ?? Layout.ToTag();
        var tag = Tag ?? Layout.ToTag();

        builder.OpenElement(0, tag);

        builder.AddAttribute(1, Layout == ToggleLayout.Button ? "type" : "role", "button");
        builder.AddAttribute(2, "class", CssClass);
        builder.AddAttribute(3, "data-bs-toggle", "dropdown");
        builder.AddAttribute(4, "data-bs-auto-close", AutoClose.ToCss());
        builder.AddAttribute(5, "aria-expanded", "false");
        builder.AddMultipleAttributes(6, UserAttrs);

        builder.AddElementReferenceCapture(7, e => _self = e);

        builder.AddContent(20, Text);
        builder.AddContent(21, ChildContent);

        builder.CloseElement(); // Tag 닫기
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
            await JS.InvokeVoidAsync("DUDROP.disp", _self);
        }
        catch (JSDisconnectedException)
        {
        }

        _drf?.Dispose();
    }

    //
    [JSInvokable("ivk_drop_show")]
    public async Task InternalHandleShowAsync()
    {
        if (DropDown is not null)
            await DropDown.InternalExpandedChangedAsync(true);

        await InvokeOnExpandedAsync(new ExpandedEventArgs(Id, true));
    }

    //
    [JSInvokable("ivk_drop_hide")]
    public async Task InternalHandleHideAsync()
    {
        if (DropDown is not null)
            await DropDown.InternalExpandedChangedAsync(false);

        await InvokeOnExpandedAsync(new ExpandedEventArgs(Id, false));
    }

    //
    private Task InvokeOnExpandedAsync(ExpandedEventArgs e) => OnExpanded.InvokeAsync(e);
}
