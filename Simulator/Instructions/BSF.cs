// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        BSF f, b
    // Code:          0101bb bfffff
    // Operands:      f: [0,31]
    //                b: [0,7]
    // Operation:     1 -> (f<b>)
    // Status bits:   -
    // Description:   Bit ‘b’ in register ‘f’ is set.
    
    public class BSF : Instruction
    {
        private readonly Register[] _registerFile;
        private int _f;
        private int _b;
        
        public BSF(Microcontroller microcontroller) : base("BSF")
        {
            _registerFile = microcontroller.RegisterFile;
        }

        public override void Prepare(int instructionCode)
        {
            _f = instructionCode & 0b_000000_011111;
            _b = (instructionCode & 0b_000011_100000) >> 5;
        }

        public override bool Execute(bool level, int cycle)
        {
            if (cycle == 3 && level)
            {
                _registerFile[_f].Value |= 1 << _b;
                return true;
            }

            return false;
        }
    }
}