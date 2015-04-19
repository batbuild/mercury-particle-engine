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

			fixed (float* xPtr = particle.X)
			fixed (float* yPtr = particle.Y)
			fixed (float* vxPtr = particle.VX)
			fixed (float* vyPtr = particle.VY)
			{
				var xDataPtr = xPtr;
				var yDataPtr = yPtr;
				var vxDataPtr = vxPtr;
				var vyDataPtr = vyPtr;

				for (var j = 0; j < count; j++)
				{
					var x = *(xDataPtr + j);
					var vx = *(vxDataPtr + j);

					if ((int)x < left)
					{
						*(xDataPtr + j) = left + (left - x);
						*(vxDataPtr + j) = -vx * RestitutionCoefficient;
					}
					else if ((int)x > right)
					{
						*(xDataPtr + j) = right - (x - right);
						*(vxDataPtr + j) = -vx * RestitutionCoefficient;
					}

					var y = *(yDataPtr + j);
					var vy = *(vyDataPtr + j);

					if ((int)y < top)
					{
						*(yDataPtr + j) = top + (top - y);
						*(vyDataPtr + j) = -vy * RestitutionCoefficient;
					}
					else if ((int)y > bottom)
					{
						*(yDataPtr + j) = bottom - (y - bottom);
						*(vyDataPtr + j) = -vy * RestitutionCoefficient;
					}
				}
			}
		}
	}
}