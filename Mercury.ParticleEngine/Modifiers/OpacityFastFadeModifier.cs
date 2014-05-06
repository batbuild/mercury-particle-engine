namespace Mercury.ParticleEngine.Modifiers
{
	public unsafe sealed class OpacityFastFadeModifier : Modifier
	{
		public Colour InitialColour;

		protected internal override void Update(float elapsedSeconds, ref Particle particle, int count)
		{
			unchecked
			{
				while (count-- > 0)
				{
					var inverseAge = 1.0f - particle.Age[count];
					var alpha = InitialColour.A * particle.Age[count];
					particle.Opacity[count] = alpha;
					particle.R[count] = InitialColour.H * inverseAge;
					particle.G[count] = InitialColour.S * inverseAge;
					particle.B[count] = InitialColour.L * inverseAge;
				}
			}
		}
	}
}