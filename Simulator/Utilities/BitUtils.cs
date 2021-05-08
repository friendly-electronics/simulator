namespace Friendly.Electronics.Simulator.Utilities
{
    public static class BitUtils
    {
        public static bool IsPowerOfTwo(int value) => (value & (value - 1)) == 0;
        public static int CreateOneBitMask(int bit) => 1 << bit;
        public static int CreateReminderMask(int value) => value - 1;
    }
}