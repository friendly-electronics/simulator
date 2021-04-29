using System.Diagnostics;

namespace Friendly.Electronics.Simulator.Registers
{
    [DebuggerDisplay("{Name} ({Bits}bits) RO: {Value}")]
    public class ReadOnlyRegister : ReadWriteRegister
    {
        public ReadOnlyRegister(string name, int bits, int value = 0)
            : base(name, bits, value)
        {
        }

        public override void Set(int value)
        {
            // Intentionally do nothing.
        }
    }
}