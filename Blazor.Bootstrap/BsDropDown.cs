namespace Du.Blazor.Bootstrap;

/// <summary>
/// 드랍다운 컴포넌트
/// </summary>
/// <remarks>
/// <para>
/// 내부에서 지원하는 컴포넌트:
/// <list type="table">
/// <listheader><term>컴포넌트</term><description>설명</description></listheader>
/// <item><term><see cref="BsToggle"/></term><description>토글 버튼 제공</description></item>
/// <item><term><see cref="BsDropMenu"/></term><description>실제 메뉴 아이템 목록의 처리</description></item>
/// </list>
/// </para>
/// <para>
/// 일반적인 구성은 이렇슴:
/// <code>
/// &lt;DropDown&gt;
///	    &lt;Toggle Text="토글러"/&gt; // 토글 메뉴 제공
///     &lt;DropMenu&gt; // 메뉴 아이템 제공
///         // 여기에 표현할 컴포넌트
///     &lt;/DropMenu&gt;
/// &lt;/DropDown&gt;
/// </code>
/// </para>
/// </remarks>
public class BsDropDown : BsComponent
{
	/// <summary>나브바. 캐스케이딩되면 아래 딸려오는 컴포넌트가 나브바를 지원하게 동작한다</summary>
	[CascadingParameter] public BsNavBar? NavBar { get; set; }

	/// <summary>드랍 방향 <see cref="BsDropDirection"/></summary>
	[Parameter] public BsDropDirection Direction { get; set; } = BsDropDirection.Down;

	/// <summary>확장 여부</summary>
	[Parameter] public bool Expanded { get; set; }
	/// <summary>확장 여부가 변경될 때 이벤트</summary>
	[Parameter] public EventCallback<bool> ExpandedChanged { get; set; }

	//
	protected override void OnComponentClass(BsCss cssc)
	{
		cssc.Add(Direction.ToCss())
			.Add(NavBar is null, "btn-group");

		// 오프캔버스에 맞춰 컬럼 옵션을 추가
		if (NavBar?.Type == BsNavBarType.OffCanvas && !cssc.TestAny("col-"))
		{
			cssc.Add("col-12")
				.Add(NavBar.Expand.ToCss("col", "auto"));
		}
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * <div class="@CssClass" id="@Id" @attributes="UserAttrs">
		 *     <CascadingValue Value="this" IsFixed="true">
		 *         @ChildContent
		 *     </CascadingValue>
		 * </div>
		 */
		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", CssClass);
		builder.AddAttribute(2, "id", Id);
		builder.AddMultipleAttributes(3, UserAttrs);

		builder.OpenComponent<CascadingValue<BsDropDown>>(4);
		builder.AddAttribute(5, "Value", this);
		builder.AddAttribute(6, "IsFixed", true);
		builder.AddAttribute(7, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(8, ChildContent)));
		builder.CloseComponent(); // CascadingValue<DropDown>

		builder.CloseElement(); // div
	}

	//
	internal Task InternalExpandedChangedAsync(bool e)
	{
		Expanded = e;
		return InvokeExpandedChangedAsync(e);
	}

	//
	private Task InvokeExpandedChangedAsync(bool e) => ExpandedChanged.InvokeAsync(e);
}
