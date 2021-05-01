using System.Diagnostics;
using System.Threading;

namespace Friendly.Electronics.Simulator
{
    public static class Clock
    {
        private static ClockEvent _nextEvent;
        private static long _time;

        public static long Now => _time;

        public static void AddEvent(IClockable target, long offset, int param)
        {
            var @event = new ClockEvent {Target = target, Time = _time + offset, Param = param};
            if (_nextEvent == null)
                _nextEvent = @event;
            else if (offset < _nextEvent.Time)
            {
                @event.Next = _nextEvent;
                _nextEvent = @event;
            }
            else
            {
                var prev = _nextEvent;
                var next = _nextEvent.Next;
                while (next != null && offset >= next.Time)
                {
                    prev = next;
                    next = next.Next;
                }

                prev.Next = @event;
                @event.Next = next;
            }
        }

        private static ClockEvent GetNextEvent()
        {
            var result = _nextEvent;
            _nextEvent = _nextEvent?.Next;
            return result;
        }

        public static void Run()
        {
            var sw = new Stopwatch();
            sw.Start();
            var @event = GetNextEvent();
            while (@event != null)
            {
                var eventTimeMs = @event.Time / 1000000;
                var currentTimeMs = sw.ElapsedMilliseconds;
                if (eventTimeMs > currentTimeMs)
                    Thread.Sleep((int)(eventTimeMs - currentTimeMs));
                _time = @event.Time;
                @event.Target.Update(@event.Time, @event.Param);
                @event = GetNextEvent();
            }
            sw.Stop();
        }
        
        private class ClockEvent
        {
            public IClockable Target;
            public long Time;
            public int Param;
            public ClockEvent Next;
        }
    }

    public interface IClockable
    {
        void Update(long time, int param);
    }
}