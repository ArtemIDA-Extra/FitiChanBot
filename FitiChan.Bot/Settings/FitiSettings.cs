using FitiChan.DL;
using FitiChanBot.Settings;

namespace FitiChanBot.Settings
{
    public class FitiSettings
    {
        public DBSettings DBSettings { get; set; }
        public TimeSpan MonitoringDelay { get; set; }
    }
}