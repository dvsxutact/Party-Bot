using Discord.Commands;
using Discord.WebSocket;
using PartyBot.Services;
using System.Threading.Tasks;

namespace PartyBot.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        /* Get our AudioService from DI */
        private AudioService AudioService { get; set; }

        /* All the below commands are ran via Lambda Expressions to keep this file as neat and closed off as possible. 
              We pass the AudioService Task into the section that would normally require an Embed as that's what all the
              AudioService Tasks are returning. */

        [Command("Join")]//[Help("Join", "Requests the bot joins a voice channel.", "Join")]
        public async Task JoinAndPlay()
            => await ReplyAsync("", false, await AudioService.JoinOrPlayAsync((SocketGuildUser)Context.User, Context.Channel, Context.Guild.Id));

        [Command("Leave")]
        public async Task Leave()
            => await ReplyAsync("", false, await AudioService.LeaveAsync(Context.Guild.Id));

        [Command("Play")]
        public async Task Play([Remainder]string search)
            => await ReplyAsync("", false, await AudioService.JoinOrPlayAsync((SocketGuildUser)Context.User, Context.Channel, Context.Guild.Id, search));

        [Command("Stop")]
        public async Task Stop()
            => await ReplyAsync("", false, await AudioService.StopAsync(Context.Guild.Id));

        [Command("List")]
        public async Task List()
            => await ReplyAsync("", false, await AudioService.ListAsync(Context.Guild.Id));

        [Command("Skip")]
        public async Task Delist(string id = null)
            => await ReplyAsync("", false, await AudioService.SkipTrackAsync(Context.Guild.Id));

        [Command("Volume")]
        public async Task Volume(int volume)
            => await ReplyAsync(await AudioService.VolumeAsync(Context.Guild.Id, volume));

        [Command("Pause")]
        public async Task Pause()
            => await ReplyAsync(await AudioService.PauseAsync(Context.Guild.Id));

        [Command("Resume")]
        public async Task Resume()
            => await ReplyAsync(await AudioService.PauseAsync(Context.Guild.Id));

        [Command("Lyrics")]
        public async Task Lyrics()
        {
            var lyrics = await AudioService.GetLyricsAsync(Context.Guild.Id);
            string part1 = null, part2 = null;
            if (lyrics.Length >= 2000)
            {
                part1 = lyrics.Remove(2000);
                part2 = lyrics.Substring(2000);
            }

            await ReplyAsync(part1);
            await ReplyAsync(part2);
        }
    }
}
