using System.Diagnostics;

namespace Friendly.Electronics.Simulator.Registers
{
    [DebuggerDisplay("{Name} ({Bits}bits) R/W: {Value}")]
    public class ReadWriteRegister : Register
    {
        private int _value;
        private readonly int _mask;

        public ReadWriteRegister(string name, int bits, int value = 0)
            : base(name, bits)
        {
            var mask = 1;
            for (var i = 0; i < bits; i++)
            {
                _mask |= mask;
                mask <<= 1;
            }
            _value = value & mask;
        }
        
        public override int Get()
        {
            return _value;
        }

        public override void Set(int value)
        {
            _value = value & _mask;
        }
    }
}