using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

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

            var serviceProvider = new ServiceCollection()
                .BuildServiceProvider();
            var commandService = new CommandService();
            await commandService.AddModulesAsync(Assembly.GetEntryAssembly());
            
            client.MessageReceived += async rawMessage =>
            {
                if (!(rawMessage is SocketUserMessage message)) 
                    return;
                
                var argPos = 0;
                if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) 
                    return;
                
                var context = new CommandContext(client, message);
                var result = await commandService.ExecuteAsync(context, argPos, serviceProvider);
                if (!result.IsSuccess)
                    Console.WriteLine("Failed to run command: {0}", result.ErrorReason);
            };

            var token = File.ReadAllText("token.txt");
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }
    }
}
