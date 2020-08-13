using System;
using Ticker;

namespace Psnfrt.Tracks
{
    public class MojepolskieTrack : ITrack
    {
        public MojepolskieTrack(int id, int channelId, string title, string artist, DateTime start, DateTime stop)
        {
            Id = id;
            Title = title;
            Artist = artist;
            Start = start;
            Stop = stop;
            ChannelId = channelId;
        }

        public int Id { get; }
        public int ChannelId { get; }
        public string Title { get; }
        public string Artist { get; }
        public DateTime Start { get; }
        public DateTime Stop { get; }

        public override string ToString()
        {
            return $"{Title} {Artist}";
        }
    }
}