using Microsoft.Extensions.Logging;

namespace Du.Blazor.Components;

/// <summary>콘텐트 아이템 컨테이너</summary>
/// <remarks>
/// 이 컴포넌트를 상속 받는 애들은 <see cref="ContentItem"/> 컴포넌트로
/// 콘텐트를 구성해야 함
/// </remarks>
/// <seealso cref="Accordion"/>
/// <seealso cref="Carousel"/>
/// <seealso cref="Pivot"/>
/// <seealso cref="Tab"/>
public abstract class ContentItemContainer : ComponentContainer<ContentItem>
{
}


/// <summary>콘텐트 아이템</summary>
/// <remarks>자체적으로 렌더링 하는 기능은 없음</remarks>
public class ContentItem : ComponentContent, IAsyncDisposable
{
	[CascadingParameter] public ComponentStorage<ContentItem>? Container { get; set; }

	[Parameter] public string? Text { get; set; }
	[Parameter] public RenderFragment? Display { get; set; }
	[Parameter] public RenderFragment? Content { get; set; }

	//
	[Inject] protected ILogger<ContentItem> Logger { get; set; } = default!;

	//
	internal object? Extend { get; set; }

	//
	internal Accordion.AcnExtend? AcnExtend
	{
		get => Extend as Accordion.AcnExtend;
		set => Extend = value;
	}

	//
	protected virtual void CheckContainer()
	{
		LogIf.ContainerIsNull(Logger, Container);
	}

	//
	protected override Task OnInitializedAsync()
	{
		CheckContainer();

		return Container is null ? Task.CompletedTask : Container.AddItemAsync(this);
	}

	//
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore().ConfigureAwait(false);
		GC.SuppressFinalize(this);
	}

	//
	protected virtual Task DisposeAsyncCore() =>
		Container is not null ? Container.RemoveItemAsync(this) : Task.CompletedTask;

#if DEBUG
	//
	public override string ToString()
	{
		return Container is Accordion 
			? $"ACN <{GetType().Name}#{Id}> Expanded:{AcnExtend?.Expanded}" 
			: base.ToString();
	}
#endif
}
