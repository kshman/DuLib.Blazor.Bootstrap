namespace Du.Blazor.Components;

/// <summary>뺏지...는 배지</summary>
/// <seealso cref="Du.Blazor.ComponentContent" />
public partial class Badge
{
	/// <summary>배지 설정</summary>
	public class Settings
	{
		public TagColor Fore { get; set; }
		public TagColor Back { get; set; }
		public BadgeLayout Layout { get; set; }
		public string? AdditionalCss { get; set; }
	}

	/// <summary>배지 기본값</summary>
	public static Settings DefaultSettings { get; set; } = new()
	{
		Fore = TagColor.Light,
		Back = TagColor.Primary,
		Layout = BadgeLayout.None,
		AdditionalCss = null,
	};
}
