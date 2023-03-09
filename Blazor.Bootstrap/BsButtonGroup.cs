namespace Du.Blazor.Bootstrap;

/// <summary>버튼 그룹</summary>
public  class BsButtonGroup : BsComponent
{
	/// <summary>버튼 그룹 형식</summary>
	[Parameter] public BsGroupType Type { get; set; } = BsGroupType.Button;
	/// <summary>컴포넌트 크기. <see cref="BsSize" /> 참고</summary>
	[Parameter] public BsSize? Size { get; set; }

	//
	protected override void OnComponentClass(BsCss cssc)
	{
		cssc.Add(Type switch
			{
				BsGroupType.Button => "btn-group",
				BsGroupType.Vertical => "btn-group-vertical",
				BsGroupType.Toolbar => "btn-toolbar",
				_ => null,
			})
			.Add(Size?.ToCss("btn-group"));
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * <div role="@Layout.ToCss()" class="@CssClass" id="@Id" @attributes="UserAttrs">
		 *     @ChildContent
		 * </div>
		 */

		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "role", Type.ToCss());
		builder.AddAttribute(2, "class", CssClass);
		builder.AddAttribute(3, "id", Id);
		builder.AddMultipleAttributes(4, UserAttrs);
		builder.AddContent(5, ChildContent);
		builder.CloseElement();
	}
}
