// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [DataTestMethod]
        [DataRow(0b_1111_0000, 0b_1010_1010, 0b_1111_1010, false)]
        [DataRow(0b_0000_0000, 0b_0000_0000, 0b_0000_0000, true)]
        public void IORLW(int value, int k, int result, bool z)
        {
            Clock.Reset();
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_110100_000000 | k;    // 1101 kkkk kkkk
            Clock.Run(false, runTime: 4000);
            debugger.AllRegisters["W"].Value = value;
            debugger.AllRegisters["STATUS"].Value = 0b_1111_1011 | ((z ? 0 : 1) << 2);
            
            Clock.Run(false, runTime: 4000);
            
            Assert.AreEqual(result, debugger.AllRegisters["W"].Value, "ANDLW should calculate W OR k correctly.");
            Assert.AreEqual(z, (debugger.AllRegisters["STATUS"].Value & 0b_0000_0100) > 0, "ANDLW should set Z flag in STATUS register if result is 0 and reset if not.");
        }
    }
}