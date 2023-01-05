// ReSharper disable InconsistentNaming
// ReSharper disable InvertIf

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        GOTO k
    // Code:          101kkk kkkkkk
    // Operands:      k: [0,511]
    // Operation:     k -> PC<8:0>;
    //                STATUS<6:5> -> PC<10:9>
    // Status bits:   -
    // Description:   GOTO is an unconditional branch. The 9-bit immediate value is loaded
    //                into PC bits <8:0>. The upper bits of PC are loaded from STATUS<6:5>.
    //                GOTO is a 2-cycle instruction.
    
    public class GOTO : Instruction
    {
        private readonly Register _pc;
        private readonly Register _pcl;
        private readonly Register _status;
        private int _k;
            
        public GOTO(Microcontroller microcontroller) : base("GOTO")
        {
            _pc = microcontroller.AllRegisters["PC"];
            _pcl = microcontroller.AllRegisters["PCL"];
            _status = microcontroller.AllRegisters["STATUS"];
        }
        
        public override void Prepare(int instructionCode)
        {
            _k = instructionCode & 0b_000111_111111;
        }

        public override bool Execute(int cycle)
        {
            if (cycle == 0)
                return false;

            _pcl.Value = _pc.Value = ((_status.Value & 0b_0110_0000) << 4) | _k;
            return true;
        }
    }
}