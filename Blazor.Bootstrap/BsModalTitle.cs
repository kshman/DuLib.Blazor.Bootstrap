namespace Du.Blazor.Bootstrap;

/// <summary>
/// 모달용 타이틀
/// </summary>
public class BsModalTitle : BsComponent
{
	[Parameter] public string Tag { get; set; } = "h5";

	/// <inheritdoc />
	protected override void OnComponentClass(BsCss cssc)
	{
		cssc.Add("modal-title");
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, Tag);
		builder.AddAttribute(1, "class", CssClass);
		builder.AddMultipleAttributes(2, UserAttrs);
		builder.AddContent(3, ChildContent);
		builder.CloseElement();
	}
}
