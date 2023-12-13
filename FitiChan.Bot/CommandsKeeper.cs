using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;

namespace FitiChanBot
{
    public class CommandsKeeper
    {
        SlashCommandBuilders _jsonCommands;
        public CommandsKeeper(SlashCommandBuilders commands)
        {
            _jsonCommands = commands;
            
        }

        public async Task ExecuteAsync(SocketSlashCommand command)
        {
            Embed result;

            NotImplementedException CommandException = new NotImplementedException($"The user called a slash command, which I don't know how to process. Name:{command.CommandName}");
            NotImplementedException OptionsCountException = new NotImplementedException($"The user called a slash command, with a number of options that I don't know how to handle. Name: {command.CommandName}, Options Count: {command.Data.Options.Count}");

            switch (command.CommandName)
            {
                case "utc":
                    switch (command.Data.Options.Count)
                    {
                        case 0:
                            result = Time();
                            await command.RespondAsync(embed: result);
                            break;
                        case 1:
                            int utcOffset = Convert.ToInt32(command.Data.Options.Where(x => x.Name == "offset").First().Value);
                            result = Time(utcOffset);
                            await command.RespondAsync(embed: result);
                            break;
                        default: throw OptionsCountException;
                    }
                    break;
                case"user":
                    switch (command.IsDMInteraction)
                    {
                        case true:
                            result = UserInfo(command.User);
                            await command.RespondAsync(embed: result);
                            break;
                        case false:
                            switch (command.Data.Options.Count)
                            {
                                case 0:
                                    result = UserInfo(command.User);
                                    await command.RespondAsync(embed: result);
                                    break;
                                case 1:
                                    SocketUser targetUser = (command.Data.Options.Where(x => x.Name == "user").First().Value as SocketUser);
                                    result = UserInfo(command.User, targetUser);
                                    await command.RespondAsync(embed: result);
                                    break;
                                default: throw OptionsCountException;
                            }
                            break;
                    }
                    break;
                default: throw CommandException;
            }
        }

        //Commands Entities

        public Embed UserInfo(SocketUser executorUser, SocketUser targetUser = null)
        {
            if (targetUser == null) targetUser = executorUser;            // The user about whom we will display information(if there is no target user, it will display information about the command caller).
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

                return embed.Build();
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

                return embed.Build();
            }
        }
        private Embed Time(int utcOffset = 0)
        {
            EmbedFieldBuilder timeField;
            switch (utcOffset)
            {
                case > 0:
                    timeField = new EmbedFieldBuilder()
                    {
                        Name = $"UTC +{utcOffset}:00",
                        Value = $"{(DateTime.UtcNow + new TimeSpan(utcOffset, 0, 0)).ToShortTimeString()}"
                    };
                    break;
                case < 0:
                    timeField = new EmbedFieldBuilder()
                    {
                        Name = $"UTC {utcOffset}:00",
                        Value = $"{(DateTime.UtcNow + new TimeSpan(utcOffset, 0, 0)).ToShortTimeString()}"
                    };
                    break;
                default:
                    timeField = new EmbedFieldBuilder()
                    {
                        Name = $"UTC",
                        Value = $"{DateTime.UtcNow.ToShortTimeString()}"
                    }; break;
            }

            EmbedBuilder resultEmbedBuilder = new EmbedBuilder()
                .WithColor(Color.Blue)
                .AddField(timeField);

            return resultEmbedBuilder.Build();
        }
    }
}
