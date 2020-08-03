using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ticker.Tests.Mocks.Tracks;
using Xunit;

namespace Ticker.Tests
{
    public class TickerTests
    {
        [Theory]
        [InlineData("mojepolskie.1")]
        [InlineData("polskieradio.1")]
        public void Ticker_AddChannel(string key)
        {
            var ticker = new Ticker();
            var tracks = new List<ExampleTrack>
            {
                new ExampleTrack {Start = DateTime.Now.AddSeconds(-1), Stop = DateTime.Now.AddSeconds(3), Title = "1"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(3), Stop = DateTime.Now.AddSeconds(5), Title = "2"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(5), Stop = DateTime.Now.AddSeconds(7), Title = "3"},
            };
            ticker.AddChannel(key, tracks);
            ticker.AddChannel(key, tracks);
        }

        [Theory]
        [InlineData("mojepolskie.1")]
        public void Ticker_Current(string key)
        {
            var ticker = new Ticker();
            var tracks = new List<ExampleTrack>
            {
                new ExampleTrack {Start = DateTime.Now.AddSeconds(-1), Stop = DateTime.Now.AddSeconds(3), Title = "1"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(3), Stop = DateTime.Now.AddSeconds(5), Title = "2"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(5), Stop = DateTime.Now.AddSeconds(7), Title = "3"},
            };
            ticker.AddChannel(key, tracks);
            var playlist = ticker[key];
            playlist.Should().NotBeNull();
            playlist.Title.Should().Be(key);
            ((ExampleTrack) playlist.Current()).Title.Should().Be("1");
        }


        [Fact]
        public void Ticker_Next()
        {
            const string key = "ticker";
            var ticker = new Ticker();
            var tracks = new List<ExampleTrack>
            {
                new ExampleTrack {Start = DateTime.Now.AddSeconds(-1), Stop = DateTime.Now.AddSeconds(3), Title = "1"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(3), Stop = DateTime.Now.AddSeconds(5), Title = "2"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(5), Stop = DateTime.Now.AddSeconds(7), Title = "3"},
            };
            ticker.AddChannel(key, tracks);
            var playlist = ticker[key];
            ((ExampleTrack) playlist.Next()).Title.Should().Be("2");
        }

        [Fact]
        public async Task Ticker_OnTick()
        {
            const string key = "ticker";
            var ticker = new Ticker();
            var tracks = new List<ExampleTrack>
            {
                new ExampleTrack {Start = DateTime.Now.AddSeconds(-1), Stop = DateTime.Now.AddSeconds(3), Title = "1"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(3), Stop = DateTime.Now.AddSeconds(5), Title = "2"},
                new ExampleTrack {Start = DateTime.Now.AddSeconds(5), Stop = DateTime.Now.AddSeconds(7), Title = "3"},
            };
            var outcome = new List<ExampleTrack>();
            ticker.AddChannel(key, tracks);
            ticker.TrackChanged += (sender, changed) => outcome.Add(changed.Current as ExampleTrack);
            while (outcome.Count != 2)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            string.Join("", outcome.Select(x => x.Title)).Should().Be("23");
        }
    }
}