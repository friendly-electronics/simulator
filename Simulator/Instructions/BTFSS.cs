// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf
// ReSharper disable ConvertIfStatementToReturnStatement

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        BTFSS f, b
    // Code:          0111bb bfffff
    // Operands:      f: [0,31]
    //                b: [0,7]
    // Operation:     skip if (f<b>) = 1
    // Status bits:   -
    // Description:   If bit ‘b’ in register ‘f’ is ‘1’, then the next instruction is skipped.
    //                If bit ‘b’ is ‘1’, then the next instruction fetched during the current
    //                instruction execution, is discarded and a NOP is executed instead, making
    //                this a 2-cycle instruction.
    
    public class BTFSS : Instruction
    {
        private readonly Register[] _registerFile;
        private int _f;
        private int _b;
        
        public BTFSS(Microcontroller microcontroller) : base("BTFSS")
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
            if (cycle == 3)
            {
                return (_registerFile[_f].Value & (1 << _b)) == 0;
            }

            if (cycle >= 4)
                return true;

            return false;
        }
    }
}