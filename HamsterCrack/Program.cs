using System.Net.Http.Headers;
using HamsterCrack.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = false;
    options.SingleLine = true;
});


builder.Services.AddHttpClient<HamstaApi>().ConfigureHttpClient((_, client) =>
{
    var token = builder.Configuration["HAMSTER_TOKEN"]
                ?? throw new InvalidOperationException("HAMSTER_TOKEN is not set");
    var authHeader = new AuthenticationHeaderValue("Bearer", token);
    client.DefaultRequestHeaders.Authorization = authHeader;
    client.BaseAddress = new Uri("https://api.hamsterkombatgame.io/clicker/");
});
builder.Services.AddTransient<HamstaEngine>();
builder.Services.AddHostedService<DailyCipherService>();
builder.Services.AddHostedService<HamstaBackgroundService>();


using var app = builder.Build();
app.Run();