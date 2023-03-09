namespace Du.Blazor.Bootstrap;

/// <summary>
/// 리스트 그룹
/// </summary>
/// <remarks>
/// 내부에서 쓸수 있는 컴포넌트:
/// <list type="table">
/// <listheader><term>컴포넌트</term><description>설명</description></listheader>
/// <item><term><see cref="BsTag"/></term><description>P 태그 제공</description></item>
/// <item><term><see cref="BsSpan"/></term><description>SPAN 태그 제공</description></item>
/// <item><term><see cref="BsDiv"/></term><description>DIV 태그 제공</description></item>
/// <item><term><see cref="BsTag"/></term><description>P 태그 제공</description></item>
/// <item><term><see cref="BsButton"/></term><description>버튼/링크</description></item>
/// <item><term><see cref="BsNavLink"/></term><description>나브 링크</description></item>
/// </list>
/// </remarks>
public class BsListGroup : BsComponent, IBsTagHandler, IBsListAgent
{
	[Parameter] public bool Flush { get; set; }
	[Parameter] public bool Numbered { get; set; }
	[Parameter] public BsExpand? Horizontal { get; set; }

	/// <inheritdoc />
	protected override void OnComponentClass(BsCss cssc)
	{
		cssc.Add("list-group")
			.Add(Flush, "list-group-flush")
			.Add(Numbered, "list-group-numbered")
			.Add(Horizontal?.ToCss("list-group-horizontal"));
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ComponentRenderer.CascadingTagFragment<BsListGroup>(this, builder);

	#region IBsListAgent
	//
	string? IBsListAgent.Tag => null;
	//
	string IBsListAgent.Class => "list-group-item list-group-item-action";
	#endregion

	#region IBsTagHandler
	/// <inheritdoc />
	void IBsTagHandler.OnClass(BsTag item, BsCss cssc)
	{
		cssc.Add("list-group-item")
			.Add(item.OnClick.HasDelegate, "list-group-item-action")
			.Add(item.Variant?.ToCss("list-group-item"));
	}

	/// <inheritdoc />
	void IBsTagHandler.OnRender(BsTag item, RenderTreeBuilder builder) =>
		ComponentRenderer.TagText(item, builder);
	#endregion
}
