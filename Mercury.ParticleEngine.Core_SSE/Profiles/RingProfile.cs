namespace Mercury.ParticleEngine.Profiles
{
    public class RingProfile : Profile
    {
        public float Radius { get; set; }
        public bool Radiate { get; set; }

		public override void GetOffsetAndHeading(ref Particle particles, int index)
		{
			float x = 0;
			float y = 0;
			float vx = 0;
			float vy = 0;
            FastRand.NextUnitVector(ref vx, ref vy);

			particles.X[index] = vx * Radius;
			particles.Y[index] = vy * Radius;
			particles.VX[index] = vx;
			particles.VY[index] = vy;

			if (!Radiate)
			{
				FastRand.NextUnitVector(ref vx, ref vy);
				particles.VX[index] = vx;
				particles.VY[index] = vy;
			}
        }
    }
}