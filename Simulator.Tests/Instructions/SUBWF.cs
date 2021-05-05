// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [DataTestMethod]
        [DataRow(2, 1, 1, 0x10, 0, false, true, true)]          // w + f -> w.
        [DataRow(2, 1, 1, 0x10, 1, false, true, true)]          // w + f -> f.
        [DataRow(16, 32, 240, 0x10, 0, false, true, false)]     // C overflow.
        [DataRow(17, 2, 15, 0x10, 0, false, false, true)]       // DC overflow.
        [DataRow(0, 0, 0, 0x10, 0, true, true, true)]           // Z result is zero.
        [DataRow(240, 240, 0, 0x10, 0, true, true, true)]       // Z result is zero.
        public void SUBWF(int value1, int value2, int result, int f, int d, bool z, bool dc, bool c)
        {
            Assert.IsTrue(f >= 0 && f <= 31, "Parameter f should be in range [0, 31].");
            Assert.IsTrue(d >= 0 && d <= 1, "Parameter d should be in range [0, 1].");
            
            Clock.Reset();
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_000010_000000 | (d << 5) | f;    // 0000 10df ffff
            Clock.Run(false, runTime: 4000);
            debugger.RegisterFile[f].Value = value1;
            debugger.AllRegisters["W"].Value = value2;
            debugger.AllRegisters["STATUS"].Value = 0b_1111_1000 | ((z ? 0 : 1) << 2) | ((dc ? 0 : 1) << 1) | ((c ? 0 : 1) << 0);
            
            Clock.Run(false, runTime: 4000);
            
            if (d == 0)
            {
                if (value2 != result)
                    Assert.AreNotEqual(value2, debugger.AllRegisters["W"].Value, "SUBWF should store result in W register if d = 0.");
                Assert.AreEqual(value1, debugger.RegisterFile[f].Value, "SUBWF should not change [f] register if d = 0.");
                Assert.AreEqual(result, debugger.AllRegisters["W"].Value, "SUBWF should calculate sub of [f] and W registers correctly.");
            }
            else
            {
                if (value1 != result)
                    Assert.AreNotEqual(value1, debugger.RegisterFile[f].Value, "SUBWF should store result in [f] register if d = 1.");
                Assert.AreEqual(value2, debugger.AllRegisters["W"].Value, "SUBWF should not change W register if d = 1.");
                Assert.AreEqual(result, debugger.RegisterFile[f].Value, "SUBWF should calculate sub of [f] and W registers correctly.");

            }
            Assert.AreEqual(z, (debugger.AllRegisters["STATUS"].Value & 0b_0000_0100) > 0, "ADDWF should set Z flag in STATUS register if result is 0 and reset if not.");
            Assert.AreEqual(dc, (debugger.AllRegisters["STATUS"].Value & 0b_0000_0010) > 0, "ADDWF should set DC flag in STATUS register if digital carry occured (or borrow not occured) and reset if not.");
            Assert.AreEqual(c, (debugger.AllRegisters["STATUS"].Value & 0b_0000_0001) > 0, "ADDWF should set C flag in STATUS register if carry occured (or borrow not occured) and reset if not.");
        }
    }
}