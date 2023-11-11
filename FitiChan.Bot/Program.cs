using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FitiChan.DL;
using Newtonsoft.Json;

namespace FitiChanBot {
    public class Program
    {
        public static Task Main(string[] args) => new Program().MainAsync();

        private readonly string _settingsRelativePath = "settings.json";
        private readonly FitiSettings? _settings;

        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly CommandHandler _cmdHandler;
        private readonly FitiDBContext _dbContext;
        private readonly MessageManagerService _msgManager;                                //
        private readonly BackgroundMonitorService _msgMonitor;                             //

        public Program()
        {
            AdvConsole.WriteLine("<<<------- Reading a settings file ------->>>", 0, ConsoleColor.DarkBlue);
            _settings = FitiUtilities.ReadJsonSettings<FitiSettings>(_settingsRelativePath);

            _services = CreateServices();
            _commands = _services.GetRequiredService<CommandService>();
            _client = _services.GetRequiredService<DiscordSocketClient>();
            _client.Log += Log;
            _cmdHandler = new CommandHandler(_client, _commands);

            _dbContext = new FitiDBContext();                                                                                             //
            _dbContext.DBConnectionStr = _settings.DBConnection;                                                                          //

            _msgManager = new MessageManagerService(_client);                                                                               //
            _msgMonitor = new BackgroundMonitorService(_msgManager, new TimeSpan(0, 5, 0), new TimeSpan(0, 0, 10));                         //
        }
        static IServiceProvider CreateServices()
        {
            var config = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.All,
                AlwaysDownloadUsers = true
            };

            var collection = new ServiceCollection()
                .AddSingleton(new DiscordSocketClient(config))
                .AddSingleton(new CommandService());

            return collection.BuildServiceProvider();
        }

        public async Task MainAsync()
        {
            AdvConsole.WriteLine("<<<------- Installing Commands ------->>>", 0, ConsoleColor.DarkBlue);
            await _cmdHandler.InstallCommandsAsync(); // Important! Need improvement!

            AdvConsole.WriteLine("<<<------- Starting Client ------->>>", 0, ConsoleColor.DarkBlue);
            await _client.LoginAsync(TokenType.Bot, _settings.BotAPIKey);
            await _client.StartAsync();

            if (!_settings.RunForMigration)
            {
                AdvConsole.WriteLine("<<<------- Starting Monitoring ------->>>", 0, ConsoleColor.DarkBlue);
                if (_client.LoginState == LoginState.LoggedIn)
                {
                    await _msgMonitor.StartAsync();
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