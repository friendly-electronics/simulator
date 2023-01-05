// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        CLRW
    // Code:          000001 000000
    // Operands:      -
    // Operation:     00h -> (W);
    //                1 -> Z
    // Status bits:   Z
    // Description:   The W register is cleared. Zero bit (Z) is set.
    
    public class CLRW : Instruction
    {
        private readonly Register _w;
        private readonly Register _status;
        
        public CLRW(Microcontroller microcontroller) : base("CLRW")
        {
            _w = microcontroller.AllRegisters["W"];
            _status = microcontroller.AllRegisters["STATUS"];
        }
        
        public override void Prepare(int instructionCode)
        {
        }

        public override bool Execute(int cycle)
        {
            // Execute Operation.
            _w.Value = 0;

            // Update C, DC, Z flags.
            var z = 0b_0000_0100;
            _status.Value = (_status.Value & 0b_1111_1011) | z;
            return true;
        }
    }
}