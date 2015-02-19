using System;

namespace Mercury.ParticleEngine.Modifiers
{
	public class MoveModifier : Modifier
	{
		protected internal override unsafe void Update(float elapsedSeconds, ref Particle particle, int count)
		{
			unchecked
			{
				var i = 0;
				while (count-- > 0)
				{
					particle.X[i] += particle.VX[i] * elapsedSeconds;
					particle.Y[i] += particle.VY[i] * elapsedSeconds;

					i++;
				}
			}
		}
	}
}