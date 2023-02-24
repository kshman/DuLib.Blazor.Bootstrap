﻿using Microsoft.Extensions.Logging;

namespace Du.Blazor.Supplement;

internal static class LogIf
{
	/// <summary>컨테이너가 널이면 로그</summary>
	/// <typeparam name="TContainer">컨테이너(자기자신) 타입</typeparam>
	/// <typeparam name="TItem">컨테이너를 갖고 있는 타입</typeparam>
	/// <param name="logger">로그 공급자</param>
	/// <param name="container">컨테이너</param>
	internal static void ContainerIsNull<TItem, TContainer>(ILogger<TItem> logger, TContainer? container)
	{
		if (container is not null)
			return;

		logger.LogError(ComponentObject.UseLocaleMesg
			? "{item}: 컨테이너가 없어요. 이 컴포넌트는 반드시 <{container}> 컨테이너 아래 있어야 해요."
			: "{item}: No container found. This component must be contained within <{container}> component.", typeof(TItem), typeof(TContainer));
	}

	//
	internal static void ContainerIsNull<TItem>(ILogger<TItem> logger, params object?[] containers)
	{
		if (containers.Any(c => c is not null))
			return;

		var names = (from c in containers where c is not null select c.GetType().Name).ToList();
		var join = string.Join(ComponentObject.UseLocaleMesg ? "또는 " : " or ", names);
		logger.LogError(ComponentObject.UseLocaleMesg
			? "{item}: 컨테이너가 없어요. 이 컴포넌트는 반드시 <{containers}> 컨테이너 아래 있어야 해요."
			: "{item}: No container found. This component must be contained within <{containers}> components.", typeof(TItem), join);
	}

	//
	internal static void FailWithMessage<TItem>(bool condition, string message, ILogger<TItem> logger)
	{
		if (condition)
			return;

		logger.LogError(message);
	}
}