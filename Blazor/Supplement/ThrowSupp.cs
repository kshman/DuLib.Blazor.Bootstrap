namespace Du.Blazor.Supplement;

internal static class ThrowSupp
{
	internal static void InsideComponent(string name, string inside) =>
		throw new InvalidOperationException($"{name}: No {inside} found. ");

	internal static void InsideComponent(string name) =>
		throw new InvalidOperationException($"{name}: No container component found.");

	internal static void MustBeComponent(string name, string mustbe) =>
		throw new InvalidOperationException($"{name}: Invalid item. must be {mustbe}.");

	//internal static void InsideGroup(string name, b)
}
