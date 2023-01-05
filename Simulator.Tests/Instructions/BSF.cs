// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [DataTestMethod]
        [DataRow(0x00, 4, 0b_0000_0000, 0b_0001_0000)]
        [DataRow(0x01, 4, 0b_0000_0000, 0b_0001_0000)]
        [DataRow(0x10, 4, 0b_0000_0000, 0b_0001_0000)]
        [DataRow(0x1F, 4, 0b_0000_0000, 0b_0001_0000)]
        [DataRow(0x10, 0, 0b_0000_0000, 0b_0000_0001)]
        [DataRow(0x10, 1, 0b_0000_0000, 0b_0000_0010)]
        [DataRow(0x10, 7, 0b_0000_0000, 0b_1000_0000)]
        public void BSF(int f, int b, int value, int result)
        {
            Assert.IsTrue(f >= 0 && f <= 31, "Parameter f should be in range [0, 31].");
            Assert.IsTrue(b >= 0 && b <= 7, "Parameter b should be in range [0, 7].");

            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_010100_000000 | (b << 5) | f;    // 0101 bbbf ffff
            micro.Update();
            debugger.RegisterFile[f].Value = value;
            debugger.AllRegisters["STATUS"].Value = 0b_0000_0000;

            micro.Update();
            Assert.AreEqual(result, debugger.RegisterFile[f].Value, "BSF should clear bit b in [f] register.");
            Assert.AreEqual(0b_0000_0000, debugger.AllRegisters["STATUS"].Value, "BSF should not change STATUS register.");
        }
    }
}