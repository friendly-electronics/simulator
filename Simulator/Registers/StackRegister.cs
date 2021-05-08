namespace Friendly.Electronics.Simulator.Registers
{
    public class StackRegister : Register
    {
        private HardwareStack _stack;
        private int _index;
        
        public StackRegister(string name, int bits, int value, HardwareStack stack, int index) : base(name, bits)
        {
            _stack = stack;
            _index = index;
            _stack[_index] = value;
        }

        public override int Get() => _stack[_index];

        public override void Set(int value) => _stack[_index] = value;
    }
}