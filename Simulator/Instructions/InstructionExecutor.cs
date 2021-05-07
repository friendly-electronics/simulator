using System;
using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    public class InstructionExecutor
    {
        private readonly InstructionDecoder _instructionDecoder;
        private readonly Register _ir;
        
        private int _cycle;
        private Instruction _instruction;
        private bool _completed;

        public InstructionExecutor(Microcontroller microcontroller, InstructionDecoder instructionDecoder)
        {
            _instructionDecoder = instructionDecoder;
            _ir = microcontroller.AllRegisters["IR"];
        }
        
        public void Update(bool level)
        {
            if (_cycle == 0 && level)
            {
                var instructionCode = _ir.Value;
                _instruction = _instructionDecoder.Decode(instructionCode);    // Decode instruction.
                _instruction.Prepare(instructionCode);
                _completed = false;
                Console.WriteLine(_instruction.Name);
            }

            if (!_completed)
                _completed = _instruction.Execute(level, _cycle);

            if ((_cycle & 0b_11) == 0b_11 && _completed && !level)
                _cycle = 0;
            else if (!level)
                _cycle++;
        }
    }
}