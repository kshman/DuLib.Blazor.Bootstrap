namespace Du.Blazor.Bootstrap;

/// <summary>
/// 기본 태그 헤더
/// </summary>
public class BsHeader : BsContent
{
	public BsHeader() : base(BsContentRole.Header)
	{
	}
}

/// <summary>
/// 기본 태그 풋타
/// </summary>
public class BsFooter : BsContent
{
	public BsFooter() : base(BsContentRole.Footer)
	{
	}
}

/// <summary>
/// 기본 태그 콘텐트
/// </summary>
public class BsContent : BsComponent
{
	[CascadingParameter] public IBsContentHandler? ContentHandler { get; set; }

	//
	[Inject] protected ILogger<BsContent> Logger { get; set; } = default!;

	//
	private BsContentRole _role;

	//
	public BsContent() => _role = BsContentRole.Content;

	//
	protected BsContent(BsContentRole role) => _role = role;

	//
	protected override void OnInitialized()
	{
		LogIf.ContainerIsNull(Logger, this, ContentHandler);

		base.OnInitialized();
	}

	//
	protected override void OnComponentClass(BsCss cssc) => ContentHandler?.OnClass(_role, this, cssc);

	// 
	protected override void BuildRenderTree(RenderTreeBuilder builder) => ContentHandler?.OnRender(_role, this, builder);
}
