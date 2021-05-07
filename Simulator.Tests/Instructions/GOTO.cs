// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [TestMethod]
        public void GOTO()
        {
            Clock.Reset();
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_101000_000000 | 5;    // GOTO 5.    101k kkkk kkkk;
            debugger.ProgramMemory[1].Value = 0b_110000_000000 | 1;    // MOVLW 1.   1100 kkkk kkkk;
            debugger.ProgramMemory[5].Value = 0b_110000_000000 | 5;    // MOVLW 1.   1100 kkkk kkkk;
            Clock.Run(false, runTime: 4000);
            debugger.AllRegisters["W"].Value = 0;
            debugger.AllRegisters["STATUS"].Value = 0b_0000_0000;
            
            Clock.Run(false, runTime: 4000);
            Assert.AreEqual(0, debugger.AllRegisters["W"].Value, "GOTO should not change W register.");
            Assert.AreEqual(0, debugger.AllRegisters["STATUS"].Value, "GOTO should not change STATUS register.");
            
            Clock.Run(false, runTime: 4000);
            Assert.AreEqual(0, debugger.AllRegisters["W"].Value, "GOTO should not change W register.");
            Assert.AreEqual(0, debugger.AllRegisters["STATUS"].Value, "GOTO should not change STATUS register.");
            Assert.AreEqual(5, debugger.AllRegisters["PC"].Value, "GOTO should change PC register.");

            Clock.Run(false, runTime: 4000);
            Assert.AreEqual(6, debugger.AllRegisters["PC"].Value, "GOTO should change PC register.");
            Assert.AreEqual(5, debugger.AllRegisters["W"].Value, "GOTO should take two cycles and the instruction at address k must be executed when GOTO is completed.");
        }
    }
}