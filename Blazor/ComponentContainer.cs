namespace Du.Blazor;

/// <summary>
/// 컴포넌트 컨테이너
/// </summary>
public class ComponentContainer : ComponentParent, IDisposable
{
	[Parameter] public string? Active { get; set; }
	[Parameter] public EventCallback<string?> ActiveChanged { get; set; }

	//
	protected List<ComponentItem> Items { get; } = new();
	//
	protected ComponentItem? Current { get; set; }
	//
	protected virtual bool SelectFirst { get; } = false;

	//
	protected override void OnAfterRender(bool firstRender)
	{
		//base.OnAfterRenderAsync(firstRender);

		if (!firstRender)
			return;

		if (Current is null)
		{
			if (Active.IsHave(true))
				SelectItemById(Active!);
			else if (SelectFirst && Items.Count > 0)
				SelectItem(Items[0]);
		}

		OnAfterFirstRender();
	}

	//
	public void AddItem(ComponentItem item)
	{
		Items.Add(item);

		if (Active is not null)
		{
			if (item.Id == Active)
				SelectItem(item);
		}
		else if (SelectFirst && item.Enabled && Current is null)
		{
			SelectItem(item);
		}

		OnItemAdded(item);
		StateHasChanged();
	}

	//
	public void RemoveItem(ComponentItem item)
	{
		if (Current == item)
			Current = null;

		Items.Remove(item);

		try
		{
			OnItemRemoved(item);
			StateHasChanged();
		}
		catch (ObjectDisposedException)
		{
			// 헐
		}
	}

	//
	public ComponentItem? GetItem(string id) =>
		Items.FirstOrDefault(x => x.Id == id);

	//
	public void SelectItem(ComponentItem? item, bool stateChage = false)
	{
		if (item == Current)
			return;

		var previous = Current;
		Current = item;

		OnItemSelected(item, previous);

		if (item is not null)
			InvokeAsync(async () => await InvokeActiveChangedAsync(item.Id));

		StateHasChanged();
	}

	//
	public void SelectItemById(string id, bool stateChage = false)
	{
		var item = Items.FirstOrDefault(i => i.Id == id);
		SelectItem(item, stateChage);
	}

	//
	public void Dispose()
	{
		Disposing();
		GC.SuppressFinalize(this);
	}

	//
	protected virtual void Disposing() { }

	//
	protected virtual void OnAfterFirstRender() { }
	protected virtual void OnItemAdded(ComponentItem item) { }
	protected virtual void OnItemRemoved(ComponentItem item) { }
	protected virtual void OnItemSelected(ComponentItem? item, ComponentItem? previous) { }
	protected virtual Task InvokeActiveChangedAsync(string? id) => ActiveChanged.InvokeAsync(id);
}

/// <summary>
/// 컴포넌트 아이템
/// </summary>
public class ComponentItem : ComponentParent, IDisposable
{
	[CascadingParameter] public ComponentContainer? Container { get; set; }

	//
	protected override void OnComponentInitialized()
	{
		if (Container is null)
			ThrowSupp.InsideComponent(nameof(ComponentItem));
		Container!.AddItem(this);
	}

	//
	public void Dispose()
	{
		Disposing();
		GC.SuppressFinalize(this);
	}

	//
	protected virtual void Disposing() => Container?.RemoveItem(this);
}
