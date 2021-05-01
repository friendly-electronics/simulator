namespace Friendly.Electronics.Simulator
{
    public class InternalOscillator : IClockable
    {
        //private float _frequency;
        private int _step;
        private bool _level;
        private bool _enabled;

        //public double Frequency => _frequency;
        
        public InternalOscillator(float frequency)
        {
            ChangeFrequency(frequency);
        }

        public event LogicLevelChanged LogicLevelChanged;

        private void DoStep()
        {
            if (!_enabled) return;
            _level = !_level;
            LogicLevelChanged?.Invoke(_level);
            Clock.AddEvent(this, _step, 0);
        }

        public void Start()
        {
            _enabled = true;
            Clock.AddEvent(this, 0, 0);
        }

        public void Stop()
        {
            _enabled = false;
            _level = false;
        }

        private void ChangeFrequency(float frequency)
        {
            //_frequency = frequency;
            _step = (int)(1000000000L / frequency / 2.0f);
        }

        public void Update(long time, int param)
        {
            DoStep();
        }
        
    }

    public delegate void LogicLevelChanged(bool level);
}