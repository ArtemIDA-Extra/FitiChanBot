using FitiChanBot.Interfaces;

namespace FitiChanBot
{
    public class FitiSettings : IFitiSettings
    {
        public string BotAPIKey { get; set; }
        public string DBConnection { get; set; }
        public bool RunForMigration { get; set; }
    }
}
