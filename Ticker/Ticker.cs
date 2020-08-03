using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Ticker
{
    public class Ticker
    {
        private readonly Dictionary<string, Playlist<ITrack>> _playlists;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly Thread _watcher;
        public Playlist<ITrack> this[string index] => _playlists[index];
        public event EventHandler<TrackChanged<ITrack>> TrackChanged;


        public Ticker()
        {
            _watcher = new Thread(OnTick) {IsBackground = true};
            _watcher.Start();
            _playlists = new Dictionary<string, Playlist<ITrack>>();
        }

        private void OnTick()
        {
            while (true)
            {
                foreach (var (channelId, playlist) in _playlists.Where(c =>
                    c.Value.Current().Stop < DateTime.UtcNow.AddHours(2)))
                {
                    if (!playlist.TryGetNext(out _)) continue;
                    playlist.PopTrack();
                    var current = playlist.Current();
                    playlist.TryGetNext(out var next);
                    OnTrackChanged(channelId, current, next);
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            // ReSharper disable once FunctionNeverReturns
        }

        private void OnTrackChanged(string channel, ITrack current, ITrack next)
        {
            TrackChanged?.Invoke(this, new TrackChanged<ITrack>
            {
                Channel = channel,
                Current = current,
                Next = next
            });
        }

        public void AddChannel(string key, IEnumerable<ITrack> tracks)
        {
            if (_playlists.TryGetValue(key, out var channel)) return;
            channel = new Playlist<ITrack>(key);
            channel.AddTracks(tracks);
            _playlists[key] = channel;
        }
    }
}