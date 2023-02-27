using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;

namespace Du.Blazor.Components;

/// <summary>태그 콘텐트 부위</summary>
public enum TagContentRole
{
	Header,
	Footer,
	Content,
}


/// <summary>
/// 태그 콘텐트 에이전시
/// </summary>
public interface ITagContentAgency
{
	/// <summary>
	/// 태그 콘텐트의 CSS클래스를 설정
	/// </summary>
	/// <param name="part">지정할 부분</param>
	/// <param name="content">콘텐트</param>
	/// <param name="cssc">CssCompose</param>
	void OnTagContentClass(TagContentRole part, TagContent content, CssCompose cssc);
	/// <summary>
	/// 태그 콘텐트의 렌더 트리를 만듦
	/// </summary>
	/// <param name="part">지정할 부분</param>
	/// <param name="content">콘텐트</param>
	/// <param name="builder">빌드 개체</param>
	void OnTagContentBuildRenderTree(TagContentRole part, TagContent content, RenderTreeBuilder builder);
}


/// <summary>
/// 기본 태그 헤더
/// </summary>
public class TagHeader : TagContent
{
	/// <inheritdoc />
	protected override TagContentRole ContentRole => TagContentRole.Header;
}


/// <summary>
/// 기본 태그 풋타
/// </summary>
public class TagFooter : TagContent
{
	/// <inheritdoc />
	protected override TagContentRole ContentRole => TagContentRole.Footer;
}


/// <summary>
/// 기본 태그 콘텐트
/// </summary>
public class TagContent : TagAbstractContent<ITagContentAgency>
{
	/// <inheritdoc />
	protected override TagContentRole ContentRole => TagContentRole.Content;

	//
	protected override void OnComponentClass(CssCompose cssc) =>
		ContentAgency?.OnTagContentClass(ContentRole, this, cssc);

	// 
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ContentAgency?.OnTagContentBuildRenderTree(ContentRole, this, builder);
}


/// <summary>
/// 태그 콘텐트 기본, 그리기 엄다
/// </summary>
/// <typeparam name="T">이 클래스를 자식으로 두는 클래스 형식</typeparam>
public abstract class TagAbstractContent<T> : ComponentFragment
	where T : ITagContentAgency
{
	[CascadingParameter] public T? ContentAgency { get; set; }

	//
	[Inject] protected ILogger<TagAbstractContent<T>> Logger { get; set; } = default!;

	//
	protected abstract TagContentRole ContentRole { get; }

	//
	protected override void OnInitialized()
	{
		LogIf.ContainerIsNull(Logger, ContentAgency);

		base.OnInitialized();
	}
}
