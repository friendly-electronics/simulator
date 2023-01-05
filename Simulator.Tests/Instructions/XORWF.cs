// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [DataTestMethod]
        [DataRow(0b_1111_0000, 0b_1010_1010, 0b_0101_1010, 0x10, 0, false)]
        [DataRow(0b_1111_0000, 0b_1010_1010, 0b_0101_1010, 0x10, 1, false)]
        [DataRow(0b_0000_0000, 0b_0000_0000, 0b_0000_0000, 0x10, 0, true)]
        [DataRow(0b_1111_1111, 0b_1111_1111, 0b_0000_0000, 0x10, 0, true)]
        public void XORWF(int value1, int value2, int result, int f, int d, bool z)
        {
            Assert.IsTrue(f >= 0 && f <= 31, "Parameter f should be in range [0, 31].");
            Assert.IsTrue(d >= 0 && d <= 1, "Parameter d should be in range [0, 1].");
            
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_000110_000000 | (d << 5) | f;    // 0001 10df ffff
            micro.Update();
            debugger.AllRegisters["W"].Value = value1;
            debugger.RegisterFile[f].Value = value2;
            debugger.AllRegisters["STATUS"].Value = 0b_1111_1011 | ((z ? 0 : 1) << 2);
            
            micro.Update();
            
            if (d == 0)
            {
                if (value1 != result)
                    Assert.AreNotEqual(value1, debugger.AllRegisters["W"].Value, "XORWF should store result in W register if d = 0.");
                Assert.AreEqual(value2, debugger.RegisterFile[f].Value, "XORWF should not change [f] register if d = 0.");
                Assert.AreEqual(result, debugger.AllRegisters["W"].Value, "XORWF should calculate W XOR [f] correctly.");
            }
            else
            {
                if (value2 != result)
                    Assert.AreNotEqual(value2, debugger.RegisterFile[f].Value, "XORWF should store result in [f] register if d = 1.");
                Assert.AreEqual(value1, debugger.AllRegisters["W"].Value, "XORWF should not change W register if d = 1.");
                Assert.AreEqual(result, debugger.RegisterFile[f].Value, "XORWF should calculate W XOR [f] registers correctly.");
            }
            Assert.AreEqual(z, (debugger.AllRegisters["STATUS"].Value & 0b_0000_0100) > 0, "XORWF should set Z flag in STATUS register if result is 0 and reset if not.");
        }
    }
}