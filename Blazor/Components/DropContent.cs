namespace Du.Blazor.Components;

/// <summary>드랍 콘텐트</summary>
/// <remarks>원래 <see cref="DropDown"/> 아래 콘텐트 구성용이지만, 단독으로 쓸 수 있음</remarks>
public class DropContent : DropMenu
{
	//
	protected override string TagName => "div";
}
