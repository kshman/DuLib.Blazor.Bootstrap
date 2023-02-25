using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>
/// 카드 컴포넌트
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
/// <item><term><see cref="CardImageContent"/></term><description>카드 이미지를 표시하기 위한 콘텐트</description></item>
/// </list>
/// </remarks>
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
