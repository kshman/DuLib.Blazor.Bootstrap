using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>
/// 카드
/// </summary>
public class Card : ComponentContent, ITagContentAdopter, ITagItemAdopter
{
	#region 기본 세팅
	public class Settings
	{
		public string? Class { get; set; }
		public string? HeaderClass { get; set; }
		public string? ContentClass { get; set; }
		public string? FooterClass { get; set; }
	}

	public static Settings DefaultSettings { get; set; } = new Settings();
	#endregion

	//
	protected override void OnComponentClass(CssCompose css)
	{
		css
			.Add("card")
			.AddIf(Class is null, DefaultSettings.Class);
	}

	//
	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		InternalRenderTreeCascadingTag<Card>(builder);
}
