namespace Friendly.Electronics.Simulator.Instructions
{
    public class TestInstruction : Instruction
    {
        public TestInstruction(string name) : base(name)
        {
        }

        public override void Prepare(int instructionCode)
        {
            
        }

        public override bool Execute(int cycle)
        {
            return true;
        }
    }
}