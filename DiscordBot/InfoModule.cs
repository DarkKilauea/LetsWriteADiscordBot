using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordBot
{
    public class InfoModule : ModuleBase
    {
        [Command("ping"), Summary("Returns a pong")]
        public async Task Ping()
        {
            await ReplyAsync("Pong!");
        }
    }
}