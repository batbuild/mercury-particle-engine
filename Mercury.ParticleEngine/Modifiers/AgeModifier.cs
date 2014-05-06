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

			var i = 0;
			unchecked
			{
				while (count-- > 0)
				{
					// TODO: There is a fairly systemic bug in the emitter code right now that means that the inception time
					// of a particle can be later than the total elapsed time in here. It happens because the modifiers are 
					// not necessarily updated every frame, while the emitter is.
					particle.Age[i] = Math.Min(1, Math.Max(0, (_totalSeconds - particle.Inception[i]) / _term));

					i++;
				}
			}
		}
	}
}