using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Bootstrap.Props;

/// <summary>
/// 태그 콘텐트 에이전시
/// </summary>
public interface ITagContentHandler
{
	/// <summary>
	/// 태그 콘텐트의 CSS클래스를 설정
	/// </summary>
	/// <param name="content">콘텐트</param>
	/// <param name="cssc">CssCompose</param>
	void OnTagContentClass(TagContent content, CssCompose cssc);
	/// <summary>
	/// 태그 콘텐트의 렌더 트리를 만듦
	/// </summary>
	/// <param name="content">콘텐트</param>
	/// <param name="builder">빌드 개체</param>
	void OnTagContentBuildRenderTree(TagContent content, RenderTreeBuilder builder);
}


/// <summary>태그 콘텐트 부위</summary>
public enum TagContentRole
{
	Header,
	Footer,
	Content,
}
