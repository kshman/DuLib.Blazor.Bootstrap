namespace Du.Blazor.Bootstrap;

/// <summary>
/// 카드 컴포넌트<br/>
/// 이미지를 출력하려면 반드시 <see cref="BsContent"/> 태그를 포함해야한다
/// </summary>
/// <remarks>
/// 내부에서 쓸 수 있는 컴포넌트:
/// <list type="table">
/// <listheader><term>컴포넌트</term><description>설명</description></listheader>
/// <item><term><see cref="BsTag"/></term><description>P 태그 제공</description></item>
/// <item><term><see cref="BsSpan"/></term><description>SPAN 태그 제공</description></item>
/// <item><term><see cref="BsDiv"/></term><description>DIV 태그 제공</description></item>
/// <item><term><see cref="BsHeader"/></term><description>헤더</description></item>
/// <item><term><see cref="BsFooter"/></term><description>푸터</description></item>
/// <item><term><see cref="BsContent"/></term><description>콘텐트</description></item>
/// </list>
/// </remarks>
public class BsCard : BsComponent, IBsContentHandler, IBsTagHandler
{
	/// <summary>카드에 넣을 이미지 URL</summary>
	[Parameter] public string? Image { get; set; }
	/// <summary>카드에 넣을 이미지의 별명(이게 없으면 브라우저에서 욕함)</summary>
	[Parameter] public string Text { get; set; } = "Card image";
	/// <summary>카드에 넣을 이미지의 가로 너비</summary>
	[Parameter] public int? Width { get; set; }
	/// <summary>카드에 넣을 이미지의 세로 높이</summary>
	[Parameter] public int? Height { get; set; }
	/// <summary>카드에 넣을 이미지가 놓여지는 방법</summary>
	[Parameter] public BsCardImage Location { get; set; }

	//
	protected override void OnComponentClass(BsCss cssc)
	{
		cssc.Add("card")
			.Add(Class is null, BsSettings.CardClass);
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ComponentRenderer.CascadingTagFragment<BsCard>(this, builder);

	#region IBsContentHandler
	/// <inheritdoc />
	void IBsContentHandler.OnClass(BsContentRole role, BsContent content, BsCss cssc)
	{
		switch (role)
		{
			case BsContentRole.Header:
				cssc.Add("card-header")
					.Add(content.Class is null, BsSettings.CardHeaderClass);
				break;
			case BsContentRole.Footer:
				cssc.Add("card-footer")
					.Add(content.Class is null, BsSettings.CardFooterClass);
				break;
			case BsContentRole.Content:
				cssc.Add(Location is BsCardImage.Overlay ? "card-img-overlay" : "card-body")
					.Add(content.Class is null, BsSettings.CardContentClass);
				break;
			default:
				ThrowIf.ArgumentOutOfRange(nameof(role), role);
				break;
		}
	}

	/// <inheritdoc />
	void IBsContentHandler.OnRender(BsContentRole role, BsContent content, RenderTreeBuilder builder)
	{
		switch (role)
		{
			case BsContentRole.Header:
			case BsContentRole.Footer:
				ComponentRenderer.TagFragment(content, builder);
				break;
			case BsContentRole.Content:
				InternalRenderTreeContent(content, builder);
				break;
			default:
				ThrowIf.ArgumentOutOfRange(nameof(role), role);
				break;
		}
	}

	//
	private void InternalRenderTreeContent(BsContent content, RenderTreeBuilder builder)
	{
		/*
		 * @if (Image.IsHave(true) && is_top)
		 * {
		 *     <img src="@Image" alt="@Alt" width="@Width" height="@Height" 
		 *         class="@(Location== BsCardImage.Top ? "card-img-top" : "card-img")" />
		 * }
		 * <div class="@CssClass" @attributes="UserAttrs">
		 *     @ChildContent
		 * </div>
		 * @if (Image.IsHave(true) && is_bottom)
		 * {
		 *     <img src="@Image" alt="@Alt" width="@Width" height="@Height" class="card-img-bottom" />
		 * }
		 */

		var image = Image.IsHave();

		if (image && (Location is BsCardImage.Top or BsCardImage.Overlay))
		{
			builder.OpenElement(0, "img");
			builder.AddAttribute(1, "class", Location == BsCardImage.Top ? "card-img-top" : "card-img");
			builder.AddAttribute(2, "src", Image);
			builder.AddAttribute(3, "alt", Text);
			builder.AddAttribute(4, "width", Width);
			builder.AddAttribute(5, "height", Height);
			builder.CloseElement(); // img
		}

		builder.OpenElement(10, "div");
		builder.AddAttribute(11, "class", content.CssClass);
		builder.AddMultipleAttributes(12, content.UserAttrs);
		builder.AddContent(13, content.ChildContent);
		builder.CloseElement(); // div

		if (image && Location is BsCardImage.Bottom)
		{
			builder.OpenElement(20, "img");
			builder.AddAttribute(21, "class", "card-img-bottom");
			builder.AddAttribute(22, "src", Image);
			builder.AddAttribute(23, "alt", Text);
			builder.AddAttribute(24, "width", Width);
			builder.AddAttribute(25, "height", Height);
			builder.CloseElement(); // img
		}
	}
	#endregion

	#region IBsTagHandler
	/// <inheritdoc />
	void IBsTagHandler.OnClass(BsTag item, BsCss cssc) =>
		cssc.Add("card-text");

	/// <inheritdoc />
	void IBsTagHandler.OnRender(BsTag item, RenderTreeBuilder builder) =>
		ComponentRenderer.TagText(item, builder);
	#endregion
}
