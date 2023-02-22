namespace Du.Blazor;

/// <summary>
/// 컴포넌트 컨테이너
/// </summary>
public class ComponentContainer : ComponentContent, IAsyncDisposable
{
	/// <summary>현재 아이템 ID</summary>
	[Parameter] public string? CurrentId { get; set; }
	/// <summary>현재 아이템 ID 바뀜 이벤트</summary>
	[Parameter] public EventCallback<string?> CurrentIdChanged { get; set; }

	/// <summary>아이템 목록</summary>
	protected List<ComponentItem> Items { get; } = new();
	/// <summary>골라둔 아이템</summary>
	protected ComponentItem? SelectedItem { get; set; }
	/// <summary>CurrentId가 없으면 맨 첫 항목을 골라둠</summary>
	protected virtual bool SelectFirst => true;

	// 
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
			return;

		//
		if (SelectedItem is null)
		{
			if (CurrentId.IsHave(true))
				await SelectItemAsync(CurrentId);
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

		if (CurrentId is not null)
		{
			if (item.Id == CurrentId)
				await SelectItemAsync(item);
		}
		else if (SelectedItem is null && SelectFirst && item.Enabled)
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
		if (SelectedItem == item)
			SelectedItem = null;

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

	/// <summary>아이템 얻기</summary>
	/// <typeparam name="TItem">ComponentItem을 상속한 아이템</typeparam>
	/// <param name="id">찾을 아이디</param>
	/// <returns>찾은 아이템</returns>
	internal TItem? GetItem<TItem>(string id) where TItem : ComponentItem =>
		GetItem(id) as TItem;

	/// <summary>아이템 선택/// </summary>
	/// <param name="item"></param>
	/// <param name="stateChange"></param>
	/// <returns>비동기 처리한 태스크</returns>
	internal async Task SelectItemAsync(ComponentItem? item, bool stateChange = false)
	{
		if (item == SelectedItem)
			return;

		var previous = SelectedItem;
		SelectedItem = item;

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
			await InvokeCurrentIdChangedAsync(item.Id);

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

	/// <summary>아이템 추가될 때</summary>
	/// <param name="item"></param>
	/// <returns>비동기 처리한 태스크</returns>
	protected virtual Task OnItemAddedAsync(ComponentItem item) => Task.CompletedTask;

	/// <summary>아이템 삭제할 때 </summary>
	/// <param name="item"></param>
	/// <returns>비동기 처리한 태스크</returns>
	protected virtual Task OnItemRemovedAsync(ComponentItem item) => Task.CompletedTask;

	/// <summary>아이템을 선택했을 때</summary>
	/// <param name="item"></param>
	/// <param name="previous"></param>
	/// <returns>
	/// 비동기 처리한 태스크<br/>
	/// false를 반환하면 그뒤에 기능(InvokeCurrentIdChangedAsync 호출)을 하지 않음
	/// </returns>
	protected virtual Task<bool> OnItemSelectedAsync(ComponentItem? item, ComponentItem? previous) => Task.FromResult(true);

	/// <summary><see cref="CurrentId"/> 항목이 변경됐을 때</summary>
	/// <param name="id"></param>
	/// <returns>비동기 처리한 태스크</returns>
	protected virtual Task InvokeCurrentIdChangedAsync(string? id) => CurrentIdChanged.InvokeAsync(id);
}

/// <summary>
/// 컴포넌트 아이템
/// </summary>
public class ComponentItem : ComponentContent, IAsyncDisposable
{
	[CascadingParameter] public ComponentContainer? Container { get; set; }

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
