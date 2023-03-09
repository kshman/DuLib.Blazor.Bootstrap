namespace Du.Blazor.Bootstrap;

/// <summary>
/// 나브 컴포넌트
/// </summary>
public class BsNav : BsComponent
{
	/// <summary>나브바. 이게 캐스케이딩되면 나브바에 맞춰 컴포넌트를 설정</summary>
	[CascadingParameter] public BsNavBar? NavBar { get; set; }

	/// <summary>이름. 이름이 지정되면 캐스캐이딩함</summary>
	[Parameter] public string? Name { get; set; }

	/// <summary>표시 방향 <see cref="BsDirection"/></summary>
	[Parameter] public BsDirection? Direction { get; set; }
	/// <summary>표시 레이아웃 <see cref="BsNavType"/></summary>
	[Parameter] public BsNavType? Type { get; set; }

	//
	protected override void OnComponentClass(BsCss cssc)
	{
		cssc.Add(NavBar is null, "nav", "navbar-nav")
			.Add(Direction is BsDirection.Vertical, "flex-column")
			.Add(NavBar?.Type == BsNavBarType.OffCanvas, "flex-row flex-wrap")
			.Add(Type?.ToCss());
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * <nav class="@CssClass" id="@Id" @attributes="UserAttrs">
		 *     <CascadingValue Value="this" IsFixed="true">
		 *         @ChildContent
		 *     </CascadingValue>
		 * </nav>
		 */
		builder.OpenElement(0, "nav");
		builder.AddAttribute(1, "class", CssClass);
		builder.AddAttribute(2, "id", Id);
		builder.AddMultipleAttributes(3, UserAttrs);

		if (Name is null)
			builder.AddContent(4, ChildContent);
		else
		{
			builder.OpenComponent<CascadingValue<BsNavBar>>(5);
			builder.AddAttribute(6, "Value", this);
			builder.AddAttribute(7, "IsFixed", true);
			builder.AddAttribute(8, "Name", Name);
			builder.AddAttribute(9, "ChildContent", (RenderFragment)((b) =>
				b.AddContent(10, ChildContent)));
			builder.CloseComponent(); // CascadingValue<NavBar>
		}

		builder.CloseElement(); // nav
	}
}
