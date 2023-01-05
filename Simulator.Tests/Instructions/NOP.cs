// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [TestMethod]
        public void NOP()
        {
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_000000_000000;
            micro.Update();

            var status = debugger.AllRegisters["STATUS"].Value;
            micro.Update();
            Assert.AreEqual(status, debugger.AllRegisters["STATUS"].Value, "NOP should not change STATUS.");
        }
    }
}