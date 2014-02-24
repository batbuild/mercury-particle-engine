namespace Mercury.ParticleEngine.Profiles
{
    public class BoxFillProfile : Profile
    {
        public float Width;
        public float Height;

        public override void GetOffsetAndHeading(ref Particle particles, int index)
        {
			particles.X[index] = FastRand.NextSingle(Width * -0.5f, Width * 0.5f);
			particles.Y[index] = FastRand.NextSingle(Height * -0.5f, Height * 0.5f);

	        var vx = 0f;
			var vy = 0f;
            FastRand.NextUnitVector(ref vx, ref vy);
	        particles.VX[index] = vx;
	        particles.VY[index] = vy;
        }
    }
}