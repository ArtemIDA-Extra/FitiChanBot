using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FitiChan.DL;
using Microsoft.EntityFrameworkCore;
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

            _settings = ReadJsonSettings(_settingsRelativePath);
            //if( _settings == null ) Environment.Exit(0);                                 // If it was not possible to read the settings file - exit

            _services = CreateServices();
            _commands = _services.GetRequiredService<CommandService>();
            _client = _services.GetRequiredService<DiscordSocketClient>();
            _client.Log += Log;
            _cmdHandler = new CommandHandler(_client, _commands);

            _dbContext = new FitiDBContext(_settings.DBConnection);                                  //

            _msgManager = new MessageManagerService(_client);                                        //
            _msgMonitor = new BackgroundMonitorService(_msgManager, 600, 60);                        //
        }

        public FitiSettings? ReadJsonSettings(string settingsRelativePath)
        {
            string settingsFileAbsolutePath = Directory.GetCurrentDirectory() + "\\" + settingsRelativePath;
            FitiSettings? settings = null;

            AdvConsole.WriteLine("<<<------- Reading a settings file ------->>>", 0, ConsoleColor.DarkBlue);
            Console.WriteLine($"Relative path to file - {settingsRelativePath}");
            Console.WriteLine($"Absolute path to file - {settingsFileAbsolutePath}");

            try
            {
                if (!File.Exists(settingsFileAbsolutePath)) throw new Exception($"File on path <{settingsFileAbsolutePath}> does not exist. Unable to load settings.");

                using (StreamReader sr = new StreamReader(settingsFileAbsolutePath))
                {
                    string json = sr.ReadToEnd();
                    settings = JsonConvert.DeserializeObject<FitiSettings>(json);
                }

                if (settings == null) throw new Exception($"Deserialized FitiSettings object from file <{settingsFileAbsolutePath}> is null. (Maybe something is wrong with the file syntax or path to it?)");
            }
            catch(Exception ex)
            {
                AdvConsole.WriteLine("<<<--!!!-- ERROR --!!!-->>>", 0, ConsoleColor.Red);
                AdvConsole.WriteLine($"{ex.Message}", 0, ConsoleColor.White);
                throw;
            }

            return settings;
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

            AdvConsole.WriteLine("<<<------- Starting Monitoring ------->>>", 0, ConsoleColor.DarkBlue);
            if (_client.LoginState == LoginState.LoggedIn)
            {
                await _msgMonitor.StartAsync();
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}