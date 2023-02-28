using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Bootstrap;

/// <summary>
/// 리스트 그룹
/// </summary>
/// <remarks>
/// 내부에서 쓸수 있는 컴포넌트:
/// <list type="table">
/// <listheader><term>컴포넌트</term><description>설명</description></listheader>
/// <item><term><see cref="TagItem"/></term><description>P 태그 제공 / 쓰지 말것 레이아웃 깨짐</description></item>
/// <item><term><see cref="TagSpan"/></term><description>SPAN 태그 제공</description></item>
/// <item><term><see cref="TagDiv"/></term><description>DIV 태그 제공</description></item>
/// <item><term><see cref="Nulo"/></term><description>버튼/링크</description></item>
/// <item><term><see cref="NavNulo"/></term><description>나브 링크</description></item>
/// </list>
/// </remarks>
public class ListGroup : ComponentFragment, ITagItemHandler, ITagListAgent
{
	[Parameter] public bool Flush { get; set; }
	[Parameter] public bool Numbered { get; set; }
	[Parameter] public TagDimension Horizontal { get; set; } = TagDimension.None;

	/// <inheritdoc />
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add("list-group")
			.AddIf(Flush, "list-group-flush")
			.AddIf(Numbered, "list-group-numbered")
			.Add(Horizontal.ToListGroupCss());
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		InternalRenderTreeCascadingTagFragment<ListGroup>(builder);

	#region ITagListAgent
	//
	bool ITagListAgent.SurroundTag => false;
	//
	string ITagListAgent.ActionClass => "list-group-item list-group-item-action";
	#endregion

	#region ITagItemHandler
	/// <inheritdoc />
	void ITagItemHandler.OnTagItemClass(TagItem item, CssCompose cssc)
	{
		cssc.Add("list-group-item")
			.AddIf(item.OnClick.HasDelegate, "list-group-item-action")
			.Add((item.Variant ?? TagVariant.None).ToCss("list-group-item"));
	}

	/// <inheritdoc />
	void ITagItemHandler.OnTagItemBuildRenderTree(TagItem item, RenderTreeBuilder builder) =>
		item.InternalRenderTreeTagText(builder); // li 안쓴다 
	#endregion
}
