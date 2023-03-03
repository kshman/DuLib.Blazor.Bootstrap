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
			BsSize.Medium or
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
				BsVariant.None or
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
			BsPosition.None or
			_ => null,
		};

		internal static string ToHtml(this BsButtonType button) => button switch
		{
			BsButtonType.Submit => "submit",
			BsButtonType.Reset => "reset",
			BsButtonType.Button or
			_ => "button",
		};

		internal static string? ToCss(this BsButtonGroup layout) => layout switch
		{
			BsButtonGroup.Button or
			BsButtonGroup.Vertical => "group",
			BsButtonGroup.Toolbar => "toolbar",
			_ => null,
		};

		internal static string ToCss(this BsDropDirection dir) => dir switch
		{
			BsDropDirection.Up => "dropup",
			BsDropDirection.Start => "dropstart",
			BsDropDirection.End => "dropend",
			BsDropDirection.Down or
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
			BsDropAlignment.None or
			_ => null
		};

		internal static string ToTag(this BsToggle layout) => layout switch
		{
			BsToggle.Div => "div",
			BsToggle.Span => "span",
			BsToggle.A => "a",
			BsToggle.Button or
			_ => "button",
		};

		internal static string? ToCss(this BsNavLayout bsNav) => bsNav switch
		{
			BsNavLayout.Pills => "nav-pills",
			BsNavLayout.Tabs => "nav-tabs",
			BsNavLayout.None or
			_ => null,
		};

		internal static string? ToCss(this BsNavBarExpand expand) => expand switch
		{
			BsNavBarExpand.None => "navbar-expand",
			BsNavBarExpand.Small => "navbar-expand-sm",
			BsNavBarExpand.Medium => "navbar-expand-md",
			BsNavBarExpand.Large => "navbar-expand-lg",
			BsNavBarExpand.ExtraLarge => "navbar-expand-xl",
			BsNavBarExpand.ExtraExtraLarge => "navbar-expand-xxl",
			BsNavBarExpand.Collapsed or
			_ => null,
		};

		internal static string? ToCss(this BsDimension dim, string lead, bool nullToNull = true)
		{
			var ds = dim switch
			{
				BsDimension.Small => "sm",
				BsDimension.Medium => "md",
				BsDimension.Large => "lg",
				BsDimension.ExtraLarge => "xl",
				BsDimension.ExtraExtraLarge => "xxl",
				BsDimension.None or
				_ => null,
			};
			// 널일 경우 nullToNull이 참이면 널을, 거짓이면 그냥 lead를 보냄
			return ds is null ? nullToNull ? null : lead : $"{lead}-{ds}";
		}

		internal static string? ToContainerCss(this BsDimension layout) =>
			layout == BsDimension.NavFluid ? "container-fluid" : layout.ToCss("container");

		internal static string? ToOffCanvasCss(this BsDimension responsive) =>
			responsive.ToCss("offcanvas", false);

		internal static string? ToListGroupCss(this BsDimension horizontal) =>
			horizontal == BsDimension.None ? null : horizontal.ToCss("list-group-horizontal", false);

		internal static string ToOffCanvasCss(this BsPlacement replacement) => replacement switch
		{
			BsPlacement.Top => "offcanvas-top",
			BsPlacement.Bottom => "offcanvas-bottom",
			BsPlacement.Left => "offcanvas-start",
			BsPlacement.Right or
			_ => "offcanvas-end",
		};

		internal static string ToBootStrap(this BsBackDrop backdrop) => backdrop switch
		{
			BsBackDrop.True => "true",
			BsBackDrop.False => "false",
			BsBackDrop.Static or
			_ => "static",
		};
		#endregion 컴포넌트

		#region 자바스크립트
		// 모듈 임포트
		internal static ValueTask<IJSObjectReference> ImportModuleAsync(this IJSRuntime js, string moduleName)
		{
			var path = "./_content/DuLib.Blazor.Bootstrap/module_" + moduleName + ".js";
			return js.InvokeAsync<IJSObjectReference>("import", path);
		}

		//
		internal static ValueTask<IJSObjectReference> ImportModuleAsync<TType>(this IJSRuntime js)
		{
			var name = typeof(TType).Name.ToLowerInvariant();
			return ImportModuleAsync(js, name);
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
