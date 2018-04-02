﻿using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using Discord.Commands;
using System;
//using DiscordBotsList.Api;

namespace EdgyBot
{
    public class EventHandler : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordSocketClient _client;
        private readonly LibEdgyBot _lib = new LibEdgyBot();
        public static SocketUser OwnerUser;
        //public static AuthDiscordBotListApi DBLApi;
        //public static DiscordBotListApi DBLApi;

        public EventHandler(DiscordSocketClient client)
        {
            _client = client;

            InstallDBLApi();
            InitEvents();
        }

        private void InstallDBLApi()
        {
            //AuthDiscordBotListApi api = new AuthDiscordBotListApi(_lib.GetBotId(), _lib.getDBLToken());
            //DiscordBotListApi api = new DiscordBotListApi();
            //DBLApi = api;
        }

        private void InitEvents()
        {
            _client.Log += _lib.Log;
            _client.Ready += Ready;
            _client.JoinedGuild += Client_JoinedGuild;
            _client.Disconnected += Client_Disconnected;
        }

        private async Task Client_Disconnected(Exception exception)
        {
            await _lib.EdgyLog(LogSeverity.Critical, "EDGYBOT HAS CRASHED WITH AN EXCEPTION, \n" + exception.Message);
        }

        public async Task Ready()
        {
            OwnerUser = _client.GetUser(_lib.GetOwnerID());

            string gameStatus = "e!help | EdgyBot for " + _client.Guilds.Count + " servers!";
            await _client.SetGameAsync(gameStatus);
            await _lib.EdgyLog(LogSeverity.Info, "Set game to " + gameStatus);
        }

        private static async Task Client_JoinedGuild(SocketGuild guild)
        {
            LibEdgyBot lib = new LibEdgyBot();
            await guild.DefaultChannel.SendMessageAsync($"SH*T THANKS FOR INVITING ME M8'S, TO SEE ME COMMANDS, USE **{lib.GetPrefix()}help**.");
        }

        //private async Task UserLeft (SocketGuildUser user)
        //{
        //    IDMChannel dm = await user.GetOrCreateDMChannelAsync();
        //    Embed e = _lib.CreateEmbedWithText("EdgyBot", "We hope you enjoyed your stay, " + user.Username + "!");
        //    await dm.SendMessageAsync("", embed: e);
        //}
    }
}