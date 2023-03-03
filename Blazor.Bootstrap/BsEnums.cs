﻿namespace Du.Blazor.Bootstrap;

/// <summary>컴포넌트 크기</summary>
public enum BsSize
{
	Medium,
	Small,
	Large,
}

/// <summary>컴포넌트 크기</summary>
public enum BsDimension
{
	None,
	Small,
	Medium,
	Large,
	ExtraLarge,
	ExtraExtraLarge,

	/// <summary>나브용 Fluid, 그외에는 Medium으로 대체</summary>
	NavFluid,

	/// <summary>리스트 그룹</summary>
	Auto
}

/// <summary>컴포넌트 색깔</summary>
/// <remarks>
/// <para>
/// 바리언트 지원 <br/>
/// - bg / text <br/>
/// - table <br/>
/// - alert <br/>
/// - border <br/>
/// </para>
/// <para>
/// 별도로 처리됨 <br/>
/// - btn / btn-outline <br/>
/// - list-group-item <br/>
/// </para>
/// <para>
/// 별도록 처리해야 하는데... <br/>
/// - link → Button 컴포넌트<br/>
/// </para>
/// </remarks>
public enum BsVariant
{
	None,
	Primary,
	Secondary,
	Success,
	Danger,
	Warning,
	Info,
	Light,
	Dark,
	Link,
}

/// <summary>오버플로우 일때 처리</summary>
public enum BsOverflow
{
	None,
	Menu,
	Scroll,
}

/// <summary>방향</summary>
public enum BsDirection
{
	Vertical,
	Horizontal,
}

/// <summary>position 속성</summary>
public enum BsPosition
{
	None,
	Static,
	Relative,
	Absolute,
	Fixed,
	Sticky,
}

/// <summary>position 또는 placement 속성</summary>
public enum BsPlacement
{
	Top,
	Right,
	Bottom,
	Left,

	Start = Left,
	End = Right,
}

/// <summary>배지 레이아웃</summary>
public enum BsBadge
{
	None,
	Pill,
	Circle,
}

/// <summary>버튼 기능</summary>
public enum BsButtonType
{
	Button,
	Submit,
	Reset,
}

/// <summary>버튼 그룹</summary>
public enum BsButtonGroup
{
	Nulo,
	Vertical,
	Toolbar,

	Button = Nulo,
}

/// <summary>carousel 재생 방법</summary>
public enum BsCarouselPlay
{
	Auto,   // 자동
	False,  // 수동
	True,   // 한 사이클
}

/// <summary>Bit's 피벗 타이틀 레이아웃</summary>
public enum BitsPivotLayout
{
	Flat,
	Box,
}

/// <summary>토글 레이아웃</summary>
public enum BsToggle
{
	Button,
	Div,
	Span,
	A,
}

/// <summary>드랍 방향</summary>
public enum BsDropDirection
{
	Down,
	Up,
	Start,
	End,
}

/// <summary>드랍 정렬</summary>
public enum BsDropAlignment
{
	None,
	Start,
	End,
}

/// <summary>드랍 자동 닫힘</summary>
public enum BsDropAutoClose
{
	True,
	False,
	Inside,
	Outside,
}

/// <summary>NAV 레이아웃 (탭)</summary>
public enum BsNavLayout
{
	None,
	Tabs,
	Pills,
}

/// <summary>NAVBAR 확장</summary>
public enum BsNavBarExpand
{
	None,
	Small,
	Medium,
	Large,
	ExtraLarge,
	ExtraExtraLarge,
	Collapsed,
}

/// <summary>카드 놓기 위치</summary>
public enum BsCardImageLocation
{
	Top,
	Bottom,
	Overlay,
}

/// <summary>오프 캔바스 백드랍</summary>
public enum BsBackDrop
{
	True,
	False,
	Static
}