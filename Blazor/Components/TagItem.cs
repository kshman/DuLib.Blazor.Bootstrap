using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;

namespace Du.Blazor.Components;

/// <summary>태그 아이템의 보호자</summary>
/// <remarks>컨테이너가 아닌것은 개체를 소유하지 않고 처리만 도와주기 때문</remarks>
public interface ITagItemAgency
{
	/// <summary>
	/// 태그 아이템의 CSS클래스를 설정
	/// </summary>
	/// <param name="item"></param>
	/// <param name="cssc"></param>
	void OnTagItemClass(TagItem item, CssCompose cssc);
	/// <summary>
	/// 태그 아이템의 렌더 트리를 만듦
	/// </summary>
	/// <param name="item"></param>
	/// <param name="builder"></param>
	void OnTagItemBuildRenderTree(TagItem item, RenderTreeBuilder builder);
}


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
public class TagItem : TagItemObject<ITagItemAgency>
{
	/// <summary>참일 경우 리스트 모드로 출력한다</summary>
	/// <remarks>드랍일경우 드랍 텍스트로 출력한다 (마우스로 활성화되지 않는 기능)</remarks>
	[Parameter] public bool TextMode { get; set; }

	/// <summary>리스트(li)에 있을 경우 사용할 css클래스</summary>
	[Parameter] public string? ListClass { get; set; }

	//
	protected override void OnComponentClass(CssCompose cssc) =>
		ItemAgency?.OnTagItemClass(this, cssc);

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (ItemAgency is not null)
			ItemAgency.OnTagItemBuildRenderTree(this, builder);
		else
		{
			// 캐스터 없이도 그릴 수 있다!
			InternalRenderTreeTextChild(builder);
		}
	}
}


/// <summary>
/// 태그 아이템 오브젝트
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class TagItemObject<T> : TagTextBase
	where T : ITagItemAgency
{
	/// <summary>컨테이너 컴포넌트</summary>
	[CascadingParameter] public T? ItemAgency { get; set; }

	//
	[Inject] protected ILogger<TagItemObject<T>> Logger { get; set; } = default!;

	//
	/// <inheritdoc />
	protected override void OnInitialized()
	{
		// 단독으로 써도 좋은데 디버그 중일 때는 표시하자
		LogIf.ContainerIsNull(Logger, ItemAgency); 

		base.OnInitialized();
	}
}


/// <summary>
/// 태그 아이템 기본. 텍스트 속성만 갖고 있음<br/>
/// 이 클래스에서는 컨테이너/부모/채용자/연결자 등을 정의하지 않음<br/>
/// </summary>
/// <remarks>
/// 태그를 정의할때 텍스트가 필요하면 이 클래스를 상속할것. <see cref="TagItemObject{T}"/>를 사용하지 말고.
/// </remarks>
public abstract class TagTextBase : ComponentFragment
{
	/// <summary>텍스트 속성</summary>
	[Parameter] public string? Text { get; set; }

	//
	internal virtual string Tag => "p";

	//
	internal void InternalRenderTreeTextChild(RenderTreeBuilder builder)
	{
		/*
		 * <div class="@CssClass" @attributes="@UserAttrs">
		 *     @Text
		 *     @ChildContent
		 * </div>
		 */
		builder.OpenElement(0, Tag);
		builder.AddAttribute(1, "class", CssClass);
		builder.AddMultipleAttributes(2, UserAttrs);
		builder.AddContent(3, Text);
		builder.AddContent(4, ChildContent);
		builder.CloseElement(); // tag
	}
}
