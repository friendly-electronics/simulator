// ReSharper disable InconsistentNaming

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        XORWF f, d
    // Code:          000110 dfffff
    // Operands:      f: [0,31]
    //                d: [0,1]
    // Operation:     (W) XOR (f) ->(dest)
    // Status bits:   Z
    // Description:   Exclusive OR the contents of the W register with register ‘f’.
    //                If ‘d’ is ‘0’, the result is stored in the W register. If ‘d’ is ‘1’,
    //                the result is stored back in register ‘f’.
    
    public class XORWF : Instruction
    {
        private readonly Register[] _registerFile;
        private readonly Register _w;
        private readonly Register _status;
        private int _f;
        private int _d;
        
        public XORWF(Microcontroller microcontroller) : base("XORWF")
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
                var w = _w.Value;
                var r = _registerFile[_f].Value;
                var result = w ^ r;
                if (_d == 0)
                    _w.Value = result;
                else
                    _registerFile[_f].Value = result;
            
                // Update Z flags.
                var z = result == 0 ? 0b_0000_0100 : 0;
                _status.Value = (_status.Value & 0b_1111_1011) | z;
                return true;
            }

            return false;
        }
    }
}