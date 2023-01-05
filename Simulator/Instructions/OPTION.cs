// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        OPTION
    // Code:          000000 000010
    // Operands:      -
    // Operation:     (W) -> OPTION
    // Status bits:   -
    // Description:   The content of the W register is loaded into the OPTION register.
    
    public class OPTION : Instruction
    {
        private readonly Register _w;
        private readonly Register _option;

        public OPTION(Microcontroller microcontroller) : base("OPTION")
        {
            _w = microcontroller.AllRegisters["W"];
            _option = microcontroller.AllRegisters["OPTION"];
        }

        public override void Prepare(int instructionCode)
        {
        }

        public override bool Execute(int cycle)
        {
            _option.Value = _w.Value;
            return true;
        }
    }
}