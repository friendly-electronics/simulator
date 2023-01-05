// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        SUBWF f, d
    // Code:          000010 dfffff
    // Operands:      f: [0,31]
    //                d: [0,1]
    // Operation:     (f) – (W) -> (dest)
    // Status bits:   C, DC, Z
    // Description:   Subtract (2’s complement method) the W register from register ‘f’.
    //                If ‘d’ is ‘0’, the result is stored in the W register. If ‘d’ is ‘1’,
    //                the result is stored back in register ‘f’.
    
    public class SUBWF : Instruction
    {
        private readonly Register[] _registerFile;
        private readonly Register _w;
        private readonly Register _status;
        private int _f;
        private int _d;
        
        public SUBWF(Microcontroller microcontroller) : base("SUBWF")
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
            var result = r - w;
            if (_d == 0)
                _w.Value = result;
            else
                _registerFile[_f].Value = result;

            // Update C, DC, Z flags.
            var z = (result & 0b_1111_1111) == 0 ? 0b_0000_0100 : 0;
            var dc = (r & 0b_1111) - (w & 0b_1111) >= 0x0 ? 0b_0000_0010 : 0;
            var c = result >= 0x00 ? 0b_0000_0001 : 0;
            _status.Value = (_status.Value & 0b_1111_1000) | z | dc | c;
            return true;
        }
    }
}