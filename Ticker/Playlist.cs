using System;
using System.Collections.Generic;
using System.Linq;

namespace Ticker
{
    public class Playlist<T> where T : class, ITrack
    {
        private readonly Queue<T> _tracks;
        private T _current;

        public Playlist(string title = null)
        {
            Title = title ?? Guid.NewGuid().ToString();
            _tracks = new Queue<T>();
        }

        public string Title { get; }

        public T Current()
        {
            return _current;
        }

        public TR Current<TR>() where TR : class, ITrack
        {
            return _current as TR;
        }

        public T Next()
        {
            return _tracks.TryPeek(out var next) ? next : default;
        }

        public bool TryGetNext(out T track)
        {
            if (_tracks.TryPeek(out var next))
            {
                track = next;
                return true;
            }

            track = default;
            return false;
        }

        public void AddTracks(IEnumerable<T> tracks)
        {
            var notYetPlayed = tracks.Where(track => track.Stop >= DateTime.UtcNow.AddHours(2));
            foreach (var track in notYetPlayed) _tracks.Enqueue(track);

            if (_current is null && _tracks.TryDequeue(out var current)) _current = current;
        }

        public void PopTrack()
        {
            if (TryGetNext(out _)) _current = _tracks.Dequeue();
        }
    }
}