using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;

namespace Du.Blazor.Components;

/// <summary>
/// 기본 태그 헤더
/// </summary>
public class TagHeader : TagContent
{
	/// <inheritdoc />
	[Parameter] public override TagContentRole Role { get; set; } = TagContentRole.Header;
}


/// <summary>
/// 기본 태그 풋타
/// </summary>
public class TagFooter : TagContent
{
	/// <inheritdoc />
	[Parameter] public override TagContentRole Role  { get; set; } = TagContentRole.Footer;
}


/// <summary>
/// 기본 태그 콘텐트
/// </summary>
public class TagContent : ComponentFragment
{
	[CascadingParameter] public ITagContentHandler? ContentHandler { get; set; }

	[Parameter] public virtual TagContentRole Role { get; set; } = TagContentRole.Content;

	//
	[Inject] protected ILogger<TagContent> Logger { get; set; } = default!;

	//
	protected override void OnInitialized()
	{
		LogIf.ContainerIsNull(Logger, ContentHandler);

		base.OnInitialized();
	}

	//
	protected override void OnComponentClass(CssCompose cssc) =>
		ContentHandler?.OnTagContentClass(this, cssc);

	// 
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		ContentHandler?.OnTagContentBuildRenderTree(this, builder);
}
