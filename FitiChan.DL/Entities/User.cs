using System.ComponentModel.DataAnnotations;

namespace FitiChan.DL.Entities;
public class User
{
    [Key]
    public ulong DSID { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Username { get; set; }
    public string? Globalname { get; set; }
    public string Discriminator { get; set; }
    public DateTime AccountCreationDate { get; set; }
    public string? AvatarID { get; set; }
    public string? AvataURL { get; set; }
}
