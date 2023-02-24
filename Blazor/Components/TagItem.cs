namespace Du.Blazor.Components;

/// <summary>
/// 태그 아이템. 기본적으로 텍스트 속성만 갖고 있음
/// </summary>
public class TagItem : ComponentContent
{
	/// <summary>텍스트 속성</summary>
	[Parameter] public string? Text { get; set; }

	/// <summary>확장 데이터</summary>
	internal object? InternalExtend { get; set; }
}
