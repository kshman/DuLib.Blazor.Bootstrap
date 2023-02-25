namespace Du.Blazor.Components;

public partial class OffCanvas : ComponentContent, IAsyncDisposable
{
	#region 기본 설정
	public class Settings
	{
		public bool EnableCloseButton { get; set; }
		public OffCanvasBackDrop BackDrop { get; set; }
		public TagDimension ResponsiveBreakPoint { get; set; }
		public TagPlacement Placement { get; set; }
		public TagSize Size { get; set; }
		public bool EnableScroll { get; set; }
		public string? Class { get; set; }
		public string? HeaderClass { get; set; }
		public string? ContentClass { get; set; }
		public string? FooterClass { get; set; }
	}

	public static Settings DefaultSettings = new()
	{
		EnableCloseButton = true,
		BackDrop = OffCanvasBackDrop.True,
		ResponsiveBreakPoint = TagDimension.None,
		Placement = TagPlacement.Right,
		Size = TagSize.Medium,
		EnableScroll = false,
	};
	#endregion

	[Parameter] public Settings? Set { get; set; }

	[Parameter] public RenderFragment? Header { get; set; }
	[Parameter] public RenderFragment? Content { get; set; }
	[Parameter] public RenderFragment? Footer { get; set; }

	[Parameter] public string? Text { get; set; }
	[Parameter] public bool EnableCloseButton { get; set; } = true;
	[Parameter] public OffCanvasBackDrop BackDrop { get; set; } = OffCanvasBackDrop.True;
	[Parameter] public TagDimension ResponsiveBreakPoint { get; set; } = TagDimension.None;
	[Parameter] public TagPlacement Placement { get; set; } = TagPlacement.Right;
	[Parameter] public TagSize Size { get; set; } = TagSize.Medium;
	[Parameter] public bool EnableScroll { get; set; } //= false;
	[Parameter] public string? HeaderClass { get; set; }
	[Parameter] public string? ContentClass { get; set; }
	[Parameter] public string? FooterClass { get; set; }

	public ValueTask DisposeAsync()
	{
		throw new NotImplementedException();
	}

	//

}
