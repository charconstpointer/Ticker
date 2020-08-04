using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Psnfrt.Tracks;
using Ticker;

namespace Psnfrt
{
    class Program
    {
        private static async Task Main()
        {
            var client = new MojepolskieClient(new HttpClient());
            foreach (var i in Enumerable.Range(4,1))
            {
                var tracks = (await client.Get(i))?.ToList();
                if (tracks is null || !tracks.Any()) continue;
                var ticker = new Ticker.Ticker();
                var key = $"mojepolskie.{i}";
                ticker.AddChannel(key, tracks);
                ticker.TrackChanged += OnTickerOnTrackChanged;
                ticker.PlaylistEnded += TickerOnPlaylistEnded;
                Console.WriteLine($"Channel : {i} {Environment.NewLine}" +
                                  $"Current track : {((MojepolskieTrack) ticker[key]?.Current())?.Title} {Environment.NewLine}" +
                                  $"Next track : {((MojepolskieTrack) ticker[key]?.Next())?.Title ?? "🙊"} {Environment.NewLine}");
               
            }
            
            Console.ReadKey();
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