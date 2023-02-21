namespace Du.Blazor.Components;

public class ContentItem : ComponentItem
{
	[Parameter] public string? Text { get; set; }
	[Parameter] public RenderFragment? Display { get; set; }
	[Parameter] public RenderFragment? Content { get; set; }

#if DEBUG
	public override string ToString()
	{
		if (Container is Accordion)
		{
			var prm = Tag as Accordion.AcnParam;
			return $"ACN#{Id}: {prm?.Expanded}";
		}
		else
		{
			return base.ToString();
		}
	}
#endif
}
