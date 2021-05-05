// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        TRIS f
    // Code:          000000 000fff
    // Operands:      f = 6
    // Operation:     (W) -> TRIS register f
    // Status bits:   -
    // Description:   TRIS register ‘f’ (f = 6 or 7) is loaded with the contents of the W register.
    
    public class TRIS : Instruction
    {
        private readonly Register[] _trisRegisters;
        private readonly Register _w;
        private int _f;
        
        public TRIS(Microcontroller microcontroller) : base("TRIS")
        {
            _trisRegisters = microcontroller.TrisRegisters;
            _w = microcontroller.AllRegisters["W"];
        }
        
        public override void Prepare(int instructionCode)
        {
            _f = instructionCode & 0b_000000_000111;
        }

        public override bool Execute(bool level, int cycle)
        {
            if (cycle == 3 && level)
            {
                _trisRegisters[_f].Value = _w.Value;
                return true;
            }

            return false;
        }
    }
}