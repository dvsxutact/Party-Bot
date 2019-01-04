using Discord.Commands;
using Discord.WebSocket;
using PartyBot.Services;
using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace PartyBot.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        public BotService BotService { get; set; }

        [Command("Info")]
        public async Task Info()
            => await ReplyAsync("", false, await BotService.DisplayInfoAsync(Context));
    }
}
