using System.Collections.Generic;
using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator
{
    public class Microcontroller
    {
        protected internal Dictionary<string, Register> AllRegisters;
        protected internal Register[] RegisterFile;
        protected internal Register[] TrisRegisters;
        protected internal Register[] ProgramMemory;
    }
}