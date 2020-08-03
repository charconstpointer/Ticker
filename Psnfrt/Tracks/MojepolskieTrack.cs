using System;

namespace Psnfrt.Tracks
{
    public class MojepolskieTrack : ITrack
    {
        public int Id { get; }
        public int ChannelId { get; }
        public string Title { get; }
        public string Artist { get; }
        public DateTime Start { get; }
        public DateTime Stop { get; }

        public MojepolskieTrack(int id, int channelId, string title, string artist, DateTime start, DateTime stop)
        {
            Id = id;
            Title = title;
            Artist = artist;
            Start = start;
            Stop = stop;
            ChannelId = channelId;
        }

        public override string ToString()
        {
            return $"{Title} {Artist}";
        }
    }
}