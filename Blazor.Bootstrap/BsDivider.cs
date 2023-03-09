namespace Du.Blazor.Bootstrap;

/// <summary>가로 줄 그리기</summary>
public class BsDivider : BsComponent
{
	/// <summary>드랍메뉴. 이 내용이 캐스케이딩되면 리스트(li)를 추가한다</summary>
	[CascadingParameter] public IBsListAgent? ListAgency { get; set; }

	/// <summary>색깔</summary>
	[Parameter] public BsVariant? Color { get; set; }
	/// <summary>리스트(li)로 출력할 때 사용하는 css클래스</summary>
	[Parameter] public string? WrapClass { get; set; }

	//
	protected override void OnComponentClass(BsCss cssc)
	{
		cssc
			.Add(ListAgency switch
			{
				BsDropMenu => "dropdown-divider",
				_ => null,
			})
			.Add(Color?.ToCss("text"));
	}

	// 
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * @if (ListAgency is not null)
		 * {
		 * 	<li class="@WrapClass">
		 * 		<hr class="@CssClass" @attributes="UserAttrs" />
		 * 	</li>
		 * }
		 * else
		 * {
		 * 	<hr class="@CssClass" @attributes="UserAttrs" />
		 * }
		 */

		var list = ListAgency?.Tag ?? null;

		if (list is not null)
		{
			builder.OpenElement(0, list);

			if (WrapClass.IsHave())
				builder.AddAttribute(1, WrapClass);
		}

		builder.OpenElement(2, "hr");

		builder.AddAttribute(3, "class", CssClass);
		builder.AddMultipleAttributes(4, UserAttrs);

		builder.CloseElement(); // hr

		if (list is not null)
			builder.CloseElement(); // li
	}
}
