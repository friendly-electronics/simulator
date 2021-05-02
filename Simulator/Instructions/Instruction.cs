namespace Friendly.Electronics.Simulator.Instructions
{
    public abstract class Instruction
    {
        protected Instruction(string name)
        {
            Name = name;
        }
        
        public string Name { get; }

        public abstract void Prepare(int instructionCode);
        public abstract bool Execute(bool level, int cycle);
    }
}