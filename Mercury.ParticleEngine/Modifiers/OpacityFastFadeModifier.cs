namespace Mercury.ParticleEngine.Modifiers
{
	public unsafe sealed class OpacityFastFadeModifier : Modifier
	{
		public Colour InitialColour;

		protected internal override void Update(float elapsedSeconds, ref Particle particle, int count)
		{
			fixed (float* agePtr = particle.Age)
			fixed (float* rPtr = particle.R)
			fixed (float* gPtr = particle.G)
			fixed (float* bPtr = particle.B)
			fixed (float* aPtr = particle.Opacity)
			{
				var ageDataPtr = agePtr;
				var rDataPtr = rPtr;
				var gDataPtr = gPtr;
				var bDataPtr = bPtr;
				var aDataPtr = aPtr;

				for (var j = 0; j < count; j++)
				{
					var age = *(ageDataPtr + j);
					var inverseAge = 1.0f - age;
					var alpha = InitialColour.A * age;
					*(aDataPtr + j) = alpha;
					*(rDataPtr + j) = InitialColour.H * inverseAge;
					*(gDataPtr + j) = InitialColour.S * inverseAge;
					*(bDataPtr + j) = InitialColour.L * inverseAge;
				}
			}
		}
	}
}