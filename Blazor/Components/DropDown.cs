using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>
/// 드랍다운 컴포넌트
/// </summary>
/// <remarks>
/// <para>
/// 내부에서 지원하는 컴포넌트:
/// <list type="table">
/// <listheader><term>컴포넌트</term><description>설명</description></listheader>
/// <item><term><see cref="Toggle"/></term><description>토글 버튼 제공</description></item>
/// <item><term><see cref="DropMenu"/></term><description>실제 메뉴 아이템 목록의 처리</description></item>
/// </list>
/// </para>
/// <para>
/// 일반적인 구성은 이렇슴:
/// <code>
/// &lt;DropDown&gt;
///	    &lt;Toggle Text="토글러"/&gt; // 토글 메뉴 제공
///     &lt;DropMenu&gt; // 메뉴 아이템 제공
///         // 여기에 표현할 컴포넌트
///     &lt;/DropMenu&gt;
/// &lt;/DropDown&gt;
/// </code>
/// </para>
/// </remarks>
public class DropDown : ComponentFragment
{
	/// <summary>나브바. 캐스케이딩되면 아래 딸려오는 컴포넌트가 나브바를 지원하게 동작한다</summary>
	[CascadingParameter] public NavBar? NavBar { get; set; }

	/// <summary>드랍 방향 <see cref="DropDirection"/></summary>
	[Parameter] public DropDirection Direction { get; set; }

	/// <summary>확장 여부</summary>
	[Parameter] public bool Expanded { get; set; }
	/// <summary>확장 여부가 변경될 때 이벤트</summary>
	[Parameter] public EventCallback<bool> ExpandedChanged { get; set; }

	//
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add(Direction.ToCss())
			.AddIf(NavBar is null, "btn-group");
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * <div class="@CssClass" id="@Id" @attributes="UserAttrs">
		 *     <CascadingValue Value="this" IsFixed="true">
		 *         @ChildContent
		 *     </CascadingValue>
		 * </div>
		 */
		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", CssClass);
		builder.AddAttribute(2, "id", Id);
		builder.AddMultipleAttributes(3, UserAttrs);

		builder.OpenComponent<CascadingValue<DropDown>>(4);
		builder.AddAttribute(5, "Value", this);
		builder.AddAttribute(6, "IsFixed", true);
		builder.AddAttribute(7, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(8, ChildContent)));
		builder.CloseComponent(); // CascadingValue<DropDown>

		builder.CloseElement(); // div
	}

	//
	internal Task InternalExpandedChangedAsync(bool e)
	{
		Expanded = e;
		return InvokeExpandedChangedAsync(e);
	}

	//
	private Task InvokeExpandedChangedAsync(bool e) => ExpandedChanged.InvokeAsync(e);
}
