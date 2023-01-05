// ReSharper disable InconsistentNaming

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        CALL k
    // Code:          1001kk kkkkkk
    // Operands:      k: [0,255]
    // Operation:     (PC) + 1 -> Top-of-Stack;
    //                k -> PC<7:0>;
    //                (STATUS<6:5>) -> PC<10:9>;
    //                0 -> PC<8>
    // Status bits:   -
    // Description:   Subroutine call. First, return address (PC + 1) is PUSHed onto the stack.
    //                The 8-bit immediate address is loaded into PC bits <7:0>. The upper bits
    //                PC<10:9> are loaded from STATUS<6:5>, PC<8> is cleared. CALL is a 2-cycle
    //                instruction.
    
    public class CALL : Instruction
    {
        private readonly HardwareStack _stack;
        private readonly Register _pc;
        private readonly Register _pcl;
        private readonly Register _status;
        private int _k;
        
        public CALL(Microcontroller microcontroller) : base("CALL")
        {
            _stack = microcontroller.Stack;
            _pc = microcontroller.AllRegisters["PC"];
            _pcl = microcontroller.AllRegisters["PCL"];
            _status = microcontroller.AllRegisters["STATUS"];
        }
        
        public override void Prepare(int instructionCode)
        {
            _k = instructionCode & 0b_000011_111111;
        }

        public override bool Execute(int cycle)
        {
            if (cycle == 0)
            {
                _stack.Push(_pc.Value);
                return false;
            }

            _pcl.Value = _pc.Value = ((_status.Value & 0b_0110_0000) << 4) | _k;
            return true;
        }
    }
}