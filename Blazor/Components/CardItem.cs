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
public class CardItem : ContentItem
{
	/// <summary>사용할 태그 지정</summary>
	protected virtual string Tag => "p";

	//
	protected override void CheckContainer()
	{
		// 카드는 안감싸므로 체크 안함
	}

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

		if (Text is not null)
			builder.AddContent(3, Text);
		if (Display is not null)
		{
			builder.AddContent(4, Display);

			if (Content is not null)
				builder.AddContent(5, Content);
		}
		else if (Content is not null)
			builder.AddContent(6, Content);
		else if (ChildContent is not null)
			builder.AddContent(7, ChildContent);

		builder.CloseElement(); // p - span - div
	}
}
