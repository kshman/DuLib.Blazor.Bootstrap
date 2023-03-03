namespace Du.Blazor.Bootstrap;

/// <summary>태그 DIV 아이템</summary>
public class BsTagDiv : BsTagItem
{
	public override string Tag => "div";
}


/// <summary>태그 SPAN 아이템</summary>
public class BsTagSpan : BsTagItem
{
	public override string Tag => "span";
}


/// <summary>
/// 태그 아이템. 바리언트 색깔 지원형
/// </summary>
public class BsTagItem : TagItem
{
	/// <summary>바리언트 색깔. 지원되는 애들만 쓸 수 있음</summary>
	[Parameter] public BsVariant? Variant { get; set; }
}
