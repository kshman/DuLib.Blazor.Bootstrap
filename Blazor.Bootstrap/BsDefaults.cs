namespace Du.Blazor.Bootstrap;

public static class BsDefaults
{
	#region 오프캔바스
	public static bool OffCanvasCloseButton { get; set; } = true;
	public static bool OffCanvasScrollable { get; set; } = false;
	public static BsBackDrop? OffCanvasBackDrop { get; set; } = null;
	public static BsExpand OffCanvasResponsive { get; set; } = BsExpand.None;
	public static BsPlacement OffCanvasPlacement { get; set; } = BsPlacement.Right;
	public static string? OffCanvasClass { get; set; } = null;
	public static string? OffCanvasHeaderClass { get; set; } = null;
	public static string? OffCanvasContentClass { get; set; } = null;
	public static string? OffCanvasFooterClass { get; set; }  = null;
	#endregion
}
