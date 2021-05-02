using System;
using System.Diagnostics;
using System.Threading;

namespace Friendly.Electronics.Simulator
{
    public static class Clock
    {
        private static ClockEvent _nextEvent;
        private static long _time;
        private static readonly Stopwatch Timer = new Stopwatch();

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

        private static ClockEvent PopNextEvent()
        {
            return _nextEvent;
        }
        
        private static void RemoveNextEvent()
        {
            _nextEvent = _nextEvent?.Next;
        }

        public static void Run(bool realTime = true, double speedCoefficient = 1, long runTime = long.MaxValue)
        {
            runTime = _time + runTime;
            Timer.Start();
            var @event = PopNextEvent();
            while (@event != null)
            {
                if (@event.Time >= runTime)
                    break;
                RemoveNextEvent();
                if (realTime)
                {
                    var eventTimeMs = (long) (@event.Time / 1000000.0 / speedCoefficient);
                    var currentTimeMs = Timer.Elapsed.TotalMilliseconds;
                    if (eventTimeMs > currentTimeMs)
                        Thread.Sleep((int) (eventTimeMs - currentTimeMs));
                }
                _time = @event.Time;
                @event.Target.Update(@event.Time, @event.Param);
                @event = PopNextEvent();
            }
            Timer.Stop();
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