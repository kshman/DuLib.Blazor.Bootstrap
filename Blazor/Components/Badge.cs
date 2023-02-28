using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>
/// 뺏지
/// </summary>
public class Badge : ComponentFragment
{
	#region 기본 세팅
	/// <summary>배지 설정</summary>
	public class Settings
	{
		public TagVariant Fore { get; set; }
		public TagVariant Back { get; set; }
		public BadgeLayout Layout { get; set; }
		public string? AdditionalCss { get; set; }
	}

	/// <summary>배지 기본값</summary>
	public static Settings DefaultSettings { get; }

	static Badge()
	{
		DefaultSettings  = new Settings
		{
			Fore = TagVariant.Light,
			Back = TagVariant.Primary,
			Layout = BadgeLayout.None,
			AdditionalCss = null,
		};
	}
	#endregion

	/// <summary>글자색</summary>
	[Parameter] public TagVariant? Fore { get; set; }
	/// <summary>배경색</summary>
	[Parameter] public TagVariant? Back { get; set; }
	/// <summary>레이아웃 <see cref="BadgeLayout"/></summary>
	[Parameter] public BadgeLayout? Layout { get; set; }

	//
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add("badge")
			.Add((Fore ?? DefaultSettings.Fore).ToCss("text"))
			.Add((Back ?? DefaultSettings.Back).ToCss("bg"))
			.Add((Layout ?? DefaultSettings.Layout) switch
			{
				BadgeLayout.Pill => "rounded-pill",
				BadgeLayout.Circle => "rounded-circle",
				_ => null
			})
			.Add(DefaultSettings.AdditionalCss);
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		InternalRenderTreeTagFragment(builder, "span");
}
