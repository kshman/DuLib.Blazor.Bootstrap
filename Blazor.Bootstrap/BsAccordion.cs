namespace Du.Blazor.Bootstrap;

/// <summary>아코디언</summary>
public class BsAccordion : BsSubsetContainer
{
	/// <summary>모서리 없는 모양</summary>
	[Parameter] public bool Flush { get; set; }
	/// <summary>언제나 열려있음</summary>
	[Parameter] public bool AlwaysOpen { get; set; }

	/// <summary>열리려고 할 때 이벤트</summary>
	[Parameter] public EventCallback<BsExpandedEventArgs> OnExpanding { get; set; }
	/// <summary>열리고 나서 이벤트</summary>
	[Parameter] public EventCallback<BsExpandedEventArgs> OnExpanded { get; set; }

	//
	protected override bool SelectFirst => false; // 처음 아이템이 열리지 않게 함

	//
	private bool _now_transition;
	//private int _transition_delay = 50;

	//
	protected override void OnComponentClass(BsCss cssc)
	{
		cssc.Add("accordion")
			.Add(Flush, "accordion-flush");
	}

	//
	protected override Task OnAfterFirstRenderAsync()
	{
		if (SelectedItem is not null && _now_transition is false)
			return InternalExpandAsync(SelectedItem);
		return Task.CompletedTask;
	}

	//
	protected override bool ShouldRender() => !_now_transition;

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * <CascadingValue Value="this" IsFixed="true">
		 * 	@ChildContent
		 * </CascadingValue>
		 * 
		 * <div class="@CssClass" id="@Id" @attributes="@UserAttrs">
		 * 	@foreach (var item in Items)
		 * 	{
		 * 		var expanded = AlwaysOpen ? item.GetAcnExpanded() : item == SelectedItem;
		 * 		<div class="accordion-item" @attributes="@item.UserAttrs">
		 * 			<h2 class="accordion-header">
		 * 				<button type="button"
		 * 					class="@CssCompose.Join("accordion-button", expanded.IfFalse("collapsed"), item.DisplayClass)"
		 * 					data-bs-toggle="collapse"
		 * 					data-bs-target="#@item.Id"
		 * 					aria-controls="@item.Id"
		 * 					aria-expanded="@expanded">
		 * 					@item.Text
		 * 					@item.Display
		 * 				</button>
		 * 			</h2>
		 * 			<BsCollapse @ref="item.GetAcnObject()!.Collapse"
		 * 				  Class="@CssCompose.Join("accordion-collapse", expanded.IfTrue("show"))"
		 * 				  ParentId="@(AlwaysOpen ? null : $"#{Id}")"
		 * 				  Id="@item.Id"
		 * 				  OnExpanding="HandleOnExpandingAsync"
		 * 				  OnExpanded="HandleOnExpandedAsync">
		 * 				<div class="@CssCompose.Join("accordion-body", item.CssClass)">
		 * 					@(item.Content ?? item.ChildContent)
		 * 				</div>
		 * 			</BsCollapse>
		 * 		</div>
		 * 	}
		 * </div>
		 */

		builder.OpenComponent<CascadingValue<BsAccordion>>(0);
		builder.AddAttribute(1, "Value", this);
		builder.AddAttribute(2, "IsFixed", true);
		builder.AddAttribute(3, "ChildContent", (RenderFragment)((b) =>
			b.AddContent(4, ChildContent)));
		builder.CloseComponent(); // CascadingValue<BsAccordion>

		builder.OpenElement(10, "div");
		builder.AddAttribute(11, "class", CssClass);
		builder.AddAttribute(12, "id", Id);
		builder.AddMultipleAttributes(13, UserAttrs);

