namespace Du.Blazor.Bootstrap;

/// <inheritdoc/>
public class ButtonGroup : NuloGroup
{
}


/// <summary>
/// 버튼 그룹 
/// </summary>
public  class NuloGroup : ComponentFragment
{
	/// <summary>버튼 그룹 형식</summary>
	[Parameter] public BsButtonGroup Layout { get; set; }
	/// <summary>컴포넌트 크기. <see cref="BsSize" /> 참고</summary>
	[Parameter] public BsSize Size { get; set; }

	//
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add(Layout switch
			{
				BsButtonGroup.Button => "btn-group",
				BsButtonGroup.Vertical => "btn-group-vertical",
				BsButtonGroup.Toolbar => "btn-toolbar",
				_ => null,
			})
			.Add(Size.ToCss("btn-group"));
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
		builder.AddAttribute(1, "role", Layout.ToCss());
		builder.AddAttribute(2, "class", CssClass);
		builder.AddAttribute(3, "id", Id);
		builder.AddMultipleAttributes(4, UserAttrs);
		builder.AddContent(5, ChildContent);
		builder.CloseElement();
	}
}
