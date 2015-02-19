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
			var i = 0;
			while (count-- > 0)
			{
				var distX = Position._x - particle.X[i];
				var distY = Position._y - particle.Y[i];

				var distance2 = (distX * distX) + (distY * distY);
				var distance = (float)Math.Sqrt(distance2);

				var m = (10000f * Mass * particle.Mass[i]) / distance2;

				m = Math.Max(Math.Min(m, MaxSpeed), -MaxSpeed) * elapsedSeconds;

				distX = (distX / distance) * m;
				distY = (distY / distance) * m;

				particle.VX[i] += distX;
				particle.VY[i] += distY;

				i++;
			}
		}
	}
}