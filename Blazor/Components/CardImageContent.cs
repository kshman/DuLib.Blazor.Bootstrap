using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>
/// 카드 이미지가 있을 때만 쓰는 콘텐트 컴포넌트
/// </summary>
public class CardImageContent : TagContentObject<Card>
{
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

	// 
	protected override void OnComponentInitialized()
	{
		LogIf.FailWithMessage(Logger, Image is not null, Settings.UseLocaleMesg
			? "이미지가 없으면 <TagContent>를 쓰세요."
			: "Use <TagContent> without image");
	}

	//
	protected override void OnComponentClass(CssCompose css)
	{
		css
			.Add(Location is CardImageLocation.Overlay ? "card-img-overlay" : "card-body")
			.AddIf(Class is null, Card.DefaultSettings.ContentClass);
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * @if (Image.IsHave(true) && istop)
		 * {
		 *     <img src="@Image" alt="@Alt" width="@Width" height="@Height" 
		 *         class="@(Location== CardImageLocation.Top ? "card-img-top" : "card-img")" />
		 * }
		 * <div class="@CssClass" @attributes="UserAttrs">
		 *     @ChildContent
		 * </div>
		 * @if (Image.IsHave(true) && isbottom)
		 * {
		 *     <img src="@Image" alt="@Alt" width="@Width" height="@Height" class="card-img-bottom" />
		 * }
		 */

		var image = Image.IsHave(true);

		if (image && (Location is CardImageLocation.Top or CardImageLocation.Overlay))
		{
			builder.OpenElement(0, "img");
			builder.AddAttribute(1, "class", Location == CardImageLocation.Top ? "card-img-top" : "card-img");
			builder.AddAttribute(2, "src", Image);
			builder.AddAttribute(3, "alt", Alt);
			builder.AddAttribute(4, "width", Width);
			builder.AddAttribute(5, "height", Height);
			builder.CloseElement(); // img
		}

		builder.OpenElement(10, "div");
		builder.AddAttribute(11, "class", CssClass);
		builder.AddMultipleAttributes(12, UserAttrs);
		builder.AddContent(13, ChildContent);
		builder.CloseElement(); // div

		if (image && Location is CardImageLocation.Bottom)
		{
			builder.OpenElement(20, "img");
			builder.AddAttribute(21, "class", "card-img-bottom");
			builder.AddAttribute(22, "src", Image);
			builder.AddAttribute(23, "alt", Alt);
			builder.AddAttribute(24, "width", Width);
			builder.AddAttribute(25, "height", Height);
			builder.CloseElement(); // img
		}
	}
}
