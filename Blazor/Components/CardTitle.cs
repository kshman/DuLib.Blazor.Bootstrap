using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>카드 타이틀</summary>
public class CardTitle : ComponentContent
{
	/// <summary>타이틀 주 이름</summary>
	[Parameter] public RenderFragment? MainText { get; set; }
	/// <summary>주 이름의 HTML 태그</summary>
	[Parameter] public string MainTag { get; set; } = "h4";
	/// <summary>주 이름의 CSS클래스</summary>
	[Parameter] public string MainClass { get; set; } = "mb-2 text-muted";

	/// <summary>타이틀 보조 이름</summary>
	[Parameter] public RenderFragment? SubText { get; set; }
	/// <summary>보조 이름의 HTML 태그</summary>
	[Parameter] public string SubTag { get; set; } = "h6";
	/// <summary>보조 이름의 CSS클래스</summary>
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
				CssCompose.Join("card-title", Class, MainClass), UserAttrs);
		else
		{
			InternalBuildRender(builder, MainText, 0, MainTag,
				CssCompose.Join("card-title", Class, MainClass), UserAttrs);
			InternalBuildRender(builder, SubText, 10, SubTag,
				CssCompose.Join("card-subtitle", SubClass), null);
		}
	}

	//
	private void InternalBuildRender(RenderTreeBuilder builder, RenderFragment? content,
		int sequence, string tag, string? css, Dictionary<string, object>? attrs)
	{
		builder.OpenElement(sequence++, tag);
		builder.AddAttribute(sequence++, "class", css);
		builder.AddMultipleAttributes(sequence++, attrs);
		builder.AddContent(sequence++, content);
		builder.CloseElement();
	}
}
