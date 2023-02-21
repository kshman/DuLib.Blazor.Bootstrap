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
			var extend = Extend as Accordion.AcnExtend;
			return $"ACN <{GetType().Name}#{Id}> Expanded:{extend?.Expanded}";
		}

		return base.ToString();
	}
#endif
}
