using System.Collections.Generic;
using Friendly.Electronics.Simulator.Instructions;
using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator
{
    public abstract class Microcontroller
    {
        protected internal Dictionary<string, Register> AllRegisters;
        protected internal Dictionary<string, Instruction> AllInstructions;
        protected internal Register[] RegisterFile;
        protected internal Register[] TrisRegisters;
        protected internal Register[] ProgramMemory;
        protected internal HardwareStack Stack;
        protected internal InternalOscillator Oscillator;

        protected event LogicLevelChanged Clock;

        private bool _currentClockLevel;
        protected void OnClock(bool level)
        {
            if (_currentClockLevel == level) return;
            _currentClockLevel = level;
            Clock?.Invoke(level);
        }

        public virtual void Program(int address, int value)
        {
            ProgramMemory[address].Value = value;
        }
    }
}