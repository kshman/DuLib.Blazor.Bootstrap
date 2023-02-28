using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;

namespace Du.Blazor.Bootstrap;

/// <summary>태그 DIV 아이템</summary>
public class TagDiv : TagItem
{
	internal override string Tag => "div";
}


/// <summary>태그 SPAN 아이템</summary>
public class TagSpan : TagItem
{
	internal override string Tag => "span";
}


/// <summary>
/// 태그 아이템. 
/// </summary>
public class TagItem : TagTextBase
{
	/// <summary>아이템 핸들러</summary>
	[CascadingParameter] public ITagItemHandler? ItemHandler { get; set; }

	/// <summary>참일 경우 리스트 모드로 출력한다</summary>
	/// <remarks>드랍일경우 드랍 텍스트로 출력한다 (마우스로 활성화되지 않는 기능)</remarks>
	[Parameter] public bool TextMode { get; set; }

	/// <summary>리스트(li)에 있을 경우 사용할 css클래스</summary>
	[Parameter] public string? ListClass { get; set; }

	/// <summary>바리언트 색깔. 지원되는 애들만 쓸 수 있음</summary>
	[Parameter] public TagVariant? Variant { get; set; }

	//
	[Inject] protected ILogger<TagItem> Logger { get; set; } = default!;

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		// 단독으로 써도 좋은데 디버그 중일 때는 표시하자
		LogIf.ContainerIsNull(Logger, ItemHandler); 

		base.OnInitialized();
	}

	//
	protected override void OnComponentClass(CssCompose cssc)
	{
		ItemHandler?.OnTagItemClass(this, cssc);
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (ItemHandler is not null)
			ItemHandler.OnTagItemBuildRenderTree(this, builder);
		else
		{
			// 핸들러 없이도 그릴 수 있다!
			InternalRenderTreeTagText(builder);
		}
	}
}


/// <summary>
/// 태그 아이템 기본. 텍스트 속성만 갖고 있음<br/>
/// 이 클래스에서는 컨테이너/부모/채용자/연결자 등을 정의하지 않음<br/>
/// </summary>
/// <remarks>
/// 태그를 정의할때 텍스트가 필요하면 이 클래스를 상속할것.
/// </remarks>
public abstract class TagTextBase : ComponentFragment
{
	/// <summary>텍스트 속성</summary>
	[Parameter] public string? Text { get; set; }
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

	//
	internal virtual string Tag => "p";

	//
	internal void InternalRenderTreeTagText(RenderTreeBuilder builder)
	{
		/*
		 * <div class="@CssClass" @attributes="@UserAttrs">
		 *     @Text
		 *     @ChildContent
		 * </div>
		 */
		builder.OpenElement(0, Tag);
		builder.AddAttribute(1, "class", CssClass);

		if (OnClick.HasDelegate)
		{
			builder.AddAttribute(2, "role", "button");
			builder.AddAttribute(3, "onclick", InvokeOnClick);
			builder.AddEventStopPropagationAttribute(4, "onclick", true);
		}

		builder.AddMultipleAttributes(5, UserAttrs);
		builder.AddContent(6, Text);
		builder.AddContent(7, ChildContent);
		builder.CloseElement(); // tag
	}

	//
	internal virtual Task InvokeOnClick(MouseEventArgs e) => OnClick.InvokeAsync(e);
}
