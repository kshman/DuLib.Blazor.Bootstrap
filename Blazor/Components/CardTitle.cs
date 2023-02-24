using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>카드 타이틀</summary>
/// <seealso cref="Du.Blazor.ComponentContent" />
public class CardTitle : ComponentContent
{
	[Parameter] public RenderFragment? MainText { get; set; }
	[Parameter] public RenderFragment? SubText { get; set; }

	[Parameter] public string MainTag { get; set; } = "h5";
	[Parameter] public string SubTag { get; set; } = "h6";
	[Parameter] public string SubClass { get; set; } = "mb-2 text-muted";

	// 
	protected override void OnInitialized()
	{
		// css를 처리 안하기 때문에 그걸 초기화하는 OnInitialize 자체를 막아버림
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (MainText is null)
			InternalBuildRender(builder, ChildContent, 0, MainTag,
				CssCompose.Join("card-title", Class), UserAttrs);
		else
		{
			InternalBuildRender(builder, MainText, 0, MainTag,
				CssCompose.Join("card-title", Class), UserAttrs);
			InternalBuildRender(builder, SubText, 10, SubTag,
				CssCompose.Join("card-subtitle", SubClass), null);
		}
	}

	//
	private int InternalBuildRender(RenderTreeBuilder builder, RenderFragment? content,
		int index, string tag, string? css, Dictionary<string, object>? attrs)
	{
		builder.OpenElement(index++, tag);

		builder.AddAttribute(index++, "class", css);
		builder.AddMultipleAttributes(index++, attrs);

		builder.AddContent(index++, content);

		builder.CloseElement();

		return index + 1;
	}
}
