using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests
{
    [TestClass]
    public class InternalOscillatorTests
    {
        [TestMethod]
        public void OscillatorShouldOscillate()
        {
            Clock.Reset();
            var oscillator = new InternalOscillator(1000000);    // MHz
            var currentLevel = false;
            var currentIteration = 0;
            var startTime = Clock.Now;
            oscillator.LogicLevelChanged += level =>
            {
                Assert.AreEqual(!currentLevel, level);
                Assert.AreEqual(startTime + currentIteration * 500, Clock.Now);
                currentLevel = !currentLevel;
                currentIteration++;
                if (currentIteration == 10)
                    oscillator.Stop();
            };
            oscillator.Start();
            Clock.Run();
            Assert.AreEqual(10, currentIteration);
        }
    }
}