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
            _completed = true;
        }
        
        public void Update()
        {
            // if previous instruction is completed then fetch next instruction.
            if (_completed)
            {
                var instructionCode = _ir.Value;
                _instruction = _instructionDecoder.Decode(instructionCode);    // Decode instruction.
                _instruction.Prepare(instructionCode);
                _completed = false;
                _cycle = 0;
            }
            
            // Execute instruction.
            _completed = _instruction.Execute(_cycle);
            _cycle++;
        }
    }
}