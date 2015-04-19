namespace Mercury.ParticleEngine.Modifiers
{
	using System;

	public class VortexModifier : Modifier
	{
		public Coordinate Position;
		public float Mass;
		public float MaxSpeed;

		protected internal override unsafe void Update(float elapsedSeconds, ref Particle particle, int count)
		{
			fixed (float* xPtr = particle.X)
			fixed (float* yPtr = particle.Y)
			fixed (float* vxPtr = particle.VX)
			fixed (float* vyPtr = particle.VY)
			fixed (float* massPtr = particle.Mass)
			{
				var x = xPtr;
				var y = yPtr;
				var vx = vxPtr;
				var vy = vyPtr;
				var mass = massPtr;
				var globalMass = Mass;
				var posX = Position._x;
				var posY = Position._y;

				for (var j = 0; j < count; j++)
				{
					var distX = posX - *(x + j);
					var distY = posY - *(y + j);

					var distance2 = (distX * distX) + (distY * distY);
					var distance = (float)Math.Sqrt(distance2);

					var m = (10000f * globalMass * *(mass + j)) / distance2;

					if (m > MaxSpeed)
						m = MaxSpeed;

					if (m < -MaxSpeed)
						m = -MaxSpeed;
					
					m *= elapsedSeconds;

					distX = (distX / distance) * m;
					distY = (distY / distance) * m;

					*(vx + j) += distX;
					*(vy + j) += distY;
				}
			}
		}
	}
}