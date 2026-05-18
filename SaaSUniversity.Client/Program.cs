using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SaaSUniversity.Client;
using SaaSUniversity.Client.Service;
using SaaSUniversity.Client.Service.CoursesService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<UserState>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICourseService, CourseService>();

builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

builder.Services.AddScoped(sp => (CookieAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());
builder.Services.AddScoped(sp =>
{
    var client = new HttpClient
    {
        BaseAddress = new Uri("https://localhost:7159/")
    };
    return client;
});

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();
