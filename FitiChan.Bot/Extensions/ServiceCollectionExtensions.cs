using FitiChan.DL;
using FitiChanBot.Settings;

namespace FitiChanBot.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSettings(this IServiceCollection serviceCollection, string settingsRelativePath)
        {
            return serviceCollection
                .AddSingleton(FitiUtilities.ReadJsonRelative<FitiSettings>(settingsRelativePath))
                .AddSingleton(FitiUtilities.ReadJsonRelative<DBSettings>(settingsRelativePath));
        }
    }
}
