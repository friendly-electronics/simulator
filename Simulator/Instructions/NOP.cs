// ReSharper disable InconsistentNaming

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        NOP
    // Code:          000000 000000
    // Operands:      -
    // Operation:     -
    // Status bits:   -
    // Description:   No operation.
    
    public class NOP : Instruction
    {
        public NOP() : base("NOP")
        {
        }
        
        public override void Prepare(int instructionCode)
        {
        }

        public override bool Execute(bool level, int cycle)
        {
            return true;
        }
    }
}