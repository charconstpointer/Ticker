using System;

namespace Psnfrt
{
    public class ExampleTrack : ITrack
    {
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}