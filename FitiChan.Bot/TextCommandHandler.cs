using Discord.Commands;
using Discord.WebSocket;

namespace FitiChanBot
{
    public class TextCommandsHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _cmdServices;
        private readonly IServiceProvider _services;

        public TextCommandsHandler(DiscordSocketClient client, CommandService cmdService, IServiceProvider services)
        {
            _cmdServices = cmdService;
            _client = client;
            _services = services;
        }

        public void SetupCommands()
        {
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            if (message.HasCharPrefix('-', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _cmdServices.ExecuteAsync(
                    context: context,
                    argPos: argPos,
                    services: _services);
                if (!result.IsSuccess)
                    await context.Channel.SendMessageAsync(result.ErrorReason);

            }
            else if (!message.Author.IsBot)
            {
                //await HandleMessageAsync(messageParam);
            }
        }

        //    private async Task HandleMessageAsync(SocketMessage arg)
        //    {
        //        var msg = arg as SocketUserMessage;
        //        if (msg != null && !msg.Author.IsBot)
        //        {
        //            Console.WriteLine($"User [{msg.Author.GlobalName}], sended message: {msg.Content}");
        //            if (msg.MentionedUsers.Count > 0)
        //            {
        //                string ans = "";
        //                foreach (SocketUser user in msg.MentionedUsers)
        //                {
        //                    if (!user.IsBot)
        //                        ans += " -> " + user.Username + "\n";
        //                }
        //                if (ans.Length > 0)
        //                {
        //                    ans = $"Mentioned users, in <@{msg.Author.Id}> message: \n" + ans;
        //                    ans += "Hi guys! Sorry for the ping :3";
        //                    await msg.Channel.SendMessageAsync(ans);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}