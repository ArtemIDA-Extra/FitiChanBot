using Discord;

namespace FitiChanBot
{
    struct SlashCommandBuilders
    {
        public List<SlashCommandBuilder> GlobalCommands { get; set; }
        public List<SlashCommandBuilder> GuildsCommands { get; set; }
    }
}
