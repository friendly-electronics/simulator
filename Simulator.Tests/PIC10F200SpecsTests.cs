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
            Assert.AreEqual(32, GetPrivateValue<Register[]>(micro, "RegisterFile").Length, "Incorrect Register FIle size.");
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