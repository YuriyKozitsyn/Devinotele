using System;

namespace Scheduler.Entities
{
    public class Message
    {
        public string Destination { get; set; }

        public string Source { get; set; }

        public string Text { get; set; }

        public int Type { get; set; }

        public long Id { get; set; }

        public DateTime DelayDateTime { get; set; }
    }
}