namespace Du.Blazor;

/// <summary>컴포넌트 크기</summary>
public enum ComponentSize
{
	Medium,
	Small,
	Large,
}

/// <summary>컴포넌트 색깔</summary>
public enum ComponentColor
{
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
public enum ComponentOverflow
{
	None,
	Menu,
	Scroll,
}

public enum ComponentPosition
{
	Top,
	Right,
	Bottom, 
	Left, 
	//Center,	
}

/// <summary>방향</summary>
public enum ComponentDirection
{
	Vertical,
	Horizontal,
}

/// <summary>버튼 기능</summary>
public enum ButtonType
{
	Button,
	Submit,
	Reset,
}

/// <summary>버튼 그룹</summary>
public enum GroupLayout
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

/// <summary>NAV 레이아웃 (탭)</summary>
public enum NavLayout
{
	Standard,
	Tabs,
	Pills,
}

/// <summary>드랍 레이아웃</summary>
public enum DropLayout
{
	Standard,
	Button,
	Flat,
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
	/* 지원안됨*/ Inside,
	/* 지원안됨*/ Outside,
}

/// <summary>배지 레이아웃</summary>
public enum BadgeLayout
{
	Standard,
	Pill,
	Circle,
}
