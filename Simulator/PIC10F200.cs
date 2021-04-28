using System.Collections.Generic;
using Friendly.Electronics.Simulator.Registers;

namespace Friendly.Electronics.Simulator
{
    // ReSharper disable once InconsistentNaming
    public class PIC10F200 : Microcontroller
    {

        public PIC10F200()
            : base()
        {
            Registers = new Dictionary<string, Register>();
            
            Registers.Add("W", new ReadWriteRegister("W", 8));
            Registers.Add("IR", new ReadWriteRegister("IR", 12));
            Registers.Add("OPTION", new ReadWriteRegister("OPTION", 8));
            Registers.Add("INDF", new ReadWriteRegister("INDF", 8));
            Registers.Add("TMR0", new ReadWriteRegister("TMR0", 8));
            Registers.Add("PCL", new ReadWriteRegister("PCL", 8));
            Registers.Add("STATUS", new ReadWriteRegister("STATUS", 8));
            Registers.Add("FSR", new ReadWriteRegister("FSR", 8));
            Registers.Add("OSCCAL", new ReadWriteRegister("OSCCAL", 8));
            Registers.Add("GPIO", new ReadWriteRegister("GPIO", 8));
            Registers.Add("CMCON0", new ReadWriteRegister("CMCON0", 8));
            Registers.Add("GP0", new ReadWriteRegister("GP0", 8));
            Registers.Add("GP1", new ReadWriteRegister("GP1", 8));
            Registers.Add("GP2", new ReadWriteRegister("GP2", 8));
            Registers.Add("GP3", new ReadWriteRegister("GP3", 8));
            Registers.Add("GP4", new ReadWriteRegister("GP4", 8));
            Registers.Add("GP5", new ReadWriteRegister("GP5", 8));
            Registers.Add("GP6", new ReadWriteRegister("GP6", 8));
            Registers.Add("GP7", new ReadWriteRegister("GP7", 8));
            Registers.Add("GP8", new ReadWriteRegister("GP8", 8));
            Registers.Add("GP9", new ReadWriteRegister("GP9", 8));
            Registers.Add("GP10", new ReadWriteRegister("GP10", 8));
            Registers.Add("GP11", new ReadWriteRegister("GP11", 8));
            Registers.Add("GP12", new ReadWriteRegister("GP12", 8));
            Registers.Add("GP13", new ReadWriteRegister("GP13", 8));
            Registers.Add("GP14", new ReadWriteRegister("GP14", 8));
            Registers.Add("GP15", new ReadWriteRegister("GP15", 8));
            Registers.Add("Unimplemented", new ReadOnlyRegister("Unimplemented", 8));

            RegisterFile = new[]
            {
                Registers["INDF"],
                Registers["TMR0"],
                Registers["PCL"],
                Registers["STATUS"],
                Registers["FSR"],
                Registers["OSCCAL"],
                Registers["GPIO"],
                Registers["CMCON0"],
                Registers["Unimplemented"],
                Registers["Unimplemented"],
                Registers["Unimplemented"],
                Registers["Unimplemented"],
                Registers["Unimplemented"],
                Registers["Unimplemented"],
                Registers["Unimplemented"],
                Registers["Unimplemented"],
                Registers["GP0"],
                Registers["GP1"],
                Registers["GP2"],
                Registers["GP3"],
                Registers["GP4"],
                Registers["GP5"],
                Registers["GP6"],
                Registers["GP7"],
                Registers["GP8"],
                Registers["GP9"],
                Registers["GP10"],
                Registers["GP11"],
                Registers["GP12"],
                Registers["GP13"],
                Registers["GP14"],
                Registers["GP15"]
            };
        }
    }
}