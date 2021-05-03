using System.Collections.ObjectModel;
using System.Linq;
using Friendly.Electronics.Simulator.Instructions;
using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator
{
    public class MicrocontrollerDebugger
    {
        public readonly Microcontroller Microcontroller;
        public readonly ReadOnlyDictionary<string, Register> AllRegisters;
        public readonly ReadOnlyDictionary<string, Instruction> AllInstructions;
        public readonly ReadOnlyCollection<Register> RegisterFile;
        public readonly ReadOnlyCollection<Register> TrisRegisters;
        public readonly ReadOnlyCollection<Register> ProgramMemory;

        public MicrocontrollerDebugger(Microcontroller microcontroller)
        {
            Microcontroller = microcontroller;
            AllRegisters = new ReadOnlyDictionary<string, Register>(microcontroller.AllRegisters);
            AllInstructions = new ReadOnlyDictionary<string, Instruction>(microcontroller.AllInstructions);
            RegisterFile = new ReadOnlyCollection<Register>(microcontroller.RegisterFile);
            TrisRegisters = new ReadOnlyCollection<Register>(microcontroller.TrisRegisters);
            ProgramMemory = new ReadOnlyCollection<Register>(microcontroller.ProgramMemory);
        }
    }
}