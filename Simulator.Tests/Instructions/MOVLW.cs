// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [DataTestMethod]
        [DataRow(0b_1111_0000)]
        [DataRow(0b_1010_1010)]
        public void MOVLW(int k)
        {
            Clock.Reset();
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_110000_000000 | k;    // 1100 kkkk kkkk
            Clock.Run(false, runTime: 4000);
            debugger.AllRegisters["W"].Value = 0;
            debugger.AllRegisters["STATUS"].Value = 0b_1111_1111;
            
            Clock.Run(false, runTime: 4000);
            
            Assert.AreEqual(k, debugger.AllRegisters["W"].Value, "MOVLW should load k to W register.");
        }
    }
}