﻿using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord;
using Discord.Commands;
using Discord.Addons.Interactive;
using EdgyBot.Core;
using EdgyBot.Core.Lib;
using EdgyBot.Database;

namespace EdgyBot.Modules
{
    public class OwnerCommands : InteractiveBase<EbShardContext>
    {
        private readonly LibEdgyBot _lib = new LibEdgyBot();

        [Command("ch"), Alias("checkup", "check", "status")]
        public async Task CheckupCmd ()
            => await ReplyAsync($"```json\n{JsonConvert.SerializeObject(Core.Handler.EventHandler.CurrentStats, Formatting.Indented)}\n```");
        

        [Command("setstatus"), RequireOwner]
        public async Task SetStatusCmd([Remainder]string input = null)
        {
            if (input == "default") {
                Core.Handler.EventHandler.StatusIsCustom = false;
                await Context.Client.SetGameAsync("e!help | EdgyBot for " + Context.Client.Guilds.Count + " servers!");
                await ReplyAsync("Changed Status. **Custom Param: " + input + "**");

                return;
            }

            Core.Handler.EventHandler.StatusIsCustom = true;
            await Context.Client.SetGameAsync(input);
            await ReplyAsync("Changed Status.");
        }

        [Command("listservers", RunMode = RunMode.Async)]
        public async Task ListServersCmd ()
        {
            await ReplyAsync("ok my dude");

            StringBuilder sb = new StringBuilder();
            foreach (IGuild guild in Context.Client.Guilds)
            {
                if (guild == null)
                    continue;

                sb.Append(guild.Name + ':');
            }
            string[] pageStr = (sb.ToString()).Split(':');

            PaginatedMessage.Page[] pages = new PaginatedMessage.Page[pageStr.Length];
            for (int x = 0; x < pageStr.Length; x++)
            {
                pages[x] = new PaginatedMessage.Page
                {
                    Description = pageStr[x]
                };
            }

            await PagedReplyAsync(new PaginatedMessage(pages));
        }

        [Command("execquery")]
        [RequireOwner]
        public async Task ExecQueryCmd (string type, [Remainder]string query)
        {
            DatabaseConnection connection = new DatabaseConnection();
            await connection.ConnectAsync();

            if (connection.OpenConnection())
            {
                if (type == "regular")
                {
                    try
                    {
                        SQLProcessor sql = new SQLProcessor(connection.getConnObj());
                        await sql.ExecuteQueryAsync(query);
                    }
                    catch (Exception e)
                    {
                        await ReplyAsync("Error: " + e.Message);
                    }
                } else
                {
                    if (type != "reader")
                    {
                        await ReplyAsync("Invalid Operation Type dumbfuck monstah . regular or reader");
                        return;
                    }

                    try
                    {
                        SQLProcessor sql = new SQLProcessor(connection.getConnObj());
                        string result = sql.ReadAsync(query);
                        await ReplyAsync(result);
                    } catch (Exception e)
                    {
                        await ReplyAsync("Error: " + e.Message);
                    }
                }

                return;
            }
        }
    }
}
