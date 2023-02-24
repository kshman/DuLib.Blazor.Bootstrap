namespace Du.Blazor.Components;

/// <summary>
/// 드랍다운 컴포넌트
/// </summary>
/// <remarks>
/// <para>
/// 내부에서 지원하는 컴포넌트:
/// <list type="table">
/// <listheader><term>컴포넌트</term><description>설명</description></listheader>
/// <item><term><see cref="Toggle"/></term><description>토글 버튼 제공</description></item>
/// <item><term><see cref="DropMenu"/></term><description>실제 메뉴 아이템 목록의 처리</description></item>
/// </list>
/// </para>
/// <para>
/// 일반적인 구성은 이렇슴:
/// <code>
/// &lt;DropDown&gt;
///	    &lt;Toggle Text="토글러"/&gt;
///     &lt;DropMenu&gt;
///         // 여기에 표현할 컴포넌트
///     &lt;/DropMenu&gt;
/// &lt;/DropDown&gt;
/// </code>
/// </para>
/// </remarks>
public partial class DropDown
{
}


/// <summary>
/// 드랍다운 메뉴 제공 컴포넌트
/// </summary>
/// <remarks>
/// <para>원래 <see cref="DropDown"/> 아래 콘텐트 구성용이지만, 단독으로 쓸 수 있음</para>
/// <para>
/// 내부에서 쓸수 있는 컴포넌트:
/// <list type="table">
/// <listheader><term>컴포넌트</term><description>설명</description></listheader>
/// <item><term><see cref="DropSpan"/></term><description>SPAN 태그 제공</description></item>
/// <item><term><see cref="DropDiv"/></term><description>DIV 태그 제공</description></item>
/// <item><term><see cref="Button"/></term><description>버튼 표시</description></item>
/// <item><term><see cref="Divider"/></term><description>구분 가로줄 표시</description></item>
/// </list>
/// </para>
/// </remarks>
public partial class DropMenu
{
}

