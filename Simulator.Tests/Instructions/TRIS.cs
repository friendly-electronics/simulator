// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [DataTestMethod]
        [DataRow(6, 0b_0000_0000)]
        [DataRow(6, 0b_1111_1111)]
        [DataRow(6, 0b_0000_0001)]
        public void TRIS(int f, int value)
        {
            Assert.IsTrue(f >= 0 && f <= 7, "Parameter f should be in range [0, 7].");
            
            Clock.Reset();
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_0000_0000_0000 | f;
            Clock.Run(false, runTime: 4000);
            debugger.AllRegisters["W"].Value = value;

            var status = debugger.AllRegisters["STATUS"].Value;
            Clock.Run(false, runTime: 4000);
            Assert.AreEqual(value, debugger.TrisRegisters[f].Value, "TRIS should load W into TRIS[f] register.");
            Assert.AreEqual(value, debugger.AllRegisters["W"].Value, "TRIS should not change W register.");
            Assert.AreEqual(status, debugger.AllRegisters["STATUS"].Value, "TRIS should not change STATUS register.");
        }
    }
}