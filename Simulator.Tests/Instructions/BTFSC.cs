// ReSharper disable InconsistentNaming

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests.Instructions
{
    public partial class Instructions
    {
        [DataTestMethod]
        [DataRow(0x00, 4, 0b_0001_0000, false)]
        [DataRow(0x01, 4, 0b_1110_1111, true)]
        [DataRow(0x10, 4, 0b_0001_0000, false)]
        [DataRow(0x1F, 4, 0b_0110_0110, true)]
        [DataRow(0x10, 0, 0b_1111_1110, true)]
        [DataRow(0x10, 1, 0b_0000_1101, true)]
        [DataRow(0x10, 7, 0b_0110_0000, true)]
        public void BTFSC(int f, int b, int value, bool shouldSkip)
        {
            Assert.IsTrue(f >= 0 && f <= 31, "Parameter f should be in range [0, 31].");
            Assert.IsTrue(b >= 0 && b <= 7, "Parameter b should be in range [0, 7].");

            Clock.Reset();
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            
            debugger.ProgramMemory[0].Value = 0b_011000_000000 | (b << 5) | f;    // BTFSC f, b.    0110 bbbf ffff
            debugger.ProgramMemory[1].Value = 0b_110000_000000 | 17;              // MOVLW 17.      1100 kkkk kkkk
            Clock.Run(false, runTime: 4000);
            debugger.RegisterFile[f].Value = value;
            debugger.AllRegisters["STATUS"].Value = 0b_0000_0000;
            debugger.AllRegisters["W"].Value = 1;

            Clock.Run(false, runTime: 4000);
            Assert.AreEqual(value, debugger.RegisterFile[f].Value, "BTFSC should not change [f] register.");
            Assert.AreEqual(0b_0000_0000, debugger.AllRegisters["STATUS"].Value, "BTFSC should not change STATUS register.");
            Assert.AreEqual(1, debugger.AllRegisters["W"].Value);
            
            Clock.Run(false, runTime: 4000);
            Assert.AreEqual(value, debugger.RegisterFile[f].Value, "BTFSC should not change [f] register.");
            Assert.AreEqual(0b_0000_0000, debugger.AllRegisters["STATUS"].Value, "BTFSC should not change STATUS register.");
            Assert.AreEqual(shouldSkip ? 1 : 17, debugger.AllRegisters["W"].Value);

        }
    }
}