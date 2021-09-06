namespace Drunkforge.Bonnia.Client
{
	using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
	using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Syncfusion.Blazor;
	using System;
	using System.Net.Http;
	using System.Threading.Tasks;

	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			builder.Services.AddSyncfusionBlazor();

			builder.Services.AddHttpClient("Drunkforge.Bonnia.API", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
				.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

			// Supply HttpClient instances that include access tokens when making requests to the server project
			builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Drunkforge.Bonnia.API"));

			builder.Services.AddMsalAuthentication(options =>
			{
				builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
				options.ProviderOptions.DefaultAccessTokenScopes.Add("api://api.id.uri/access_as_user");
			});

			await builder.Build().RunAsync();
		}
	}
}
