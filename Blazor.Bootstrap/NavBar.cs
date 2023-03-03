namespace Du.Blazor.Bootstrap;

/// <summary>
/// 나브바 컴포넌트
/// </summary>
public class NavBar : ComponentFragment
{
	#region 나브
	/// <summary>나브 크기 <see cref="BsNavBarExpand"/></summary>
	[Parameter] public BsNavBarExpand Expand { get; set; } = BsNavBarExpand.Large;
	/// <summary>나브 색깔 <see cref="BsVariant"/></summary>
	[Parameter] public BsVariant Variant { get; set; } = BsVariant.None;
	/// <summary>nav 대신 header 태그를 사용합니다.</summary>
	[Parameter] public bool AsHeader { get; set; }
	/// <summary>오프캔바스 모드 사용.</summary>
	[Parameter] public bool OffCanvas { get; set; }
	#endregion

	#region 컨테이너
	/// <summary>컨테이너의 css클래스</summary>
	[Parameter] public string? ContainerClass { get; set; }
	/// <summary>커네이너의 레이아웃 <see cref="BsExpand"/></summary>
	/// <remarks>딱히 지정하지 않는게 좋긴하다! 기본은 container-fluid</remarks>
	[Parameter] public BsExpand ContainerLayout { get; set; } = BsExpand.NavFluid;
	#endregion

	//
	/// <summary>나브바 토글과 충돌에 쓰이는 아이디</summary>
	public string TargetId { get; private set; } = default!;
	/// <summary>나브바에 등록된 토글의 아이디</summary>
	public string? ToggleId { get; set; }

	//
	private string? ContainerCssClass => CssCompose.Join(ContainerLayout.ToContainerCss(), ContainerClass);

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		TargetId = Id + "_target";
	}

	//
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add("navbar")
			.Add(Expand.ToCss())
			.Add(Variant.ToCss("bg"));
	}
	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * <nav class="@CssClass" id="@Id" @attributes="UserAttrs">
		 *     <div class="@ContainerCssClass">
		 *         <CascadingValue Value="this" IsFixed="true">
		 *             @ChildContent
		 *         </CascadingValue>
		 *     </div>
		 * </nav>
		 */
		builder.OpenElement(0, AsHeader ? "header" : "nav");
		builder.AddAttribute(1, "class", CssClass);
		builder.AddAttribute(2, "id", Id);
		builder.AddMultipleAttributes(3, UserAttrs);

		builder.OpenElement(4, AsHeader ? "nav" : "div");
		builder.AddAttribute(5, "class", ContainerCssClass);

		builder.OpenComponent<CascadingValue<NavBar>>(6);
		builder.AddAttribute(7, "Value", this);
		builder.AddAttribute(8, "IsFixed", true);
		builder.AddAttribute(9, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(10, ChildContent)));
		builder.CloseComponent(); // CascadingValue<NavBar>

		builder.CloseElement(); // div

		builder.CloseElement(); // nav
	}
}
