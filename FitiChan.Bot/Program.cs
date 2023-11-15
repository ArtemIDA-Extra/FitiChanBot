using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FitiChanBot.Interfaces;
using FitiChan.DL;
using Microsoft.Extensions.DependencyInjection;

namespace FitiChanBot {
    public class Program
    {
        public static Task Main(string[] args) => new Program().MainAsync();

        private readonly string _settingsRelativePath = "settings.json";
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
            var config = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.All,
                AlwaysDownloadUsers = true
            };

            var collection = new ServiceCollection()
                .AddSingleton<IFitiSettings>(_settings)
                .AddSingleton<IDBSetting>(_settings)
                .AddSingleton<FitiDBContext>()
                .AddSingleton<DiscordSocketClient>(new DiscordSocketClient(config))
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<MessageManagerService>()
                .AddSingleton<BackgroundMonitorService>();
            return collection.BuildServiceProvider();
        }

        public async Task MainAsync()
        {
            AdvConsole.WriteLine("<<<------- Installing Commands ------->>>", 0, ConsoleColor.DarkBlue);
            await _services.GetRequiredService<CommandHandler>().InstallCommandsAsync();                

            AdvConsole.WriteLine("<<<------- Starting Client ------->>>", 0, ConsoleColor.DarkBlue);
            await _client.LoginAsync(TokenType.Bot, _settings.BotAPIKey);
            await _client.StartAsync();

            if (!_settings.RunForMigration)
            {
                AdvConsole.WriteLine("<<<------- Starting Monitoring ------->>>", 0, ConsoleColor.DarkBlue);
                if (_client.LoginState == LoginState.LoggedIn)
                {
                    await _services.GetRequiredService<BackgroundMonitorService>().StartAsync(new TimeSpan(0, 5, 0), new TimeSpan(0, 0, 10));
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