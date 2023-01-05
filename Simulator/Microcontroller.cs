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

        public virtual void Program(int address, int value)
        {
            ProgramMemory[address].Value = value;
        }
    }
}