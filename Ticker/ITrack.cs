using System;

namespace Ticker
{
    public interface ITrack
    {
        public DateTime Start { get; }
        public DateTime Stop { get; }
    }
}