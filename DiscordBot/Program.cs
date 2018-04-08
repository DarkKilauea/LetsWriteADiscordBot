using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordBot
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            var client = new DiscordSocketClient();
            client.Log += message =>
            {
                Console.WriteLine(message);
                return Task.CompletedTask;
            };
            
            client.MessageReceived += async message =>
            {
                if (message.Content == "!ping")
                    await message.Channel.SendMessageAsync("Pong!");
            };

            var token = File.ReadAllText("token.txt");
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }
    }
}
