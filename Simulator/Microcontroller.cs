using System.Collections.Generic;
using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator
{
    public class Microcontroller
    {
        protected Dictionary<string, Register> Registers;
        protected Register[] RegisterFile; 
    }
}