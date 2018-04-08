using System;
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
        
        [Command("roll"), Summary("Rolls the specified sided dice")]
        public async Task Roll([Summary("Number of sides the dice should have")]
            int numberOfSides)
        {
            var random = new Random();

            await ReplyAsync($"You rolled a {random.Next(1, numberOfSides + 1)} on a {numberOfSides} sided die!");
        }
    }
}