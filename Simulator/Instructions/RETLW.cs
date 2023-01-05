// ReSharper disable InconsistentNaming

using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    
    // Syntax:        RETLW k
    // Code:          1000kk kkkkkk
    // Operands:      k: [0,255]
    // Operation:     k -> (W);
    //                TOS -> PC
    // Status bits:   -
    // Description:   The W register is loaded with the 8-bit literal ‘k’. The program
    //                counter is loaded from the top of the stack (the return address).
    //                This is a 2-cycle instruction.
    
    public class RETLW : Instruction
    {
        private readonly HardwareStack _stack;
        private readonly Register _w;
        private readonly Register _pc;
        private readonly Register _pcl;
        private int _k;

        public RETLW(Microcontroller microcontroller) : base("RETLW")
        {
            _stack = microcontroller.Stack;
            _w = microcontroller.AllRegisters["W"];
            _pc = microcontroller.AllRegisters["PC"];
            _pcl = microcontroller.AllRegisters["PCL"];
        }

        public override void Prepare(int instructionCode)
        {
            _k = instructionCode & 0b_000011_111111;
        }

        public override bool Execute(int cycle)
        {
            switch (cycle)
            {
                case 3:
                    _w.Value = _k;
                    return false;
                case 7:
                    _pcl.Value = _pc.Value = _stack.Pop();
                    return true;
                default:
                    return false;
            }
        }
    }
}