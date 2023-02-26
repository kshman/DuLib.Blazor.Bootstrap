using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;

namespace Du.Blazor.Components;


/// <summary>
/// 태그 콘텐트 채용자
/// </summary>
public interface ITagContentWard
{
	/// <summary>
	/// 태그 콘텐트의 CSS클래스를 설정
	/// </summary>
	/// <param name="part">지정할 부분</param>
	/// <param name="content">콘텐트</param>
	/// <param name="cssc">CssCompose</param>
	void OnTagContentClass(TagPart part, TagContentBase content, CssCompose cssc);
	/// <summary>
	/// 태그 콘텐트의 렌더 트리를 만듦
	/// </summary>
	/// <param name="part">지정할 부분</param>
	/// <param name="content">콘텐트</param>
	/// <param name="builder">빌드 개체</param>
	void OnTagContentBuildRenderTree(TagPart part, TagContentBase content, RenderTreeBuilder builder);
}


/// <summary>
/// 기본 태그 헤더
/// </summary>
public class TagHeader : TagContentBase
{
	//
	protected override void OnComponentClass(CssCompose cssc) =>
		Ward?.OnTagContentClass(TagPart.Header, this, cssc);

	// 
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		Ward?.OnTagContentBuildRenderTree(TagPart.Header, this, builder);
}


/// <summary>
/// 기본 태그 풋타
/// </summary>
public class TagFooter : TagContentBase
{
	//
	protected override void OnComponentClass(CssCompose cssc) =>
		Ward?.OnTagContentClass(TagPart.Footer, this, cssc);

	// 
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		Ward?.OnTagContentBuildRenderTree(TagPart.Footer, this, builder);
}


/// <summary>
/// 기본 태그 콘텐트
/// </summary>
public class TagContent : TagContentBase
{
	//
	protected override void OnComponentClass(CssCompose cssc) =>
		Ward?.OnTagContentClass(TagPart.Content, this, cssc);

	// 
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		Ward?.OnTagContentBuildRenderTree(TagPart.Content, this, builder);
}


/// <summary>
/// 태그 콘텐트 기본
/// </summary>
public abstract class TagContentBase : TagContentObject<ITagContentWard>
{
}


/// <summary>
/// 태그 콘텐트 기본, 그리기 엄다
/// </summary>
/// <typeparam name="T">이 클래스를 자식으로 두는 클래스 형식</typeparam>
public abstract class TagContentObject<T> : ComponentContent
	where T : ITagContentWard
{
	[CascadingParameter] public T? Ward { get; set; }

	//
	[Inject] protected ILogger<T> Logger { get; set; } = default!;

	//
	protected override void OnInitialized()
	{
		LogIf.ContainerIsNull(Logger, Ward);

		base.OnInitialized();
	}
}
