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
	//
	protected override void OnComponentClass(CssCompose css)
	{
		if (Adopter is Card)
		{
			css
				.Add("card-header")
				.AddIf(Class is null, Card.DefaultSettings.HeaderClass);
		}
	}

	// 
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		InternalRenderTreeTag(builder);
}


/// <summary>
/// 기본 태그 풋타
/// </summary>
public class TagFooter : TagContentObject<ITagContentAdopter>
{
	//
	protected override void OnComponentClass(CssCompose css)
	{
		if (Adopter is Card)
		{
			css
				.Add("card-footer")
				.AddIf(Class is null, Card.DefaultSettings.FooterClass);
		}
	}

	// 
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		InternalRenderTreeTag(builder);
}


/// <summary>
/// 기본 태그 콘텐트
/// </summary>
public class TagContent : TagContentObject<ITagContentAdopter>
{
	//
	protected override void OnComponentClass(CssCompose css)
	{
		if (Adopter is Card)
		{
			css
				.Add("card-body")
				.AddIf(Class is null, Card.DefaultSettings.ContentClass);
		}
	}

	// 
	protected override void BuildRenderTree(RenderTreeBuilder builder) =>
		InternalRenderTreeTag(builder);
}


/// <summary>
/// 태그 콘텐트 기본, 그리기 엄다
/// </summary>
/// <typeparam name="T">이 클래스를 자식으로 두는 클래스 형식</typeparam>
public abstract class TagContentObject<T> : ComponentContent
	where T : ITagContentAdopter
{
	[CascadingParameter] public T? Adopter { get; set; }

	//
	[Inject] protected ILogger<T> Logger { get; set; } = default!;

	//
	protected override void OnInitialized()
	{
		LogIf.ContainerIsNull(Logger, Adopter);

		base.OnInitialized();
	}
}
