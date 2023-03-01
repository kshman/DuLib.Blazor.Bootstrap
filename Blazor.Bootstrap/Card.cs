namespace Du.Blazor.Bootstrap;

/// <summary>
/// 카드 컴포넌트<br/>
/// 이미지를 출력하려면 반드시 <see cref="TagContent"/> 태그를 포함해야한다
/// </summary>
/// <remarks>
/// 내부에서 쓸 수 있는 컴포넌트:
/// <list type="table">
/// <listheader><term>컴포넌트</term><description>설명</description></listheader>
/// <item><term><see cref="TagItem"/></term><description>P 태그 제공</description></item>
/// <item><term><see cref="TagSpan"/></term><description>SPAN 태그 제공</description></item>
/// <item><term><see cref="TagDiv"/></term><description>DIV 태그 제공</description></item>
/// <item><term><see cref="TagHeader"/></term><description>헤더</description></item>
/// <item><term><see cref="TagFooter"/></term><description>푸터</description></item>
/// <item><term><see cref="TagContent"/></term><description>콘텐트</description></item>
/// </list>
/// </remarks>
public class Card : ComponentFragment, ITagContentHandler, ITagItemHandler
{
	#region 기본 세팅
	public class Settings
	{
		public string? Class { get; set; }
		public string? HeaderClass { get; set; }
		public string? FooterClass { get; set; }
		public string? ContentClass { get; set; }
	}

	public static Settings DefaultSettings { get; }

	static Card()
	{
		DefaultSettings = new Settings();
	}
	#endregion

	/// <summary>카드에 넣을 이미지 URL</summary>
	[Parameter] public string? Image { get; set; }
	/// <summary>카드에 넣을 이미지의 별명(이게 없으면 브라우저에서 욕함)</summary>
	[Parameter] public string Alt { get; set; } = "Card image";
	/// <summary>카드에 넣을 이미지의 가로 너비</summary>
	[Parameter] public int? Width { get; set; }
	/// <summary>카드에 넣을 이미지의 세로 높이</summary>
	[Parameter] public int? Height { get; set; }
	/// <summary>카드에 넣을 이미지가 놓여지는 방법</summary>
	[Parameter] public BsCardImageLocation Location { get; set; }

	//
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add("card")
			.Add(Class is null, DefaultSettings.Class);
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ComponentRenderer.CascadingTagFragment<Card>(this, builder);

	#region ITagContentHandler
	/// <inheritdoc />
	void ITagContentHandler.OnClass(TagContentRole role, TagContent content, CssCompose cssc)
	{
		switch (role)
		{
			case TagContentRole.Header:
				cssc.Add("card-header")
					.Add(Class is null, DefaultSettings.HeaderClass);
				break;
			case TagContentRole.Footer:
				cssc.Add("card-footer")
					.Add(Class is null, DefaultSettings.FooterClass);
				break;
			case TagContentRole.Content:
				cssc.Add(Location is BsCardImageLocation.Overlay ? "card-img-overlay" : "card-body")
					.Add(Class is null, DefaultSettings.ContentClass);
				break;
			default:
				ThrowIf.ArgumentOutOfRange(nameof(role), role);
				break;
		}
	}

	/// <inheritdoc />
	void ITagContentHandler.OnRender(TagContentRole role, TagContent content, RenderTreeBuilder builder)
	{
		switch (role)
		{
			case TagContentRole.Header:
			case TagContentRole.Footer:
				ComponentRenderer.TagFragment(content, builder);
				break;
			case TagContentRole.Content:
				InternalRenderTreeContent(content, builder);
				break;
			default:
				ThrowIf.ArgumentOutOfRange(nameof(role), role);
				break;
		}
	}

	//
	private void InternalRenderTreeContent(TagContent content, RenderTreeBuilder builder)
	{
		/*
		 * @if (Image.IsHave(true) && istop)
		 * {
		 *     <img src="@Image" alt="@Alt" width="@Width" height="@Height" 
		 *         class="@(Location== BsCardImageLocation.Top ? "card-img-top" : "card-img")" />
		 * }
		 * <div class="@CssClass" @attributes="UserAttrs">
		 *     @ChildContent
		 * </div>
		 * @if (Image.IsHave(true) && isbottom)
		 * {
		 *     <img src="@Image" alt="@Alt" width="@Width" height="@Height" class="card-img-bottom" />
		 * }
		 */

		var image = Image.IsHave();

		if (image && (Location is BsCardImageLocation.Top or BsCardImageLocation.Overlay))
		{
			builder.OpenElement(0, "img");
			builder.AddAttribute(1, "class", Location == BsCardImageLocation.Top ? "card-img-top" : "card-img");
			builder.AddAttribute(2, "src", Image);
			builder.AddAttribute(3, "alt", Alt);
			builder.AddAttribute(4, "width", Width);
			builder.AddAttribute(5, "height", Height);
			builder.CloseElement(); // img
		}

		builder.OpenElement(10, "div");
		builder.AddAttribute(11, "class", content.CssClass);
		builder.AddMultipleAttributes(12, content.UserAttrs);
		builder.AddContent(13, content.ChildContent);
		builder.CloseElement(); // div

		if (image && Location is BsCardImageLocation.Bottom)
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
	#endregion

	#region ITagItemHandler
	/// <inheritdoc />
	void ITagItemHandler.OnClass(TagItem item, CssCompose cssc) =>
		cssc.Add("card-text");

	/// <inheritdoc />
	void ITagItemHandler.OnRender(TagItem item, RenderTreeBuilder builder) =>
		ComponentRenderer.TagText(item, builder);
	#endregion
}
