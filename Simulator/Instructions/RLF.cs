// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        RLF f, d
    // Code:          001101 dfffff
    // Operands:      f: [0,31]
    //                d: [0,1]
    // Operation:     <- (C) <- (f) <-
    // Status bits:   C
    // Description:   The contents of register ‘f’ are rotated one bit to the left
    //                through the Carry flag. If ‘d’ is ‘0’, the result is placed
    //                in the W register. If ‘d’ is ‘1’, the result is stored back
    //                in register ‘f’.
    
    public class RLF : Instruction
    {
        private readonly Register[] _registerFile;
        private readonly Register _w;
        private readonly Register _status;
        private int _f;
        private int _d;
        
        public RLF(Microcontroller microcontroller) : base("RLF")
        {
            _registerFile = microcontroller.RegisterFile;
            _w = microcontroller.AllRegisters["W"];
            _status = microcontroller.AllRegisters["STATUS"];
        }
        
        public override void Prepare(int instructionCode)
        {
            _f = instructionCode & 0b_011111;
            _d = instructionCode & 0b_100000;
        }

        public override bool Execute(bool level, int cycle)
        {
            if (cycle == 3 && level)
            {
                // Execute Operation.
                var r = _registerFile[_f].Value;
                var c = _status.Value & 0b_0000_0001;
                var result = (r << 1) | c;
                if (_d == 0)
                    _w.Value = result;
                else
                    _registerFile[_f].Value = result;

                // Update C flag.
                c = (result & 0b_1_0000_0000) > 0 ? 1 : 0;
                _status.Value = (_status.Value & 0b_1111_1110) | c;
                return true;
            }

            return false;
        }
    }
}