namespace FitiChan.DL
{
    public interface IDBSetting
    {
        string DBConnection { get; set; }
        bool RunForMigration { get; set; }
    }
}