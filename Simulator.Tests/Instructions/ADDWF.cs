// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [DataTestMethod]
        [DataRow(1, 2, 3, 0x10, 0, false, false, false)]        // w + f -> w.
        [DataRow(1, 2, 3, 0x10, 1, false, false, false)]        // w + f -> f.
        [DataRow(254, 1, 255, 0x10, 0, false, false, false)]    // No overflow.
        [DataRow(240, 17, 1, 0x10, 0, false, false, true)]      // C overflow.
        [DataRow(15, 1, 16, 0x10, 0, false, true, false)]       // DC overflow.
        [DataRow(0, 0, 0, 0x10, 0, true, false, false)]         // Z result is zero.
        [DataRow(240, 16, 0, 0x10, 0, true, false, true)]       // Z result is zero and C overflow.
        public void ADDWF(int value1, int value2, int result, int f, int d, bool z, bool dc, bool c)
        {
            Assert.IsTrue(f >= 0 && f <= 31, "Parameter f should be in range [0, 31].");
            Assert.IsTrue(d >= 0 && d <= 1, "Parameter d should be in range [0, 1].");
            
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_000111_000000 | (d << 5) | f;    // 0001 11df ffff
            micro.Update();
            debugger.AllRegisters["W"].Value = value1;
            debugger.RegisterFile[f].Value = value2;
            debugger.AllRegisters["STATUS"].Value = 0b_1111_1000 | ((z ? 0 : 1) << 2) | ((dc ? 0 : 1) << 1) | ((c ? 0 : 1) << 0);
            
            micro.Update();
            
            if (d == 0)
            {
                if (value1 != result)
                    Assert.AreNotEqual(value1, debugger.AllRegisters["W"].Value, "ADDWF should store result in W register if d = 0.");
                Assert.AreEqual(value2, debugger.RegisterFile[f].Value, "ADDWF should not change [f] register if d = 0.");
                Assert.AreEqual(result, debugger.AllRegisters["W"].Value, "ADDWF should calculate sum of W and [f] registers correctly.");
            }
            else
            {
                if (value2 != result)
                    Assert.AreNotEqual(value2, debugger.RegisterFile[f].Value, "ADDWF should store result in [f] register if d = 1.");
                Assert.AreEqual(value1, debugger.AllRegisters["W"].Value, "ADDWF should not change W register if d = 1.");
                Assert.AreEqual(result, debugger.RegisterFile[f].Value, "ADDWF should calculate sum of W and [f] registers correctly.");

            }
            Assert.AreEqual(z, (debugger.AllRegisters["STATUS"].Value & 0b_0000_0100) > 0, "ADDWF should set Z flag in STATUS register if result is 0 and reset if not.");
            Assert.AreEqual(dc, (debugger.AllRegisters["STATUS"].Value & 0b_0000_0010) > 0, "ADDWF should set DC flag in STATUS register if digital carry occured (or borrow not occured) and reset if not.");
            Assert.AreEqual(c, (debugger.AllRegisters["STATUS"].Value & 0b_0000_0001) > 0, "ADDWF should set C flag in STATUS register if carry occured (or borrow not occured) and reset if not.");
        }
    }
}