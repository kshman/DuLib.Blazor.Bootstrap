﻿namespace Du.Blazor.Bootstrap;

/// <summary>
/// CSS 클래스 만들어 주기
/// </summary>
public class BsCss
{
	private const char class_separator = ' ';
	private const char style_separator = ';';

	private readonly List<string> _sts = new();
	private readonly List<Func<string?>> _fns = new();

	/// <summary>
	/// 값 넣기
	/// </summary>
	/// <param name="className"></param>
	/// <returns></returns>
	public BsCss Add(string? className)
	{
		if (className.IsHave())
			_sts.Add(className);
		return this;
	}

	/// <summary>
	/// 조건이 참이면 값 넣기
	/// </summary>
	/// <param name="condition"></param>
	/// <param name="trueName"></param>
	/// <returns></returns>
	public BsCss Add(bool condition, string? trueName)
	{
		if (condition && trueName.IsHave())
			_sts.Add(trueName);
		return this;
	}

	/// <summary>
	/// 조건에 따라 값 넣기
	/// </summary>
	/// <param name="condition"></param>
	/// <param name="trueName"></param>
	/// <param name="falseName"></param>
	/// <returns></returns>
	public BsCss Add(bool condition, string? trueName, string? falseName) =>
		Add(condition ? trueName : falseName);

	/// <summary>
	/// 값을 만들어줄 함수를 등록
	/// </summary>
	/// <param name="classFunc"></param>
	/// <returns></returns>
	public BsCss Register(Func<string?> classFunc)
	{
		_fns.Add(classFunc);
		return this;
	}

	/// <summary>
	/// 값이 있나 확인. 클래스 이름 전부가 맞아야함 <br/>
	/// 단 <see cref="Register"/>로 넣은거는 못찼는다
	/// </summary>
	/// <param name="className"></param>
	/// <returns></returns>
	public bool Test(string className) 
		=> _sts.Contains(className);

	/// <summary>
	/// 값이 있나 확인. 부분이라도 있으면 ㅇㅋ<br/>
	/// 단 <see cref="Register"/>로 넣은거는 못찼는다
	/// </summary>
	/// <param name="className"></param>
	/// <returns></returns>
	public bool TestAny(string className) =>
		_sts.Any(s => s.Contains(className));

	private string? InternalJoin(char separator)
	{
		var s = string.Join(separator, _sts);
		var f = string.Join(separator, _fns.Select(x => x()).Where(g => g.IsHave()));
        return s.Length == 0 ? f.Length == 0 ? null : f : f.Length == 0 ? s : $"{s}{separator}{f}";
	}

	/// <summary>만들어진 CSS 클래스</summary>
	public string? Class =>
		InternalJoin(class_separator);

	/// <summary>만들어진 CSS 스타일</summary>
	public string? Style =>
		InternalJoin(style_separator);

	public override string ToString() => $"({_sts.Count}/{_fns.Count}) {Class}";

	/// <summary>
	/// 분리자로 합진다
	/// </summary>
	/// <param name="separator"></param>
	/// <param name="args"></param>
	/// <returns></returns>
	public static string? Join(char separator, params string?[] args)
	{
		var j = string.Join(separator, args.Where(x => x.IsHave()));
		return j.Length == 0 ? null : j;
	}

	/// <summary>
	/// CSS 클래스로 합친다
	/// </summary>
	/// <param name="args"></param>
	/// <returns></returns>
	public static string? Join(params string?[] args) =>
		Join(class_separator, args);
}
