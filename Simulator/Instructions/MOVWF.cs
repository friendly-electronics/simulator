// ReSharper disable InconsistentNaming

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        MOVWF f
    // Code:          000000 1fffff
    // Operands:      f: [0,31]
    // Operation:     (W) -> (f)
    // Status bits:   -
    // Description:   Move data from the W register to register ‘f’.
    
    public class MOVWF : Instruction
    {
        private readonly Register[] _registerFile;
        private readonly Register _w;
        private int _f;
        
        public MOVWF(Microcontroller microcontroller) : base("MOVWF")
        {
            _registerFile = microcontroller.RegisterFile;
            _w = microcontroller.AllRegisters["W"];
        }

        public override void Prepare(int instructionCode)
        {
            _f = instructionCode & 0b_000000_011111;
        }

        public override bool Execute(bool level, int cycle)
        {
            if (cycle == 3 && level)
            {
                _registerFile[_f].Value = _w.Value;
                return true;
            }

            return false;
        }
    }
}