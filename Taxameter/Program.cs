using Microsoft.AspNetCore.Components;
using Taxameter.Components;
using Taxameter.Hubs;
using Taxameter.Servies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped(sp =>
{
    var navManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navManager.BaseUri) };
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSignalR();
builder.Services.AddSingleton<TaxameterService>();
builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapHub<TaxameterHub>("/taxameterHub");
app.MapControllers();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
