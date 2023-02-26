namespace Du.Blazor.Unused;

public static class RemoveIntellisence
{
	public static void RemoveComponents()
	{
		// 카드
		Card.DefaultSettings.Class = null;
		Card.DefaultSettings.HeaderClass = null;
		Card.DefaultSettings.FooterClass = null;
		Card.DefaultSettings.ContentClass = null;

		// 오프캔바스
		OffCanvas.DefaultSettings.CloseButton = true;
		OffCanvas.DefaultSettings.Scrollable = false;
		OffCanvas.DefaultSettings.BackDrop =OffCanvasBackDrop.True;
		OffCanvas.DefaultSettings.Responsive = TagDimension.None;
		OffCanvas.DefaultSettings.Placement = TagPlacement.Right;
		OffCanvas.DefaultSettings.Class = null;
		OffCanvas.DefaultSettings.HeaderClass = null;
		OffCanvas.DefaultSettings.FooterClass = null;
		OffCanvas.DefaultSettings.ContentClass = null;
	}
}
