using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;

namespace DiscordBot
{
    public class MusicModule : ModuleBase
    {
        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayInChannel(IVoiceChannel channel = null)
        {
            // Get the audio channel
            channel = channel ?? (Context.Message.Author as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await ReplyAsync("User must be in a voice channel, or a voice channel must be passed as an argument."); 
                return;
            }

            // For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
            using (var audioClient = await channel.ConnectAsync())
            {
                await SendAsync(audioClient, "Pump.mp3");
            }
        }
        
        private static Process CreateStream(string path)
        {
            var ffmpeg = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i {path} -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            
            return Process.Start(ffmpeg);
        }
        
        private static async Task SendAsync(IAudioClient client, string path)
        {
            // Create FFmpeg using the previous example
            var ffmpeg = CreateStream(path);
            var output = ffmpeg.StandardOutput.BaseStream;
            var discord = client.CreatePCMStream(AudioApplication.Music);
            await output.CopyToAsync(discord);
            await discord.FlushAsync();
        }
    }
}