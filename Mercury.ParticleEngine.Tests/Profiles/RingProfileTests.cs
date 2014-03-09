namespace Mercury.ParticleEngine.Profiles
{
	using System;
	using Xunit;
	using FluentAssertions;

	public class RingProfileTests
	{
		public class GetOffsetAndHeadingMethod
		{
			[Fact]
			public void ReturnsOffsetEqualToRadius()
			{
				var subject = new RingProfile
				{
					Radius = 10f
				};

				var particles = ProfilesTestHelper.CreateParticleStructure();
				subject.GetOffsetAndHeading(ref particles, 0);

				var length = Math.Sqrt((particles.X[0] * particles.X[0]) + (particles.Y[0] * particles.Y[0]));
				length.Should().BeApproximately(10f, 0.000001f);
			}

			[Fact]
			public void WhenRadiateIsTrue_HeadingIsEqualToNormalizedOffset()
			{
				var subject = new RingProfile
				{
					Radius = 10f,
					Radiate = true
				};

				var particles = ProfilesTestHelper.CreateParticleStructure();
				subject.GetOffsetAndHeading(ref particles, 0);

				particles.VX[0].Should().BeApproximately(particles.X[0] / 10f, 0.000001f);
				particles.VY[0].Should().BeApproximately(particles.Y[0] / 10f, 0.000001f);
			}
		}
	}
}