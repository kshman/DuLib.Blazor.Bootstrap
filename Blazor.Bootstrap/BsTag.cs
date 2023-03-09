namespace Du.Blazor.Bootstrap;

/// <summary>태그 DIV 아이템</summary>
public class BsDiv : BsTag
{
	public BsDiv() : base("div")
	{ }
}


/// <summary>태그 SPAN 아이템</summary>
public class BsSpan : BsTag
{
	public BsSpan() : base("span")
	{ }
}


/// <summary>
/// 태그 아이템. 바리언트 색깔 지원형
/// </summary>
public class BsTag : BsTagBase
{
	/// <summary>아이템 핸들러</summary>
	[CascadingParameter]
	public IBsTagHandler? ItemHandler { get; set; }

	/// <summary>참일 경우 감싸는태그의 모드로 출력한다</summary>
	/// <remarks>예컨데 드랍일경우 드랍 텍스트로 출력한다 (마우스로 활성화되지 않는 기능)</remarks>
	[Parameter]
	public bool WrapRepresent { get; set; }

	/// <summary>감싸는 태그안에 있을 때 css클래스, 주로 리스트이므로 "li"의 클래스</summary>
	[Parameter]
	public string? WrapClass { get; set; }
	
	/// <summary>바리언트 색깔. 지원되는 애들만 쓸 수 있음</summary>
	[Parameter] public BsVariant? Variant { get; set; }

	//
	public BsTag() : base("p")
	{ }

	//
	protected BsTag(string tag) : base(tag)
	{ }

	//
	protected override void OnComponentClass(BsCss cssc) =>
		ItemHandler?.OnClass(this, cssc);

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (ItemHandler is not null)
			ItemHandler.OnRender(this, builder);
		else
		{
			// 핸들러 없이도 그릴 수 있다!
			ComponentRenderer.TagText(this, builder);
		}
	}
}

public abstract class BsTagBase : BsComponent
{
	/// <summary>텍스트 속성</summary>
	[Parameter] public string? Text { get; set; }
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

	//
	public string Tag { get; }

	//
	protected BsTagBase(string tag) => Tag = tag;

	//
	protected virtual Task InvokeOnClick(MouseEventArgs e) => OnClick.InvokeAsync(e);

	//
	internal Task InternalOnClick(MouseEventArgs e) => InvokeOnClick(e);
}
