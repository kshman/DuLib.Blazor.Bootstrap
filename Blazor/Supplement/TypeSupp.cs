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

		internal static string? ToCss(this TagVariant variant, string lead)
		{
			var cs = variant switch
			{
				TagVariant.Primary => "primary",
				TagVariant.Secondary => "secondary",
				TagVariant.Success => "success",
				TagVariant.Danger => "danger",
				TagVariant.Warning => "warning",
				TagVariant.Info => "info",
				TagVariant.Light => "light",
				TagVariant.Dark => "dark",
				TagVariant.None or
				_ => null,
			};
			return cs is null ? null : $"{lead}-{cs}";
		}

		internal static string? ToButtonCss(this TagVariant variant, bool outline) => variant switch
		{
			TagVariant.Link => "btn-link",
			_ => variant.ToCss(outline ? "btn-outline" : "btn"),
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

		internal static string? ToCss(this TagDimension dim, string lead, bool nullToNull = true)
		{
			var ds = dim switch
			{
				TagDimension.Small => "sm",
				TagDimension.Medium => "md",
				TagDimension.Large => "lg",
				TagDimension.ExtraLarge => "xl",
				TagDimension.ExtraExtraLarge => "xxl",
				TagDimension.None or
				_ => null,
			};
			// 널일 경우 nullToNull이 참이면 널을, 거짓이면 그냥 lead를 보냄
			return ds is null ? nullToNull ? null : lead : $"{lead}-{ds}";
		}

		internal static string? ToContainerCss(this TagDimension layout) =>
			layout == TagDimension.NavFluid ? "container-fluid" : layout.ToCss("container");

		internal static string? ToOffCanvasCss(this TagDimension responsive) =>
			responsive.ToCss("offcanvas", false);

		internal static string? ToListGroupCss(this TagDimension horizontal) =>
			horizontal == TagDimension.None ? null : horizontal.ToCss("list-group-horizontal", false);

		internal static string ToOffCanvasCss(this TagPlacement replacement) => replacement switch
		{
			TagPlacement.Top => "offcanvas-top",
			TagPlacement.Bottom => "offcanvas-bottom",
			TagPlacement.Left => "offcanvas-start",
			TagPlacement.Right or
			_ => "offcanvas-end",
		};

		internal static string ToBootStrap(this OffCanvasBackDrop backdrop) => backdrop switch
		{
			OffCanvasBackDrop.True => "true",
			OffCanvasBackDrop.False => "false",
			OffCanvasBackDrop.Static or
			_ => "static",
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
