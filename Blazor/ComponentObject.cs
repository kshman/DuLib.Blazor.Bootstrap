using Microsoft.AspNetCore.Components.Rendering;

namespace Du.Blazor;

/// <summary>
/// Du.Blazor 컴포넌트 맨 밑단
/// </summary>
public abstract class ComponentObject : ComponentBase
{
	/// <summary>사용하지 여부 (disabled)</summary>
	[Parameter] public bool Enabled { get; set; } = true;
	/// <summary>클래스 지정</summary>
	[Parameter] public string? Class { get; set; }
	/// <summary>컴포넌트 아이디</summary>
	[Parameter] public string Id { get; set; } = $"D_Z_{NextAtomicIndex:X}";
	/// <summary>사용자가 설정한 속성 지정</summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? UserAttrs { get; set; }

	/// <summary>만들어진 최종 CSS 클래스</summary>
	public string? CssClass => _css_compose.Class;

	/// <summary>CSS 작성 도움꾼</summary>
	private readonly CssCompose _css_compose = new();

	/// <summary>
	/// 초기화 + 컴포넌트 속성 만들기.<br/>
	/// 내부에서 컴포넌트 속성을 만들기에, 초기화 할 때는 이 메소드를 재정의 하지 말고,
	/// <see cref="OnComponentInitialized"/> 메소드를 재정의해서 쓸 것.
	/// </summary>
	protected override void OnInitialized()
	{
		OnComponentInitialized();

		OnComponentClass(_css_compose);
		_css_compose.Add(Class).Register(() => Enabled.IfFalse("disabled"));
	}

	/// <summary>
	/// 컴포넌트가 초기화 될 때 호출.<br/>
	/// <see cref="OnInitialized"/> 메소드를 재정의 하지 말고 이걸 재정의 할것.<br/>
	/// <br/>
	/// 한편, 특별할 일 없는한 비었기 땜에 부모 호출 안해도 됨. <br/>
	/// 또한 비동기 버전은 없기 때문에 그럴 때는 
	/// <see cref="ComponentBase.OnInitializedAsync"/> 메소드를 재정의해서 쓸 것.
	/// </summary>
	protected virtual void OnComponentInitialized()
	{ }

	/// <summary>컴포넌트의 CSS 클래스 값을 지정</summary>
	protected virtual void OnComponentClass(CssCompose css)
	{ }

	// 태스크 상태를 봐서 기다렸다가 StateHasChanged 호출
	protected async Task StateHasChangedOnAsyncCompletion(Task task)
	{
		if (task.ShouldAwaitTask())
		{
			try
			{
				await task;
			}
			catch
			{
				if (task.IsCanceled) return;
				throw;
			}
		}

		StateHasChanged();
	}

	//
	internal static uint _atomic_index = 1;
	internal static uint NextAtomicIndex => Interlocked.Increment(ref _atomic_index);

	//
	public override string ToString() => $"<{GetType().Name}#{Id}>";
}


/// <summary>
/// 자식 콘텐트를 가지는 컴포넌트
/// </summary>
public abstract class ComponentContent : ComponentObject
{
	/// <summary>자식 콘텐트</summary>
	[Parameter] public RenderFragment? ChildContent { get; set; }

	//
	protected void InternalRenderTreeTag(RenderTreeBuilder builder, string tag = "div")
	{
		/*
		 * <tag class="@CssClass" @attributes="UserAttrs">
		 *    @ChildContent
		 * </tag>
		 */

		builder.OpenElement(0, tag);
		builder.AddAttribute(1, "class", CssClass);
		builder.AddMultipleAttributes(2, UserAttrs);
		builder.AddContent(3, ChildContent);
		builder.CloseElement(); // tag
	}

	//
	protected void InternalRenderTreeCascadingTag<TType>(RenderTreeBuilder builder, string tag = "div")
	{
		/*
		 * <tag class="@CssClass" @attributes="@UserAttrs">
		 *     <CascadingValue Value="this" IsFixed="true>
		 *         @Content
		 *     </CascadingValue>
		 * </tag>
		 */
		builder.OpenElement(0, tag);
		builder.AddAttribute(1, "class", CssClass);
		builder.AddMultipleAttributes(2, UserAttrs);

		builder.OpenComponent<CascadingValue<TType>>(3);
		builder.AddAttribute(4, "Value", this);
		builder.AddAttribute(5, "IsFixed", true);
		builder.AddAttribute(6, "ChildContent", (RenderFragment)((b) =>
				b.AddContent(7, ChildContent)));
		builder.CloseComponent(); // CascadingValue<TType>

		builder.CloseElement(); // tag
	}
}


// 검토
// 팝업/다이얼로그
//		https://stackoverflow.com/questions/72004471/creating-a-popup-in-blazor-using-c-sharp-method
//		https://stackoverflow.com/questions/72005345/bootstrap-modal-popup-using-blazor-asp-net-core
//		https://stackoverflow.com/questions/73617831/pass-parameters-to-modal-popup
//		아니 찾다보니 많네..
// 트리
//		https://stackoverflow.com/questions/70311596/how-to-create-a-generic-treeview-component-in-blazor
