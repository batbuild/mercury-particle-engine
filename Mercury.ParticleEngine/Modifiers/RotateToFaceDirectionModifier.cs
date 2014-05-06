using System;

namespace Mercury.ParticleEngine.Modifiers
{
	public class RotateToFaceDirectionModifier : Modifier
	{
		protected internal override unsafe void Update(float elapsedSeconds, ref Particle particle, int count)
		{
			const float piOver2 = (float)Math.PI / 2f;

			while (count-- > 0)
			{
				particle.Rotation[count] = (float) Math.Atan2(particle.VY[count], particle.VX[count]);
			}
		}
	}
}