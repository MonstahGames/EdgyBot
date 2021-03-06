﻿using System;
using System.Threading.Tasks;
using System.Linq;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using EdgyBot.Core;
using EdgyBot.Core.Lib;
using System.Reflection;

namespace EdgyBot.Modules
{
    [Name("Info Commands"), Summary("Commands providing information about a certain thingy")]
    public class InfoCommands : ModuleBase<EbShardContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        private readonly LibEdgyCore _core = new LibEdgyCore();

        [Command("channelinfo")]
        [Name("channelinfo")]
        [Summary("Gives you info about the channel you are in")]
        public async Task ChannelInfoCmd (IChannel channel = null)
        {
            if (channel == null)
                channel = Context.Channel;
       
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            var users = await channel.GetUsersAsync().FlattenAsync();

            eb.AddField((string)Context.Language["info"]["chName"], channel.Name);
            eb.AddField((string)Context.Language["info"]["usrChannel"], users.Count());
            eb.AddField((string)Context.Language["info"]["chnlId"], channel.Id.ToString());
            eb.AddField((string)Context.Language["info"]["created"], channel.CreatedAt.ToString());

            await ReplyAsync("", embed: eb.Build());
        }

        [Command("userinfo"), Alias("whois")]
        [Name("userinfo")]
        [Summary("Mention an user or leave it empty for a description on the user")]
        public async Task UserInfoCmd(IGuildUser usr = null)
        {
            if (usr == null) usr = (IGuildUser)Context.Message.Author;
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults(true, Context.Message.Author.Username);
            SocketGuildUser guildUserMnt = (SocketGuildUser)usr;

            #region UserInfo
            string username = usr.Username;         
            string nickname = guildUserMnt.Nickname;
            string discriminator = usr.Discriminator;
            string userID = usr.Id.ToString();
            bool isBot = usr.IsBot;
            string status = usr.Status.ToString();
            string pfpUrl = usr.GetAvatarUrl();
            string playing = usr.Activity?.Name;
            string joinedAt = usr.JoinedAt.ToString();
            string createdOn = usr.CreatedAt.ToUniversalTime().ToString();
            GuildPermissions perms = usr.GuildPermissions;
            #endregion

            eb.ThumbnailUrl = pfpUrl;
            eb.AddField((string)Context.Language["info"]["username"], username, true);

            if (string.IsNullOrEmpty(nickname))
                eb.AddField((string)Context.Language["info"]["username"], (string)Context.Language["info"]["none"], true);
            else
                eb.AddField((string)Context.Language["info"]["nickname"], nickname, true);
            
            eb.AddField((string)Context.Language["info"]["discrim"], discriminator, true);
            eb.AddField((string)Context.Language["info"]["status"], status, true);
            eb.AddField((string)Context.Language["info"]["joined"], joinedAt, true);
            eb.AddField((string)Context.Language["info"]["created"], createdOn, true);

            if (string.IsNullOrEmpty(playing))
                eb.AddField((string)Context.Language["info"]["playing"], (string)Context.Language["info"]["none"], true);
            else
                eb.AddField((usr.Activity?.Type).ToString(), playing, true);

            eb.AddField((string)Context.Language["info"]["usrId"], userID, true);
            eb.AddField((string)Context.Language["info"]["isBot"], isBot.ToString(), true);
            eb.AddField((string)Context.Language["info"]["perms"], _lib.GetPermissionsString(perms));

            await ReplyAsync("", embed: eb.Build());
        }

        [Command("serverinfo")]
        [Name("serverinfo")]
        [Alias("guildinfo")]
        [Summary("Gives you info about the server you are in")]
        public async Task ServerInfoCmd()
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults(true, Context.User.Username);

