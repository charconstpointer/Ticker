using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Ticker.Events;

namespace Ticker
{
    public class Ticker
    {
        private readonly ConcurrentDictionary<string, Playlist<ITrack>> _playlists;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly Thread _watcher;
        private readonly TimeSpan _tickDelay;


        public Ticker()
        {
            _watcher = new Thread(OnTick) {IsBackground = true};
            _playlists = new ConcurrentDictionary<string, Playlist<ITrack>>();
            _tickDelay = TimeSpan.FromSeconds(1);
        }


        public Ticker(TimeSpan timeSpan)
        {
            _watcher = new Thread(OnTick) {IsBackground = true};
            _playlists = new ConcurrentDictionary<string, Playlist<ITrack>>();
            _tickDelay = timeSpan;
        }

        public Playlist<ITrack> this[string index] => _playlists[index];
        public event EventHandler<TrackChanged<ITrack>> TrackChanged;
        public event EventHandler<PlaylistEnded> PlaylistEnded;

        private void OnTick()
        {
            while (true)
            {
                foreach (var playlistsKey in _playlists.Keys)
                {
                    if (!_playlists.TryGetValue(playlistsKey, out var channel)) continue;

                    var isOutdated = channel.Current()?.Stop < DateTime.UtcNow.AddHours(2);
                    if (!isOutdated) continue;
                    if (!channel.TryGetNext(out _))
                    {
                        OnPlaylistEnded(channel.Title);
                        _playlists.Remove(playlistsKey, out _);
                    }
                    else
                    {
                        channel.PopTrack();
                        var current = channel.Current();
                        channel.TryGetNext(out var next);
                        OnTrackChanged(channel.Title, current, next);
                    }
                }

                Thread.Sleep(_tickDelay);
            }

            // ReSharper disable once FunctionNeverReturns
        }

        public void Start()
        {
            _watcher.Start();
        }

        public void AddChannel(string key, IEnumerable<ITrack> tracks)
        {
            if (_playlists.TryGetValue(key, out var channel)) return;
            channel = new Playlist<ITrack>(key);
            channel.AddTracks(tracks);
            _playlists[key] = channel;
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

        protected virtual void OnPlaylistEnded(string playlist)
        {
            PlaylistEnded?.Invoke(this, new PlaylistEnded
            {
                PlaylistName = playlist
            });
        }
    }
}