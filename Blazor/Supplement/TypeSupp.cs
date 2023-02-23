using System.Diagnostics.CodeAnalysis;

namespace Du.Blazor.Supplement
{
	internal static class TypeSupp
	{
		#region 기본 타입
		internal static bool IsEmpty([NotNullWhen(false)] this string? s) =>
			string.IsNullOrEmpty(s);

		internal static bool IsWhiteSpace([NotNullWhen(false)] this string? s) =>
			string.IsNullOrWhiteSpace(s);

		internal static bool IsHave([NotNullWhen(true)] this string? s, bool alsoTestSpace = false) =>
			alsoTestSpace ? !string.IsNullOrWhiteSpace(s) : !string.IsNullOrEmpty(s);

		internal static string? ToCss(this string? value, string rootElement) =>
			value.IsHave() ? $"{rootElement}-{value}" : null;

		internal static string ToBootStrap(this bool value) =>
			value ? "true" : "false";

		internal static string? IfTrue(this bool condition, string? value) =>
			condition ? value : null;

		internal static string? IfFalse(this bool condition, string? value) =>
			condition ? null : value;

		internal static bool ShouldAwaitTask(this Task task) =>
			task.Status is not TaskStatus.RanToCompletion and not TaskStatus.Canceled;
		#endregion 기본 타입

		#region 컴포넌트
		internal static string? ToCss(this TagSize size, string lead) => size switch
		{
			TagSize.Small => $"{lead}-sm",
			TagSize.Large => $"{lead}-lg",
			TagSize.Medium or
			_ => null,
		};

		internal static string? ToCss(this TagColor color, string lead)
		{
			var cs = color switch
			{
				TagColor.Primary => "primary",
				TagColor.Secondary => "secondary",
				TagColor.Success => "success",
				TagColor.Danger => "danger",
				TagColor.Warning => "warning",
				TagColor.Info => "info",
				TagColor.Light => "light",
				TagColor.Dark => "dark",
				TagColor.None or 
				_ => null,
			};
			return cs is null ? null : $"{lead}-{cs}";
		}

		internal static string? ToButtonCss(this TagColor color, bool outline) => color switch
		{
			TagColor.Link => "btn-link",
			_ => color.ToCss(outline ? "btn-outline" : "btn"),
		};

		internal static string? ToCss(this TagPosition pos) => pos switch
		{
			TagPosition.Static => "pos-static",
			TagPosition.Relative => "pos-relative",
			TagPosition.Absolute => "pos-absolute",
			TagPosition.Fixed => "pos-fixed",
			TagPosition.Sticky => "pos-sticky",
			TagPosition.None or
			_ => null,
		};

		internal static string ToHtml(this ButtonType button) => button switch
		{
			ButtonType.Submit => "submit",
			ButtonType.Reset => "reset",
			ButtonType.Button or
			_ => "button",
		};

		internal static string? ToCss(this ButtonLayout layout) => layout switch
		{
			ButtonLayout.Button or
			ButtonLayout.Vertical => "group",
			ButtonLayout.Toolbar => "toolbar",
			_ => null,
		};

		internal static string ToCss(this DropDirection dir) => dir switch
		{
			DropDirection.Up => "dropup",
			DropDirection.Start => "dropstart",
			DropDirection.End => "dropend",
			DropDirection.Down or
			_ => "dropdown",
		};

		internal static string? ToCss(this DropAutoClose close) => close switch
		{
			DropAutoClose.True => "true",
			DropAutoClose.False => "false",
			DropAutoClose.Inside => "inside",
			DropAutoClose.Outside => "outside",
			_ => null,
		};

		internal static string? ToCss(this DropAlignment alignment) => alignment switch
		{
			DropAlignment.Start => "dropdown-menu-start",
			DropAlignment.End => "dropdown-menu-end",
			DropAlignment.None or
			_ => null
		};

		internal static string ToTag(this ToggleLayout layout) => layout switch
		{
			ToggleLayout.Div => "div",
			ToggleLayout.Span => "span",
			ToggleLayout.A => "a",
			ToggleLayout.Button or
			_ => "button",
		};

		internal static string? ToCss(this NavLayout nav) => nav switch
		{
			NavLayout.Pills => "nav-pills",
			NavLayout.Tabs => "nav-tabs",
			NavLayout.None or
			_ => null,
		};

		internal static string? ToCss(this NavBarExpand expand) => expand switch
		{
			NavBarExpand.None => "navbar-expand",
			NavBarExpand.Small => "navbar-expand-sm",
			NavBarExpand.Medium => "navbar-expand-md",
			NavBarExpand.Large => "navbar-expand-lg",
			NavBarExpand.ExtraLarge => "navbar-expand-xl",
			NavBarExpand.ExtraExtraLarge => "navbar-expand-xxl",
			NavBarExpand.Collapsed or 
			_ => null,
		};

		internal static string? ToCss(this NavContainerLayout layout) => layout switch
		{
			NavContainerLayout.Fluid => "container-fluid",
			NavContainerLayout.Small => "container-sm",
			NavContainerLayout.Medium => "container-md",
			NavContainerLayout.Large => "container-lg",
			NavContainerLayout.ExtraLarge => "container-xl",
			NavContainerLayout.ExtraExtraLarge => "container-xxl",
			NavContainerLayout.None or
			_ => null
		};
		#endregion 컴포넌트

		#region 자바스크립트

		// 모듈 임포트
		internal static ValueTask<IJSObjectReference> ImportModuleAsync(this IJSRuntime js, string moduleName, string? subPath)
		{
			var path = subPath.IsHave() ?
				"./_content/Du.Blazor/" + subPath + "/" + moduleName + ".razor.js" :
				"./_content/Du.Blazor/" + moduleName + ".js";
			return js.InvokeAsync<IJSObjectReference>("import", path);
		}

		#endregion
	}
}
