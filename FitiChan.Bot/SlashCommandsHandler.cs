using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FitiChanBot.Extensions;
using FitiChanBot.Settings;

namespace FitiChanBot
{
    public class SlashCommandsHandler
    {
        private readonly FitiSettings _settings;
        private readonly DiscordSocketClient _client;
        private readonly CommandsKeeper _cmdKeeper;
        private readonly IServiceProvider _services;

        private SlashCommandBuilders _slashCommandBuilders;
        
        public SlashCommandsHandler(SlashCommandBuilders commands, FitiSettings settings, DiscordSocketClient client, CommandsKeeper cmdKeeper, IServiceProvider services)
        {
            _settings = settings;
            _client = client;
            _cmdKeeper = cmdKeeper;
            _services = services;
            _slashCommandBuilders = commands;
        }

        public void SetupCommands()
        {
            _client.Ready += UpdateCommandsOnOldGuilds;
            _client.JoinedGuild += InitCommandsOnNewGuild;
            _client.SlashCommandExecuted += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketSlashCommand cmd)
        {
            await _cmdKeeper.ExecuteAsync(cmd);
        }

        private async Task UpdateCommandsOnOldGuilds()
        {
            if (_settings.UpdateGuildCommands)
            {
                foreach (SocketGuild guild in _client.Guilds)
                {
                    foreach (var command in _slashCommandBuilders.GuildsCommands)
                    {
                        await guild.CreateApplicationCommandAsync(command.Build());
                    }
                }
            }
        }

        private async Task InitCommandsOnNewGuild(SocketGuild guild)
        {
            foreach (var command in _slashCommandBuilders.GuildsCommands)
            {
                await guild.CreateApplicationCommandAsync(command.Build());
            }
        }
    }
}
