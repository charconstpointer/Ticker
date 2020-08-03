using System;

namespace Ticker.Tests.Mocks.Tracks
{
    public class ExampleTrack : ITrack
    {
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
    }
}