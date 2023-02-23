using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

public class TagElement : ComponentBase
{
	[Parameter] public RenderFragment? ChildContent { get; set; }

	[Parameter] public string Tag { get; set; } = "span";
	[Parameter] public ElementReference Reference { get; set; }
	[Parameter] public string? Class { get; set; }

	[Parameter] public Action<ElementReference>? ReferenceChanged { get; set; }
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
	[Parameter] public bool OnClickStopPropagation { get; set; }
	[Parameter] public bool OnClickPreventDefault { get; set; }

	[Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object>? UserAttrs { get; set; }

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, Tag);

		if (Class.IsHave(true))
			builder.AddAttribute(1, "class", Class);

		builder.AddAttribute(2, "onclick", InvokeOnClickAsync);
		builder.AddEventPreventDefaultAttribute(3, "onclick", OnClickPreventDefault);
		builder.AddEventStopPropagationAttribute(4, "onclick", OnClickStopPropagation);

		builder.AddMultipleAttributes(5, UserAttrs);

		builder.AddElementReferenceCapture(6, p =>
		{
			Reference = p;
			ReferenceChanged?.Invoke(p);
		});

		builder.AddContent(7, ChildContent);

		builder.CloseElement();
	}

	//
	protected virtual Task InvokeOnClickAsync(MouseEventArgs e) => OnClick.InvokeAsync(e);
}
