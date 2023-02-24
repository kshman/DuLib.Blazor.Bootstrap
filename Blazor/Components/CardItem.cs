using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>DIV 아이템</summary>
public class CardDiv : CardItem
{
	protected override string Tag => "div";
}


/// <summary>SPAN 아이템</summary>
public class CardSpan : CardItem
{
	protected override string Tag => "span";
}


/// <summary>카드 아이템</summary>
/// <seealso cref="Du.Blazor.ComponentContent" />
public class CardItem : TagItem
{
	/// <summary>사용할 태그 지정</summary>
	protected virtual string Tag => "p";

	//
	protected override void OnComponentClass(CssCompose css) =>
		css.Add("card-text");

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * <p class="@CssClass" @attributes="@UserAttrs">
		 *	@Text
		 *	@ChildContent
		 * </p>
		 */

		builder.OpenElement(0, Tag);

		builder.AddAttribute(1, "class", CssClass);
		builder.AddMultipleAttributes(2, UserAttrs);

		builder.AddContent(3, Text);
		builder.AddContent(4, ChildContent);

		builder.CloseElement(); // p - span - div
	}
}
