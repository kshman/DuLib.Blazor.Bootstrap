namespace Du.Blazor.Bootstrap;

/// <summary>
/// 뺏지
/// </summary>
public class BsBadge : ComponentFragment
{
	/// <summary>글자색</summary>
	[Parameter] public BsVariant? Fore { get; set; }
	/// <summary>배경색</summary>
	[Parameter] public BsVariant? Back { get; set; }
	/// <summary>레이아웃 <see cref="BsBadgeType"/></summary>
	[Parameter] public BsBadgeType? Type { get; set; }

	//
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add("badge")
			.Add((Fore ?? BsDefaults.BadgeFore).ToCss("text"))
			.Add((Back ?? BsDefaults.BadgeBack).ToCss("bg"))
			.Add((Type ?? BsDefaults.BadgeType) switch
			{
				BsBadgeType.Pill => "rounded-pill",
				BsBadgeType.Circle => "rounded-circle",
				_ => null
			})
			.Add(BsDefaults.BadgeAdditionalCss);
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ComponentRenderer.TagFragment(this, builder, "span");
}
