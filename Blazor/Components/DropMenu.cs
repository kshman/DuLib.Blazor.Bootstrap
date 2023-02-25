using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>드랍 콘텐트</summary>
/// <remarks>원래 <see cref="DropDown"/> 아래 콘텐트 구성용이지만, 단독으로 쓸 수 있음</remarks>
public class DropContent : DropMenu
{
	//
	protected override string TagName => "div";
}


/// <summary>
/// 드랍다운 메뉴 제공 컴포넌트
/// </summary>
/// <remarks>
/// <para>원래 <see cref="DropDown"/> 아래 콘텐트 구성용이지만, 단독으로 쓸 수 있음</para>
/// <para>
/// 내부에서 쓸수 있는 컴포넌트:
/// <list type="table">
/// <listheader><term>컴포넌트</term><description>설명</description></listheader>
/// <item><term><see cref="TagItem"/></term><description>P 태그 제공</description></item>
/// <item><term><see cref="TagSpan"/></term><description>SPAN 태그 제공</description></item>
/// <item><term><see cref="TagDiv"/></term><description>DIV 태그 제공</description></item>
/// <item><term><see cref="Button"/></term><description>버튼 표시</description></item>
/// <item><term><see cref="Divider"/></term><description>구분 가로줄 표시</description></item>
/// </list>
/// </para>
/// </remarks>
public class DropMenu : ComponentContent, ITagItemAdopter
{
	/// <summary>드랍다운. 이게 캐스케이딩되면 드랍다운에 맞게 콤포넌트가 동작한다</summary>
	[CascadingParameter] public DropDown? DropDown { get; set; }

	/// <summary>정렬 방법 <see cref="DropAlignment"/></summary>
	[Parameter] public DropAlignment Alignment { get; set; }
	/// <summary>표시 방법 <see cref="TagPosition"/></summary>
	[Parameter] public TagPosition Position { get; set; } = TagPosition.Static;

	/// <summary>태그를 지정할 수 있습니다</summary>
	/// <remarks>기본값은 ul</remarks>
	protected virtual string TagName => "ul";

	//
	protected override void OnComponentClass(CssCompose css)
	{
		css
			.Add("dropdown-menu")
			.Add(Alignment.ToCss());

		if (DropDown is not null)
			css.Register(() => (DropDown.Expanded).IfTrue("show"));
		else
		{
			css
				.Add(Position.ToCss())
				.Add("show");
		}
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		InternalRenderTreeCascadingTag<DropMenu>(builder, TagName);
#if false
	{
		/*
		 * <TagElement Tag="@TagName" Class="@CssClass" UserAttrs="@UserAttrs">
		 *     <CascadingValue Value="this" IsFixed="true">
		 *         @ChildContent
		 *     </CascadingValue>
		 * </TagElement>
		 */
		builder.OpenComponent<Web.TagElement>(0);

		builder.AddAttribute(1, "Tag", TagName);
		builder.AddAttribute(2, "Class", CssClass);
		builder.AddMultipleAttributes(3, UserAttrs);
		builder.AddAttribute(4, "ChildContent", (RenderFragment)((bt) =>
		{
			bt.OpenComponent<CascadingValue<DropMenu>>(5);
			bt.AddAttribute(6, "Value", this);
			bt.AddAttribute(7, "IsFixed", true);
			bt.AddAttribute(8, "ChildContent", (RenderFragment)((bc) =>
				bc.AddContent(9, ChildContent)));
			bt.CloseComponent(); // CascadingValue<DropMenu>
		}));

		builder.CloseElement(); // TagElement
	}
#endif
}
