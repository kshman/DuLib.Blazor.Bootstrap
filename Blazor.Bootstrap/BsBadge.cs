namespace Du.Blazor.Bootstrap;

/// <summary> 뺏지 </summary>
public class BsBadge : BsComponent
{
	/// <summary>글자색</summary>
	[Parameter] public BsVariant? Fore { get; set; }
	/// <summary>배경색</summary>
	[Parameter] public BsVariant? Back { get; set; }
	/// <summary>레이아웃 <see cref="BsBadgeType"/></summary>
	[Parameter] public BsBadgeType? Type { get; set; }

	//
	protected override void OnComponentClass(BsCss cssc)
	{
		cssc.Add("badge")
			.Add((Fore ?? BsSettings.BadgeFore).ToCss("text"))
			.Add((Back ?? BsSettings.BadgeBack).ToCss("bg"))
			.Add((Type ?? BsSettings.BadgeType) switch
			{
				BsBadgeType.Pill => "rounded-pill",
				BsBadgeType.Circle => "rounded-circle",
				_ => null
			})
			.Add(BsSettings.BadgeAdditionalCss);
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ComponentRenderer.TagFragment(this, builder, "span");
}
