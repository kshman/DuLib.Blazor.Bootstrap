using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor.Components;

/// <summary>태그 아이템의 채용자</summary>
/// <remarks>컨테이너가 아니고 채용자것은 그냥 포함만 하지 관리는 않기 때문</remarks>
public interface ITagItemAdopter
{
}


/// <summary>태그 DIV 아이템</summary>
public class TagDiv : TagItem
{
	protected override string Tag => "div";
}


/// <summary>태그 SPAN 아이템</summary>
public class TagSpan : TagItem
{
	protected override string Tag => "span";
}


/// <summary>
/// 태그 아이템. 
/// </summary>
public class TagItem : TagItemObject<ITagItemAdopter>
{
	/// <summary>참일 경우 리스트 모드로 출력한다</summary>
	/// <remarks>드랍일경우 드랍 텍스트로 출력한다 (마우스로 활성화되지 않는 기능)</remarks>
	[Parameter] public bool TextMode { get; set; }

	/// <summary>리스트(li)에 있을 경우 사용할 css클래스</summary>
	[Parameter] public string? ListClass { get; set; }

	//
	protected override void OnComponentClass(CssCompose css)
	{
		if (Adopter is DropMenu)
			css.AddSelect(TextMode, "dropdown-item-text", "dropdown-item");
		else if (Adopter is CardContent)
			css.Add("card-text");
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (Adopter is DropMenu)
			BuildRenderDropMenu(builder);
		else
			BuildRenderCommon(builder);
	}

	//
	private void BuildRenderDropMenu(RenderTreeBuilder builder)
	{
		/*
		 * 	<li>
		 * 		<div class="@CssClass" @attributes="@UserAttrs">
		 * 			@Text
		 * 			@ChildContent
		 * 		</div>
		 * 	</li>
		 */

		builder.OpenElement(0, "li");

		if (ListClass.IsHave(true))
			builder.AddAttribute(1, ListClass);

		builder.OpenElement(2, Tag);
		builder.AddAttribute(3, "class", CssClass);
		builder.AddMultipleAttributes(4, UserAttrs);
		builder.AddContent(5, Text);
		builder.AddContent(6, ChildContent);
		builder.CloseElement(); // tag

		builder.CloseElement(); // li
	}

	//
	private void BuildRenderCommon(RenderTreeBuilder builder)
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


/// <summary>
/// 태그 아이템 오브젝트
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class TagItemObject<T> : TagItemBase
	where T : ITagItemAdopter
{
	/// <summary>컨테이너 컴포넌트</summary>
	[CascadingParameter] public T? Adopter { get; set; }
}


/// <summary>
/// 태그 아이템 기본. 텍스트 속성만 갖고 있음<br/>
/// 이 클래스에서는 컨테이너/부모/채용자/연결자 등을 정의하지 않는다
/// </summary>
public abstract class TagItemBase : ComponentContent
{
	/// <summary>텍스트 속성</summary>
	[Parameter] public string? Text { get; set; }

	//
	protected virtual string Tag => "p";
}
