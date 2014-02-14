namespace Mercury.ParticleEngine
{
    using System;
    using Mercury.ParticleEngine.Modifiers;
    using Mercury.ParticleEngine.Profiles;

    public unsafe class Emitter : IDisposable
    {
        public Emitter(int capacity, TimeSpan term, Profile profile)
        {
            _term = (float)term.TotalSeconds;

            Buffer = new ParticleBuffer(capacity);
            Profile = profile;
            Modifiers = new ModifierCollection();
            ModifierExecutionStrategy = ModifierExecutionStrategy.Parallel;
            Parameters = new ReleaseParameters();
        }

        private readonly float _term;
        private float _totalSeconds;

        internal ParticleBuffer Buffer { get; private set; }

        public int ActiveParticles
        {
            get { return Buffer.Count; }
        }

        public ModifierCollection Modifiers { get; set; }
        public ModifierExecutionStrategy ModifierExecutionStrategy { get; set; }
        public Profile Profile { get; private set; }
        public ReleaseParameters Parameters { get; set; }
        public BlendMode BlendMode { get; set; }

        public float ReclaimInterval { get; set; }

        private float _secondsSinceLastReclaim;

        private void ReclaimExpiredParticles()
        {
	        var index = Buffer.Index;
	        var particle = Buffer.Particles;
            var count = Buffer.Count;

            var expired = 0;
            
            while (count-- > 0)
            {
                if ((_totalSeconds - particle.Inception[index]) < _term)
                    break;
                
                expired++;
                index++;
            }

            Buffer.Reclaim(expired);
        }

        public void Update(float elapsedSeconds)
        {
            _totalSeconds += elapsedSeconds;
            _secondsSinceLastReclaim += elapsedSeconds;

            if (Buffer.Count == 0)
                return;

            if (_secondsSinceLastReclaim > ReclaimInterval)
            {
                ReclaimExpiredParticles();
                _secondsSinceLastReclaim = 0;
            }

            if (Buffer.Count > 0)
            {
                var particle = Buffer.Particles;
	            var index = Buffer.Index;
                var count = Buffer.Count;

                ModifierExecutionStrategy.ExecuteModifiers(Modifiers, elapsedSeconds, ref particle, index, count);
            }
        }

        public void Trigger(Coordinate position)
        {
            var numToRelease = FastRand.NextInteger(Parameters.Quantity);

            int i;
            var count = Buffer.Release(numToRelease, out i);
	        var particle = Buffer.Particles;

            while (count-- > 0)
            {
	            var coordinate = new Coordinate(particle.X[i], particle.Y[i]);
				var axis = new Axis(particle.VX[i], particle.VY[i]);
				Profile.GetOffsetAndHeading(&coordinate, &axis);

                particle.Age[i] = 0f;
                particle.Inception[i] = _totalSeconds;

                particle.X[i] += position._x;
                particle.Y[i] += position._y;

                var speed = FastRand.NextSingle(Parameters.Speed);

                particle.VX[i] *= speed;
                particle.VY[i] *= speed;

	            var r = particle.R[i];
	            var g = particle.G[i];
	            var b = particle.B[i];
	            FastRand.NextColour(ref r, ref g, ref b, Parameters.Colour);

	            particle.R[i] = r;
	            particle.G[i] = g;
	            particle.B[i] = b;
                
                particle.Opacity[i]	= FastRand.NextSingle(Parameters.Opacity);
				particle.Scale[i]		= FastRand.NextSingle(Parameters.Scale);
				particle.Rotation[i]	= FastRand.NextSingle(Parameters.Rotation);
				particle.Mass[i]		= FastRand.NextSingle(Parameters.Mass);

                i++;
            }
        }

        public void Dispose()
        {
            Buffer.Dispose();
            GC.SuppressFinalize(this);
        }

        ~Emitter()
        {
            Dispose();
        }
    }
}