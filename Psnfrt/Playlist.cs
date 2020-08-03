using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Psnfrt
{
    public class Playlist<T> where T : ITrack
    {
        private readonly Queue<T> _tracksQueue;
        private T _currentTrack;

        public T CurrentTrack => _currentTrack;

        public T NextTrack()
        {
            return _tracksQueue.TryPeek(out var next) ? next : default;
        }

        public Playlist()
        {
            _tracksQueue = new Queue<T>();
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_currentTrack == null && _tracksQueue.Any())
                {
                    _currentTrack = _tracksQueue.Dequeue();
                    Console.WriteLine($"CT : {CurrentTrack} NT : {NextTrack()}");
                    continue;
                }

                if (NextTrack() is null)
                {
                    Console.WriteLine("Koniec");
                    return;
                }
                
                Console.WriteLine($"CT : {CurrentTrack} Stop : {CurrentTrack.Stop} NT : {NextTrack()} Start {NextTrack().Start}");
                if (TrackHasChanged())
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"CT : {CurrentTrack} Stop : {CurrentTrack.Stop} NT : {NextTrack()} Start {NextTrack().Start}");
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                await Task.Delay(1000, cancellationToken);
            }
        }

        private bool TrackHasChanged()
        {
            if (!_tracksQueue.TryPeek(out var current)) return true;
            var now = DateTime.UtcNow.AddHours(2);
            Console.WriteLine($"{current.Stop} > {now}");
            if (current.Stop > now)
            {
                return false;
            }
            _currentTrack = _tracksQueue.Dequeue();
            return true;
        }

        public void AddSchedule(IEnumerable<T> tracks)
        {
            var notYetFinished = tracks.Where(track => track.Stop >= DateTime.UtcNow.AddHours(2));
            foreach (var track in notYetFinished)
            {
                _tracksQueue.Enqueue(track);
            }
        }
    }
}