using FitiChan.DL;
using FitiChanBot.Interfaces;

namespace FitiChanBot.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSettings(this IServiceCollection serviceCollection, string settingsRelativePath)
        {
            return serviceCollection
                .AddSingleton(FitiUtilities.ReadJsonSettings<FitiSettings>(settingsRelativePath))
                .AddSingleton(FitiUtilities.ReadJsonSettings<DBSetting>(settingsRelativePath));
        }
    }
}
