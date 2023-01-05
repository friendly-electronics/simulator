// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        SWAPF f, d
    // Code:          001110 dfffff
    // Operands:      f: [0,31]
    //                d: [0,1]
    // Operation:     (f<3:0>) -> (dest<7:4>);
    //                (f<7:4>) -> (dest<3:0>)
    // Status bits:   -
    // Description:   The upper and lower nibbles of register ‘f’ are exchanged. If ‘d’ is ‘0’,
    //                the result is placed in W register. If ‘d’ is ‘1’, the result is placed
    //                in register ‘f’.
    
    public class SWAPF : Instruction
    {
        private readonly Register[] _registerFile;
        private readonly Register _w;
        private int _f;
        private int _d;

        public SWAPF(Microcontroller microcontroller) : base("SWAPF")
        {
            _registerFile = microcontroller.RegisterFile;
            _w = microcontroller.AllRegisters["W"];
        }

        public override void Prepare(int instructionCode)
        {
            _f = instructionCode & 0b_011111;
            _d = instructionCode & 0b_100000;
        }

        public override bool Execute(int cycle)
        {
            // Execute Operation.
            var r = _registerFile[_f].Value;
            var result = ((r & 0b_1111_0000) >> 4) | ((r & 0b_0000_1111) << 4);
            if (_d == 0)
                _w.Value = result;
            else
                _registerFile[_f].Value = result;
            return true;
        }
    }
}