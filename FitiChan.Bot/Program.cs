using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FitiChanBot.Settings;
using FitiChan.DL;
using Microsoft.EntityFrameworkCore;
using FitiChanBot.Extensions;
using System.Reflection;

namespace FitiChanBot
{
    public class Program
    {
        public static Task Main(string[] args) => new Program().MainAsync();

        private readonly string _settingsRelativePath = "appsettings.json";
        private readonly string _commandsRelativePath = "commands.json";

        private readonly FitiSettings _settings;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;

        public Program()
        {
            AdvConsole.WriteLine("<<<------- Reading Settings ------->>>", 0, ConsoleColor.DarkBlue);
            _settings = FitiUtilities.ReadJsonRelative<FitiSettings>(_settingsRelativePath);

            _services = CreateServices();

            _client = _services.GetRequiredService<DiscordSocketClient>();
            _client.Log += Log;
        }

        private IServiceProvider CreateServices()
        {
            var collection = new ServiceCollection()
                .AddDbContext<FitiDBContext>(builder => builder.UseMySql(_settings.DBSettings.DBConnection, new MySqlServerVersion(new Version(8, 1, 0))), ServiceLifetime.Singleton)
                .AddSingleton(new DiscordSocketConfig()
                {
                    GatewayIntents = GatewayIntents.All,
                    AlwaysDownloadUsers = true
                }) // add socket configs to DI
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<TextCommandsHandler>()
                .AddSingleton<SlashCommandsHandler>(s => new SlashCommandsHandler(
                _commandsRelativePath,
                s.GetRequiredService<FitiSettings>(),
                s.GetRequiredService<DiscordSocketClient>(),
                s.GetRequiredService<CommandService>(),
                s))
                .AddSingleton<MessageManager>()
                .AddSingleton<BackgroundMonitor>()
                .AddSettings(_settingsRelativePath); // new Extension method.
            return collection.BuildServiceProvider();
        }

        private async Task InitCommands()
        {
            var CMDService = _services.GetRequiredService<CommandService>();

            await CMDService.AddModuleAsync<InfoModule>(null);
            await CMDService.AddModuleAsync<ADModule>(_services);
        }

        public async Task MainAsync()
        {
            AdvConsole.WriteLine("<<<------- Init Commands Entities ------->>>", 0, ConsoleColor.DarkBlue);
            await InitCommands();

            AdvConsole.WriteLine("<<<------- Setup Text CMD-Hanbler ------->>>", 0, ConsoleColor.DarkBlue);
            _services.GetRequiredService<TextCommandsHandler>().SetupCommands();

            AdvConsole.WriteLine("<<<------- Setup Slash CMD-Hanbler ------->>>", 0, ConsoleColor.DarkBlue);
            _services.GetRequiredService<SlashCommandsHandler>().SetupCommands();

            AdvConsole.WriteLine("<<<------- Loggin ------->>>", 0, ConsoleColor.DarkBlue);
            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("FitiToken"));

            AdvConsole.WriteLine("<<<------- Starting Client ------->>>", 0, ConsoleColor.DarkBlue);
            await _client.StartAsync();

            if (!_settings.DBSettings.RunForMigration)
            {
                AdvConsole.WriteLine("<<<------- Starting Monitoring ------->>>", 0, ConsoleColor.DarkBlue);
                if (_client.LoginState == LoginState.LoggedIn)
                {
                    await _services.GetRequiredService<BackgroundMonitor>().StartAsync();
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