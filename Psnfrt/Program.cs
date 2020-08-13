using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Psnfrt.Tracks;
using Ticker;
using Ticker.Events;

namespace Psnfrt
{
    internal class Program
    {
        private static async Task Main()
        {
            var client = new MojepolskieClient(new HttpClient());
            var builder = new TickerBuilder();
            var ticker = builder
                .OnTrackChanged(Console.WriteLine)
                .OnTrackChanged(Console.WriteLine)
                .OnTrackChanged(async e => await OnChanged(e))
                .Precision(TimeSpan.FromSeconds(1))
                .Build();
            ticker.Start();

            foreach (var i in Enumerable.Range(1, 100))
            {
                var tracks = (await client.Get(i))?.ToList();
            }

            Console.ReadKey();
        }

        private static async Task OnChanged(TrackChanged<ITrack> changed)
        {
            Console.WriteLine("async");
        }

        private static void TickerOnPlaylistEnded(object? sender, PlaylistEnded e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{e.PlaylistName} has ended");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void OnTickerOnTrackChanged(object? sender, TrackChanged<ITrack> changed)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"Channel : {changed.Channel} {Environment.NewLine}" +
                              $"Current track : {((MojepolskieTrack) changed.Current).Title} {Environment.NewLine}" +
                              $"Next track : {((MojepolskieTrack) changed.Next)?.Title ?? "🙊"} {Environment.NewLine}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}