            #region ServerInfo
            string name = Context.Guild.Name;
            string createdAt = Context.Guild.CreatedAt.ToString();
            string serverGuildIconUrl = Context.Guild.IconUrl;
            string serverId = Context.Guild.Id.ToString();
            string memberCount = Context.Guild.MemberCount.ToString();
            string emoteCount = Context.Guild.Emotes.Count.ToString();
            string roleCount = Context.Guild.Roles.Count.ToString();
            string categoriesCount = Context.Guild.CategoryChannels.Count.ToString();
            string channelCount = Context.Guild.Channels.Count.ToString();
            int shard = Context.Client.GetShardIdFor(Context.Guild);
            #endregion

            eb.ThumbnailUrl = serverGuildIconUrl;
            eb.AddField((string)Context.Language["info"]["guildName"], name);
            eb.AddField((string)Context.Language["info"]["guildId"], serverId);
            eb.AddField((string)Context.Language["info"]["memberCount"], memberCount);
            eb.AddField((string)Context.Language["info"]["emoteCount"], emoteCount);
            eb.AddField((string)Context.Language["info"]["roleCount"], roleCount);
            eb.AddField((string)Context.Language["info"]["catCount"], categoriesCount);
            eb.AddField((string)Context.Language["info"]["channelCount"], channelCount);
            eb.AddField((string)Context.Language["info"]["created"], createdAt);

            eb.WithFooter(new EmbedFooterBuilder
            {
                IconUrl = Context.Guild.IconUrl,
                Text = $"{Context.Guild.Name} in Shard {Context.Client.GetShardIdFor(Context.Guild)}"
            });

            await ReplyAsync("", embed: eb.Build());
        }

        [Command("ping")]
        [Name("ping")]
        [Summary("Checks the latency to the Discord API Gateway Server")]
        public async Task PingCmd()
        {
            EmbedFooterBuilder fb = new EmbedFooterBuilder
            {
                Text = $"Response Time: {Context.Client.Latency.ToString()}ms | {DateTime.Now.ToUniversalTime()} GMT"
            };
            Embed e = _lib.CreateEmbedWithText("Ping", "Pong! :ping_pong:", fb);

            await ReplyAsync("", embed: e);
        }

        [Command("uptime"), Alias("time", "up")]
        [Name("uptime"), Summary("Shows how long the bot has been running since the last restart")]
        public async Task UptimeCmd () 
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults();
            eb.AddField("Uptime", $":timer: {_lib.GetUptime()}");
            eb.WithFooter(new EmbedFooterBuilder 
            {
                Text = $"Current Time: {DateTime.UtcNow.ToShortTimeString()}, Shard {Context.Client.GetShardIdFor(Context.Guild)}"
            });
            
            await ReplyAsync("", embed: eb.Build());
        }

        [Command("info"), Alias("botinfo", "me", "stats", "about")]
        [Name("info"), Summary("Info about EdgyBot")]
        public async Task BotInfoCmd ()
        {
            EmbedBuilder eb = _lib.SetupEmbedWithDefaults(true, Context.User.Username);
            eb.WithThumbnailUrl("http://i0.kym-cdn.com/photos/images/original/001/256/183/9d5.png");

            eb.AddField("EdgyBot", "A multipurpose bot with a great variety of commands ranging from fun to well.. not so fun", true);
            eb.AddField("Version", Assembly.GetExecutingAssembly().GetName().Version);
            eb.AddField("Library", $"Discord.Net {DiscordConfig.Version}", true);
            eb.AddField("Server Count", Context.Client.Guilds.Count, true);
            eb.AddField("Total Users", Core.Handler.EventHandler.MemberCount, true);
            eb.AddField("Current Shard", Context.Client.GetShardIdFor(Context.Guild), true);
            eb.AddField("Total Shards", Context.Client.Shards.Count, true);
            eb.AddField("Commands Ran (This Session)", Core.Handler.CommandHandler.CommandsRan);
            eb.AddField("Uptime", _lib.GetUptime());
            eb.AddField("Status", Context.Client.Activity.Name);
            eb.AddField("Developer", _core.GetOwnerDiscordName());

            await ReplyAsync("", embed: eb.Build());
        }
    }
}