using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator.Instructions
{
    public class InstructionExecutor
    {
        private Register _ir;
        private int _cycle;
        private Instruction _instruction;
        private bool _completed;

        public InstructionExecutor(Microcontroller microcontroller)
        {
            _ir = microcontroller.AllRegisters["IR"];
        }
        
        public void Update(bool level)
        {
            if (level && _cycle == 0)
            {
                var instructionCode = _ir.Value;
                _instruction = new TestInstruction("");    // Decode instruction.
                _completed = false;
                _instruction.Prepare(instructionCode);
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