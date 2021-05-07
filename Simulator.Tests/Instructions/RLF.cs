// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [DataTestMethod]
        [DataRow(0b_0101_0101, false, 0b_1010_1010, false, 0x00, 0)]
        [DataRow(0b_0101_0101, false, 0b_1010_1010, false, 0x00, 1)]
        [DataRow(0b_0101_0101, false, 0b_1010_1010, false, 0x10, 1)]
        [DataRow(0b_0101_0101, false, 0b_1010_1010, false, 0x1F, 1)]
        [DataRow(0b_0101_0101, true, 0b_1010_1011, false, 0x00, 0)]
        [DataRow(0b_1101_0101, false, 0b_1010_1010, true, 0x00, 0)]
        public void RLF(int value, bool c, int result, bool nc, int f, int d)
        {
            Assert.IsTrue(f >= 0 && f <= 31, "Parameter f should be in range [0, 31].");
            Assert.IsTrue(d >= 0 && d <= 1, "Parameter d should be in range [0, 1].");

            Clock.Reset();
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_001101_000000 | (d << 5) | f;    // 0011 01df ffff
            Clock.Run(false, runTime: 4000);
            debugger.AllRegisters["W"].Value = 0b_0000_0000;
            debugger.RegisterFile[f].Value = value;
            debugger.AllRegisters["STATUS"].Value = 0b_0000_0000 | (c ? 1 : 0);

            Clock.Run(false, runTime: 4000);
            
            if (d == 0)
            {
                if (value != result)
                    Assert.AreNotEqual(value, debugger.AllRegisters["W"].Value, "RLF should store result in W register if d = 0.");
                Assert.AreEqual(value, debugger.RegisterFile[f].Value, "RLF should not change [f] register if d = 0.");
                Assert.AreEqual(result, debugger.AllRegisters["W"].Value, "RLF should rotate bits in [f] register correctly.");
            }
            else
            {
                if (value != result)
                    Assert.AreNotEqual(value, debugger.RegisterFile[f].Value, "RLF should store result in [f] register if d = 1.");
                Assert.AreEqual(0b_0000_0000, debugger.AllRegisters["W"].Value, "RLF should not change W register if d = 1.");
                Assert.AreEqual(result, debugger.RegisterFile[f].Value, "RLF should rotate bits in [f] register correctly.");
            }
            Assert.AreEqual(nc, (debugger.AllRegisters["STATUS"].Value & 0b_0000_0001) > 0, "RLF should set/reset C flag in STATUS register correctly.");
        }
    }
}