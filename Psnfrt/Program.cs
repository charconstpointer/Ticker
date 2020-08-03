using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticker;

namespace Psnfrt
{
    class Program
    {
        private static async Task Main()
        {
            const string key = "ticker";
            var ticker = new Ticker.Ticker();
            var tracks = new List<ExampleTrack>
            {
                new ExampleTrack {Start = DateTime.Now.AddSeconds(-1), Stop = DateTime.Now.AddSeconds(3), Title = "1"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(3), Stop = DateTime.Now.AddSeconds(5), Title = "2"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(5), Stop = DateTime.Now.AddSeconds(7), Title = "3"},
            };
            ticker.AddChannel(key, tracks);
            ticker.TrackChanged += OnTickerOnTrackChanged;
            Console.ReadKey();
        }

        private static void OnTickerOnTrackChanged(object? sender, TrackChanged<ITrack> changed)
        {
            Console.WriteLine($"Channel : {changed.Channel} {Environment.NewLine} " +
                              $"Current track : {((ExampleTrack) changed.Current).Title} {Environment.NewLine}" +
                              $"Next track : {((ExampleTrack) changed.Next)?.Title ?? "🙊"} {Environment.NewLine}");
        }
    }
}