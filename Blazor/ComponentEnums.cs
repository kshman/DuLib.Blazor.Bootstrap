namespace Du.Blazor;

/// <summary>컴포넌트 크기</summary>
public enum TagSize
{
	Medium,
	Small,
	Large,
}

/// <summary>컴포넌트 색깔</summary>
public enum TagColor
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
public enum TagOverflow
{
	None,
	Menu,
	Scroll,
}

/// <summary>방향</summary>
public enum TagDirection
{
	Vertical,
	Horizontal,
}

/// <summary>position 속성</summary>
public enum TagPosition
{
	None,
	Static,
	Relative,
	Absolute,
	Fixed,
	Sticky,
}

/// <summary>배지 레이아웃</summary>
public enum BadgeLayout
{
	None,
	Pill,
	Circle,
}

/// <summary>버튼 기능</summary>
public enum ButtonType
{
	Button,
	Submit,
	Reset,
}

/// <summary>버튼 그룹</summary>
public enum ButtonLayout
{
	Button,
	Vertical,
	Toolbar,
}

/// <summary>carousel 재생 방법</summary>
public enum CarouselPlay
{
	Auto,	// 자동
	False,	// 수동
	True,	// 한 사이클
}

/// <summary>Bit's 피벗 타이틀 레이아웃</summary>
public enum PivotLayout
{
	Flat,
	Box,
}

/// <summary>피벗 위치</summary>
public enum PivotPosition
{
	Top,
	Right,
	Bottom, 
	Left, 
	//Center,	
}

/// <summary>토글 레이아웃</summary>
public enum ToggleLayout
{
	Button,
	Div,
	Span,
	A,
}

/// <summary>드랍 방향</summary>
public enum DropDirection
{
	Down,
	Up,
	Start,
	End,
}

/// <summary>드랍 정렬</summary>
public enum DropAlignment
{
	None,
	Start,
	End,
}

/// <summary>드랍 자동 닫힘</summary>
public enum DropAutoClose
{
	True,
	False,
	Inside,
	Outside,
}

/// <summary>NAV 레이아웃 (탭)</summary>
public enum NavLayout
{
	None,
	Tabs,
	Pills,
}

/// <summary>NAVBAR 확장</summary>
public enum NavBarExpand
{
	None,
	Small,
	Medium,
	Large,
	ExtraLarge,
	ExtraExtraLarge,
	Collapsed,
}

/// <summary>NAV 컨테이너 레이아웃</summary>
public enum NavContainerLayout
{
	None,
	Fluid,
	Small,
	Medium,
	Large,
	ExtraLarge,
	ExtraExtraLarge,
}

/// <summary>카드 놓기 위치</summary>
public enum CardImageLocation
{
	Top,
	Bottom,
	Overlay,
}
