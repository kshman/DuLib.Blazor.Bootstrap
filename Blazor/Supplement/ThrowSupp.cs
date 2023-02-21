using System.Diagnostics.CodeAnalysis;

namespace Du.Blazor.Supplement;

internal static class ThrowSupp
{
	/// <summary>자기 자신이 널이면 예외</summary>
	/// <typeparam name="TThis">컨테이너(자기자신) 타입</typeparam>
	/// <typeparam name="TItem">컨테이너를 갖고 있는 타입</typeparam>
	/// <param name="container">컨테이너</param>
	/// <param name="item">컨테이너를 갖고 있는 컴포넌트</param>
	/// <exception cref="InvalidOperationException">컨테이너가 널이면 예외</exception>
	internal static void ThrowIfContainerIsNull<TThis, TItem>([NotNull] this TThis? container, TItem item)
	{
		if (container is not null)
			return;

		const string nameThis = nameof(TThis);
		const string nameItem = nameof(TItem);
		throw new InvalidOperationException(
			$"{nameItem}: No container found. Must be inside of <{nameThis}> container component.");
	}

	/// <summary>컴포넌트 캐스트가 안되면 예외</summary>
	/// <typeparam name="TConv">대상 캐스트 형식</typeparam>
	/// <param name="component">캐스트할 컴포넌트</param>
	/// <returns>캐스트한 컴포넌트</returns>
	/// <exception cref="InvalidOperationException">캐스트가 실패하면 예외</exception>
	internal static TConv ThrowIfConvertFail<TConv>([NotNull] this object? component)
	{
		if (component is TConv converted)
			return converted;

		var nameComponent = component is null ? nameof(component) : component.GetType().ToString();
		const string nameConvert = nameof(TConv);
		throw new InvalidOperationException(
			$"{nameComponent}: Invalid component type. Must be <{nameConvert}> type.");
	}
}
