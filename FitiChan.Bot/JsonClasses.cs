using Discord;

namespace FitiChanBot
{
    public class SlashCommandBuilders
    {
        public List<SlashCommandBuilder> GlobalCommands { get; set; }
        public List<SlashCommandBuilder> GuildsCommands { get; set; }
    }
}
