using Discord;
using Discord.WebSocket;
using FitiChan.DL;
using FitiChan.DL.Entities;
using FitiChanBot.Settings;
using System.Security.Cryptography.X509Certificates;

namespace FitiChanBot
{

    public class MessageManagerService
    {
        private List<Message> Messages { get; set; } // Test!!!
        private List<Message> ShortMessagesQuerry { get; set; }
        public bool IsMessagesListEmpty
        {
            get
            {
                if (Messages.Count < 1) { isMessagesListEmpty = true; return isMessagesListEmpty; }
                else { isMessagesListEmpty = false; return isMessagesListEmpty; }
            }
        }
        public bool IsShortMessagesListEmpty
        {
            get
            {
                if (ShortMessagesQuerry.Count < 1) { isShortMessagesListEmpty = true; return isShortMessagesListEmpty; }
                else { isShortMessagesListEmpty = false; return isShortMessagesListEmpty; }
            }
        }

        private readonly DiscordSocketClient _client;
        private readonly FitiDBContext _db;
        private readonly FitiSettings _settings;

        private bool isMessagesListEmpty;
        private bool isShortMessagesListEmpty;

        public MessageManagerService(DiscordSocketClient client, FitiDBContext db, FitiSettings settings)
        {
            _client = client;
            _db = db;
            _settings = settings;
            Messages = new List<Message>();
            ShortMessagesQuerry = new List<Message>();
            Messages.Add(new Message        // Test!!!
            {
                Id = 1,
                TargetChannelID = 1171857937837334578,
                Text = $"Test Delay-delivery message, created at [{DateTime.UtcNow}](UTC). Delivery time: [{DateTime.UtcNow + TimeSpan.FromSeconds(120)}]\n",
                DeliveryTime = DateTime.UtcNow + TimeSpan.FromSeconds(120),
                Status = MessageStatus.New
            });   // Test!!!
        }

        public void UpdateShortList(TimeSpan maxTimeToSend)
        {
            foreach (var message in Messages
                .Where(a => a.DeliveryTime - DateTime.UtcNow <= maxTimeToSend && a.Status == MessageStatus.New))
            {
                ShortMessagesQuerry.Add(message);
                message.Status = MessageStatus.ReadyToSend;
            }
        }

        public async Task SendShortListAsync()
        {
            List<ulong> HandledMessagesIDs = new List<ulong>();
            foreach (Message message in ShortMessagesQuerry.ToList())      // We can't use foreach when changing instances of the contained list we're iterating over.                                                                          
            {                                                              // Therefore, we create a copy of list.ToList() directly in the iteration loop 
                if (message.DeliveryTime.Minute == DateTime.UtcNow.Minute) // https://stackoverflow.com/questions/604831/collection-was-modified-enumeration-operation-may-not-execute
                {
                    try
                    {
                        await SendMessage(message);
                        HandledMessagesIDs.Add(message.Id);
                        ShortMessagesQuerry.Find(m => m.Id == message.Id)!.Status = MessageStatus.Delivered;
                        ShortMessagesQuerry.RemoveAll(m => m.Id == message.Id);
                    }
                    catch (Exception ex)
                    {
                        AdvConsole.WriteLine($"!!!ERROR!!! ({DateTime.Now.ToShortTimeString()}) " + ex.Message);
                        HandledMessagesIDs.Add(message.Id);
                        ShortMessagesQuerry.Find(m => m.Id == message.Id)!.Status = MessageStatus.Ignored;
                        ShortMessagesQuerry.RemoveAll(m => m.Id == message.Id);
                    }
                }
            }
            foreach (var ID in HandledMessagesIDs)
            {
                Messages.RemoveAll(m => m.Id == ID);
                ShortMessagesQuerry.RemoveAll(m => m.Id == ID);
            }
        }

        public string GetDebugInfo()
        {
            return
                $"Messages in DB: **{_db.Messages.Count()}**\n" +
                $"Messages in Runtime List: **{Messages.Count}**\n" +
                $"Messages in Short Messages Querry: **{ShortMessagesQuerry.Count}**";
        }

        public void CreateMessage(DateTime deliveryTime, ISocketMessageChannel channel, string message)
        {
            if (deliveryTime < DateTime.UtcNow) throw new Exception("Messages with a past delivery date are not allowed.");
            Messages.Add(new Message
            {
                TargetChannelID = channel.Id,
                Text = message,
                DeliveryTime = deliveryTime,
                Status = MessageStatus.New
            });
            UpdateShortList(_settings.MonitoringDelay);
        }

        public async Task SendMessage(Message message)
        {
            var textChannel = _client.GetChannel(message.TargetChannelID) as IMessageChannel;
            if (textChannel == null) { throw new Exception("Channel with ID[{message.TargetChannelID}] not found. This message will be ignored."); }
            else { await textChannel.SendMessageAsync(message.Text); }
        }
    }
}
