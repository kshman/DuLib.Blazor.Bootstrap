namespace Du.Blazor.Bootstrap.Web;

/// <summary>
/// URI로 이동한다
/// </summary>
public class NavUri : ComponentBase
{
	[Parameter] public string? Link { get; set; }
	[Parameter] public bool ForceLoad { get; set; }

	//
	[Inject] private NavigationManager NavMan { get; set; } = default!;

	//
	protected override void OnInitialized()
	{
		if (Link is not null)
			NavMan.NavigateTo(Link, ForceLoad);
	}
}
