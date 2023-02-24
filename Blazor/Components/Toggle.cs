﻿using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;

namespace Du.Blazor.Components;

/// <summary>
///   <para>토글러</para>
///   <para>드랍다운의 펼치기/닫기 버튼으로 쓰이며, NavBar 버튼으로도 쓸 수 있음</para>
/// </summary>
/// <seealso cref="DropDown" />
public class Toggle : ComponentContent, IAsyncDisposable
{
	// 우선순위
	//	1. 드랍다운			=> nav-link dropdown-toggle
	//	2. 나브바			=> navbar-toggler
	//	3. 붕괴(=나브바)	=> (none)
	// 우선 순위를 지키지 않으면 나브바 아래 드랍다운이 동작안한다
	// 한편, 나브바와 붕괴가 같이 있으면... 안된다. 동작이 제대로 안됨

	[CascadingParameter(Name = "DropDown")] public DropDown? DropDown { get; set; }
	[CascadingParameter(Name = "NavBar")] public NavBar? NavBar { get; set; }
	[Parameter] public string? CollapseId { get; set; }

	[Parameter] public string? Tag { get; set; }

	[Parameter] public string? Text { get; set; }
	[Parameter] public string? AriaLabel { get; set; }
	[Parameter] public ToggleLayout Layout { get; set; } = ToggleLayout.Button;
	[Parameter] public DropAutoClose AutoClose { get; set; } = DropAutoClose.True;
	[Parameter] public TagColor? Color { get; set; }
	[Parameter] public TagSize? Size { get; set; }
	[Parameter] public bool? Outline { get; set; }
	[Parameter] public bool Caret { get; set; }
	[Parameter] public bool Split { get; set; } // 당분간 안만듬

	[Parameter] public EventCallback<ExpandedEventArgs> OnExpanded { get; set; }
	// OnClick 구현해야하나?
	//[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

	//
	[Inject] private IJSRuntime JS { get; set; } = default!;
	[Inject] private ILogger<Toggle> Logger { get; set; } = default!;

	//
	protected TagColor ActualColor => Color ?? Button.DefaultSettings.Color;
	protected TagSize ActualSize => Size ?? Button.DefaultSettings.Size;
	protected bool ActualOutline => Outline ?? Button.DefaultSettings.Outline;

	//
	private ElementReference _self;
	private DotNetObjectReference<Toggle>? _drf;
	private bool _use_collapse;

	//
	protected override void OnComponentInitialized()
	{
		// 나브바가 있네
		if (NavBar is not null)
		{
			// 드랍이 있음
			if (DropDown is not null)
			{
				// 이럴 때는 링크만사용
				Layout = ToggleLayout.A;
			}
			// 드랍이 없고, 나브바엔 토글이 없음 
			else if (NavBar.ToggleId.IsWhiteSpace())
			{
				// 토글이 여러개면 곤란하다 하나만
				// 아니면, 토글 내 드랍에서 캐치하는 것도 곤란
				NavBar.ToggleId = Id;

				// 나브바에서 아이디 셋팅
				CollapseId ??= '#' + NavBar.CollapseId;

				AriaLabel ??= "Toggle navigation"; // 이거 지정안하면 브라우저에서 욕함
			}
		}

		if (CollapseId.IsHave(true))
		{
			// 콜랩스 모드
			if (Split) // 스플릿 못씀
			{
				Logger.LogCritical(UseLocaleMesg
						? "{name}: 붕괴?! 컨트롤의 분리 기능과 나브바를 함께 쓰면 안되요."
						: "{name}: Cannot use with Collapse control or NavBar.", nameof(Split));
				Split = false;
			}

			if (NavBar is not null) // 나브바 처리
			{
				Color ??= TagColor.None;

				if (Layout is not ToggleLayout.Button) // 버튼만 됨
				{
					Logger.LogCritical(UseLocaleMesg
						? "{name}: 나브바 안에서 쓸 때는 반드시 {type} 이어야 해요."
						: "{name}: Must be {type} when contained within NavBar.", nameof(Layout), nameof(ToggleLayout.Button));
					Layout = ToggleLayout.Button;
				}
			}

			_use_collapse = true;
		}
		else
		{
			// 클랩스가 아닌데 나브바면 이상하쟎아. 예컨데 아이디가 잘못됐다든가
			// 나브바 밑에 드랍이면 있다.
			//ThrowIf.ItemNotNull(NavBar);

			// 이 경우엔 반드시 드랍다운이어야 함
			ThrowIf.ContainerIsNull(this, DropDown);
		}

		if (Split && Layout is not ToggleLayout.Button)
		{
			Logger.LogError(UseLocaleMesg
				? "{name}: 스플릿 모드를 쓰려거든 레이아웃을 반드시 버튼으로 하세요."
				: "{name}: Layout must be button when split mode.", nameof(Split));
			Layout = ToggleLayout.Button;
		}

		Logger.LogTrace(UseLocaleMesg
			? "{name}: 성공적으로 초기화 했어요."
			: "{name}: initialized successfully.", nameof(Toggle));
	}

