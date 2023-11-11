using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.AspNetCore.Http.Features;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters;

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
            if (targetUser == executorUser || !Context.IsPrivate)
            {
                if (executorUser.GlobalName == "Artem_IDA")       // The bot developer gets more functionality.
                {
                    var embedAuthor = new EmbedAuthorBuilder
                    {
                        Name = $"{targetUser.Username}",
                        IconUrl = "https://discord.com/assets/28174a34e77bb5e5310ced9f95cb480b.png"
                    };

                    var embedFooter = new EmbedFooterBuilder
                    {
                        Text = "Have a nice day! <3"
                    };

                    var embed = new EmbedBuilder
                    {
                        Author = embedAuthor,
                        Title = "User Info",
                        Color = Color.Blue,
                        Footer = embedFooter,
                    };

                    if (executorUser != targetUser)
                    {
                        embed.Description = "Here's what I know about this user:";
                    }
                    else
                    {
                        embed.Description = "Here's what I know about you :3";
                    }

                    EmbedFieldBuilder embedUsernameField;
                    if (targetUser.Discriminator != "0000")        // Checking whether the user has switched to the current naming system. If Discriminator is 0000 - means old.
                    {
                        embedUsernameField = new EmbedFieldBuilder
                        {
                            Name = "Username",
                            Value = targetUser.Username + "#" + targetUser.Discriminator
                        };
                        embed.AddField(embedUsernameField);
                    }
                    else
                    {
                        embedUsernameField = new EmbedFieldBuilder
                        {
                            Name = "Username",
                            Value = targetUser.Username
                        };
                        embed.AddField(embedUsernameField);
                    }

                    EmbedFieldBuilder embedGlobalnameField;
                    if (targetUser.GlobalName != null)
                    {
                        embedGlobalnameField = new EmbedFieldBuilder
                        {
                            Name = "Globalname",
                            Value = targetUser.GlobalName
                        };
                        embed.AddField(embedGlobalnameField);
                    }

                    EmbedFieldBuilder embedIdField = new EmbedFieldBuilder
                    {
                        Name = "ID",
                        Value = targetUser.Id
                    };
                    embed.AddField(embedIdField);

                    EmbedFieldBuilder embedCreationDateField = new EmbedFieldBuilder
                    {
                        Name = "Account create date",
                        Value = targetUser.CreatedAt
                    };
                    embed.AddField(embedCreationDateField);

                    EmbedFieldBuilder embedAvatarIdField;
                    if (targetUser.GetAvatarUrl() != null)
                    {
                        embedAvatarIdField = new EmbedFieldBuilder
                        {
                            Name = "Avatar ID",
                            Value = targetUser.AvatarId
                        };
                        embed.AddField(embedAvatarIdField);
                    }

                    EmbedFieldBuilder embedAvatarUrlField;
                    if (targetUser.GetAvatarUrl() != null)
                    {
                        embedAvatarUrlField = new EmbedFieldBuilder
                        {
                            Name = "Avatar Url",
                            Value = $"[Link]({targetUser.GetAvatarUrl(Discord.ImageFormat.Auto, 512)})"    // HyperLink to user avatar in Markdown markup language.
                        };
                        embed.AddField(embedAvatarUrlField);
                        embed.ImageUrl = targetUser.GetAvatarUrl();
                    }

                    await ReplyAsync(embed: embed.Build());
                }
                else                                              // Functionality for ordinary users.
                {
                    var embedAuthor = new EmbedAuthorBuilder
                    {
                        Name = $"{targetUser.Username}",
                        IconUrl = "https://discord.com/assets/28174a34e77bb5e5310ced9f95cb480b.png"
                    };

                    var embedFooter = new EmbedFooterBuilder
                    {
                        Text = "Have a nice day!"
                    };

                    var embed = new EmbedBuilder
                    {
                        Author = embedAuthor,
                        Title = "User Info",
                        Color = Color.Blue,
                        Footer = embedFooter,
                        Description = "Here's what I know about this user:"
                    };

                    EmbedFieldBuilder embedUsernameField;
                    if (targetUser.Discriminator != "0000")      // Checking whether the user has switched to the current naming system. If Discriminator is 0000 - means old.
                    {
                        embedUsernameField = new EmbedFieldBuilder
                        {
                            Name = "Username",
                            Value = targetUser.Username + "#" + targetUser.Discriminator
                        };
                        embed.AddField(embedUsernameField);
                    }
                    else
                    {
                        embedUsernameField = new EmbedFieldBuilder
                        {
                            Name = "Username",
                            Value = targetUser.Username
                        };
                        embed.AddField(embedUsernameField);
                    }

                    EmbedFieldBuilder embedGlobalnameField;
                    if (targetUser.GlobalName != null)
                    {
                        embedGlobalnameField = new EmbedFieldBuilder
                        {
                            Name = "Globalname",
                            Value = targetUser.GlobalName
                        };
                        embed.AddField(embedGlobalnameField);
                    }

                    EmbedFieldBuilder embedCreationDateField = new EmbedFieldBuilder
                    {
                        Name = "Account create date",
                        Value = targetUser.CreatedAt
                    };
                    embed.AddField(embedCreationDateField);

                    EmbedFieldBuilder embedAvatarUrlField;
                    if (targetUser.GetAvatarUrl() != null)
                    {
                        embedAvatarUrlField = new EmbedFieldBuilder
                        {
                            Name = "Avatar Url",
                            Value = $"[Link]({targetUser.GetAvatarUrl(Discord.ImageFormat.Auto, 512)})"    // HyperLink to user avatar in Markdown markup language.
                        };
                        embed.AddField(embedAvatarUrlField);
                        embed.ImageUrl = targetUser.GetAvatarUrl();
                    }

                    await ReplyAsync(embed: embed.Build());
                }
            }
            else if (Context.IsPrivate)
            {
                await ReplyAsync("Sorry, I can't view information about another user if we are not on the same server where he is present. " +
                                 "But I can provide some information about you.");
            }
        }

        [Command("UTC")]
        [Summary("Returns the time according to the specified UTC offset. If you don't enter anything, it will display UTC +00:00 time")]
        [Alias("utc")]
        public async Task TimeAsync([Summary("The UTC offset in range [-12 to +14]")] int UtcOffset = 0)
        {
            EmbedFieldBuilder timeField;
            switch (UtcOffset)
            {
                case > 0:
                    timeField = new EmbedFieldBuilder()
                    {
                        Name = $"UTC +{UtcOffset}:00",
                        Value = $"{(DateTime.UtcNow + new TimeSpan(UtcOffset, 0, 0)).ToShortTimeString()}"
                    };
                    break;
                case < 0:
                    timeField = new EmbedFieldBuilder()
                    {
                        Name = $"UTC {UtcOffset}:00",
                        Value = $"{(DateTime.UtcNow + new TimeSpan(UtcOffset, 0, 0)).ToShortTimeString()}"
                    };
                    break; 
                default:
                    timeField = new EmbedFieldBuilder()
                    {
                        Name = $"UTC",
                        Value = $"{DateTime.UtcNow.ToShortTimeString()}"
                    }; break;
            }
            
            var embed = new EmbedBuilder()
                .WithColor(Color.Blue)
                .AddField(timeField);
            await ReplyAsync(embed: embed.Build());
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
        public async Task CreateDSMessage([Summary("Delivery date")] string Date, [Remainder] [Summary("Message")] string message)
        {
            if (!Context.IsPrivate)
            {
                await ReplyAsync($"Message will be delivered - **{Date}**\n" +
                                 $"Message text:\n    *{message.Replace("\n", " ")}*\n" +
                                 $"Server(Guild): {Context.Guild.Name}\n" +
                                 $"Server ID: {Context.Guild.Id}\n" +
                                 $"Channel: {Context.Channel.Name}\n" +
                                 $"Channel ID: {Context.Channel.Id}\n");
            }
            else await ReplyAsync("Oops, sorry. I can't create ads in a private message :/. Try asking me about it on the server.");
        }
    }
}
