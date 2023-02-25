using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace Du.Blazor.Components;

/// <summary>
/// 태그 콘텐트 채용자
/// </summary>
public interface ITagContentAdopter
{
}


/// <summary>
/// 기본 태그 헤더
/// </summary>
public class TagHeader : TagContentObject<ITagContentAdopter>
{
	// 해야함

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * <div class="@CssClass">
		 *     @ChildContent
		 * </div>
		 */

		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", CssClass);
		builder.AddMultipleAttributes(2, UserAttrs);
		builder.AddContent(3, ChildContent);
		builder.CloseElement(); // div
	}
}


/// <summary>
/// 기본 태그 풋타
/// </summary>
public class TagFooter : TagContentObject<ITagContentAdopter>
{
	// 해야함

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * <div class="@CssClass">
		 *     @ChildContent
		 * </div>
		 */

		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", CssClass);
		builder.AddMultipleAttributes(2, UserAttrs);
		builder.AddContent(3, ChildContent);
		builder.CloseElement(); // div
	}
}


/// <summary>
/// 기본 태그 콘텐트
/// </summary>
public class TagContent : TagContentObject<ITagContentAdopter>
{
	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * <div class="@CssClass">
		 *     <CascadeValue Value="this" IsFixed="true">
		 *         @ChildContent
		 *     </CascadeValue>
		 * </div>
		 */

		builder.OpenElement(0, "div");
		builder.AddAttribute(1, "class", CssClass);
		builder.AddMultipleAttributes(2, UserAttrs);

		if (ChildContent is not null)
		{
			builder.OpenComponent<CascadingValue<CardContent>>(13);
			builder.AddAttribute(4, "Value", this);
			builder.AddAttribute(5, "IsFixed", true);
			builder.AddAttribute(6, "ChildContent", (RenderFragment)((b) =>
					b.AddContent(7, ChildContent)));
			builder.CloseComponent(); // CascadingValue<Card>
		}

		builder.CloseElement(); // div
	}
}


/// <summary>
/// 태그 콘텐트 기본
/// </summary>
/// <typeparam name="T">이 클래스를 자식으로 두는 클래스 형식</typeparam>
public abstract class TagContentObject<T> : ComponentContent
	where T : ITagContentAdopter
{
	[CascadingParameter] public T? Adopter { get; set; }

	//
	[Inject] protected ILogger<T> Logger { get; set; } = default!;

	//
	protected override void OnComponentInitialized()
	{
		LogIf.ContainerIsNull(Logger, Adopter);
	}
}
