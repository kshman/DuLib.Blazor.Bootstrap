namespace Du.Blazor.Bootstrap;

/// <summary>
/// 뺏지
/// </summary>
public class Badge : ComponentFragment
{
	#region 기본 세팅
	/// <summary>배지 설정</summary>
	public class Settings
	{
		public BsVariant Fore { get; set; }
		public BsVariant Back { get; set; }
		public BsBadgeType Layout { get; set; }
		public string? AdditionalCss { get; set; }
	}

	/// <summary>배지 기본값</summary>
	public static Settings DefaultSettings { get; set; }

	static Badge()
	{
		DefaultSettings = new Settings
		{
			Fore = BsVariant.Light,
			Back = BsVariant.Primary,
			Layout = BsBadgeType.None,
			AdditionalCss = null,
		};
	}
	#endregion

	/// <summary>글자색</summary>
	[Parameter] public BsVariant? Fore { get; set; }
	/// <summary>배경색</summary>
	[Parameter] public BsVariant? Back { get; set; }
	/// <summary>레이아웃 <see cref="BsBadgeType"/></summary>
	[Parameter] public BsBadgeType? Layout { get; set; }

	//
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add("badge")
			.Add((Fore ?? DefaultSettings.Fore).ToCss("text"))
			.Add((Back ?? DefaultSettings.Back).ToCss("bg"))
			.Add((Layout ?? DefaultSettings.Layout) switch
			{
				BsBadgeType.Pill => "rounded-pill",
				BsBadgeType.Circle => "rounded-circle",
				_ => null
			})
			.Add(DefaultSettings.AdditionalCss);
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ComponentRenderer.TagFragment(this, builder, "span");
}
