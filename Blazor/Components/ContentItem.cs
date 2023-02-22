namespace Du.Blazor.Components;

/// <summary>콘텐트 아이템 컨테이너</summary>
/// <seealso cref="Accordion"/>
/// <seealso cref="Carousel"/>
/// <seealso cref="Pivot"/>
/// <seealso cref="Tab"/>
public class ContentItemContainer : ComponentContainer<ContentItem>
{
}

/// <summary>콘텐트 아이템</summary>
public class ContentItem : ComponentContent, IAsyncDisposable
{
	[CascadingParameter] public ComponentStorage<ContentItem>? Container { get; set; }

	[Parameter] public string? Text { get; set; }
	[Parameter] public RenderFragment? Display { get; set; }
	[Parameter] public RenderFragment? Content { get; set; }

	//
	internal object? Extend { get; set; }

	//
	internal Accordion.AcnExtend? AcnExtend
	{
		get => Extend as Accordion.AcnExtend;
		set => Extend = value;
	}

	//
	protected override Task OnInitializedAsync()
	{
		Container.ThrowIfContainerIsNull(this);
		return Container.AddItemAsync(this);
	}

	//
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore().ConfigureAwait(false);
		GC.SuppressFinalize(this);
	}

	//
	protected virtual async Task DisposeAsyncCore()
	{
		if (Container is not null)
			await Container.RemoveItemAsync(this);
	}

#if DEBUG
	//
	public override string ToString()
	{
		if (Container is Accordion)
			return $"ACN <{GetType().Name}#{Id}> Expanded:{AcnExtend?.Expanded}";

		return base.ToString();
	}
#endif
}
