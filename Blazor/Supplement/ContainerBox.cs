namespace Du.Blazor.Supplement;

public class ContainerBox<TItem>
	where TItem : ComponentObject
{
	//
	public List<TItem> Items { get; } = new();
	public TItem? Current { get; set; }

	//
	private readonly Action? _state_has_changed_async;
	private readonly Action<TItem?, TItem?>? _selected;
	private readonly Action<TItem>? _added;
	private readonly Action<TItem>? _removed;

	//
	public ContainerBox(
		Action? funcStateHasChangedAsync,
		Action<TItem?, TItem?>? actionSelected = null,
		Action<TItem>? actionAdded = null,
		Action<TItem>? actionRemoved = null)
	{
		_state_has_changed_async = funcStateHasChangedAsync;
		_selected = actionSelected;
		_added = actionAdded;
		_removed = actionRemoved;
	}

	//
	public void AfterRenderFirst(string? activeId, bool selectFirstWhenNoActive = true)
	{
		if (Current is not null)
			return;

		if (activeId.IsHave(true))
			SelectById(activeId);
		else if (selectFirstWhenNoActive && Items.Count > 0)
			Select(Items[0]);
	}

	//
	public void Add(TItem item)
	{
		Items.Add(item);

		_added?.Invoke(item);
		_state_has_changed_async?.Invoke();
	}

	//
	public void Remove(TItem item)
	{
		Items.Remove(item);

		try
		{
			_removed?.Invoke(item);
			_state_has_changed_async?.Invoke();
		}
		catch (ObjectDisposedException)
		{
			// 이건... 어쩔 수 엄지
		}
	}

	//
	public void Select(TItem? item)
	{
		if (item == Current)
			return;

		if (_selected is null)
		{
			Current = item;
			return;
		}

		var previous = Current;
		Current = item;

		_selected.Invoke(item, previous);
	}

	//
	public void SelectById(string id) =>
		Select(GetItemById(id));

	//
	public TItem? GetItemById(string id) =>
		Items.FirstOrDefault(x => x.Id == id);

	//
	public override string ToString()
	{
		var name = typeof(TItem).Name;
		var current = Current is not null ? Current.ToString() : "(none)";
		return $"<{name}> Count={Items.Count}, Current={current}";
	}
}
