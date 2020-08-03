using System;
using System.Collections.Generic;

namespace Psnfrt.Responses
{
    public class MojepolskieResponse
    {
        public int Id { get; set; }
        public IEnumerable<MojepolskieTrackResponse> Songs { get; set; }
    }

    public class MojepolskieTrackResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public DateTime ScheduleTime { get; set; }
        public DateTime Stop { get; set; }
    }
}