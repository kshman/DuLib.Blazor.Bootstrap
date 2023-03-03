﻿namespace Du.Blazor.Bootstrap;

/// <summary>드랍 콘텐트</summary>
/// <remarks>원래 <see cref="DropDown"/> 아래 콘텐트 구성용이지만, 단독으로 쓸 수 있음</remarks>
/// <seealso cref="DropMenu"/>
public class DropContent : DropMenu
{
	//
	protected override string TagName => "div";
}


/// <summary>
/// 드랍다운 메뉴 제공 컴포넌트
/// </summary>
/// <remarks>
/// <para>원래 <see cref="DropDown"/> 아래 콘텐트 구성용이지만, 단독으로 쓸 수 있음</para>
/// <para>
/// 내부에서 쓸수 있는 컴포넌트:
/// <list type="table">
/// <listheader><term>컴포넌트</term><description>설명</description></listheader>
/// <item><term><see cref="TagVariantItem"/></term><description>P 태그 제공</description></item>
/// <item><term><see cref="TagSpan"/></term><description>SPAN 태그 제공</description></item>
/// <item><term><see cref="TagDiv"/></term><description>DIV 태그 제공</description></item>
/// <item><term><see cref="Nulo"/></term><description>버튼/링크</description></item>
/// <item><term><see cref="Divider"/></term><description>구분 가로줄</description></item>
/// <item><term><see cref="NavNulo"/></term><description>나브 링크</description></item>
/// </list>
/// </para>
/// </remarks>
public class DropMenu : ComponentFragment, ITagItemHandler, ITagListAgent
{
	/// <summary>드랍다운. 이게 캐스케이딩되면 드랍다운에 맞게 콤포넌트가 동작한다</summary>
	[CascadingParameter] public DropDown? DropDown { get; set; }

	/// <summary>정렬 방법 <see cref="BsDropAlignment"/></summary>
	[Parameter] public BsDropAlignment Alignment { get; set; }
	/// <summary>표시 방법 <see cref="BsPosition"/></summary>
	[Parameter] public BsPosition Position { get; set; } = BsPosition.Static;

	/// <summary>태그를 지정할 수 있습니다</summary>
	/// <remarks>기본값은 ul</remarks>
	protected virtual string TagName => "ul";

	//
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add("dropdown-menu")
			.Add(Alignment.ToCss());

		if (DropDown is not null)
			cssc.Register(() => (DropDown.Expanded).IfTrue("show"));
		else
		{
			cssc.Add(Position.ToCss())
				.Add("show");
		}
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ComponentRenderer.CascadingTagFragment<DropMenu>(this, builder, TagName);

	#region ITagListAgency
	//
	string ITagListAgent.Tag => "li";

	//
	string ITagListAgent.Class => "dropdown-item";
	#endregion

	#region ITagItemHandler
	//
	void ITagItemHandler.OnClass(TagItem item, CssCompose cssc) =>
		cssc.Add(item.TextMode, "dropdown-item-text", "dropdown-item");

	//
	void ITagItemHandler.OnRender(TagItem item, RenderTreeBuilder builder) =>
		ComponentRenderer.SurroundTagText(item, builder, "li");
	#endregion
}