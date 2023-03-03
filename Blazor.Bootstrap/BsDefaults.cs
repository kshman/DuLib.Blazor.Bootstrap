﻿namespace Du.Blazor.Bootstrap;

public static class BsDefaults
{
	#region 버튼
	public static BsButtonType ButtonType { get; set; } = BsButtonType.Button;
	public static BsVariant ButtonVariant { get; set; } = BsVariant.Primary;
	public static BsSize ButtonSize { get; set; } = BsSize.Medium;
	public static bool ButtonOutline { get; set; } = false;
	#endregion

	#region 뺏지
	public static BsVariant BadgeFore { get; set; } = BsVariant.Light;
	public static BsVariant BadgeBack { get; set; } = BsVariant.Primary;
	public static BsBadgeType BadgeType { get; set; } = BsBadgeType.None;
	public static string? BadgeAdditionalCss { get; set; } = null;
	#endregion

	#region 카드
	public static string? CardClass { get; set; } = null;
	public static string? CardHeaderClass { get; set; } = null;
	public static string? CardFooterClass { get; set; } = null;
	public static string? CardContentClass { get; set; } = null;
	#endregion

	#region 오프캔바스
	public static bool OffCanvasCloseButton { get; set; } = true;
	public static bool OffCanvasScrollable { get; set; } = false;
	public static BsBackDrop? OffCanvasBackDrop { get; set; } = null;
	public static BsExpand OffCanvasResponsive { get; set; } = BsExpand.None;
	public static BsPlacement OffCanvasPlacement { get; set; } = BsPlacement.Right;
	public static string? OffCanvasClass { get; set; } = null;
	public static string? OffCanvasHeaderClass { get; set; } = null;
	public static string? OffCanvasContentClass { get; set; } = null;
	public static string? OffCanvasFooterClass { get; set; } = null;
	#endregion
}
