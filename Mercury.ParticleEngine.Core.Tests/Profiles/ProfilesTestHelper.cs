namespace Mercury.ParticleEngine.Profiles
{
	public static class ProfilesTestHelper
	{
		public static Particle CreateParticleStructure()
		{
			var particles = new Particle
			{
				X = new float[1],
				Y = new float[1],
				VX = new float[1],
				VY = new float[1],
			};
			return particles;
		}
	}
}