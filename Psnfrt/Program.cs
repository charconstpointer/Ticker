using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Psnfrt.Tracks;

namespace Psnfrt
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var playlist = new Playlist<MojepolskieTrack>();
            var client = new MojepolskieClient(new HttpClient());
            var tracks = await client.Get(1);
            playlist.AddSchedule(tracks);
            await playlist.Start(CancellationToken.None);
        }
    }
}