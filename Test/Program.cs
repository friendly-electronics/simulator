using System;
using Friendly.Electronics.Simulator;

namespace Test
{
    internal static class Program
    {
        private static void Main()
        {
            var micro = new PIC10F200();
            
            var speed = 1.0; // 1.0 / 1000000 * 4;    // 1 instruction / sec.
            var runTime = 4000L * 5;
            Clock.Run(false, speed, runTime);
            
            Console.WriteLine("Finished!");
        }
    }
}