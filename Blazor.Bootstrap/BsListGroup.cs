﻿namespace Du.Blazor.Bootstrap;

/// <summary>
/// 리스트 그룹
/// </summary>
/// <remarks>
/// 내부에서 쓸수 있는 컴포넌트:
/// <list type="table">
/// <listheader><term>컴포넌트</term><description>설명</description></listheader>
/// <item><term><see cref="BsTagItem"/></term><description>P 태그 제공</description></item>
/// <item><term><see cref="BsTagSpan"/></term><description>SPAN 태그 제공</description></item>
/// <item><term><see cref="BsTagDiv"/></term><description>DIV 태그 제공</description></item>
/// <item><term><see cref="TagItem"/></term><description>P 태그 제공</description></item>
/// <item><term><see cref="TagSpan"/></term><description>SPAN 태그 제공</description></item>
/// <item><term><see cref="TagDiv"/></term><description>DIV 태그 제공</description></item>
/// <item><term><see cref="BsButton"/></term><description>버튼/링크</description></item>
/// <item><term><see cref="BsNavLink"/></term><description>나브 링크</description></item>
/// </list>
/// </remarks>
public class BsListGroup : ComponentFragment, ITagItemHandler, ITagListAgent
{
	[Parameter] public bool Flush { get; set; }
	[Parameter] public bool Numbered { get; set; }
	[Parameter] public BsExpand Horizontal { get; set; } = BsExpand.None;

	/// <inheritdoc />
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add("list-group")
			.Add(Flush, "list-group-flush")
			.Add(Numbered, "list-group-numbered")
			.Add(Horizontal.ToListGroupCss());
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ComponentRenderer.CascadingTagFragment<BsListGroup>(this, builder);

	#region ITagListAgent
	//
	string? ITagListAgent.Tag => null;
	//
	string ITagListAgent.Class => "list-group-item list-group-item-action";
	#endregion

	#region ITagItemHandler
	/// <inheritdoc />
	void ITagItemHandler.OnClass(TagItem item, CssCompose cssc)
	{
		cssc.Add("list-group-item")
			.Add(item.OnClick.HasDelegate, "list-group-item-action")
			.Add(((item as BsTagItem)?.Variant ?? BsVariant.None).ToCss("list-group-item"));
	}

	/// <inheritdoc />
	void ITagItemHandler.OnRender(TagItem item, RenderTreeBuilder builder) =>
		ComponentRenderer.TagText(item, builder);
	#endregion
}