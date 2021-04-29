using System.Collections.Generic;
using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator
{
    // ReSharper disable once InconsistentNaming
    public class PIC10F200 : Microcontroller
    {

        public PIC10F200()
        {
            // ALL REGISTERS.
            AllRegisters = new Dictionary<string, Register>();
            AllRegisters.Add("W", new ReadWriteRegister("W", 8));
            AllRegisters.Add("IR", new ReadWriteRegister("IR", 12));
            AllRegisters.Add("OPTION", new ReadWriteRegister("OPTION", 8));
            AllRegisters.Add("INDF", new ReadWriteRegister("INDF", 8));
            AllRegisters.Add("TMR0", new ReadWriteRegister("TMR0", 8));
            AllRegisters.Add("PCL", new ReadWriteRegister("PCL", 8));
            AllRegisters.Add("STATUS", new ReadWriteRegister("STATUS", 8));
            AllRegisters.Add("FSR", new ReadWriteRegister("FSR", 8));
            AllRegisters.Add("OSCCAL", new ReadWriteRegister("OSCCAL", 8));
            AllRegisters.Add("GPIO", new ReadWriteRegister("GPIO", 8));
            AllRegisters.Add("TRISGPIO", new ReadWriteRegister("TRISGPIO", 8));
            AllRegisters.Add("CMCON0", new ReadWriteRegister("CMCON0", 8));
            AllRegisters.Add("Unimplemented", new ReadOnlyRegister("Unimplemented", 8));
            for (var i = 0; i < 16; i++)
                AllRegisters.Add($"GP{i.ToString()}", new ReadWriteRegister($"GP{i.ToString()}", 8));
            for (var i = 0; i < 256; i++)
                AllRegisters.Add($"PM{i.ToString()}", new ReadWriteRegister($"PM{i.ToString()}", 8));

            // REGISTER FILE.
            RegisterFile = new Register[32];
            RegisterFile[0x00] = AllRegisters["INDF"];
            RegisterFile[0x01] = AllRegisters["TMR0"];
            RegisterFile[0x02] = AllRegisters["PCL"];
            RegisterFile[0x03] = AllRegisters["STATUS"];
            RegisterFile[0x04] = AllRegisters["FSR"];
            RegisterFile[0x05] = AllRegisters["OSCCAL"];
            RegisterFile[0x06] = AllRegisters["GPIO"];
            RegisterFile[0x07] = AllRegisters["CMCON0"];
            for (var i = 0x08; i <= 0x0F; i++)
                RegisterFile[i] = AllRegisters["Unimplemented"];
            for (var i = 0; i < 16; i++)
                RegisterFile[0x10 + i] = AllRegisters[$"GP{i.ToString()}"];
            
            TrisRegisters = new Register[8];
            for (var i = 0; i < 8; i++)
                TrisRegisters[i] = AllRegisters["Unimplemented"];
            TrisRegisters[0x06] = AllRegisters["TRISGPIO"];

            // PROGRAM MEMORY.
            ProgramMemory = new Register[512];
            for (var i = 0; i < 512; i++)
                ProgramMemory[i] = AllRegisters[$"PM{(i % 256).ToString()}"];
        }
    }
}