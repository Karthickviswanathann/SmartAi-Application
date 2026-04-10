using Microsoft.AspNetCore.Components.Authorization;
using SkillTrackerFront.Components;
using SkillTrackerFront.Services;
using SkillTrackerFront.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddAuthenticationCore();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider,CustAuthStateProvider>();
builder.Services.AddScoped<IDataFlow ,DataFlow>();
builder.Services.AddSingleton<LoadingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
