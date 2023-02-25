﻿using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>
/// 나브
/// </summary>
public class Nav : ComponentContent
{
	/// <summary>나브바. 이게 캐스케이딩되면 나브바에 맞춰 컴포넌트를 설정</summary>
	[CascadingParameter] public NavBar? NavBar { get; set; }

	/// <summary>이름. 이름이 지정되면 캐스캐이딩함</summary>
	[Parameter] public string? Name { get; set; }

	/// <summary>표시 방향 <see cref="TagDirection"/></summary>
	[Parameter] public TagDirection Direction { get; set; } = TagDirection.Horizontal;
	/// <summary>표시 레이아웃 <see cref="NavLayout"/></summary>
	[Parameter] public NavLayout Layout { get; set; } = NavLayout.None;

	//
	protected override void OnComponentClass(CssCompose css)
	{
		css
			.AddSelect(NavBar is null, "nav", "navbar-nav")
			.Add(Direction switch
			{
				TagDirection.Vertical => "flex-column",
				TagDirection.Horizontal or
					_ => null,
			}).
			Add(Layout.ToCss());
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
			builder.OpenComponent<CascadingValue<NavBar>>(5);
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