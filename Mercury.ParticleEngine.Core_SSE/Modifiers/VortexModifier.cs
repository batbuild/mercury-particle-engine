namespace Mercury.ParticleEngine.Modifiers
{
	using System;

	public class VortexModifier : Modifier
	{
		public Coordinate Position;
		public float Mass;
		public float MaxSpeed;

		protected internal override unsafe void Update(float elapsedSeconds, ref Particle particle, int index, int count)
		{
			var distX = new float[YepppConstants.BufferLength];
			var distY = new float[YepppConstants.BufferLength];
			var distXSquared = new float[YepppConstants.BufferLength];
			var distYSquared = new float[YepppConstants.BufferLength];
			var distanceSquared = new float[YepppConstants.BufferLength];
			var distance = new float[YepppConstants.BufferLength];
			var oneOverDistance = new float[YepppConstants.BufferLength];
			var mass = new float[YepppConstants.BufferLength];
			var minM = new float[YepppConstants.BufferLength];
			var maxM = new float[YepppConstants.BufferLength];

			
			var i = 0;
			while (i < count)
			{
				var blockLength = Math.Min(YepppConstants.BufferLength, count - i);
				Yeppp.Core.Subtract_S32fV32f_V32f(Position._x, particle.X, i, distX, 0, blockLength);
				Yeppp.Core.Subtract_S32fV32f_V32f(Position._y, particle.Y, i, distY, 0, blockLength);
				Yeppp.Core.Multiply_V32fV32f_V32f(distX, 0, distX, 0, distXSquared, 0, blockLength);
				Yeppp.Core.Multiply_V32fV32f_V32f(distY, 0, distY, 0, distYSquared, 0, blockLength);
				Yeppp.Core.Add_V32fV32f_V32f(distXSquared, 0, distYSquared, 0, distanceSquared, 0, blockLength);

				var j = 0;
				while (j < YepppConstants.BufferLength)
				{
					distance[j] = (float) Approximate.Sqrt(distanceSquared[j]);
					distanceSquared[j] = 1/ distanceSquared[j];
					oneOverDistance[j] = 1/ distance[j];

					j++;
				}

				var constantMass = 10000f * Mass;
				Yeppp.Core.Multiply_V32fS32f_V32f(particle.Mass, i, constantMass, mass, 0, blockLength);		// (10000f * Mass * particle.Mass[i])
				Yeppp.Core.Multiply_IV32fV32f_IV32f(mass, 0, distanceSquared, 0, blockLength);	// (10000f * Mass * particle.Mass[i]) / distance2

				Yeppp.Core.Min_V32fS32f_V32f(mass, 0, MaxSpeed, minM, 0, blockLength);
				Yeppp.Core.Max_V32fS32f_V32f(minM, 0, -MaxSpeed, maxM, 0, blockLength);
				Yeppp.Core.Multiply_IV32fS32f_IV32f(maxM, 0, elapsedSeconds, blockLength);

				Yeppp.Core.Multiply_IV32fV32f_IV32f(distX, 0, oneOverDistance, 0, blockLength);
				Yeppp.Core.Multiply_IV32fV32f_IV32f(distY, 0, oneOverDistance, 0, blockLength);

				Yeppp.Core.Multiply_IV32fV32f_IV32f(distX, 0, maxM, 0, blockLength);
				Yeppp.Core.Multiply_IV32fV32f_IV32f(distY, 0, maxM, 0, blockLength);

				Yeppp.Core.Add_IV32fV32f_IV32f(particle.VX, i, distX, 0, blockLength);
				Yeppp.Core.Add_IV32fV32f_IV32f(particle.VY, i, distY, 0, blockLength);

				i += YepppConstants.BufferLength;
			}
		}
	}
}