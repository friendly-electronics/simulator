using System.Linq;
using System.Reflection;
using Friendly.Electronics.Simulator.Registers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friendly.Electronics.Simulator.Tests
{
    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class PIC10F200SpecsTests
    {
        [TestMethod]
        public void SpecsShouldBeCorrect()
        {
            var micro = new PIC10F200();

            // DATA MEMORY.
            var registerFile = GetPrivateValue<Register[]>(micro, "RegisterFile");
            var dataMemorySize = registerFile.Select(r => r.Name).Distinct().Count(r => r.StartsWith("GP") && r[2] >= '0' && r[2] <= '9');
            Assert.AreEqual(16, dataMemorySize, "Incorrect Data Memory size.");
            Assert.AreEqual(32, registerFile.Length, "Incorrect Register FIle size.");
            
            // PROGRAM MEMORY.
            var programMemory = GetPrivateValue<Register[]>(micro, "ProgramMemory");
            var programMemorySize = programMemory.Select(r => r.Name).Distinct().Count(r => r.StartsWith("PM"));
            Assert.AreEqual(256, programMemorySize, "Incorrect Program Memory size.");
            Assert.AreEqual(512, programMemory.Length, "Incorrect addressable Program Memory size.");
        }

        private T GetPrivateValue<T>(object obj, string field)
        {
            return (T)obj.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(f => f.Name == field)
                .GetValue(obj);
        }
    }
}