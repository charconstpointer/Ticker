﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Psnfrt.Extensions;
using Psnfrt.Responses;
using Psnfrt.Tracks;

namespace Psnfrt
{
    public class MojepolskieClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _base;

        public MojepolskieClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _base = "https://moje.polskieradio.pl/api/";
        }

        public async Task<IEnumerable<MojepolskieTrack>> Get(int id)
        {
            var moje = await _httpClient.GetAsync(
                $"{_base}?mobilestationid={id}&key=d590cafd-31c0-4eef-b102-d88ee2341b1a");
            if (moje.IsSuccessStatusCode) return null;
            var mojePolskieResponse =
                JsonSerializer.Deserialize<MojepolskieResponse>(await moje.Content.ReadAsStringAsync());
            for (var i = 0; i < mojePolskieResponse.Songs.Count() - 1; i++)
            {
                var current = mojePolskieResponse.Songs.ElementAt(i);
                var next = mojePolskieResponse.Songs.ElementAt(i + 1);
                current.Stop = next.ScheduleTime;
            }

            var tracks = mojePolskieResponse.AsEntity();
            return tracks;
        }
    }
}