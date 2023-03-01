namespace Du.Blazor.Bootstrap;

/// <summary>태그 DIV 아이템</summary>
public class TagVariantDiv : TagVariantItem
{
	public override string Tag => "div";
}


/// <summary>태그 SPAN 아이템</summary>
public class TagVariantSpan : TagVariantItem
{
	public override string Tag => "span";
}


/// <summary>
/// 태그 아이템. 
/// </summary>
public class TagVariantItem : TagItem
{
	/// <summary>바리언트 색깔. 지원되는 애들만 쓸 수 있음</summary>
	[Parameter] public BsVariant? Variant { get; set; }
}
