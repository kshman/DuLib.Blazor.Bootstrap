namespace Du.Blazor.Components;

/// <summary>
/// 리스트 그룹
/// </summary>
/// <remarks>
/// 내부에서 쓸수 있는 컴포넌트:
/// <list type="table">
/// <listheader><term>컴포넌트</term><description>설명</description></listheader>
/// <item><term><see cref="TagItem"/></term><description>P 태그 제공</description></item>
/// <item><term><see cref="TagSpan"/></term><description>SPAN 태그 제공</description></item>
/// <item><term><see cref="TagDiv"/></term><description>DIV 태그 제공</description></item>
/// <item><term><see cref="Button"/></term><description>버튼/링크</description></item>
/// <item><term><see cref="Divider"/></term><description>구분 가로줄</description></item>
/// <item><term><see cref="NavButton"/></term><description>나브 링크</description></item>
/// </list>
/// </remarks>
public class ListGroup : ComponentFragment, ITagItemAgency, ITagListAgency
{
}
