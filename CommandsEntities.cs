using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace FitiChanBot
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("user")]
        [Summary("Returns info about the user who used this command, or the user parameter, if one passed.")]
        [Alias("info", "whois", "usr")]
        public async Task UserInfoAsync([Summary("The (optional) user to get info from")] SocketUser user = null)
        {
            var executorUser = Context.User;                  // Explicitly get the user who called the command.
            var targetUser = user ?? executorUser;            // The user about whom we will display information(if there is no target user, it will display information about the command caller).
            if (executorUser.GlobalName == "Artem_IDA")       // The bot developer gets more functionality.
            {
                string ans = "";
                if (executorUser != targetUser)
                    ans += "Here's what I know about this user:\n";
                else
                    ans += "Here's what I know about you :3\n";
                ans += $"   **Username** - *{targetUser.Username}*";
                if (targetUser.Discriminator != "0000")        // Checking whether the user has switched to the current naming system. If Discriminator is 0000 - means old.
                    ans += $"#{targetUser.Discriminator}\n";
                else
                    ans += "\n";
                if (targetUser.GlobalName != null) ans += $"   **Globalname** - *{targetUser.GlobalName}*\n";
                ans += $"   **Id** - *{targetUser.Id}*\n";
                ans += $"   **Account create date** - *{targetUser.CreatedAt}*\n";
                if (targetUser.GetAvatarUrl() != null) ans += $"   **Avatar Id** - *{targetUser.AvatarId}*\n";
                if (targetUser.GetAvatarUrl() != null) ans += $"   **Avatar Url** - *[Link]({targetUser.GetAvatarUrl(Discord.ImageFormat.Auto, 512)})*\n";  // HyperLink to user avatar in Markdown markup language.
                //ans += $"";
                await ReplyAsync(ans);
            }
            else                                              // Functionality for ordinary users.
            {
                string ans = "";
                if (executorUser != targetUser)
                    ans += "Here's what I know about this user:\n";
                else
                    ans += "Here's what I know about you:\n";
                ans += $"   **Username** - *{targetUser.Username}*";
                if (targetUser.Discriminator != "0000")       // Checking whether the user has switched to the current naming system. If Discriminator is 0000 - means old.
                    ans += $"#{targetUser.Discriminator}\n";
                else
                    ans += "\n";
                if (targetUser.GlobalName != null) ans += $"   **Globalname** - *{targetUser.GlobalName}*\n";
                ans += $"   **Account create date** - *{targetUser.CreatedAt}*\n";
                if (targetUser.GetAvatarUrl() != null) ans += $"   **Avatar Url** - *[Link]({targetUser.GetAvatarUrl()})*\n";  // HyperLink to user avatar in Markdown markup language.
                //ans += $"";
                await ReplyAsync(ans);
            }
        }
    }

    [Group("dsmessage")]
    [Summary("*Delayed sending messages.*")]
    [Alias("dsm", "DSM", "DSMessage")]
    public class DSMessagesModule : ModuleBase<SocketCommandContext>
    {
        [Command("create")]
        [Summary("*Create delayed sending message.*")]
        [Alias("\ncreate", "Create", "\nCreate")]
        public async Task CreateDSMessage([Summary("Delivery date.")] string Date, [Remainder] [Summary("Message")] string message)
        {
            await ReplyAsync($"Message will be delivered - **{Date}**\n" + $"Message text:\n    *{message.Replace("\n", " ")}*");
        }
    }
}
