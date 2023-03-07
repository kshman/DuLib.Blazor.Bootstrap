using Microsoft.AspNetCore.Components.Routing;

namespace Du.Blazor.Bootstrap;

/// <summary>나브 링크 제공.</summary>
public class BsNavLink : ComponentFragment, IDisposable
{
	/// <summary>리스트 에이전시. 이게 캐스케이딩되어 있으면 리스트(li)를 추가한다</summary>
	[CascadingParameter] public ITagListAgent? ListAgency { get; set; }
	/// <summary>나브바. 이게 캐스케이딩되면 나브바에 맞춰 컴포넌트를 설정</summary>
	[CascadingParameter] public BsNavBar? NavBar { get; set; }

	/// <summary>나브 링크 매치</summary>
	[Parameter] public NavLinkMatch Match { get; set; }
	/// <summary>이동할 URL 링크</summary>
	[Parameter] public string? Link { get; set; }

	/// <summary>리스트(li)의 css클래스</summary>
	[Parameter] public string? ListClass { get; set; }
	/// <summary>현재 URL과 일치하면 표시할 css클래스</summary>
	/// <remarks>기본값은 'active'</remarks>
	[Parameter] public string ActiveClass { get; set; } = "active";

	/// <summary>마우스로 눌렀을 때 이벤트</summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

	//
	[Inject] private NavigationManager NavMan { get; set; } = default!;

	//
	private bool _is_active;
	private string? _href;

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		NavMan.LocationChanged += OnLocationChanged;
	}

	//
	protected override void OnComponentClass(CssCompose cssc)
	{
		cssc.Add(ListAgency?.Class ?? "nav-link")
			.Register(() => _is_active ? ActiveClass : null);

		// 오프캔버스에 맞춰 컬럼 옵션을 추가
		if (NavBar?.Type == BsNavBarType.OffCanvas && !cssc.TestAny("col-"))
		{
			cssc.Add("col-12")
				.Add(NavBar.Expand.ToCss("col", "auto"));
		}
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
		var list = ListAgency?.Tag ?? null;

		if (list is not null)
		{
			builder.OpenElement(0, list);
			builder.AddAttribute(1, "class", ListClass);
		}

		builder.OpenElement(10, "a");

		builder.AddAttribute(11, "class", CssClass);
		builder.AddAttribute(12, "href", Link);

		if (NavBar is not null)
		{
			builder.AddAttribute(13, "role", "button");
			builder.AddAttribute(14, "data-bs-target", '#' + NavBar.TargetId);

			if (NavBar.Type == BsNavBarType.OffCanvas)
				builder.AddAttribute(15, "data-bs-dismiss", "offcanvas");

			builder.AddAttribute(16, "aria-label", "Close");

			if (OnClick.HasDelegate)
			{
				builder.AddAttribute(17, "onclick", OnClick);
				builder.AddEventPreventDefaultAttribute(18, "onclick", true);
				builder.AddEventStopPropagationAttribute(19, "onclick", true);
			}
		}
		else if (OnClick.HasDelegate)
		{
			builder.AddAttribute(13, "role", "button");
			builder.AddAttribute(14, "onclick", OnClick);
			builder.AddEventPreventDefaultAttribute(15, "onclick", true);
			builder.AddEventStopPropagationAttribute(16, "onclick", true);
		}

		builder.AddMultipleAttributes(20, UserAttrs);

		builder.AddContent(21, ChildContent);

		builder.CloseElement();

		if (list is not null)
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

		if (ListAgency is not null)
		{
			// 이전 드랍다운은 이거했어야 했지만... 
			//InvokeAsync(ListAgency.HideAsync);
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
		if (_href[^1] != '/' || !_href.StartsWith(uri, StringComparison.OrdinalIgnoreCase))
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
	public void Dispose() =>
		NavMan.LocationChanged -= OnLocationChanged;
}
