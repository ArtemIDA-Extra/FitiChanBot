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
        private readonly CommandService _cmdService;
        private readonly IServiceProvider _services;

        private readonly string _pathToJsonCommand;

        private SlashCommandBuilders _slashCommandBuilders;
        
        public SlashCommandsHandler(string pathToJsonCommands, FitiSettings settings, DiscordSocketClient client, CommandService cmdService, IServiceProvider services)
        {
            _settings = settings;
            _client = client;
            _cmdService = cmdService;
            _services = services;

            _pathToJsonCommand = pathToJsonCommands;
        }

        public void SetupCommands()
        {
            _client.Ready += UpdateCommandsOnOldGuilds;
            _client.JoinedGuild += InitCommandsOnNewGuild;
            _client.SlashCommandExecuted += HandleCommandAsync;

            try { _slashCommandBuilders = FitiUtilities.ReadJsonRelative<SlashCommandBuilders>(_pathToJsonCommand); }
            catch { throw; }
        }

        private async Task HandleCommandAsync(SocketSlashCommand cmd)
        {
            throw new NotImplementedException();
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
