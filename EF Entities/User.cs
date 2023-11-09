namespace FitiChanBot.EF_Entities
{
    public class User
    {
        public ulong DSID { get; set; }
        public string? Username { get; set; }
        public string? Globalname { get; set; }
        public string? Discriminator { get; set; }
        public string? AvatarID { get; set; }
        public string? AvataURL { get; set; }
    }
}
