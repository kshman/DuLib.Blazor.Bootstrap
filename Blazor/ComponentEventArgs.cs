using Du.Blazor.Components;

namespace Du.Blazor;

/// <summary>
/// 선택 이벤트
/// </summary>
public class SelectEventArgs : EventArgs
{
	public string Id { get; set; }
	public string? Content { get; set; }

	public SelectEventArgs(string id, string? content)
	{
		Id = id;
		Content = content;
	}
}

/// <summary>
/// 펼침 이벤트
/// </summary>
public class ExpandedEventArgs : EventArgs
{
	public string Id { get; set; }
	public bool Expanded { get; set; }

	public ExpandedEventArgs(string id, bool expanded)
	{
		Id = id;
		Expanded = expanded;
	}
}

/// <summary>
/// Carousel 이벤트
/// </summary>
public class CarouselSlideEventArgs : EventArgs
{
	public int From { get; set; }
	public int To { get; set; }
	public bool IsLeft { get; set; }

	public CarouselSlideEventArgs(int from, int to, bool isLeft)
	{
		From=from; 
		To=to;
		IsLeft=isLeft;
	}
}
