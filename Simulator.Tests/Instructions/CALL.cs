// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [TestMethod]
        public void CALL()
        {
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_100100_000000 | 5;    // CALL 5.    1001 kkkk kkkk
            debugger.ProgramMemory[1].Value = 0b_110000_000000 | 1;    // MOVLW 1.   1100 kkkk kkkk;
            debugger.ProgramMemory[5].Value = 0b_110000_000000 | 5;    // MOVLW 5.   1100 kkkk kkkk;
            micro.Update();
            debugger.AllRegisters["W"].Value = 0;
            debugger.AllRegisters["STATUS"].Value = 0b_0000_0000;
            
            // CALL 5 (1)
            micro.Update();
            Assert.AreEqual(0, debugger.AllRegisters["W"].Value, "CALL should not change W register.");
            Assert.AreEqual(0, debugger.AllRegisters["STATUS"].Value, "CALL should not change STATUS register.");
            
            // CALL 5 (2)
            micro.Update();
            Assert.AreEqual(0, debugger.AllRegisters["W"].Value, "CALL should not change W register.");
            Assert.AreEqual(0, debugger.AllRegisters["STATUS"].Value, "CALL should not change STATUS register.");
            Assert.AreEqual(5, debugger.AllRegisters["PC"].Value, "CALL should change PC register.");
            Assert.AreEqual(1, debugger.AllRegisters["ST0"].Value, "CALL should PUSH PC+1 in to stack.");

            micro.Update();
            Assert.AreEqual(6, debugger.AllRegisters["PC"].Value, "CALL should change PC register.");
            Assert.AreEqual(5, debugger.AllRegisters["W"].Value, "CALL should take two cycles and the instruction at address k must be executed when CALL is completed.");
        }
    }
}