﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EdgyBot.Core.Lib;

namespace EdgyBot.Core.Handler.API
{
    public class DiscordBotsPinger
    {
        private JsonHelper helper = new JsonHelper("https://bots.discord.pw/api/bots/" + Bot.Credentials.clientID + "/stats");
        private LibEdgyBot _lib = new LibEdgyBot();

        public async Task PostServerCountAsync (int serverCount)
        {
            var dict = new Dictionary<string, object>();
            dict.Add("server_count", serverCount);

            try { helper.postBotlist(dict, Botlist.DISCORDBOTS); } catch (Exception e)
            {
                await _lib.EdgyLog(Discord.LogSeverity.Error, "Failed to post data to Discord Bots:\n" + e.Message);
            }
        }
    }
}
