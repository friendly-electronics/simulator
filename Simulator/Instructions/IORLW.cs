// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        IORLW k
    // Code:          1101kk kkkkkk
    // Operands:      k: [0,255]
    // Operation:     (W) OR (k) -> (W)
    // Status bits:   Z
    // Description:   The contents of the W register are OR’ed with the 8-bit literal ‘k’.
    //                The result is placed in the W register.
    
    public class IORLW : Instruction
    {
        private readonly Register _w;
        private readonly Register _status;
        private int _k;
        
        public IORLW(Microcontroller microcontroller) : base("IORLW")
        {
            _w = microcontroller.AllRegisters["W"];
            _status = microcontroller.AllRegisters["STATUS"];
        }

        public override void Prepare(int instructionCode)
        {
            _k = instructionCode & 0b_000011_111111;
        }

        public override bool Execute(int cycle)
        {
            // Execute Operation.
            var w = _w.Value;
            var result = w | _k;
            _w.Value = result;

            // Update Z flags.
            var z = result == 0 ? 0b_0000_0100 : 0;
            _status.Value = (_status.Value & 0b_1111_1011) | z;
            return true;
        }
    }
}