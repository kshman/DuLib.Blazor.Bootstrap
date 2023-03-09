namespace Du.Blazor.Bootstrap;

/// <summary>
/// 드랍다운 메뉴 제공 컴포넌트
/// </summary>
/// <remarks>
/// <para>원래 <see cref="BsDropDown"/> 아래 콘텐트 구성용이지만, 단독으로 쓸 수 있음</para>
/// <para>
/// 내부에서 쓸수 있는 컴포넌트:
/// <list type="table">
/// <listheader><term>컴포넌트</term><description>설명</description></listheader>
/// <item><term><see cref="BsTag"/></term><description>P 태그 제공</description></item>
/// <item><term><see cref="BsSpan"/></term><description>SPAN 태그 제공</description></item>
/// <item><term><see cref="BsDiv"/></term><description>DIV 태그 제공</description></item>
/// <item><term><see cref="BsButton"/></term><description>버튼/링크</description></item>
/// <item><term><see cref="BsDivider"/></term><description>구분 가로줄</description></item>
/// <item><term><see cref="BsNavLink"/></term><description>나브 링크</description></item>
/// </list>
/// </para>
/// </remarks>
public class BsDropMenu : BsDropContent, IBsTagHandler, IBsListAgent
{
	//
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ComponentRenderer.CascadingTagFragment<BsDropMenu>(this, builder, "ul");

	#region IBsListAgent
	//
	string IBsListAgent.Tag => "li";

	//
	string IBsListAgent.Class => "dropdown-item";
	#endregion

	#region IBsTagHandler
	//
	void IBsTagHandler.OnClass(BsTag item, BsCss cssc) =>
		cssc.Add(item.WrapRepresent, "dropdown-item-text", "dropdown-item");

	//
	void IBsTagHandler.OnRender(BsTag item, RenderTreeBuilder builder) =>
		ComponentRenderer.SurroundTagText(item, builder, "li");
	#endregion
}



/// <summary>드랍 콘텐트</summary>
/// <remarks>원래 <see cref="BsDropDown"/> 아래 콘텐트 구성용이지만, 단독으로 쓸 수 있음</remarks>
/// <seealso cref="BsDropMenu"/>
public class BsDropContent : BsComponent
{
	/// <summary>드랍다운. 이게 캐스케이딩되면 드랍다운에 맞게 콤포넌트가 동작한다</summary>
	[CascadingParameter] public BsDropDown? DropDown { get; set; }

	/// <summary>정렬 방법 <see cref="BsDropAlignment"/></summary>
	[Parameter] public BsDropAlignment? Align { get; set; }
	/// <summary>표시 방법 <see cref="BsPosition"/></summary>
	[Parameter] public BsPosition? Position { get; set; }

	//
	/// <inheritdoc />
	protected override void OnInitialized()
	{
		if (DropDown is null)
			Position ??= BsPosition.Static;
	}

	//
	protected override void OnComponentClass(BsCss cssc)
	{
		cssc.Add("dropdown-menu")
			.Add(Align?.ToCss());

		if (DropDown is not null)
			cssc.Register(() => (DropDown.Expanded).IfTrue("show"));
		else
		{
			cssc.Add(Position?.ToCss())
				.Add("show");
		}
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ComponentRenderer.TagFragment(this, builder);
}
