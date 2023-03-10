namespace Du.Blazor.Bootstrap;

/// <summary>아이콘을 표시합니다. oi/bi/fa 같은데 쓸 수 있음</summary>
public class BsIcon : BsComponent
{
	/// <summary>아이콘 이름</summary>
	[Parameter] public string Name { get; set; } = string.Empty;
	/// <summary>색깔</summary>
	[Parameter] public BsVariant? Color { get; set; }

	//
	protected override void OnComponentClass(BsCss cssc)
	{
		if (Name.Length > 3)
		{
			if (Name[2] == '-')
			{
				// oi / bi / fa 같은거는 세번째 빼기가 있다
				cssc.Add(Name[..2]);
				cssc.Add(Name);
			}
			else
			{
				// 없으면 걍 넣음
				cssc.Add(Name);
			}
		}
		else
		{
			// OI 경고
			cssc.Add("oi oi-warning");
		}

		cssc.Add(Color?.ToCss("text"));
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, "i");
		builder.AddAttribute(1, "class", CssClass);
		builder.AddAttribute(2, "aria-hidden", "true");
		builder.AddMultipleAttributes(99, UserAttrs);
		builder.CloseElement();
	}
}