		foreach (var item in Items)
		{
			var expanded = AlwaysOpen ? item.GetAcnExpanded() : item == SelectedItem;

			builder.OpenElement(20, "div");
			builder.AddAttribute(21, "class", "accordion-item");
			builder.AddMultipleAttributes(22, item.UserAttrs);

			builder.OpenElement(23, "h2");
			builder.AddAttribute(24, "class", "accordion-header");

			builder.OpenElement(25, "button");
			builder.AddAttribute(26, "type", "button");
			builder.AddAttribute(27, "class", BsCss.Join(
				"accordion-button",
				expanded.IfFalse("collapsed"),
				item.DisplayClass));
			builder.AddAttribute(28, "data-bs-toggle", "collapse");
			builder.AddAttribute(29, "data-bs-target", '#' + item.Id);
			builder.AddAttribute(30, "aria-controls", item.Id);
			builder.AddAttribute(31, "aria-expanded", expanded);
			builder.AddContent(32, item.Text);
			builder.AddContent(33, item.Display);
			builder.CloseElement(); // button

			builder.CloseElement(); // h2

			builder.OpenComponent<BsCollapse>(40);
			builder.AddAttribute(41, "Class", BsCss.Join("accordion-collapse", expanded.IfTrue("show")));
			builder.AddAttribute(42, "ParentId", AlwaysOpen ? null : '#' + Id);
			builder.AddAttribute(43, "Id", item.Id);
			builder.AddAttribute(44, "OnExpanding", new EventCallback<BsExpandedEventArgs>(this, HandleOnExpandingAsync));
			builder.AddAttribute(45, "OnExpanded", new EventCallback<BsExpandedEventArgs>(this, HandleOnExpandedAsync));
			builder.AddAttribute(46, "ChildContent", (RenderFragment)((b) =>
			{
				b.OpenElement(47, "div");
				b.AddAttribute(48, "class", BsCss.Join("accordion-body", item.CssClass));
				b.AddContent(49, item.Content ?? item.ChildContent);
				b.CloseElement();
			}));
			builder.AddComponentReferenceCapture(50, p => item.SetAcnCollapse((BsCollapse)p));
			builder.CloseElement(); // BsCollapse

			builder.CloseElement(); // div
		}

		builder.CloseElement(); // div
	}

	//
	protected override async Task OnItemAddedAsync(BsSubset item)
	{
		item.ExtendObject = new BsAcnExtend();

		if (item == SelectedItem && !_now_transition)
			await InternalExpandAsync(item);
	}

	/// <summary>엽니다</summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public Task ExpandAsync(string id) => InternalExpandAsync(GetItem(id));

	/// <summary>닫아요</summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public Task CollapseAsync(string id) => InternalCollapseAsync(GetItem(id));

	// 열려라
	private Task InternalExpandAsync(BsSubset? item)
	{
		var collapse = item?.GetAcnCollapse();

		if (collapse is null)
			return Task.CompletedTask;

		_now_transition = true;
		return collapse.ExpandAsync();
	}

	// 닫아라
	private Task InternalCollapseAsync(BsSubset? item)
	{
		var collapse = item?.GetAcnCollapse();

		if (collapse is null)
			return Task.CompletedTask;

		_now_transition = true;
		return collapse.CollapseAsync();
	}

	// 열릴때
	private async Task HandleOnExpandingAsync(BsExpandedEventArgs e)
	{
		//await Task.Delay(_transition_delay);
		_now_transition = true;

		await InvokeOnExpanding(e);
	}

	// 열린다음
	private async Task HandleOnExpandedAsync(BsExpandedEventArgs e)
	{
		//await Task.Delay(_transition_delay);
		_now_transition = false;

		var item = GetItem(e.Id);
		if (item?.ExtendObject is null)
		{
			// 아니 이럴수가 있나. 걍 무시
			return;
		}

		if (AlwaysOpen is false && item != SelectedItem)
		{
			item.SetAcnExpanded(false);
			//언제나 열기가 아닐 대 닫는것도 받으려면 이 주석 지우면됨
			//await InvokeOnExpanded(new ExpandedEventArgs(cur.Id, false));
		}

		item.SetAcnExpanded(e.Expanded);
		await InvokeOnExpanded(e);

		await SelectItemAsync(item);
	}

	//
	private Task InvokeOnExpanding(BsExpandedEventArgs e) => OnExpanding.InvokeAsync(e);
	private Task InvokeOnExpanded(BsExpandedEventArgs e) => OnExpanded.InvokeAsync(e);
}

//
internal class BsAcnExtend
{
	internal bool Expanded { get; set; }
	internal BsCollapse? Collapse { get; set; }
}

//
internal static class BsAcnSupp
{
	internal static bool GetAcnExpanded(this BsSubset item) => (item.ExtendObject as BsAcnExtend)!.Expanded;

	internal static void SetAcnExpanded(this BsSubset item, bool value) =>
		(item.ExtendObject as BsAcnExtend)!.Expanded = value;

	internal static BsCollapse? GetAcnCollapse(this BsSubset item) => (item.ExtendObject as BsAcnExtend)!.Collapse;

	internal static void SetAcnCollapse(this BsSubset item, BsCollapse collapse) =>
		(item.ExtendObject as BsAcnExtend)!.Collapse = collapse;
}
