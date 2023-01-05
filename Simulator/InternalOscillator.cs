namespace Friendly.Electronics.Simulator
{
    public class InternalOscillator
    {
        //private float _frequency;
        private bool _enabled;

        //public double Frequency => _frequency;
        
        public InternalOscillator()
        {
        }

        private void DoStep()
        {
            if (!_enabled) return;
        }

        public void Start()
        {
            _enabled = true;
        }

        public void Stop()
        {
            _enabled = false;
        }

    }
}