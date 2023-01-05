// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [DataTestMethod]
        [DataRow(0b_0000_0000)]
        [DataRow(0b_1111_1111)]
        [DataRow(0b_0000_0001)]
        public void OPTION(int value)
        {
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_0000_0000_0010;
            micro.Update();
            debugger.AllRegisters["W"].Value = value;

            var status = debugger.AllRegisters["STATUS"].Value;
            micro.Update();
            Assert.AreEqual(value, debugger.AllRegisters["OPTION"].Value, "OPTION should load W into OPTION register.");
            Assert.AreEqual(value, debugger.AllRegisters["W"].Value, "OPTION should not change W register.");
            Assert.AreEqual(status, debugger.AllRegisters["STATUS"].Value, "OPTION should not change STATUS register.");
        }
    }
}