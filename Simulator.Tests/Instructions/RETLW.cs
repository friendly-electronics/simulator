// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [TestMethod]
        public void RETLW()
        {
            Clock.Reset();
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_100000_000000 | 10;   // RETLW 10.  1000 kkkk kkkk
            debugger.ProgramMemory[1].Value = 0b_110000_000000 | 1;    // MOVLW 1.   1100 kkkk kkkk;
            debugger.ProgramMemory[5].Value = 0b_110000_000000 | 5;    // MOVLW 5.   1100 kkkk kkkk;
            Clock.Run(false, runTime: 4000);
            debugger.AllRegisters["W"].Value = 0;
            debugger.AllRegisters["STATUS"].Value = 0b_0000_0000;
            debugger.AllRegisters["ST0"].Value = 5;
            debugger.AllRegisters["ST1"].Value = 255;
            
            // RETLW 10 (1)
            Clock.Run(false, runTime: 4000);
            Assert.AreEqual(0, debugger.AllRegisters["STATUS"].Value, "RETLW should not change STATUS register.");
            
            // RETLW 10 (2)
            Clock.Run(false, runTime: 4000);
            Assert.AreEqual(10, debugger.AllRegisters["W"].Value, "RETLW should load k in to W register.");
            Assert.AreEqual(0, debugger.AllRegisters["STATUS"].Value, "RETLW should not change STATUS register.");
            Assert.AreEqual(5, debugger.AllRegisters["PC"].Value, "RETLW should change PC register.");
            Assert.AreEqual(255, debugger.AllRegisters["ST0"].Value, "RETLW should POP return address from stack.");

            Clock.Run(false, runTime: 4000);
            Assert.AreEqual(6, debugger.AllRegisters["PC"].Value, "RETLW should change PC register.");
            Assert.AreEqual(5, debugger.AllRegisters["W"].Value, "RETLW should take two cycles and the instruction at address k must be executed when RETLW is completed.");
        }
    }
}