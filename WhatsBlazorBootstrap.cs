global using Du.Blazor;
global using Du.Blazor.Bootstrap.Supp;
global using Du.Blazor.Components;
global using Du.Blazor.Supp;
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Forms;
global using Microsoft.AspNetCore.Components.Rendering;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.Extensions.Logging;
global using Microsoft.JSInterop;

namespace Du;

public static class WhatsBlazor
{
	public static string Name => Du.Properties.Resources.WhatsBlazorBootstrap;

	public const int Id = 8;
}

// # 줄 삭제 => #(.*)\n
