namespace Mercury.ParticleEngine.Profiles
{
	public class BoxProfile : Profile
	{
		public float Width;
		public float Height;

		public override void GetOffsetAndHeading(ref Particle particles, int index)
		{
			switch (FastRand.NextInteger(3))
			{
				case 0: // Left
					{
						particles.X[index] = Width * -0.5f;
						particles.Y[index] = FastRand.NextSingle(Height * -0.5f, Height * 0.5f);
						break;
					}
				case 1: // Top
					{
						particles.X[index] = FastRand.NextSingle(Width * -0.5f, Width * 0.5f);
						particles.Y[index] = Height * -0.5f;
						break;
					}
				case 2: // Right
					{
						particles.X[index] = Width * 0.5f;
						particles.Y[index] = FastRand.NextSingle(Height * -0.5f, Height * 0.5f);
						break;
					}
				case 3: // Bottom
					{
						particles.X[index] = FastRand.NextSingle(Width * -0.5f, Width * 0.5f);
						particles.Y[index] = Height * 0.5f;
						break;
					}
			}

			var vx = 0f;
			var vy = 0f;
			FastRand.NextUnitVector(ref vx, ref vy);
			particles.VX[index] = vx;
			particles.VY[index] = vy;
		}
	}
}