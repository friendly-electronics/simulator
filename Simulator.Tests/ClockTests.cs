using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests
{
    [TestClass]
    public class ClockTests
    {
        [TestMethod]
        public void EventsShouldBeSorted()
        {
            var calls = new List<Tuple<object, long>>();
            var call = new Action<object, long, int>((target, time, i) => calls.Add(new Tuple<object, long>(target, time)));
            var target1 = new TestClockable(call);
            var target2 = new TestClockable(call);
            
            Clock.AddEvent(target1, 3, 0);
            Clock.AddEvent(target1, 1, 0);
            Clock.AddEvent(target1, 2, 0);
            Clock.AddEvent(target2, 1, 0);
            
            Clock.Run();
            Assert.AreEqual(1, calls[0].Item2);
            Assert.AreEqual(target1, calls[0].Item1);
            Assert.AreEqual(1, calls[1].Item2);
            Assert.AreEqual(target2, calls[1].Item1);
            Assert.AreEqual(2, calls[2].Item2);
            Assert.AreEqual(target1, calls[2].Item1);
            Assert.AreEqual(3, calls[3].Item2);
            Assert.AreEqual(target1, calls[3].Item1);
        }
        
        private class TestClockable : IClockable
        {
            private readonly Action<object, long, int> _clockableCall;

            public TestClockable(Action<object, long, int> clockableCall)
            {
                _clockableCall = clockableCall;
            }

            public void Clock(long time, int param)
            {
                _clockableCall(this, time, param);
            }
        }
    }
}