namespace Mercury.ParticleEngine.Modifiers
{
	public sealed unsafe class ContainerModifier : Modifier
	{
		public Coordinate Position;
		public int Width;
		public int Height;
		public float RestitutionCoefficient;

		protected internal override void Update(float elapsedSeconds, ref Particle particle, int count)
		{
			var left = Position._x + Width * -0.5f;
			var right = Position._x + Width * 0.5f;
			var top = Position._y + Height * -0.5f;
			var bottom = Position._y + Height * 0.5f;

			unchecked
			{
				while (count-- > 0)
				{
					if ((int)particle.X[count] < left)
					{
						particle.X[count] = left + (left - particle.X[count]);
						particle.VX[count] = -particle.VX[count] * RestitutionCoefficient;
					}
					else if ((int)particle.X[count] > right)
					{
						particle.X[count] = right - (particle.X[count] - right);
						particle.VX[count] = -particle.VX[count] * RestitutionCoefficient;
					}

					if ((int)particle.Y[count] < top)
					{
						particle.Y[count] = top + (top - particle.Y[count]);
						particle.VY[count] = -particle.VY[count] * RestitutionCoefficient;
					}
					else if ((int)particle.Y[count] > bottom)
					{
						particle.Y[count] = bottom - (particle.Y[count] - bottom);
						particle.VY[count] = -particle.VY[count] * RestitutionCoefficient;
					}
				}
			}
		}
	}
}