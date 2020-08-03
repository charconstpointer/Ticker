using System;

namespace Psnfrt
{
    public interface ITrack
    {
        public DateTime Start { get; }
        public DateTime Stop { get; }
    }
}