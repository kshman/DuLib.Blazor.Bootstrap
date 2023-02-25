namespace Du.Blazor.Components;

public partial class Card
{
	public class Settings
	{
		public string? Class { get; set; }
		public string? HeaderClass { get; set; }
		public string? ContentClass { get; set; }
		public string? FooterClass { get; set; }
	}

	public static Settings DefaultSettings { get; set; } = new Settings();
}
