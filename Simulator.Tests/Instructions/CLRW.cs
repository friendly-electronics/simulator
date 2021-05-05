// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [TestMethod]
        public void CLRW()
        {
            Clock.Reset();
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_0000_0100_0000;    // 0000 0100 0000
            Clock.Run(false, runTime: 4000);
            debugger.AllRegisters["W"].Value = 0b_1111_1111;
            debugger.AllRegisters["STATUS"].Value = 0b_0000_0000;

            Clock.Run(false, runTime: 4000);
            Assert.AreEqual(0, debugger.AllRegisters["W"].Value, "CLRW should clear W register.");
            Assert.AreEqual(0b_0000_0100, debugger.AllRegisters["STATUS"].Value, "CLRW should set Z flag in STATUS register.");
        }
    }
}