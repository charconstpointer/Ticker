using System;
using System.Collections.Generic;
using FluentAssertions;
using Ticker.Tests.Mocks.Tracks;
using Xunit;

namespace Ticker.Tests
{
    public class PlaylistTests
    {
        [Fact]
        public void Playlist_AddTracks()
        {
            var playlist = new Playlist<ExampleTrack>("#1");
            var tracks = new List<ExampleTrack>
            {
                new ExampleTrack {Start = DateTime.Now.AddSeconds(-1), Stop = DateTime.Now.AddSeconds(3), Title = "1"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(3), Stop = DateTime.Now.AddSeconds(5), Title = "2"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(5), Stop = DateTime.Now.AddSeconds(7), Title = "3"},
            };
            playlist.AddTracks(tracks);
            playlist.Title.Should().Be("#1");
        }

        [Fact]
        public void Playlist_Next_ReturnsNextTrack()
        {
            var playlist = new Playlist<ExampleTrack>("#1");
            var tracks = new List<ExampleTrack>
            {
                new ExampleTrack {Start = DateTime.Now.AddSeconds(-1), Stop = DateTime.Now.AddSeconds(3), Title = "1"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(3), Stop = DateTime.Now.AddSeconds(5), Title = "2"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(5), Stop = DateTime.Now.AddSeconds(7), Title = "3"},
            };
            playlist.AddTracks(tracks);
            playlist.Next().Should().NotBeNull();
        }

        [Fact]
        public void Playlist_Next_Throws()
        {
            var playlist = new Playlist<ExampleTrack>("#1");
            var tracks = new List<ExampleTrack>
            {
                new ExampleTrack {Start = DateTime.Now.AddSeconds(-1), Stop = DateTime.Now.AddSeconds(3), Title = "1"}
            };
            playlist.AddTracks(tracks);
            Assert.Throws<ApplicationException>(() => playlist.Next());
        }

        [Fact]
        public void Playlist_ChangeTrack()
        {
            var playlist = new Playlist<ExampleTrack>("#1");
            var tracks = new List<ExampleTrack>
            {
                new ExampleTrack {Start = DateTime.Now.AddSeconds(-1), Stop = DateTime.Now.AddSeconds(3), Title = "1"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(3), Stop = DateTime.Now.AddSeconds(5), Title = "2"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(5), Stop = DateTime.Now.AddSeconds(7), Title = "3"},
            };
            playlist.AddTracks(tracks);
            playlist.Current().Title.Should().Be("1");
            playlist.PopTrack();
            playlist.Current().Title.Should().Be("2");
            playlist.PopTrack();
            playlist.Current().Title.Should().Be("3");
            playlist.PopTrack();
            playlist.Current().Title.Should().Be("3");
        }
    }
}