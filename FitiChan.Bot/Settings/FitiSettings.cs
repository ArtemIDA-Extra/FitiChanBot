using FitiChan.DL;
using FitiChanBot.Settings;

namespace FitiChanBot.Settings
{
    public class FitiSettings
    {
        public bool UpdateGuildCommands { get; set; }
        public DBSettings DBSettings { get; set; }
        public TimeSpan MonitoringDelay { get; set; }
    }
}