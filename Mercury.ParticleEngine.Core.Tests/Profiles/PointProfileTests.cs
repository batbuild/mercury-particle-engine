namespace Mercury.ParticleEngine.Profiles
{
	using System;
	using Xunit;
	using FluentAssertions;

	public class PointProfileTests
	{
		public class GetOffsetAndHeadingMethod
		{
			[Fact]
			public void ReturnsZeroOffset()
			{
				var subject = new PointProfile();

				var particles = ProfilesTestHelper.CreateParticleStructure();
				subject.GetOffsetAndHeading(ref particles, 0);

				particles.X[0].Should().Be(0f);
				particles.Y[0].Should().Be(0f);
			}

			[Fact]
			public void ReturnsHeadingAsUnitVector()
			{
				var subject = new PointProfile();

				var particles = ProfilesTestHelper.CreateParticleStructure();
				subject.GetOffsetAndHeading(ref particles, 0);

				var length = Math.Sqrt((particles.VX[0] * particles.VX[0]) + (particles.VY[0] * particles.VY[0]));
				length.Should().BeApproximately(1f, 0.000001);
			}
		}
	}
}