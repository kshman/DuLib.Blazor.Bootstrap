namespace Du.Blazor.Bootstrap;

/// <summary>
/// Du.Blazor 컴포넌트 맨 밑단
/// </summary>
public class BsComponent : ComponentBase
{
	/// <summary>자식 콘텐트</summary>
	[Parameter] 
	public RenderFragment? ChildContent { get; set; }
	
	/// <summary>사용 여부 (disabled)</summary>
	[Parameter]
	public bool Disabled { get; set; }

	/// <summary>클래스 지정</summary>
	[Parameter]
	public string? Class { get; set; }

	/// <summary>컴포넌트 아이디</summary>
	[Parameter]
	public string Id { get; set; } = $"D_Z_{NextAtomicIndex:X}";

	/// <summary>사용자가 설정한 속성 지정</summary>
	[Parameter(CaptureUnmatchedValues = true)]
	public Dictionary<string, object>? UserAttrs { get; set; }

	/// <summary>만들어진 최종 CSS 클래스</summary>
	public string? CssClass => _css_compose.Class;

	// 
	private bool _init_css;
	private readonly BsCss _css_compose = new();

	/// <inheritdoc />
	public override async Task SetParametersAsync(ParameterView parameters)
	{
		await base.SetParametersAsync(parameters);

		// 여기다 이걸 넣은것은, 어떤 컴포넌트든 반드시 여기를 거쳐야하기 때문임
		// 게다가 이건 어지간해선 재정의 안함
		if (!_init_css)
		{
			_init_css = true;

			OnComponentClass(_css_compose);
			_css_compose.Add(Class).Register(() => Disabled.IfTrue("disabled"));

			OnComponentOnce();
		}
	}

	/// <summary>
	/// 컴포넌트의 CSS 클래스 값을 지정<br/>
	/// <see cref="ComponentBase.OnInitialized"/>와 <see cref="ComponentBase.OnParametersSet"/>을
	/// 모두 거친 다음에 호출됨
	/// </summary>
	protected virtual void OnComponentClass(BsCss cssc)
	{ }

	/// <summary>
	/// 무조건 한번만 호출. 모든 초기화 콜이 끝나고 불림
	/// </summary>
	protected virtual void OnComponentOnce()
	{ }

	/// <summary>태스크 상태를 봐서 기다렸다가 StateHasChanged 호출</summary>
	/// <param name="task"></param>
	/// <returns></returns>
	protected async Task StateHasChangedOnAsyncCompletion(Task task)
	{
		if (task.ShouldAwaitTask())
		{
			try
			{
				await task;
			}
			catch
			{
				if (task.IsCanceled)
					return;
				throw;
			}
		}

		StateHasChanged();
	}

	//
	private static uint _atomic_index = 1;
	private static uint NextAtomicIndex => Interlocked.Increment(ref _atomic_index);

	//
	public override string ToString() => $"<{GetType().Name}#{Id}>";
}
