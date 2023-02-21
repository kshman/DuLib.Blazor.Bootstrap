namespace Du.Blazor;

/// <summary>
/// 컴포넌트 컨테이너
/// </summary>
public class ComponentContainer : ComponentParent, IAsyncDisposable
{
	[Parameter] public string? Active { get; set; }
	[Parameter] public EventCallback<string?> ActiveChanged { get; set; }

	//
	protected List<ComponentItem> Items { get; } = new();
	//
	protected ComponentItem? Current { get; set; }
	//
	protected virtual bool SelectFirst => true;

	// 
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		//await base.OnAfterRenderAsync(firstRender);

		if (!firstRender)
			return;

		//
		if (Current is null)
		{
			if (Active.IsHave(true))
				await SelectItemAsync(Active);
			else if (SelectFirst && Items.Count > 0)
				await SelectItemAsync(Items[0]);
		}

		//
		var task = OnAfterFirstRenderAsync();
		if (task.ShouldAwaitTask())
		{
			try
			{
				await task;
			}
			catch
			{
				if (!task.IsCanceled)
					throw;
			}
		}
	}

	/// <summary>아이템 추가</summary>
	/// <param name="item"></param>
	/// <returns>비동기 처리한 태스크</returns>
	internal async Task AddItemAsync(ComponentItem item)
	{
		Items.Add(item);

		if (Active is not null)
		{
			if (item.Id == Active)
				await SelectItemAsync(item);
		}
		else if (SelectFirst && item.Enabled && Current is null)
		{
			await SelectItemAsync(item);
		}

		var task = OnItemAddedAsync(item);
		await StateHasChangedOnAsyncCompletion(task);
	}

	/// <summary>아이템 삭제</summary>
	/// <param name="item"></param>
	/// <returns>비동기 처리한 태스크</returns>
	internal async Task RemoveItemAsync(ComponentItem item)
	{
		if (Current == item)
			Current = null;

		Items.Remove(item);

		try
		{
			var task = OnItemRemovedAsync(item);
			await StateHasChangedOnAsyncCompletion(task);
		}
		catch (ObjectDisposedException)
		{
			// 헐
			// 이건 디스포즈하다가 개체가 사라진거겟지
		}
	}

	/// <summary>아이템 얻기</summary>
	/// <param name="id">찾을 아이디</param>
	/// <returns>찾은 아이템</returns>
	internal ComponentItem? GetItem(string id) =>
		Items.FirstOrDefault(x => x.Id == id);

	/// <summary>아이템 선택/// </summary>
	/// <param name="item"></param>
	/// <param name="stateChange"></param>
	/// <returns>비동기 처리한 태스크</returns>
	internal async Task SelectItemAsync(ComponentItem? item, bool stateChange = false)
	{
		if (item == Current)
			return;

		var previous = Current;
		Current = item;

		var task = OnItemSelectedAsync(item, previous);
		if (task.ShouldAwaitTask())
		{
			try
			{
				await task;
			}
			catch
			{
				if (task.IsCanceled)
					return;
				throw;
			}
		}

		if (item is not null && task.Result)
			await InvokeActiveChangedAsync(item.Id);

		if (stateChange)
			StateHasChanged();
	}

	/// <summary>아이디로 아이템 선택</summary>
	/// <param name="id"></param>
	/// <param name="stateChange"></param>
	/// <returns>비동기 처리한 태스크</returns>
	internal Task SelectItemAsync(string id, bool stateChange = false)
	{
		var item = Items.FirstOrDefault(i => i.Id == id);
		return SelectItemAsync(item, stateChange);
	}

	/// <summary>아이템 선택 해제</summary>
	/// <param name="stateChange"></param>
	/// <returns>비동기 처리한 태스크</returns>
	internal Task SelectItemNoneAsync(bool stateChange = false) =>
		SelectItemAsync((ComponentItem?)null, stateChange);

	//
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore().ConfigureAwait(false);
		GC.SuppressFinalize(this);
	}

	/// <summary><see cref="DisposeAsync"/>의 처리 메소드</summary>
	/// <returns>비동기 처리한 태스크</returns>
	protected virtual ValueTask DisposeAsyncCore() => ValueTask.CompletedTask;

	/// <summary>
	/// <see cref="OnAfterRenderAsync(bool)"/> 메소드가 처리될 때
	/// 특별하게 firstRender의 경우에만 호출
	/// </summary>
	/// <returns>비동기 처리한 태스크</returns>
	protected virtual Task OnAfterFirstRenderAsync() => Task.CompletedTask;

	/// <summary>
	/// 아이템 추가될 때
	/// </summary>
	/// <param name="item"></param>
	/// <returns>비동기 처리한 태스크</returns>
	protected virtual Task OnItemAddedAsync(ComponentItem item) => Task.CompletedTask;

	/// <summary>
	/// 아이템 삭제할 때 
	/// </summary>
	/// <param name="item"></param>
	/// <returns>비동기 처리한 태스크</returns>
	protected virtual Task OnItemRemovedAsync(ComponentItem item) => Task.CompletedTask;

	/// <summary>
	/// 아이템을 선택했을 때
	/// </summary>
	/// <param name="item"></param>
	/// <param name="previous"></param>
	/// <returns>
	/// 비동기 처리한 태스크<br/>
	/// false를 반환하면 그뒤에 기능(InvokeActiveChangedAsync 호출)을 하지 않음
	/// </returns>
	protected virtual Task<bool> OnItemSelectedAsync(ComponentItem? item, ComponentItem? previous) => Task.FromResult(true);

	/// <summary>
	/// <see cref="Active"/> 항목이 변경됐을 때
	/// </summary>
	/// <param name="id"></param>
	/// <returns>비동기 처리한 태스크</returns>
	protected virtual Task InvokeActiveChangedAsync(string? id) => ActiveChanged.InvokeAsync(id);
}

/// <summary>
/// 컴포넌트 아이템
/// </summary>
public class ComponentItem : ComponentParent, IAsyncDisposable
{
	[CascadingParameter] public ComponentContainer? Container { get; set; }

	//
	internal object? Extend { get; set; }

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
	protected virtual async ValueTask DisposeAsyncCore()
	{
		if (Container is not null)
			await Container.RemoveItemAsync(this);
	}

	//
	public override string ToString() => $"<{GetType().Name}#{Id}> [{Container}]";
}
