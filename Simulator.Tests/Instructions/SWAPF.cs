// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [DataTestMethod]
        [DataRow(0b_1111_0000, 0b_0000_1111, 0x10, 0, false)]
        [DataRow(0b_1111_0000, 0b_0000_1111, 0x10, 1, false)]
        [DataRow(0b_1111_0000, 0b_0000_1111, 0x00, 1, false)]
        [DataRow(0b_1111_0000, 0b_0000_1111, 0x1F, 1, false)]
        [DataRow(0b_1010_0101, 0b_0101_1010, 0x10, 0, false)]
        public void SWAPF(int value, int result, int f, int d, bool z)
        {
            Assert.IsTrue(f >= 0 && f <= 31, "Parameter f should be in range [0, 31].");
            Assert.IsTrue(d >= 0 && d <= 1, "Parameter d should be in range [0, 1].");
            
            Clock.Reset();
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_001110_000000 | (d << 5) | f;    // 0011 10df ffff
            Clock.Run(false, runTime: 4000);
            debugger.AllRegisters["W"].Value = 0b_1100_1100;
            debugger.RegisterFile[f].Value = value;
            debugger.AllRegisters["STATUS"].Value = 0b_1111_1011 | ((z ? 0 : 1) << 2);
            
            Clock.Run(false, runTime: 4000);
            
            if (d == 0)
            {
                if (0b_1100_1100 != result)
                    Assert.AreNotEqual(0b_1100_1100, debugger.AllRegisters["W"].Value, "SWAPF should store result in W register if d = 0.");
                Assert.AreEqual(value, debugger.RegisterFile[f].Value, "SWAPF should not change [f] register if d = 0.");
                Assert.AreEqual(result, debugger.AllRegisters["W"].Value, "SWAPF should decrement [f] correctly.");
            }
            else
            {
                if (value != result)
                    Assert.AreNotEqual(value, debugger.RegisterFile[f].Value, "SWAPF should store result in [f] register if d = 1.");
                Assert.AreEqual(0b_1100_1100, debugger.AllRegisters["W"].Value, "SWAPF should not change W register if d = 1.");
                Assert.AreEqual(result, debugger.RegisterFile[f].Value, "SWAPF should decrement [f] registers correctly.");
            }
        }
    }
}