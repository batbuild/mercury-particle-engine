using System.CodeDom;

namespace Mercury.ParticleEngine.Modifiers
{
	using System;

	public class VelocityHueModifier : Modifier
	{
		public float StationaryHue;
		public float VelocityHue;
		public float VelocityThreshold;

		protected internal override unsafe void Update(float elapsedSeconds, ref Particle particle, int index, int count)
		{
			var velocityThreshold2 = VelocityThreshold * VelocityThreshold;

			var xVelSquared = new float[YepppConstants.BufferLength];
			var yVelSquared = new float[YepppConstants.BufferLength];
			var sumVelSquared = new float[YepppConstants.BufferLength];

			var i = 0;
			while (i < count)
			{
				var blockLength = Math.Min(YepppConstants.BufferLength, count - i);

				Yeppp.Core.Multiply_V32fV32f_V32f(particle.VX, i, particle.VX, i, xVelSquared, 0, blockLength);
				Yeppp.Core.Multiply_V32fV32f_V32f(particle.VY, i, particle.VY, i, yVelSquared, 0, blockLength);
				Yeppp.Core.Add_V32fV32f_V32f(xVelSquared, 0, yVelSquared, 0, sumVelSquared, 0, blockLength);

				for (var j = 0; j < blockLength; j++)
				{
					if (sumVelSquared[j] > velocityThreshold2)
					{
						particle.R[i + j] = VelocityHue;
					}
					else
					{
						var t = Approximate.Sqrt(sumVelSquared[j]) / VelocityThreshold;
						particle.R[i + j] = ((VelocityHue - StationaryHue) * t) + StationaryHue;
					}
				}

				i += YepppConstants.BufferLength;
			}
		}
	}
}