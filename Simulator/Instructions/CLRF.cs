// ReSharper disable InconsistentNaming

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        CLRF f
    // Code:          000001 1fffff
    // Operands:      f: [0,31]
    // Operation:     00h -> (f);
    //                1 -> Z
    // Status bits:   Z
    // Description:   The contents of register ‘f’ are cleared and the Z bit is set.
    
    public class CLRF : Instruction
    {
        private readonly Register[] _registerFile;
        private readonly Register _status;
        private int _f;
        
        public CLRF(Microcontroller microcontroller) : base("CLRF")
        {
            _registerFile = microcontroller.RegisterFile;
            _status = microcontroller.AllRegisters["STATUS"];
        }
        
        public override void Prepare(int instructionCode)
        {
            _f = instructionCode & 0b_011111;
        }

        public override bool Execute(bool level, int cycle)
        {
            if (cycle == 3 && level)
            {
                // Execute Operation.
                _registerFile[_f].Value = 0;
            
                // Update C, DC, Z flags.
                var z = 0b_0000_0100;
                _status.Value = (_status.Value & 0b_1111_1011) | z;
                return true;
            }

            return false;
        }
    }
}