using System;

namespace Assets.Scripts.Helpers
{
    public class AnalyticItem
    {
        public DateTime TimeStamp { get; set; }
        public string Event { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0};{1};{2}", TimeStamp, Event, Value);
        }
    }
}
