﻿using Du.Blazor.Components;

namespace Du.Blazor;

/// <summary>
/// 컴포넌트 이벤트
/// </summary>
public class DuComponentEvent : EventArgs
{
}

public class AccordionExpandEvent : DuComponentEvent
{
	public DuAccordion? Accordion { get; set; }
	public bool Expanded { get; set; }
}

/// <summary>
/// Carousel 이벤트
/// </summary>
public class CarouselSlideEvent : DuComponentEvent
{
	public int From { get; set; }
	public int To { get; set; }
	public bool IsLeft { get; set; }
}

public class TabChangeEvent : DuComponentEvent
{
	public DuTab? Tab { get; set; }
	public DuTab? Previous { get; set; }
}
