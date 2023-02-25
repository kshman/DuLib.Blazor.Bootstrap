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
public class ContentItem : TagItemBase, IAsyncDisposable
{
	/// <summary>이 컴포넌트를 포함하는 컨테이너</summary>
	[CascadingParameter] public ComponentStorage<ContentItem>? Container { get; set; }

	/// <summary>디스플레이 태그. 제목에 쓰임</summary>
	[Parameter] public RenderFragment? Display { get; set; }
	/// <summary>콘텐트 태그. 내용에 쓰임</summary>
	[Parameter] public RenderFragment? Content { get; set; }

	//
	internal object? InternalExtend { get; set; }

	//
	internal Accordion.AcnExtend? AcnExtend
	{
		get => InternalExtend as Accordion.AcnExtend;
		set => InternalExtend = value;
	}

	//
	protected override Task OnInitializedAsync()
	{
		ThrowIf.ContainerIsNull(this, Container);

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

	//
	public override string ToString() => Container is Accordion
			? $"ACN <{GetType().Name}#{Id}> Expanded:{AcnExtend?.Expanded}"
			: base.ToString();
}
