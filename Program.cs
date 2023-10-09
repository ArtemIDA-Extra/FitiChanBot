using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FitiChanBot {
    public class Program
    {
        public static Task Main(string[] args) => new Program().MainAsync();

        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly CommandHandler _cmd_handler;

        public Program()
        {
            _services = CreateServices();
            _commands = _services.GetRequiredService<CommandService>();
            _client = _services.GetRequiredService<DiscordSocketClient>();
            _client.Log += Log;
            _cmd_handler = new CommandHandler(_client, _commands);

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
            await _cmd_handler.InstallCommandsAsync(); // Important! Need improvement!

            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("FitiToken"));
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}