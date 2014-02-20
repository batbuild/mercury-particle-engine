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
					particle.Age[i] = (_totalSeconds - particle.Inception[i]) / _term;

					i++;
				}
			}
		}
	}
}