using System;
using System.Collections.Generic;
using Friendly.Electronics.Simulator.Instructions;
using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator
{
    // ReSharper disable once InconsistentNaming
    public class PIC10F200 : Microcontroller
    {
        private ProgramCounterUpdater _programCounterUpdater;
        private InstructionDecoder _instructionDecoder;
        private InstructionExecutor _instructionExecutor;
        
        public PIC10F200()
        {
            // ALL REGISTERS.
            AllRegisters = new Dictionary<string, Register>();
            AllRegisters.Add("W", new ReadWriteRegister("W", 8));
            AllRegisters.Add("IR", new ReadWriteRegister("IR", 12));
            AllRegisters.Add("PC", new ReadWriteRegister("PC", 9, 0b_1_1111_1111));
            AllRegisters.Add("OPTION", new ReadWriteRegister("OPTION", 8, 0b_1111_1111));
            AllRegisters.Add("INDF", new ReadWriteRegister("INDF", 8));
            AllRegisters.Add("TMR0", new ReadWriteRegister("TMR0", 8));
            AllRegisters.Add("PCL", new ReadWriteRegister("PCL", 8, 0b_1111_1111));
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
                AllRegisters.Add($"PM{i.ToString()}", new ReadWriteRegister($"PM{i.ToString()}", 12));

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
            
            // INSTRUCTIONS.
            AllInstructions = new Dictionary<string, Instruction>();
            AllInstructions.Add("NOP", new NOP());
            AllInstructions.Add("OPTION", new OPTION(this));
            AllInstructions.Add("TRIS", new TRIS(this));
            AllInstructions.Add("MOVWF", new MOVWF(this));
            AllInstructions.Add("CLRW", new CLRW(this));
            AllInstructions.Add("CLRF", new CLRF(this));
            AllInstructions.Add("ADDWF", new ADDWF(this));
            AllInstructions.Add("SUBWF", new SUBWF(this));
            AllInstructions.Add("ANDWF", new ANDWF(this));
            AllInstructions.Add("IORWF", new IORWF(this));
            AllInstructions.Add("XORWF", new XORWF(this));
            AllInstructions.Add("ANDLW", new ANDLW(this));
            AllInstructions.Add("IORLW", new IORLW(this));
            AllInstructions.Add("XORLW", new XORLW(this));

            // Internal Oscillator.
            Oscillator = new InternalOscillator(1000000);
            Oscillator.LogicLevelChanged += OnClock;
            
            _instructionDecoder = new InstructionDecoder(this);

            _instructionExecutor = new InstructionExecutor(this, _instructionDecoder);
            Clock += _instructionExecutor.Update;
            
            _programCounterUpdater = new ProgramCounterUpdater(this);
            Clock += level => { if (level) Console.WriteLine($"Clock: {(Simulator.Clock.Now / 1000).ToString()}: PC: {AllRegisters["PC"].Value.ToString("X4")}, IR: {AllRegisters["IR"].Value.ToString("X4")}"); };
            Clock += _programCounterUpdater.Update;
            
            Oscillator.Start();
        }
    }
}