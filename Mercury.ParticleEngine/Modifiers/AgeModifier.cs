using System.Diagnostics;

namespace Mercury.ParticleEngine.Modifiers
{
	using System;

	public class AgeModifier : Modifier
	{
		public AgeModifier(TimeSpan term)
		{
			_term = (float)term.TotalSeconds;
		}

		private float _term;
		private float _totalSeconds;

		protected internal override unsafe void Update(float elapsedSeconds, ref Particle particle, int count)
		{
			_totalSeconds += elapsedSeconds;
			
			fixed (float* agePtr = particle.Age)
			fixed (float* inceptionPtr = particle.Inception)
			{
				var ageDataPtr = agePtr;
				var inceptionDataPtr = inceptionPtr;
				var term = _term;
				var totalSeconds = _totalSeconds;

				for (var j = 0; j < count; j++)
				{
					var age = (totalSeconds - *(inceptionDataPtr + j)) / term;

					// TODO: There is a fairly systemic bug in the emitter code right now that means that the inception time
					// of a particle can be later than the total elapsed time in here. It happens because the modifiers are 
					// not necessarily updated every frame, while the emitter is.
					if (age < 0)
						age = 0;
					if (age > 1)
						age = 1;

					*(ageDataPtr + j) = age;
				}
			}
		}
	}
}