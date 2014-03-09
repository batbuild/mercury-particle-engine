namespace Mercury.ParticleEngine.Profiles
{
    public class PointProfile : Profile
    {
        public override void GetOffsetAndHeading(ref Particle particles, int index)
        {
	        particles.X[index] = 0;
	        particles.Y[index] = 0;

			var vx = 0f;
			var vy = 0f;
			FastRand.NextUnitVector(ref vx, ref vy);
			particles.VX[index] = vx;
			particles.VY[index] = vy;
        }
    }
}