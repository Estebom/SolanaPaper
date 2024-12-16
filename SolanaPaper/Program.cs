using SolanaPaper.Data.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SolanaPaper.Data;
using SolanaPaper.Data.Models.ServiceSettings;
using SolanaPaper.Services.Solana;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.Configure<SolanaSettings>(builder.Configuration.GetSection("Solana"));
builder.Services.AddSingleton<UsersRepository>();
builder.Services.AddSingleton<TokenRepository>();
builder.Services.AddSingleton<MarketDataRepository>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
