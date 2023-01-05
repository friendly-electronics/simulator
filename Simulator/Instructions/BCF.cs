// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        BCF f, b
    // Code:          0100bb bfffff
    // Operands:      f: [0,31]
    //                b: [0,7]
    // Operation:     0 -> (f<b>)
    // Status bits:   -
    // Description:   Bit ‘b’ in register ‘f’ is cleared.
    
    public class BCF : Instruction
    {
        private readonly Register[] _registerFile;
        private int _f;
        private int _b;
        
        public BCF(Microcontroller microcontroller) : base("BCF")
        {
            _registerFile = microcontroller.RegisterFile;
        }

        public override void Prepare(int instructionCode)
        {
            _f = instructionCode & 0b_000000_011111;
            _b = (instructionCode & 0b_000011_100000) >> 5;
        }

        public override bool Execute(int cycle)
        {
            _registerFile[_f].Value &= ~(1 << _b);
            return true;
        }
    }
}