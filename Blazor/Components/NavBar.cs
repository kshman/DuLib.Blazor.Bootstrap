﻿using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>
/// 나브바
/// </summary>
public class NavBar : ComponentContent
{
	#region 나브
	/// <summary>나브 크기 <see cref="NavBarExpand"/></summary>
	[Parameter] public NavBarExpand Expand { get; set; } = NavBarExpand.Large;
	/// <summary>나브 색깔 <see cref="TagColor"/></summary>
	[Parameter] public TagColor Color { get; set; } = TagColor.None;
	#endregion

	#region 컨테이너
	/// <summary>컨테이너의 css클래스</summary>
	[Parameter] public string? ContainerClass { get; set; }
	/// <summary>커네이너의 레이아웃 <see cref="NavContainerLayout"/></summary>
	/// <remarks>딱히 지정하지 않는게 좋긴하다! 기본은 container-fluid</remarks>
	[Parameter] public NavContainerLayout ContainerLayout { get; set; } = NavContainerLayout.Fluid;
	#endregion

	//
	/// <summary>나브바 토글과 충돌에 쓰이는 아이디</summary>
	public string CollapseId => Id + "_collapse";
	/// <summary>나브바에 등록된 토글의 아이디</summary>
	public string? ToggleId { get; set; }

	//
	private string? ContainerCssClass => CssCompose.Join(ContainerLayout.ToCss(), ContainerClass);

	//
	protected override void OnComponentClass(CssCompose css)
	{
		css
			.Add("navbar")
			.Add(Expand.ToCss())
			.Add(Color.ToCss("bg"));
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
		builder.OpenElement(0, "nav");
		builder.AddAttribute(1, "class", CssClass);
		builder.AddAttribute(2, "id", Id);
		builder.AddMultipleAttributes(3, UserAttrs);

		builder.OpenElement(4, "div");
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