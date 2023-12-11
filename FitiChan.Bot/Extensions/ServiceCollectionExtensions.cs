using FitiChan.DL;
using FitiChanBot.Settings;

namespace FitiChanBot.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsons(this IServiceCollection serviceCollection, string settingsRelativePath, string commandsRelativePath)
        {
            return serviceCollection
                .AddSingleton(FitiUtilities.ReadJsonRelative<FitiSettings>(settingsRelativePath))
                .AddSingleton(FitiUtilities.ReadJsonRelative<DBSettings>(settingsRelativePath))
                .AddSingleton(FitiUtilities.ReadJsonRelative<SlashCommandBuilders>(commandsRelativePath));
        }
    }
}
