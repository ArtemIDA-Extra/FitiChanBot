using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FitiChanBot.Interfaces;
using FitiChan.DL;
using Microsoft.EntityFrameworkCore;
using FitiChanBot.Extensions;

namespace FitiChanBot
{
    public class Program
    {
        public static Task Main(string[] args) => new Program().MainAsync();

        private readonly string _settingsRelativePath = "appsettings.json";
        private readonly FitiSettings _settings;

        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public Program()
        {
            AdvConsole.WriteLine("<<<------- Reading Settings ------->>>", 0, ConsoleColor.DarkBlue);
            _settings = FitiUtilities.ReadJsonSettings<FitiSettings>(_settingsRelativePath);

            _services = CreateServices();

            _client = _services.GetRequiredService<DiscordSocketClient>();
            _client.Log += Log;
        }

        private IServiceProvider CreateServices()
        {
            var collection = new ServiceCollection()
                .AddDbContext<FitiDBContext>(builder => builder.UseMySql(_settings.DBSetting.DBConnection, new MySqlServerVersion(new Version(8, 1, 0))), ServiceLifetime.Singleton)
                .AddSingleton(new DiscordSocketConfig()
                {
                    GatewayIntents = GatewayIntents.All,
                    AlwaysDownloadUsers = true
                }) // add socket configs to DI
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<MessageManagerService>()
                .AddSingleton<BackgroundMonitorService>()
                .AddSettings(_settingsRelativePath); // new Extension method.
            return collection.BuildServiceProvider();
        }

        public async Task MainAsync()
        {
            AdvConsole.WriteLine("<<<------- Installing Commands ------->>>", 0, ConsoleColor.DarkBlue);
            await _services.GetRequiredService<CommandHandler>().InstallCommandsAsync();

            AdvConsole.WriteLine("<<<------- Starting Client ------->>>", 0, ConsoleColor.DarkBlue);
            await _client.LoginAsync(TokenType.Bot, _settings.BotAPIKey);
            await _client.StartAsync();

            if (!_settings.DBSetting.RunForMigration)
            {
                AdvConsole.WriteLine("<<<------- Starting Monitoring ------->>>", 0, ConsoleColor.DarkBlue);
                if (_client.LoginState == LoginState.LoggedIn)
                {
                    await _services.GetRequiredService<BackgroundMonitorService>().StartAsync();
                }
            }
            else
            {
                AdvConsole.WriteLine("<<<------- Creating Mogration ------->>>", 0, ConsoleColor.DarkBlue);
                await Task.Delay(new TimeSpan(0, 0, 5));
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}