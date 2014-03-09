namespace Mercury.ParticleEngine.Profiles
{
    public class CircleProfile : Profile
    {
        public float Radius;
        public bool Radiate;

        public override void GetOffsetAndHeading(ref Particle particles, int index)
        {
            var dist = FastRand.NextSingle(0f, Radius);

			var vx = 0f;
			var vy = 0f;
			FastRand.NextUnitVector(ref vx, ref vy);
			particles.VX[index] = vx;
			particles.VY[index] = vy;

			particles.X[index] = vx * dist;
			particles.Y[index] = vy * dist;

	        if (!Radiate)
	        {
				FastRand.NextUnitVector(ref vx, ref vy);
				particles.VX[index] = vx;
				particles.VY[index] = vy;
	        }
        }
    }
}