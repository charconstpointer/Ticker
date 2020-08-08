﻿using System;
using System.Collections.Generic;

namespace Ticker
{
    public class TickerBuilder
    {
        private ICollection<Action<TrackChanged<ITrack>>> _actions;
        private readonly Ticker _ticker;

        public TickerBuilder()
        {
            _ticker = new Ticker();
            _actions = new List<Action<TrackChanged<ITrack>>>();
        }

        public Ticker Build()
        {
            _ticker.TrackChanged += (sender, changed) =>
            {
                foreach (var action in _actions)
                {
                    action(changed);
                }
            };
            return _ticker;
        }

        public TickerBuilder OnTrackChanged(Action<TrackChanged<ITrack>> action)
        {
            _actions.Add(action);
            return this;
        }
    }
}