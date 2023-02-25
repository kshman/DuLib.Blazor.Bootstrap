﻿using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

public class NavBrand : ComponentContent
{
		/// <summary>브랜드를 눌렀을 때 이동할 링크</summary>
	[Parameter] public string? Link { get; set; }

	//
	protected override void OnComponentClass(CssCompose css) =>
		css.Add("navbar-brand");

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * <a class="@CssClass" href="@Link">
		 *     @ChildContent
		 * </a>
		 */
		builder.OpenElement(0, "a");
		builder.AddAttribute(1, "class", CssClass);
		builder.AddAttribute(2, "href", Link);
		builder.AddContent(3, ChildContent);
		builder.CloseElement();
	}
}