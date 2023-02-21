namespace Du.Blazor.Components;

public class ContentItem : ComponentItem
{
	[Parameter] public string? Text { get; set; }
	[Parameter] public RenderFragment? Display { get; set; }
	[Parameter] public RenderFragment? Content { get; set; }

	//
	public bool Active { get; set; }

	//
	public object? Tag { get; set; }

	//
	public override string ToString()
	{
		return $"{Id}: Active={Active}, Css={CssClass}";
	}
}
