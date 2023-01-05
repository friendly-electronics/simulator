// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        MOVF f, d
    // Code:          001000 dfffff
    // Operands:      f: [0,31]
    //                d: [0,1]
    // Operation:     (f) -> (dest)
    // Status bits:   Z
    // Description:   The contents of register ‘f’ are moved to destination ‘d’. If ‘d’ is ‘0’,
    //                destination is the W register. If ‘d’ is ‘1’, the destination is file
    //                register ‘f’. ‘d’ = 1 is useful as a test of a file register, since status
    //                flag Z is affected.
    
    public class MOVF : Instruction
    {
        private readonly Register[] _registerFile;
        private readonly Register _w;
        private readonly Register _status;
        private int _f;
        private int _d;

        public MOVF(Microcontroller microcontroller) : base("MOVF")
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
            if (cycle == 3)
            {
                // Execute Operation.
                var result = _registerFile[_f].Value;
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