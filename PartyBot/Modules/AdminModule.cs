using Discord.Commands;
using Discord.WebSocket;
using PartyBot.Services;
using System.Linq;
using System.Threading.Tasks;

namespace PartyBot.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        public BotService BotService { get; set; }
        public AudioService AudioService { get; }

        [Command("Info")]
        public async Task Info()
            => await ReplyAsync("", false, await BotService.DisplayInfoAsync(Context));

        [Command("Prefix")]
        public async Task Prefix()
        {
            var user = (SocketGuildUser)Context.User;
            var gotRole = from a in user.Roles
                          where a.Name == "Role Name Here"
                          select a;
            bool check = gotRole == null ? true : false;
            await ReplyAsync($"{check}");
        }
    }
}
