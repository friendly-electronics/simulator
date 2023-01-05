// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        ADDWF f, d
    // Code:          000111 dfffff
    // Operands:      f: [0,31]
    //                d: [0,1]
    // Operation:     (W) + (f) -> (dest)
    // Status bits:   C, DC, Z
    // Description:   Add the contents of the W register and register ‘f’. If ‘d’ is ‘0’, the
    //                result is stored in the W register. If ‘d’ is ‘1’, the result is stored
    //                back in register ‘f’.
    
    public class ADDWF : Instruction
    {
        private readonly Register[] _registerFile;
        private readonly Register _w;
        private readonly Register _status;
        private int _f;
        private int _d;
        
        public ADDWF(Microcontroller microcontroller) : base("ADDWF")
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

        public override bool Execute(int cycle)
        {
            // Execute Operation.
            var w = _w.Value;
            var r = _registerFile[_f].Value;
            var result = w + r;
            if (_d == 0)
                _w.Value = result;
            else
                _registerFile[_f].Value = result;

            // Update C, DC, Z flags.
            var z = (result & 0b_1111_1111) == 0 ? 0b_0000_0100 : 0;
            var dc = (w & 0b_1111) + (r & 0b_1111) > 0xF ? 0b_0000_0010 : 0;
            var c = result > 0xFF ? 0b_0000_0001 : 0;
            _status.Value = (_status.Value & 0b_1111_1000) | z | dc | c;
            return true;
        }
    }
}