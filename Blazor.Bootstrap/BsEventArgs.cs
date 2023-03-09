namespace Du.Blazor.Bootstrap;

public class BsEventArgs :EventArgs
{
}

/// <summary>펼침 이벤트</summary>
public class BsExpandedEventArgs : BsEventArgs
{
	public string Id { get; }
	public bool Expanded { get; }

	public BsExpandedEventArgs(string id, bool expanded)
	{
		Id = id;
		Expanded = expanded;
	}
}

/// <summary>슬라이드 이벤트</summary>
public class BsSlideEventArgs : BsEventArgs
{
	public int From { get; }
	public int To { get; }

	public BsSlideEventArgs(int from, int to)
	{
		From = from;
		To = to;
	}
}
