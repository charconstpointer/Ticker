namespace Ticker.Events
{
    public class TrackChanged<T> where T : ITrack
    {
        public string Channel { get; set; }
        public T Current { get; set; }
        public T Next { get; set; }
    }
}