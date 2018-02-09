﻿using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgyBot.Modules.Categories
{
    public class XPCommands : ModuleBase<SocketCommandContext>
    {
        private Database database = new Database();

        [Command("xp")][Name("xp")][Summary("[MENTION] (Optional), Shows your xp.")]
        public async Task XPCheckCommand (SocketGuildUser usr = null)
        {
            if (usr == null) usr = (SocketGuildUser)Context.Message.Author;
            if (!database.DoesUserExist(usr.Id)) database.InsertUser(usr.Id, usr.Username);
        }
    }
}
