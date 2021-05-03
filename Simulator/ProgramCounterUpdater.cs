using System;
using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator
{
    public class ProgramCounterUpdater
    {
        private readonly Register _pc;
        private readonly Register _pcl;
        private readonly Register _ir;
        private readonly Register[] _programMemory;
        private int _cycle;
        
        public ProgramCounterUpdater(Microcontroller microcontroller)
        {
            _pc = microcontroller.AllRegisters["PC"];
            _pcl = microcontroller.AllRegisters["PCL"];
            _ir = microcontroller.AllRegisters["IR"];
            _programMemory = microcontroller.ProgramMemory;
        }
        
        public void Update(bool level)
        {
            if (_cycle == 3 && !level)
            {
                var pc = _pc.Value;
                pc++;
                _pc.Value = pc;
                _pcl.Value = pc;
                _ir.Value = _programMemory[_pc.Value].Value;
                Console.WriteLine("---");
            }
            if (!level)
                _cycle = (_cycle + 1) & 0b_11;
        }
    }
}