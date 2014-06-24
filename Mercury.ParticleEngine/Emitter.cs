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

        public ParticleBuffer Buffer { get; private set; }

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
	        var i = 0;
	        var particle = Buffer.Particles;
            var count = Buffer.Count;

            var expired = 0;
            
            while (count-- > 0)
            {
                if ((_totalSeconds - particle.Inception[i]) < _term)
                    break;
                
                expired++;
                i++;
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

            var count = Buffer.Release(numToRelease);
			var i = Buffer.Index;
	        var particle = Buffer.Particles;

            while (count-- > 0)
            {
				Profile.GetOffsetAndHeading(ref particle, i);

                particle.Age[i] = 0f;
                particle.Inception[i] = _totalSeconds;

                particle.X[i] += position._x;
                particle.Y[i] += position._y;

                var speed = FastRand.NextSingle(Parameters.Speed);

                particle.VX[i] *= speed;
                particle.VY[i] *= speed;

	            float r = 0;
	            float g = 0;
	            float b = 0;
	            FastRand.NextColour(ref r, ref g, ref b, Parameters.Colour);

	            particle.R[i] = r;
	            particle.G[i] = g;
	            particle.B[i] = b;
                
                particle.Opacity[i]		= FastRand.NextSingle(Parameters.Opacity);
				particle.Scale[i]		= FastRand.NextSingle(Parameters.Scale);
				particle.Rotation[i]	= FastRand.NextSingle(Parameters.Rotation);
				particle.Mass[i]		= FastRand.NextSingle(Parameters.Mass);

                i++;
            }
        }

		/// <summary>
		/// Use this overload of Trigger when you want the emitter to interpolate the emission position between two points.
		/// Useful for fast moving emitters where the particles would otherwise be emitted in clumps.
		/// Note that the horrible duplication of code between the overloads of Trigger is because this is performance
		/// critical code, and I don't want to introduce even extra method calls if it can be avoided.
		/// </summary>
		/// <param name="startPosition"></param>
		/// <param name="endPosition"></param>
	    public void Trigger(Coordinate startPosition, Coordinate endPosition)
	    {
			var numToRelease = FastRand.NextInteger(Parameters.Quantity);

			var count = Buffer.Release(numToRelease);
			var i = Buffer.Index;
			var particle = Buffer.Particles;
		    var t = 0f;
		    var totalToRelease = count;
		    var released = 0f;

			while (count-- > 0)
			{
				var positionX = startPosition._x + (endPosition._x - startPosition._x) * t;
				var positionY = startPosition._y + (endPosition._y - startPosition._y) * t;

				Profile.GetOffsetAndHeading(ref particle, i);

				particle.Age[i] = 0f;
				particle.Inception[i] = _totalSeconds;

				particle.X[i] += positionX;
				particle.Y[i] += positionY;

				var speed = FastRand.NextSingle(Parameters.Speed);

				particle.VX[i] *= speed;
				particle.VY[i] *= speed;

				float r = 0;
				float g = 0;
				float b = 0;
				FastRand.NextColour(ref r, ref g, ref b, Parameters.Colour);

				particle.R[i] = r;
				particle.G[i] = g;
				particle.B[i] = b;

				particle.Opacity[i] = FastRand.NextSingle(Parameters.Opacity);
				particle.Scale[i] = FastRand.NextSingle(Parameters.Scale);
				particle.Rotation[i] = FastRand.NextSingle(Parameters.Rotation);
				particle.Mass[i] = FastRand.NextSingle(Parameters.Mass);

				i++;
				t = ++released / totalToRelease;
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