	//
	protected override void OnComponentClass(CssCompose css)
	{
		if (NavBar is not null)
		{
			if (DropDown is null)
				css.Add("navbar-toggler");
			else
			{
				css
					.Add("nav-link")
					.Add("dropdown-toggle"); // 밑에도 나오지만, 그때는 _use_collapse가 false임 여긴 true
			}
		}

		if (Layout == ToggleLayout.Button)
		{
			css
				.AddIf(_use_collapse is false, "dropdown-toggle")
				.Add("btn")
				.Add(ActualColor.ToButtonCss(ActualOutline))
				.Add(ActualSize.ToCss("btn"));
		}
		else
		{
			css.AddIf(Caret && _use_collapse is false, "dropdown-toggle");
		}

		css
			.AddIf(Split, "dropdown-toggle-split")
			.Register(() => (DropDown?.Expanded ?? false).IfTrue("show"));
	}

	//
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
			return;

		if (_use_collapse)
			return;

		_drf ??= DotNetObjectReference.Create(this);
		await JS.InvokeVoidAsync("DUDROP.init", _self, _drf);
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		/*
		 * 	<TAG @ref="_self"
		 * 		type="button"	<--- 버튼이면
		 *		role="button"	<--- 버튼이 아니면
		 * 		class="@CssClass"
		 * 		data-bs-toggle="dropdown"
		 * 		data-bs-auto-close="@AutoClose.ToCss()"
		 * 		aria-expanded="false"
		 * 		@attributes="@UserAttrs">
		 * 	@Text
		 * 	@ChildContent
		 * 	</TAG>
		 */

		var tag = Tag ?? Layout.ToTag();

		builder.OpenElement(0, tag);

		builder.AddAttribute(1, Layout == ToggleLayout.Button ? "type" : "role", "button");
		builder.AddAttribute(2, "class", CssClass);

		if (_use_collapse && DropDown is null)
		{
			// 붕괴 또는 나브바 일 떄
			builder.AddAttribute(3, "data-bs-toggle", "collapse");
			builder.AddAttribute(4, "data-bs-target", CollapseId);
			if (CollapseId![0] == '#')
				builder.AddAttribute(5, "aria-controls", CollapseId[1..]);
		}
		else
		{
			// 드랍다운 일 때
			builder.AddAttribute(6, "data-bs-toggle", "dropdown");
			builder.AddAttribute(7, "data-bs-auto-close", AutoClose.ToCss());
		}

		builder.AddAttribute(8, "aria-expanded", "false");
		builder.AddAttribute(9, "aria-label", AriaLabel);
		builder.AddMultipleAttributes(10, UserAttrs);

		if (_use_collapse is false)
		{
			// 리퍼런스는 자바스크립트에서 쓰는데 드랍다운에서만 씀
			builder.AddElementReferenceCapture(11, e => _self = e);
		}

		if (Text.IsHave() || ChildContent is not null)
		{
			builder.AddContent(20, Text);
			builder.AddContent(21, ChildContent);
		}
		else if (NavBar is not null)
		{
			// 나브바일 때 기본 토글러 아이콘 뿌려줌
			builder.AddMarkupContent(22, @"<span class=""navbar-toggler-icon""></span>");
		}

		builder.CloseElement(); // Tag 닫기
	}

	//
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore().ConfigureAwait(false);
		GC.SuppressFinalize(this);
	}

	//
	protected virtual async Task DisposeAsyncCore()
	{
		if (_use_collapse)
			return;

		try
		{
			await JS.InvokeVoidAsync("DUDROP.disp", _self);
		}
		catch (JSDisconnectedException)
		{
		}

		_drf?.Dispose();
	}

	//
	[JSInvokable("ivk_drop_show")]
	public async Task InternalHandleShowAsync()
	{
		if (DropDown is not null)
			await DropDown.InternalExpandedChangedAsync(true);

		await InvokeOnExpandedAsync(new ExpandedEventArgs(Id, true));
	}

	//
	[JSInvokable("ivk_drop_hide")]
	public async Task InternalHandleHideAsync()
	{
		if (DropDown is not null)
			await DropDown.InternalExpandedChangedAsync(false);

		await InvokeOnExpandedAsync(new ExpandedEventArgs(Id, false));
	}

	//
	private Task InvokeOnExpandedAsync(ExpandedEventArgs e) => OnExpanded.InvokeAsync(e);
}