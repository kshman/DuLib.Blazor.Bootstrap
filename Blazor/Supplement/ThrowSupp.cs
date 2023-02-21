using System.Diagnostics.CodeAnalysis;

namespace Du.Blazor.Supplement;

internal static class ThrowSupp
{
	[DoesNotReturn]
	internal static void InsideComponent(string name, string inside) =>
		throw new InvalidOperationException($"{name}: No {inside} found. ");

	[DoesNotReturn]
	internal static void InsideComponent(string name) =>
		throw new InvalidOperationException($"{name}: No container component found.");

	[DoesNotReturn]
	internal static void MustBeComponent(string name, string mustbe) =>
		throw new InvalidOperationException($"{name}: Invalid item. must be {mustbe}.");

	//internal static void InsideGroup(string name, b)
}
