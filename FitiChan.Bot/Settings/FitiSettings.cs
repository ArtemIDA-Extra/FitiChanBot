using FitiChan.DL;
using FitiChanBot.Settings;

namespace FitiChanBot.Settings
{
    public class FitiSettings
    {
        public string BotAPIKey { get; set; }
        public DBSettings DBSettings { get; set; }
        public TimeSpan MonitoringDelay { get; set; }
    }
}