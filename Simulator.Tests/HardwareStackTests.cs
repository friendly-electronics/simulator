using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests
{
    [TestClass]
    public class HardwareStackTests
    {
        [TestMethod]
        public void HardwareStackShouldBehaveAccordingToTheSpecs()
        {
            // Stack Init.
            // 254
            // 255
            var stack = new HardwareStack(2);
            stack[0] = 254;
            stack[1] = 255;
            Assert.AreEqual(2, stack.Capacity, "Capacity should be correct.");
            Assert.AreEqual(254, stack[0]);
            Assert.AreEqual(255, stack[1]);

            // Stack Push 1
            // 1
            // 254
            stack.Push(1);
            Assert.AreEqual(1, stack[0]);
            Assert.AreEqual(254, stack[1]);
            
            // Stack Pop -> 1
            // 254
            // 254
            var value = stack.Pop();
            Assert.AreEqual(1, value);
            Assert.AreEqual(254, stack[0]);
            Assert.AreEqual(254, stack[1]);
            
            // Stack Push 1
            // Stack Push 2
            // 2
            // 1
            stack.Push(1);
            stack.Push(2);
            Assert.AreEqual(2, stack[0]);
            Assert.AreEqual(1, stack[1]);
            
            // Stack Pop -> 2
            // 1
            // 1
            value = stack.Pop();
            Assert.AreEqual(2, value);
            Assert.AreEqual(1, stack[0]);
            Assert.AreEqual(1, stack[1]);
            
            // Stack Pop -> 1
            // 1
            // 1
            value = stack.Pop();
            Assert.AreEqual(1, value);
            Assert.AreEqual(1, stack[0]);
            Assert.AreEqual(1, stack[1]);
            
            // Stack Push 1
            // Stack Push 2
            // Stack Push 3
            // 3
            // 2
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            Assert.AreEqual(3, stack[0]);
            Assert.AreEqual(2, stack[1]);
            
            // Stack Pop -> 3
            // 2
            // 2
            value = stack.Pop();
            Assert.AreEqual(3, value);
            Assert.AreEqual(2, stack[0]);
            Assert.AreEqual(2, stack[1]);
            
            // Stack Pop -> 2
            // 2
            // 2
            value = stack.Pop();
            Assert.AreEqual(2, value);
            Assert.AreEqual(2, stack[0]);
            Assert.AreEqual(2, stack[1]);
            
            // Stack Pop -> 2
            // 2
            // 2
            value = stack.Pop();
            Assert.AreEqual(2, value);
            Assert.AreEqual(2, stack[0]);
            Assert.AreEqual(2, stack[1]);
            
            // Stack Pop -> 2
            // 2
            // 2
            value = stack.Pop();
            Assert.AreEqual(2, value);
            Assert.AreEqual(2, stack[0]);
            Assert.AreEqual(2, stack[1]);
        }
    }
}