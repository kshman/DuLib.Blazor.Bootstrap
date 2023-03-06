namespace Du.Blazor.Bootstrap;

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
	internal static AcnExtend? GetAcnObject(this TagSubset item) =>
		item.ExtendObject as AcnExtend;

	internal static bool GetAcnExpanded(this TagSubset item) =>
		(item.ExtendObject as AcnExtend)!.Expanded;

	internal static void SetAcnExpanded(this TagSubset item, bool value) =>
		(item.ExtendObject as AcnExtend)!.Expanded = value;

	internal static BsCollapse? GetAcnCollapse(this TagSubset item) =>
		(item.ExtendObject as AcnExtend)!.Collapse;
}
