using System;
using System.Collections.Generic;
using Ticker.Events;

namespace Ticker
{
    public class TickerBuilder
    {
        private Ticker _ticker;
        private readonly ICollection<Action<TrackChanged<ITrack>>> _actions;
        private TimeSpan _precision = TimeSpan.FromSeconds(1);

        public TickerBuilder()
        {
            _actions = new List<Action<TrackChanged<ITrack>>>();
        }

        public Ticker Build()
        {
            _ticker = new Ticker(_precision);
            _ticker.TrackChanged += (sender, changed) =>
            {
                foreach (var action in _actions) action(changed);
            };
            return _ticker;
        }

        public TickerBuilder OnTrackChanged(Action<TrackChanged<ITrack>> action)
        {
            _actions.Add(action);
            return this;
        }

        public TickerBuilder Precision(TimeSpan timeSpan)
        {
            _precision = timeSpan;
            return this;
        }
    }
}