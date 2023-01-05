// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        MOVLW k
    // Code:          1100kk kkkkkk
    // Operands:      k: [0,255]
    // Operation:     (k) -> (W)
    // Status bits:   -
    // Description:   The 8-bit literal ‘k’ is loaded into the W register. The “don’t cares”
    //                will assembled as ‘0’s.
    
    public class MOVLW : Instruction
    {
        private readonly Register _w;
        private readonly Register _status;
        private int _k;
        
        public MOVLW(Microcontroller microcontroller) : base("MOVLW")
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
            if (cycle == 3)
            {
                // Execute Operation.
                _w.Value = _k;
                return true;
            }

            return false;
        }
    }
}