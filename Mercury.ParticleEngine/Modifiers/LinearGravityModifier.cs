namespace Mercury.ParticleEngine.Modifiers
{
    public class LinearGravityModifier : Modifier
    {
        public LinearGravityModifier(Axis direction, float strength)
        {
            Direction = direction;
            Strength = strength;
        }

        public LinearGravityModifier(Vector vector)
            : this(vector.Axis, vector.Magnitude)
        {
        }

        public Axis Direction { get; set; }
        public float Strength { get; set; }

        protected internal override unsafe void Update(float elapsedSeconds, ref Particle particle, int count)
        {
            var vector = Direction * (Strength * elapsedSeconds);

	        var i = 0;
	        unchecked
	        {
		        while (count-- > 0)
		        {
			        particle.VX[i] += vector._x * particle.Mass[i];
			        particle.VY[i] += vector._y * particle.Mass[i];

			        i++;
		        }
	        }
        }
    }
}