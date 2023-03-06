﻿namespace Du.Blazor.Bootstrap;

/// <summary>아코디언</summary>
public partial class Accordion
{
}


//
internal class AcnExtend
{
	internal bool Expanded { get; set; }
	internal BsCollapse? Collapse { get; set; }
}


//
internal static class AcnSupp
{
	internal static AcnExtend? GetAcnObject(this AddendumItem item) =>
		item.ExtendObject as AcnExtend;

	internal static bool GetAcnExpanded(this AddendumItem item) =>
		(item.ExtendObject as AcnExtend)!.Expanded;

	internal static void SetAcnExpanded(this AddendumItem item, bool value) =>
		(item.ExtendObject as AcnExtend)!.Expanded = value;

	internal static BsCollapse? GetAcnCollapse(this AddendumItem item) =>
		(item.ExtendObject as AcnExtend)!.Collapse;
}