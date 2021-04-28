namespace Friendly.Electronics.Simulator.Registers
{
    public abstract class Register
    {
        protected Register(string name, int bits)
        {
            Name = name;
            Bits = bits;
        }
        
        public string Name { get; }
        public int Bits { get; }
        
        public int Value
        {
            get => Get();
            set => Set(value);
        }

        public abstract int Get();
        public abstract void Set(int value);
    }
}