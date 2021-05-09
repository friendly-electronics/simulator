using System;
using Friendly.Electronics.Simulator;

namespace Test
{
    internal static class Program
    {
        private static void Main()
        {
            var micro = new PIC10F200();
            var debugger = new MicrocontrollerDebugger(micro);
            ProgramMicro(micro);
            Console.Clear();
            
            var speed = 1.0; // 1.0 / 1000000 * 4;    // 1 instruction / sec.
            var runTime = 16666666;

            while (true)
            {
                Clock.Run(true, speed, runTime);
                PrintStatus(debugger);
            }
            
        }

        private static void PrintStatus(MicrocontrollerDebugger debugger)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write($"Time: {Clock.Now / 1000000000.0:F2}");
            for (var i = 0; i < debugger.RegisterFile.Count; i++)
            {
                var register = debugger.RegisterFile[i];
                //if (register.Name != "GPIO") continue;
                Console.SetCursorPosition(0, i+1);
                Console.Write(register.Name);
                Console.SetCursorPosition(16, i+1);
                Console.Write(Convert.ToString(register.Value, 2).PadLeft(8, '0'));
            }
        }

        private static void ProgramMicro(Microcontroller microcontroller)
        {
            var filename = $"test.hex";
            var records = HexUtils.Load(filename);
            var baseAddress = 0;
            var address = 0;
            foreach (var record in records)
            {
                switch (record.Type)
                {
                    case 0:
                        address = record.Address;
                        for (var i = 0; i < record.Data.Length; i++)
                            microcontroller.Program(baseAddress + address + i, record.Data[i]);
                        break;
                    case 1:
                        break;
                    case 4:
                        baseAddress = (record.Data[0] << 24) | (record.Data[1] << 16);
                        break;
                }
            }
        }
    }
}