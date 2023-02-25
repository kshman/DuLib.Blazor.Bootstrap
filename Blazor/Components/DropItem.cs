using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>DIV 아이템</summary>
public class DropDiv : DropItem
{
	protected override string Tag => "div";
}


/// <summary>SPAN 아이템</summary>
public class DropSpan : DropItem
{
	protected override string Tag => "span";
}


/// <summary>태그 아이템. 기본은 그냥 P 감싸기</summary>
public class DropItem : TagItem
{
	/// <summary>드랍메뉴</summary>
	[CascadingParameter] public DropMenu? DropMenu { get; set; }

	/// <summary>참일 경우 드랍 텍스트로 출력한다 (마우스로 활성화되지 않는 기능)</summary>
	[Parameter] public bool DropText { get; set; }

	/// <summary>드랍메뉴가 있을 경우 리스트(li)에 사용할 css클래스</summary>
	[Parameter] public string? ListClass { get; set; }

	/// <summary>사용할 태그 지정</summary>
	protected virtual string Tag => "p";

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
		builder.AddContent(5, Text);
		builder.AddContent(6, ChildContent);
		builder.CloseElement(); // tag

		if (DropMenu is not null)
			builder.CloseElement(); // li
	}

	//
	public override string ToString() => DropMenu is not null
			? $"DROPITEM <{GetType().Name}#{{Id}}>"
			: base.ToString();
}
