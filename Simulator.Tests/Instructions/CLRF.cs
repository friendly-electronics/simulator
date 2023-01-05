// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [DataTestMethod]
        [DataRow(0x00)]
        [DataRow(0x01)]
        [DataRow(0x10)]
        [DataRow(0x1F)]
        public void CLRF(int f)
        {
            Assert.IsTrue(f >= 0 && f <= 31, "Parameter f should be in range [0, 31].");

            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_000001_100000 | f;    // 0000 011f ffff
            micro.Update();
            debugger.RegisterFile[f].Value = 0b_1111_1111;
            debugger.AllRegisters["STATUS"].Value = 0b_0000_0000;

            micro.Update();
            Assert.AreEqual(0, debugger.RegisterFile[f].Value, "CLRF should clear [f] register.");
            Assert.AreEqual(0b_0000_0100, debugger.AllRegisters["STATUS"].Value, "CLRF should set Z flag in STATUS register.");
        }
    }
}