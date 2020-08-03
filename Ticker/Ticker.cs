using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Ticker
{
    public class Ticker
    {
        private readonly ConcurrentDictionary<string, Playlist<ITrack>> _playlists;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly Thread _watcher;
        public Playlist<ITrack> this[string index] => _playlists[index];
        public event EventHandler<TrackChanged<ITrack>> TrackChanged;


        public Ticker()
        {
            _watcher = new Thread(OnTick) {IsBackground = true};
            _playlists = new ConcurrentDictionary<string, Playlist<ITrack>>();
            _watcher.Start();
        }

        private void OnTick()
        {
            while (true)
            {
                foreach (var playlistsKey in _playlists.Keys)
                {
                    if (!_playlists.TryGetValue(playlistsKey, out var channel))
                    {
                        continue;
                    }

                    var isOutdated = channel.Current()?.Start < DateTime.UtcNow.AddHours(2);
                    if (!isOutdated) continue;
                    if (!channel.TryGetNext(out _)) continue;
                    channel.PopTrack();
                    var current = channel.Current();
                    channel.TryGetNext(out var next);
                    OnTrackChanged(channel.Title, current, next);
                }
                // foreach (var (channelId, playlist) in _playlists.Where(c =>
                //     c.Value?.Current()?.Stop < DateTime.UtcNow.AddHours(2)))
                // {
                //     if(playlist is null) continue;
                //     if (!playlist.TryGetNext(out _)) continue;
                //     playlist.PopTrack();
                //     var current = playlist.Current();
                //     playlist.TryGetNext(out var next);
                //     OnTrackChanged(channelId, current, next);
                // }

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