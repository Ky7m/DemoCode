using System;
using ZeroFormatter;

namespace MakingDotNETApplicationsFaster.Runners.Models
{
    [ZeroFormattable]
    public class Summary
    {
        [Index(0)]
        public virtual string UserId { get; set; }

        [Index(1)]
        public virtual DateTime? StartTime { get; set; }

        [Index(2)]
        public virtual DateTime? EndTime { get; set; }

        [Index(3)]
        public virtual string Period { get; set; }

        [Index(4)]
        public virtual string Duration { get; set; }

        [Index(5)]
        public virtual int StepsTaken { get; set; }

        [Index(6)]
        public virtual int ActiveHours { get; set; }
    }
}