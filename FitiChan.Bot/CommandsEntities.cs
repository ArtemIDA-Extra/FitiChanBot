using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace FitiChanBot
{
    [Summary("*Info about commands and help utilities*")]
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        IServiceProvider _services;
        public InfoModule(IServiceProvider services)
        {
            _services = services;
        }

        [Command("User")]
        [Summary("Returns info about the user who used this command, or the user parameter, if one passed.\n*Example: -user @Artem_IDA*")]
        [Alias("info", "whois", "usr", "user")]
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

        [Command("Utc")]
        [Summary("Returns the time according to the specified UTC offset. If you don't enter anything, it will display UTC +00:00 time\n*Example: -UTC +2*")]
        [Alias("utc", "UTC")]
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

        [Command("Help")]
        [Summary("Displays information about all bot commands.")]
        [Alias("HELP, help")]
        public async Task Help()
        {
            List<CommandInfo> commands = _services.GetRequiredService<CommandService>().Commands.ToList();
            
            EmbedBuilder embedBuilder = new EmbedBuilder();
            foreach (CommandInfo command in commands)
            {
                // Get the command Summary attribute information
                string embedFieldText = command.Summary ?? "No description available\n";

                embedBuilder.AddField(command.Name, embedFieldText);
            }

            await ReplyAsync("Here's a list of commands and their description: ", false, embedBuilder.Build());
        }
    }

    [Group("Ad")]
    [Summary("*Delayed sending messages (ADs).*")]
    [Alias("ad", "AD")]
    public class ADModule : ModuleBase<SocketCommandContext>
    {
        IServiceProvider _services;
        public ADModule(IServiceProvider services)
        {
            _services = services;
        }

        [Command("Create")]
        [Summary("Create ad message. Data format - UTC+0.\n*Example: -ad create \"23/12/2023 15:56\" #main \"Hello!\"*")]
        [Alias("create", "\ncreate", "\nCreate")]
        public async Task CreateAD([Summary("Delivery date")] string date, ISocketMessageChannel channel, [Remainder][Summary("Message")] string message)
        {
            if (!Context.IsPrivate)
            {
                try { _services.GetRequiredService<MessageManagerService>().CreateMessage(DateTime.Parse(date), channel, message); }
                catch { await ReplyAsync("Sorry, I can't recognize the delivery date for the message. Write the date as follows: \"06/15/2008 08:30\""); return; }
                await ReplyAsync($"Message will be delivered - **{date} UTC+0**\n" +
                                 $"Message text:\n    *{message.Replace("\n", " ")}*\n" +
                                 $"Server(Guild): {Context.Guild.Name}\n" +
                                 $"Server ID: {Context.Guild.Id}\n" +
                                 $"Channel: {$"<#{channel.Id}>"}\n" +
                                 $"Channel ID: {channel.Id}\n");
            }
            else await ReplyAsync("Oops, sorry. I can't create ads in a private message :/. Try asking me about it on the server.");
        }

        [Command("Info")]
        [Alias("INFO", "info")]
        public async Task Info()
        {
            if(Context.User.GlobalName == "Artem_IDA")
                await ReplyAsync(_services.GetRequiredService<MessageManagerService>().GetDebugInfo());
        }
    }
}
