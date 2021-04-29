using Friendly.Electronics.Simulator.Registers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Registers
{
    [TestClass]
    public class ReadWriteRegisterTests
    {
        [TestMethod]
        public void OverflowBitsShouldBeMaskedOut()
        {
            var register = new ReadWriteRegister("TEST", 8);

            register.Set(0b_01111111_11111111_11111111_11111111);
            Assert.AreEqual(0b_11111111, register.Get(), "Overflow bits are not masked or masked incorrectly.");
        }
    }
}