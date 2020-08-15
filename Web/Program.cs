using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using Core.Catalogue;

namespace Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
            var items = await httpClient.GetFromJsonAsync<List<Item>>("api/data.json");

            builder.RootComponents.Add<App>("app");
            builder.Services.AddScoped<IItemService>(_ => new InMemoryItemService(items));
            builder.Services.AddAntDesign();

            await builder.Build().RunAsync();
        }
    }
}
