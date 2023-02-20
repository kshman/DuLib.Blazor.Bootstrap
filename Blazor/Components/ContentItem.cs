namespace Du.Blazor.Components;

public class ContentItem : ComponentItem
{
	[Parameter] public string? Title { get; set; }
	[Parameter] public RenderFragment? Display { get; set; }
	[Parameter] public RenderFragment? Content { get; set; }

	//
	public bool Active { get; set; }
}

