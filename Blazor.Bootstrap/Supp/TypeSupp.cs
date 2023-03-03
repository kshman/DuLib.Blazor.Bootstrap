using System.Diagnostics.CodeAnalysis;

namespace Du.Blazor.Bootstrap.Supp
{
	internal static class TypeSupp
	{
		#region 기본 타입
		internal static bool IsEmpty([NotNullWhen(false)] this string? s) =>
			string.IsNullOrEmpty(s);

		internal static bool IsWhiteSpace([NotNullWhen(false)] this string? s) =>
			string.IsNullOrWhiteSpace(s);

		internal static bool IsHave([NotNullWhen(true)] this string? s) =>
			!string.IsNullOrWhiteSpace(s);

		internal static string? ToCss(this string? value, string rootElement) =>
			value.IsHave() ? $"{rootElement}-{value}" : null;

		internal static string ToHtml(this bool value) =>
			value ? "true" : "false";

		internal static string? IfTrue(this bool condition, string? value) =>
			condition ? value : null;

		internal static string? IfFalse(this bool condition, string? value) =>
			condition ? null : value;

		internal static bool ShouldAwaitTask(this Task task) =>
			task.Status is not TaskStatus.RanToCompletion and not TaskStatus.Canceled;
		#endregion 기본 타입

		#region 컴포넌트
		internal static string? ToCss(this BsSize size, string lead) => size switch
		{
			BsSize.Small => $"{lead}-sm",
			BsSize.Large => $"{lead}-lg",
			_ => null,
		};

		internal static string? ToCss(this BsVariant variant, string lead)
		{
			var cs = variant switch
			{
				BsVariant.Primary => "primary",
				BsVariant.Secondary => "secondary",
				BsVariant.Success => "success",
				BsVariant.Danger => "danger",
				BsVariant.Warning => "warning",
				BsVariant.Info => "info",
				BsVariant.Light => "light",
				BsVariant.Dark => "dark",
				_ => null,
			};
			return cs is null ? null : $"{lead}-{cs}";
		}

		internal static string? ToButtonCss(this BsVariant variant, bool outline) => variant switch
		{
			BsVariant.Link => "btn-link",
			_ => variant.ToCss(outline ? "btn-outline" : "btn"),
		};

		internal static string? ToCss(this BsPosition pos) => pos switch
		{
			BsPosition.Static => "pos-static",
			BsPosition.Relative => "pos-relative",
			BsPosition.Absolute => "pos-absolute",
			BsPosition.Fixed => "pos-fixed",
			BsPosition.Sticky => "pos-sticky",
			_ => null,
		};

		internal static string ToHtml(this BsButtonType button) => button switch
		{
			BsButtonType.Submit => "submit",
			BsButtonType.Reset => "reset",
			_ => "button",
		};

		internal static string? ToCss(this BsGroupType layout) => layout switch
		{
			BsGroupType.Button or
			BsGroupType.Vertical => "group",
			BsGroupType.Toolbar => "toolbar",
			_ => null,
		};

		internal static string ToCss(this BsDropDirection dir) => dir switch
		{
			BsDropDirection.Up => "dropup",
			BsDropDirection.Start => "dropstart",
			BsDropDirection.End => "dropend",
			_ => "dropdown",
		};

		internal static string? ToCss(this BsDropAutoClose close) => close switch
		{
			BsDropAutoClose.True => "true",
			BsDropAutoClose.False => "false",
			BsDropAutoClose.Inside => "inside",
			BsDropAutoClose.Outside => "outside",
			_ => null,
		};

		internal static string? ToCss(this BsDropAlignment alignment) => alignment switch
		{
			BsDropAlignment.Start => "dropdown-menu-start",
			BsDropAlignment.End => "dropdown-menu-end",
			_ => null
		};

		internal static string ToTag(this BsToggleType layout) => layout switch
		{
			BsToggleType.Div => "div",
			BsToggleType.Span => "span",
			BsToggleType.A => "a",
			_ => "button",
		};

		internal static string? ToCss(this BsNavType bsNav) => bsNav switch
		{
			BsNavType.Pills => "nav-pills",
			BsNavType.Tabs => "nav-tabs",
			_ => null,
		};

		private static string? ToCssName(this BsExpand expand) =>expand switch
		{
			BsExpand.Small => "sm",
			BsExpand.Medium => "md",
			BsExpand.Large => "lg",
			BsExpand.ExtraLarge => "xl",
			BsExpand.ExtraExtraLarge => "xxl",
			_ => null,
		};

		private static string ToCss(this BsExpand expand, string lead)
		{
			var ds = expand.ToCssName();
			return ds is null ? lead : $"{lead}-{ds}";
		}

		internal static string? ToCss(this BsExpand expand, string lead, string tail)
		{
			var ds = expand.ToCssName();
			return ds is null ? null : $"{lead}-{ds}-{tail}";
		}

		internal static string ToContainerCss(this BsExpand layout) =>
			layout == BsExpand.NavFluid ? "container-fluid" : layout.ToCss("container");

		internal static string ToNavBarCss(this BsExpand expand) =>
			expand.ToCss("navbar-expand");

		internal static string ToOffCanvasCss(this BsExpand responsive) =>
			responsive.ToCss("offcanvas");

		internal static string? ToListGroupCss(this BsExpand horizontal) =>
			horizontal == BsExpand.None ? null : horizontal.ToCss("list-group-horizontal");

		internal static string ToOffCanvasCss(this BsPlacement replacement) => replacement switch
		{
			BsPlacement.Top => "offcanvas-top",
			BsPlacement.Bottom => "offcanvas-bottom",
			BsPlacement.Left => "offcanvas-start",
			_ => "offcanvas-end",
		};

		internal static string ToBootStrap(this BsBackDrop backdrop) => backdrop switch
		{
			BsBackDrop.True => "true",
			BsBackDrop.False => "false",
			_ => "static",
		};

		internal static string ToCss(this BsItemAlignment alignment, string lead)
		{
			var s = alignment.ToString("F").ToLowerInvariant();
			return $"{lead}-{s}";
		}

		internal static string ToCss(this BsJustify justify, string lead)
		{
			var s = justify.ToString("F").ToLowerInvariant();
			return $"{lead}-{s}";
		}
		#endregion 컴포넌트

		#region 자바스크립트
		// 모듈 임포트
		internal static ValueTask<IJSObjectReference> ImportModuleAsync(this IJSRuntime js, string moduleName)
		{
			var path = "./_content/DuLib.Blazor.Bootstrap/module_" + moduleName + ".js";
			return js.InvokeAsync<IJSObjectReference>("import", path);
		}

		//
		internal static async ValueTask DisposeModuleAsync(this IJSObjectReference js, ElementReference self)
		{
			try
			{
				await js.InvokeVoidAsync("dispose", self);
				await js.DisposeAsync();
			}
#if DEBUG
			catch (JSDisconnectedException ex)
			{
				System.Diagnostics.Debug.WriteLine("[자바스크립트 끊김 시작]");
				System.Diagnostics.Debug.WriteLine(ex);
				System.Diagnostics.Debug.WriteLine("[자바스크립트 끊김 끝]");
			}
#else
			catch (JSDisconnectedException)
			{
				// 이 예외는 무시함
			}
#endif
		}
		#endregion
	}
}
