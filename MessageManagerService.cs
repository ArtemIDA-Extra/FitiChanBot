using Discord;
using Discord.WebSocket;
using FitiChanBot.EF_Entities;

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

        private bool isMessagesListEmpty;
        private bool isShortMessagesListEmpty;

        public MessageManagerService(DiscordSocketClient client)
        {
            _client = client;
            Messages = new List<Message>();
            Messages.Add(new Message        // Test!!!
            {
                Id = 1,
                TargetChannelID = 1171857937837334578,
                Text = $"Test Delay-delivery message, created at [{DateTime.Now}](Local). Delivery time: [{DateTime.UtcNow + TimeSpan.FromSeconds(120)}]",
                DeliveryTime = DateTime.UtcNow + TimeSpan.FromSeconds(120),
                Status = MessageStatus.New
            });   // Test!!!
            ShortMessagesQuerry = new List<Message>();
        }

        public void PrepareShortList(TimeSpan maxTimeToSend)
        {
            foreach (var message in Messages
                .Where(a => a.DeliveryTime - DateTime.UtcNow >= maxTimeToSend && a.Status == MessageStatus.New))
            {
                ShortMessagesQuerry.Add(message);
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
            foreach (var ID in HandledMessagesIDs) ShortMessagesQuerry.RemoveAll(m => m.Id == ID);
        }

        public async Task SendMessage(Message message)
        {
            var textChannel = _client.GetChannel(message.TargetChannelID) as IMessageChannel;
            if (textChannel == null) { throw new Exception("Channel with ID[{message.TargetChannelID}] not found. This message will be ignored."); }
            else { await textChannel.SendMessageAsync(message.Text); }
        }
    }
}
