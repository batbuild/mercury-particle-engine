using System.CodeDom;

namespace Mercury.ParticleEngine.Modifiers
{
	using System;

	public class VelocityHueModifier : Modifier
	{
		public float StationaryHue;
		public float VelocityHue;
		public float VelocityThreshold;

		protected internal override unsafe void Update(float elapsedSeconds, ref Particle particle, int count)
		{
			var velocityThreshold2 = VelocityThreshold * VelocityThreshold;

			var i = 0;
			while (count-- > 0)
			{
				var velocity2 = ((particle.VX[i] * particle.VX[i]) + (particle.VY[i] * particle.VY[i]));

				if (velocity2 >= velocityThreshold2)
				{
					particle.R[i] = VelocityHue;
				}
				else
				{
					var t = (float)Math.Sqrt(velocity2) / VelocityThreshold;
					particle.R[i] = ((VelocityHue - StationaryHue) * t) + StationaryHue;
				}

				i++;
			}
		}
	}
}