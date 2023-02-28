global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Forms;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.JSInterop;
global using Du.Blazor.Bootstrap;
global using Du.Blazor.Bootstrap.Props;
global using Du.Blazor.Supplement;

namespace Du;

public static class WhatsBlazor
{
	public static string Name => Du.Properties.Resources.WhatsBlazorBootstrap;

	public const int Id = 6;
}

// # 줄 삭제 => #(.*)\n
