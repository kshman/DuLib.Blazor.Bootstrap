﻿namespace Du.Blazor.Components;

/// <summary>탭 아이템</summary>
public class TabItem : ComponentParent, IAsyncDisposable
{
	/// <summary>그룹</summary>
	[CascadingParameter] public Tab? Group { get; set; }

	/// <summary>타이틀 <see cref="Display"/></summary>
	[Parameter] public string? Text { get; set; }

	/// <summary>헤더 <see cref="Text"/></summary>
	[Parameter] public RenderFragment? Display { get; set; }
	/// <summary>내용 
	/// <see cref="Display"/>와 짝꿍<br/>이 내용이 있을 경우,
	/// 태그 밖 <see cref="ComponentParent.ChildContent"/>는 처리하지 않는다
	/// </summary>
	[Parameter] public RenderFragment? Content { get; set; }

	//
	protected override string CssName => "nav-link";

	//
	protected override async Task OnInitializedAsync()
	{
		if (Group != null)
			await Group.AddItemAsync(this);
	}

	//
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore().ConfigureAwait(false);
		GC.SuppressFinalize(this);
	}

	//
	protected virtual async ValueTask DisposeAsyncCore()
	{
		if (Group != null)
			await Group.RemoveItemAsync(this);
	}
}
