using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>가로 줄 그리기</summary>
public class Divider : ComponentObject
{
	/// <summary>드랍메뉴. 이 내용이 캐스케이딩되면 리스트(li)를 추가한다</summary>
	[CascadingParameter] public DropMenu? DropMenu { get; set; }

	/// <summary>리스트(li)로 출력할 때 사용하는 css클래스</summary>
	[Parameter] public string? ListClass { get; set; }

	//
	protected override void OnComponentClass(CssCompose css) => 
		css.AddIf(DropMenu is not null, "dropdown-divider");

	// 
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * @if (DropDown is not null)
		 * {
		 * 	<li class="@ListClass">
		 * 		<hr class="@CssClass" @attributes="UserAttrs" />
		 * 	</li>
		 * }
		 * else
		 * {
		 * 	<hr class="@CssClass" @attributes="UserAttrs" />
		 * }
		 */
		if (DropMenu is not null)
		{
			builder.OpenElement(0, "li");

			if (ListClass.IsHave(true))
				builder.AddAttribute(1, ListClass);
		}

		builder.OpenElement(2, "hr");

		builder.AddAttribute(3, "class", CssClass);
		builder.AddMultipleAttributes(4, UserAttrs);

		builder.CloseElement(); // hr

		if (DropMenu is not null)
			builder.CloseElement(); // li
	}
}
