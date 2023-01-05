// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [DataTestMethod]
        [DataRow(0x00, 0b_0000_0000)]
        [DataRow(0x00, 0b_1111_1111)]
        [DataRow(0x00, 0b_0000_0001)]
        [DataRow(0x01, 0b_0000_0001)]
        [DataRow(0x10, 0b_0000_0001)]
        [DataRow(0x1F, 0b_0000_0001)]
        public void MOVWF(int f, int value)
        {
            Assert.IsTrue(f >= 0 && f <= 31, "Parameter f should be in range [0, 31].");
            
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_0000_0010_0000 | f;    // 0000 001f ffff
            micro.Update();
            debugger.AllRegisters["W"].Value = value;

            var status = debugger.AllRegisters["STATUS"].Value;
            micro.Update();
            Assert.AreEqual(value, debugger.RegisterFile[f].Value, "MOVWF should load W into TRIS[f] register.");
            Assert.AreEqual(value, debugger.AllRegisters["W"].Value, "MOVWF should not change W register.");
            Assert.AreEqual(status, debugger.AllRegisters["STATUS"].Value, "MOVWF should not change STATUS register.");
        }
    }
}