using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Du.Blazor.Supplement;

internal static class ThrowIf
{
	/// <summary>컨테이너가 널이면 예외</summary>
	/// <typeparam name="TContainer">컨테이너(자기자신) 타입</typeparam>
	/// <typeparam name="TItem">컨테이너를 갖고 있는 타입</typeparam>
	/// <param name="container">컨테이너</param>
	/// <param name="item">컨테이너를 갖고 있는 컴포넌트</param>
	/// <exception cref="InvalidOperationException">컨테이너가 널이면 예외</exception>
	internal static void ContainerIsNull<TContainer, TItem>([NotNull] TContainer? container, TItem item)
	{
		if (container is not null)
			return;

		throw new InvalidOperationException(
			$"{typeof(TItem)}: No container found. This component must be contained within <{typeof(TContainer)}> container component.");
	}

	internal static void ContainerIsNull<TContainer1, TContainer2, TItem>(TContainer1? container1, TContainer2? container2, TItem item)
	{
		if (container1 is not null || container2 is not null)
			return;

		throw new InvalidOperationException(
			$"{typeof(TItem)}: No container found. This component must be contained within <{typeof(TContainer1)}> or <{typeof(TContainer2)}> component.");
	}

	/// <summary>컴포넌트 캐스트가 안되면 예외</summary>
	/// <typeparam name="TConv">대상 캐스트 형식</typeparam>
	/// <param name="component">캐스트할 컴포넌트</param>
	/// <returns>캐스트한 컴포넌트</returns>
	/// <exception cref="InvalidOperationException">캐스트가 실패하면 예외</exception>
	internal static TConv CastFail<TConv>([NotNull] this object? component)
	{
		if (component is TConv converted)
			return converted;

		var nameComponent = component is null ? nameof(component) : component.GetType().ToString();
		throw new InvalidOperationException(
			$"{nameComponent}: Invalid component casting. Must be <{typeof(TConv)}>.");
	}


	internal static void ConditionFail<TItem>([DoesNotReturnIf(false)] bool condition)
	{
		if (condition)
			return;

		throw new InvalidOperationException("Condition failed!");
	}

	internal static void NotImplementedWithCondition<TType>([DoesNotReturnIf(false)] bool condition)
	{
		if (condition)
			return;

		throw new NotImplementedException($"{typeof(TType)}: Not implementate in such a condition yet.");
	}
}
