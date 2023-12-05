namespace FitiChan.DL.Entities;
public enum MessageStatus
{
    New,
    ReadyToSend,
    Delivered,
    Ignored
}
public class Message
{
    public ulong Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public ulong TargetChannelID { get; set; }
    public string Text { get; set; }
    public DateTime DeliveryTime { get; set; }
    public MessageStatus Status { get; set; }
}
