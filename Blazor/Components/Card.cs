using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>
/// 카드
/// </summary>
public class Card : ComponentContent, ITagContentAdopter
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

	#region 렌더 프래그먼트

	/// <summary>카드 머리</summary>
	[Parameter] public RenderFragment? Header { get; set; }
	/// <summary>카드 콘텐트</summary>
	[Parameter] public RenderFragment? Content { get; set; }
	/// <summary>카드 꼬리</summary>
	[Parameter] public RenderFragment? Footer { get; set; }

	#endregion

	#region CSS클래스

	/// <summary>카드 머리의 CSS클래스</summary>
	[Parameter] public string? HeaderClass { get; set; }
	/// <summary>카드 콘텐트의 CSS클래스</summary>
	[Parameter] public string? ContentClass { get; set; }
	/// <summary>카드 꼬리의 CSS클래스</summary>
	[Parameter] public string? FooterClass { get; set; }

	#endregion

	#region 이미지

	/// <summary>카드에 넣을 이미지 URL</summary>
	[Parameter] public string? Image { get; set; }
	/// <summary>카드에 넣을 이미지의 별명(이게 없으면 브라우저에서 욕함)</summary>
	[Parameter] public string Alt { get; set; } = "Card image";
	/// <summary>카드에 넣을 이미지의 가로 너비</summary>
	[Parameter] public int? Width { get; set; }
	/// <summary>카드에 넣을 이미지의 세로 높이</summary>
	[Parameter] public int? Height { get; set; }
	/// <summary>카드에 넣을 이미지가 놓여지는 방버ㅓ</summary>
	[Parameter] public CardImageLocation Location { get; set; }

	#endregion

	//
	public string? HeaderCssClass => CssCompose.Join(
		"card-header",
		HeaderClass ?? DefaultSettings.HeaderClass);
	public string? ContentCssClass => CssCompose.Join(
		Location is CardImageLocation.Overlay ? "card-img-overlay" : "card-body",
		ContentClass ?? DefaultSettings.ContentClass);
	public string? FooterCssClass => CssCompose.Join(
		"card-footer",
		FooterClass ?? DefaultSettings.FooterClass);

	//
	protected override void OnComponentClass(CssCompose css)
	{
		css
			.Add("card")
			.AddIf(Class is null, DefaultSettings.Class);
	}

	//
	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * <div class="@CssClass" @attributes="@UserAttrs">
		 *     @if (Header is not null)
		 *     {
		 *         <div class="@HeaderCssClass">
		 *             @Header
		 *         </div>
		 *     }
		 *     @if (Image.IsHave(true) && IsTopImage)
		 *     {
		 *         <img src="@Image" alt="@Alt" width="@Width" height="@Height" class="@(Location== CardImageLocation.Top ? "card-img-top" : "card-img")" />
		 *     }
		 *     @if (Content is not null)
		 *     {
		 *         <div class="@ContentCssClass">
		 *             @Content
		 *         </div>
		 *     }
		 *     @ChildContent
		 *     @if (Image.IsHave(true) && IsBottomImage)
		 *     {
		 *         <img src="@Image" alt="@Alt" width="@Width" height="@Height" class="card-img-bottom" />
		 *     }
		 *     @if (Footer is not null)
		 *     {
		 *         <div class="@FooterCssClass">
		 *             @Footer
		 *         </div>
		 *     }
		 * </div>
		 */
		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", CssClass);
		builder.AddMultipleAttributes(2, UserAttrs);

		if (Header is not null)
		{
			builder.OpenElement(10, "div");
			builder.AddAttribute(11, "class", HeaderCssClass);
			builder.AddContent(12, Header);
			builder.CloseElement();
		}

		if (Image.IsHave(true) && (Location is CardImageLocation.Top or CardImageLocation.Overlay))
		{
			builder.OpenElement(20, "img");
			builder.AddAttribute(21, "class", Location == CardImageLocation.Top ? "card-img-top" : "card-img");
			builder.AddAttribute(22, "src", Image);
			builder.AddAttribute(23, "alt", Alt);
			builder.AddAttribute(24, "width", Width);
			builder.AddAttribute(25, "height", Height);
			builder.CloseElement();
		}

		if (Content is not null)
		{
			builder.OpenElement(30, "div");
			builder.AddAttribute(31, "class", ContentCssClass);
			builder.AddContent(32, Content);
			builder.CloseElement();
		}

		builder.OpenComponent<CascadingValue<Card>>(40);
		builder.AddAttribute(41, "Value", this);
		builder.AddAttribute(42, "IsFixed", true);
		builder.AddAttribute(43, "ChildContent", (RenderFragment)((b) =>
				b.AddContent(44, ChildContent)));
		builder.CloseComponent(); // CascadingValue<Card>

		if (Image.IsHave(true) && Location is CardImageLocation.Bottom)
		{
			builder.OpenElement(50, "img");
			builder.AddAttribute(51, "class", "card-img-bottom");
			builder.AddAttribute(52, "src", Image);
			builder.AddAttribute(53, "alt", Alt);
			builder.AddAttribute(54, "width", Width);
			builder.AddAttribute(55, "height", Height);
			builder.CloseElement();
		}

		if (Footer is not null)
		{
			builder.OpenElement(60, "div");
			builder.AddAttribute(61, "class", FooterCssClass);
			builder.AddContent(62, Footer);
			builder.CloseElement();
		}

		builder.CloseElement(); // div
	}
}
