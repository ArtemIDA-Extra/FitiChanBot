using FitiChan.DL;

namespace FitiChanBot.Interfaces
{
    public interface IFitiSettings : IDBSetting
    {
        string BotAPIKey { get; set; }
    }
}