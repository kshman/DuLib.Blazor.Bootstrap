﻿using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;

namespace Du.Blazor.Components;

public class NavHref : ComponentContent, IDisposable
{
	[CascadingParameter] public DropMenu? DropDown { get; set; }

	[Parameter] public NavLinkMatch Match { get; set; }
	[Parameter] public string? Link { get; set; }

	[Parameter] public string? ListClass { get; set; }
	[Parameter] public string ActiveClass { get; set; } = "active";

	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

	//
	[Inject] private NavigationManager NavMan { get; set; } = default!;

	//
	private bool _is_active;
	private string? _href;

	//
	protected override void OnComponentInitialized()
	{
		NavMan.LocationChanged += OnLocationChanged;
	}

	//
	protected override void OnComponentClass(CssCompose css)
	{
		css
			.Add(DropDown is null ? "nav-link" : "dropdown-item")
			.Register(() => _is_active ? ActiveClass : null);
	}

	//
	protected override void OnParametersSet()
	{
		_href = Link == null ? null : NavMan.ToAbsoluteUri(Link).AbsoluteUri;
		_is_active = ShouldMatch(NavMan.Uri);
	}

	//
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		var list = DropDown is not null;

		if (list)
		{
			builder.OpenElement(0, "li");
			builder.AddAttribute(1, "class", ListClass);
		}

		builder.OpenElement(10, "a");

		builder.AddAttribute(11, "class", CssClass);
		builder.AddAttribute(12, "href", Link);

		if (OnClick.HasDelegate)
		{
			builder.AddAttribute(13, "role", "button");
			builder.AddAttribute(14, "onclick", OnClick);
			builder.AddEventPreventDefaultAttribute(15, "onclick", true);
			builder.AddEventStopPropagationAttribute(16, "onclick", true);
		}

		builder.AddMultipleAttributes(17, UserAttrs);

		builder.AddContent(18, ChildContent);

		builder.CloseElement();

		if (list)
			builder.CloseElement(); // li
	}

	//
	private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
	{
		var active = ShouldMatch(e.Location);

		if (active != _is_active)
		{
			_is_active = active;
			StateHasChanged();
		}

		if (DropDown is not null)
		{
			// 이전 드랍다운은 이거했어야 했지만... 
			//InvokeAsync(DropDown.HideAsync);
		}
	}

	//
	private bool ShouldMatch(string uri)
	{
		if (_href is null)
			return false;

		if (EqualsHrefExactlyOrIfTrailingSlashAdded(uri))
			return true;

		if (Match == NavLinkMatch.Prefix && IsStrictlyPrefixWithSeparator(uri, _href))
			return true;

		return false;
	}

	//
	private bool EqualsHrefExactlyOrIfTrailingSlashAdded(string uri)
	{
		if (string.Equals(uri, _href, StringComparison.OrdinalIgnoreCase))
			return true;
		if (uri.Length != _href!.Length - 1)
			return false;
		if (_href[_href!.Length - 1] != '/' || !_href.StartsWith(uri, StringComparison.OrdinalIgnoreCase))
			return false;
		return true;
	}

	//
	private static bool IsStrictlyPrefixWithSeparator(string value, string prefix)
	{
		var l = prefix.Length;
		if (value.Length > l)
		{
			return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
							&& (l == 0
								|| !char.IsLetterOrDigit(prefix[l - 1])
								|| !char.IsLetterOrDigit(value[l]));
		}
		return false;
	}

	//
	public void Dispose()
	{
		NavMan.LocationChanged -= OnLocationChanged;
	}
}