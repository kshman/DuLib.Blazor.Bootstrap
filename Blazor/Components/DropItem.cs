using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>DIV 아이템</summary>
public class DropDiv : DropItem
{
}


/// <summary>SPAN 아이템</summary>
public class DropSpan : DropItem
{
	protected override string Tag => "span";
}


/// <summary>태그 아이템. 기본은 그냥 DIV 감싸기</summary>
public class DropItem : ContentItem
{
	[CascadingParameter] public DropMenu? DropMenu { get; set; }

	[Parameter] public bool DropText { get; set; }

	[Parameter] public string? ListClass { get; set; }

	/// <summary>사용할 태그 지정</summary>
	protected virtual string Tag => "div";

	//
	protected override void CheckContainer()
	{
		LogIf.ContainerIsNull(Logger, Container, DropMenu);
	}

	//
	protected override void OnComponentClass(CssCompose css)
	{
		if (DropMenu is not null)
			css.AddSelect(DropText, "dropdown-item-text", "dropdown-item");
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * @if (DropMenu is not null)
		 * {
		 * 	<li>
		 * 		<div class="@CssClass" @attributes="@UserAttrs">
		 * 			@Text
		 * 			@ChildContent
		 * 		</div>
		 * 	</li>
		 * }
		 * else
		 * {
		 * 	<div class="@CssClass" @attributes="@UserAttrs">
		 * 		@Text
		 * 		@ChildContent
		 * 	</div>
		 * }
		 */

		if (DropMenu is not null)
		{
			builder.OpenElement(0, "li");

			if (ListClass.IsHave(true))
				builder.AddAttribute(1, ListClass);
		}

		builder.OpenElement(2, Tag);

		builder.AddAttribute(3, "class", CssClass);
		builder.AddMultipleAttributes(4, UserAttrs);

		if (Text is not null)
			builder.AddContent(5, Text);
		if (Display is not null)
		{
			builder.AddContent(6, Display);

			if (Content is not null)
				builder.AddContent(7, Content);
		}
		else if (Content is not null)
			builder.AddContent(8, Content);
		else if (ChildContent is not null)
			builder.AddContent(9, ChildContent);

		builder.CloseElement(); // span or div

		if (DropMenu is not null)
			builder.CloseElement(); // li
	}

	//
	public override string ToString()
	{
		return DropMenu is not null
			? $"DROPITEM <{GetType().Name}#{{Id}}>"
			: base.ToString();
	}
}
