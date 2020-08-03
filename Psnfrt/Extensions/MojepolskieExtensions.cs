using System.Collections.Generic;
using System.Linq;
using Psnfrt.Responses;
using Psnfrt.Tracks;

namespace Psnfrt.Extensions
{
    public static class MojepolskieExtensions
    {
        public static IEnumerable<MojepolskieTrack> AsEntity(this MojepolskieResponse moje)
            => moje.Songs?.Select(t => new MojepolskieTrack(t.Id, moje.Id, t.Title, t.Artist, t.ScheduleTime, t.Stop));
    }
}