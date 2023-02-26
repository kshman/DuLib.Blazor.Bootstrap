using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>
/// 오프캔바스
/// </summary>
public partial class OffCanvas : ComponentFragment, IAsyncDisposable, ITagContentAgency
{
	#region 기본 설정
	public class Settings
	{
		public bool CloseButton { get; set; }
		public bool Scrollable { get; set; }
		public OffCanvasBackDrop BackDrop { get; set; }
		public TagDimension Responsive { get; set; }
		public TagPlacement Placement { get; set; }
		public string? Class { get; set; }
		public string? HeaderClass { get; set; }
		public string? ContentClass { get; set; }
		public string? FooterClass { get; set; }
	}

	public static Settings DefaultSettings { get; }

	static OffCanvas()
	{
		DefaultSettings = new Settings
		{
			CloseButton = true,
			Scrollable = false,
			BackDrop = OffCanvasBackDrop.True,
			Responsive = TagDimension.None,
			Placement = TagPlacement.Right,
		};
	}
	#endregion

	[Parameter] public Settings? Set { get; set; }

	[Parameter] public string? Text { get; set; }
	[Parameter] public bool? CloseButton { get; set; }
	[Parameter] public bool? Scrollable { get; set; }
	[Parameter] public OffCanvasBackDrop? BackDrop { get; set; }
	[Parameter] public TagDimension? Responsive { get; set; }
	[Parameter] public TagPlacement? Placement { get; set; }

	// 언제나 그린다
	[Parameter] public bool Always { get; set; } //= false;

	//
	internal bool ActualCloseButton => CloseButton ?? Set?.CloseButton ?? DefaultSettings.CloseButton;
	internal bool ActualScrollable => Scrollable ?? Set?.Scrollable ?? DefaultSettings.Scrollable;
	private OffCanvasBackDrop ActualBackDrop => BackDrop ?? Set?.BackDrop ?? DefaultSettings.BackDrop;
	private TagDimension ActualResponsive => Responsive ?? Set?.Responsive ?? DefaultSettings.Responsive;
	private TagPlacement ActualPlacement => Placement ?? Set?.Placement ?? DefaultSettings.Placement;
	
	//
	private ElementReference _self;
	internal bool _expanded;

	//
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc
			.Add(ActualResponsive.ToOffCanvasCss())
			.Add(ActualPlacement.ToOffCanvasCss())
			.AddIf(Class is null, Set?.Class ?? DefaultSettings.Class)
			.Register(() => _expanded.IfTrue("show"));
	}

	//
	public ValueTask DisposeAsync()
	{
		throw new NotImplementedException();
	}

	//
	void ITagContentAgency.OnTagContentClass(TagContentRole part, TagContentBase content, CssCompose cssc)
	{
	}

	//
	void ITagContentAgency.OnTagContentBuildRenderTree(TagContentRole part, TagContentBase content, RenderTreeBuilder builder)
	{
	}
}
