using FitiChan.DL;
using FitiChanBot.Settings;

namespace FitiChanBot.Interfaces
{
    public class FitiSettings
    {
        public string BotAPIKey { get; set; }
        public DBSetting DBSetting { get; set; }

        public DelaySettings DelaySettings { get; set; }
    }
}