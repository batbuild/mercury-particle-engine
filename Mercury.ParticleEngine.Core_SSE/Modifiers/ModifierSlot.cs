namespace Mercury.ParticleEngine.Modifiers
{
    internal class ModifierSlot
    {
        public Modifier Modifier { get; set; }

        public float Frequency { get; set; }

        private float _secondsSinceLastUpdate;

        public ModifierSlot(Modifier modifier, float frequency = 60f)
        {
            Modifier = modifier;
            Frequency = frequency;
        }

        public unsafe void Update(float elapsedSeconds, ref Particle particle, int index, int count)
        {
            _secondsSinceLastUpdate += elapsedSeconds;

            if (_secondsSinceLastUpdate > (1f / Frequency))
            {
                Modifier.Update(_secondsSinceLastUpdate, ref particle, index, count);

                _secondsSinceLastUpdate = 0f;
            }
        }
    }
}