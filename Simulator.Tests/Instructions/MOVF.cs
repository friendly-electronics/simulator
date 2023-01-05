// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [DataTestMethod]
        [DataRow(137, 137, 0x10, 0, false)]
        [DataRow(137, 137, 0x10, 1, false)]
        [DataRow(111, 111, 0x00, 1, false)]
        [DataRow(110, 110, 0x1F, 1, false)]
        [DataRow(0, 0, 0x10, 0, true)]
        [DataRow(255, 255, 0x10, 0, false)]
        public void MOVF(int value, int result, int f, int d, bool z)
        {
            Assert.IsTrue(f >= 0 && f <= 31, "Parameter f should be in range [0, 31].");
            Assert.IsTrue(d >= 0 && d <= 1, "Parameter d should be in range [0, 1].");
            
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_001000_000000 | (d << 5) | f;    // 0010 00df ffff
            micro.Update();
            debugger.AllRegisters["W"].Value = 0b_1100_1100;
            debugger.RegisterFile[f].Value = value;
            debugger.AllRegisters["STATUS"].Value = 0b_1111_1011 | ((z ? 0 : 1) << 2);
            
            micro.Update();
            
            if (d == 0)
            {
                if (0b_1100_1100 != result)
                    Assert.AreNotEqual(0b_1100_1100, debugger.AllRegisters["W"].Value, "MOVF should store result in W register if d = 0.");
                Assert.AreEqual(value, debugger.RegisterFile[f].Value, "MOVF should not change [f] register if d = 0.");
                Assert.AreEqual(result, debugger.AllRegisters["W"].Value, "MOVF should move [f] register to [w] register.");
            }
            else
            {
                if (value != result)
                    Assert.AreNotEqual(value, debugger.RegisterFile[f].Value, "MOVF should store result in [f] register if d = 1.");
                Assert.AreEqual(0b_1100_1100, debugger.AllRegisters["W"].Value, "MOVF should not change W register if d = 1.");
                Assert.AreEqual(result, debugger.RegisterFile[f].Value, "MOVF should move [f] register to [f] register.");
            }
            Assert.AreEqual(z, (debugger.AllRegisters["STATUS"].Value & 0b_0000_0100) > 0, "MOVF should set Z flag in STATUS register if result is 0 and reset if not.");
        }
    }
}