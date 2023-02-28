using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Bootstrap.Props;

/// <summary>태그 아이템의 핸들러</summary>
/// <remarks>컨테이너가 아닌것은 개체를 소유하지 않고 처리만 도와주기 때문</remarks>
public interface ITagItemHandler
{
	/// <summary>
	/// 태그 아이템의 CSS클래스를 설정
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cssc"></param>
	void OnTagItemClass(TagItem item, CssCompose cssc);
	/// <summary>
	/// 태그 아이템의 렌더 트리를 만듦
	/// </summary>
	/// <param name="item"></param>
	/// <param name="builder"></param>
	void OnTagItemBuildRenderTree(TagItem item, RenderTreeBuilder builder);
}
