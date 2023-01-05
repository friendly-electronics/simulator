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
        
        public void Update()
        {
            if (_cycle == 0)
            {
                var instructionCode = _ir.Value;
                _instruction = _instructionDecoder.Decode(instructionCode);    // Decode instruction.
                _instruction.Prepare(instructionCode);
                _completed = false;
            }

            if (!_completed)
                _completed = _instruction.Execute(true, _cycle);

            if (!_completed)
                _cycle++;
        }
    }
}