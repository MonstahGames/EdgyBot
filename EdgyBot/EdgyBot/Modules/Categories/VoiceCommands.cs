﻿using System.Threading.Tasks;
using EdgyCore.Services;
using Discord;
using Discord.Commands;

namespace EdgyBot.Modules.Categories
{
    //Disabling Voice Commands for now, doesn't even worek.
    //[Name("Voice Commands"), Summary("Music Commands!")]
    public class VoiceCommands : ModuleBase<ShardedCommandContext>
    {
        private readonly AudioService _service;

        public VoiceCommands (AudioService service)
        {
            _service = service;
        }

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinCmd ()
        {
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
            await ReplyAsync($"Joined Voice on {Context.Guild.Id}");
        }
        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveCmd ()
        {
            await _service.LeaveAudio(Context.Guild);
            await ReplyAsync($"Left Voice on {Context.Guild.Id}");
        }
        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayCmd ([Remainder]string link)
        {
            await _service.SendYTAudioAsync(Context.Guild, Context.Channel, link);
            await Context.Message.AddReactionAsync(new Emoji("👍"));
        }
    }
}
