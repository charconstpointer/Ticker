using System;
using System.Collections.Generic;
using System.Linq;

namespace Ticker
{
    public class Playlist<T> where T : ITrack
    {
        public string Title => _title;
        private readonly Guid _id;
        private readonly string _title;
        private readonly Queue<T> _tracks;
        private T _current;

        public Playlist(string title = null)
        {
            _id = Guid.NewGuid();
            _title = title ?? _id.ToString();
            _tracks = new Queue<T>();
        }

        public T Current()
        {
            return _current;
        }

        public T Next()
        {
            return _tracks.TryPeek(out var next) ? next : default;

            // throw new ApplicationException($"Playlist #{_id} #{_title} is empty");
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
            foreach (var track in notYetPlayed)
            {
                _tracks.Enqueue(track);
            }

            if (_current is null && _tracks.TryDequeue(out var current))
            {
                _current = current;
            }
        }

        public void PopTrack()
        {
            if (TryGetNext(out _))
            {
                _current = _tracks.Dequeue();
            }
        }
    }
}