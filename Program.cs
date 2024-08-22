using GeneralPurposeLib;
using OmicronSocial.Components;
using OmicronSocial.Data;
using OmicronSocial.Data.Storage;
using LogLevel = GeneralPurposeLib.LogLevel;

namespace OmicronSocial;

internal class Program {
    public static IStorageManager StorageManager { get; private set; } = null!;
    private static readonly Dictionary<string, Property> DefaultConfig = new() {  // Will be copied to config.json
        { "mysql_host", "mysql.serble.net" },
        { "mysql_user", "omicron" },
        { "mysql_pass", "omicron" },
        { "mysql_db", "omicron" },
        { "serble_client_id", "xxxxxxxxxxxxx" },
        { "serble_client_secret", "xxxxxxxxxxxxxx" },
        { "serble_api_url", "https://api.serble.net/api/v1/" },
        { "serble_oauth_url", "https://serble.net/oauth/authorize" }
    };
    
    public static void Main(string[] args) {
        Logger.Init(LogLevel.Debug);
        
        Config config = new(DefaultConfig);  // Load the config
        GlobalConfig.Init(config);  // Global config so we can access it from everywhere
        
        StorageManager = new MySqlStorage();  // Init the storage manager
        StorageManager.Init();
        
        CancellationTokenSource cts = new();
        Chats.Init(cts.Token);  // Init the Chat service

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        // This blocks until the app exits
        app.Run();
        
        // Stop the chat service before exiting
        cts.Cancel();
    }
}