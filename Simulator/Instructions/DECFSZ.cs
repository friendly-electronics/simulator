// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf
// ReSharper disable ConvertIfStatementToReturnStatement

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        DECFSZ f, d
    // Code:          001011 dfffff
    // Operands:      f: [0,31]
    //                d: [0,1]
    // Operation:     (f) – 1 -> (dest); skip if result = 0
    // Status bits:   -
    // Description:   The contents of register ‘f’ are decremented. If ‘d’ is ‘0’,
    //                the result is placed in the W register. If ‘d’ is ‘1’, the result
    //                is placed back in register ‘f’. If the result is ‘0’, the next
    //                instruction, which is already fetched, is discarded and a NOP
    //                is executed instead making it a 2-cycle instruction.
    
    public class DECFSZ : Instruction
    {
        private readonly Register[] _registerFile;
        private readonly Register _w;
        private readonly Register _status;
        private int _f;
        private int _d;
        
        public DECFSZ(Microcontroller microcontroller) : base("DECFSZ")
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
                var r = _registerFile[_f].Value;
                var result = r - 1;
                if (_d == 0)
                    _w.Value = result;
                else
                    _registerFile[_f].Value = result;
            
                // Complete instruction execution if result is not zero.
                if ((result & 0b_1111_1111) != 0)
                    return true;
            }

            if (cycle >= 4)
                return true;

            return false;
        }
    }
}