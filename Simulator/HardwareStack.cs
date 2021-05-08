using System;
using Friendly.Electronics.Simulator.Utilities;

namespace Friendly.Electronics.Simulator
{
    public class HardwareStack
    {
        private readonly int _capacity;
        private int _top;
        private readonly int[] _items;
        private readonly int _mask;

        public HardwareStack(int capacity)
        {
            if (!BitUtils.IsPowerOfTwo(capacity))
                throw new ArgumentException("Capacity is not a power of 2.");
            _mask = BitUtils.CreateReminderMask(capacity);
            _items = new int[capacity];
            _capacity = capacity;
            _top = 0;
        }

        public int Capacity => _capacity;

        public void Push(int value)
        {
            _top = (_top - 1) & _mask;
            _items[_top] = value;
        }

        public int Pop()
        {
            var result = _items[_top];
            _items[_top] = _items[(_top - 1) & _mask];
            _top = (_top + 1) & _mask;
            return result;
        }

        public int this[int i]
        {
            get => _items[(_top + i) & _mask];
            set => _items[(_top + i) & _mask] = value;
        }
    }
}