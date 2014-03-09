namespace Mercury.ParticleEngine
{
	using System;
	using Xunit;
	using FluentAssertions;

	public class ParticleBufferTests
	{
		public class AvailableProperty
		{
			[Fact]
			public void WhenNoParticlesReleased_ReturnsBufferSize()
			{
				var subject = new ParticleBuffer(100);

				subject.Available.Should().Be(100);
			}

			[Fact]
			public void WhenSomeParticlesReleased_ReturnsAvailableCount()
			{
				var subject = new ParticleBuffer(100);

				int index;
				subject.Release(10);

				subject.Available.Should().Be(90);
			}

			[Fact]
			public void WhenAllParticlesReleased_ReturnsZero()
			{
				var subject = new ParticleBuffer(100);

				int index;
				subject.Release(100);

				subject.Available.Should().Be(0);
			}
		}

		public class CountProperty
		{
			[Fact]
			public void WhenNoParticlesReleased_ReturnsZero()
			{
				var subject = new ParticleBuffer(100);
				subject.Count.Should().Be(0);
			}

			[Fact]
			public void WhenSomeParticlesReleased_ReturnsCount()
			{
				var subject = new ParticleBuffer(100);

				unsafe
				{
					int index;
					subject.Release(10);
				}

				subject.Count.Should().Be(10);
			}

			[Fact]
			public void WhenAllParticlesReleased_ReturnsZero()
			{
				var subject = new ParticleBuffer(100);

				int index;
				subject.Release(100);

				subject.Count.Should().Be(100);
			}
		}

		public class ReleaseMethod
		{
			[Fact]
			public void WhenPassedReasonableQuantity_ReturnsNumberReleased()
			{
				var subject = new ParticleBuffer(100);

				int index;
				var count = subject.Release(50);

				count.Should().Be(50);
			}

			[Fact]
			public void WhenPassedImpossibleQuantity_ReturnsNumberActuallyReleased()
			{
				var subject = new ParticleBuffer(100);

				int index;
				var count = subject.Release(200);
				count.Should().Be(100);
			}
		}

		public class ReclaimMethod
		{
			[Fact]
			public void WhenPassedReasonableNumber_ReclaimsParticles()
			{
				var subject = new ParticleBuffer(100);

				unsafe
				{
					int index;
					subject.Release(100);
				}

				subject.Count.Should().Be(100);

				subject.Reclaim(50);

				subject.Count.Should().Be(50);
			}
		}

		public class CopyToMethod
		{
			[Fact]
			public void WhenBufferIsSequential_CopiesParticlesInOrder()
			{
				unsafe
				{
					var subject = new ParticleBuffer(10);
					var count = subject.Release(5);
					var index = subject.Index;

					do
					{
						subject.Particles.Age[index] = 1f;
						index++;
					}
					while (count-- > 0);

					var destination = new float[10];

					fixed (float* buffer = destination)
					{
						subject.CopyTo((IntPtr)buffer);
					}

					destination[0].Should().BeApproximately(1f, 0.0001f);
					destination[1].Should().BeApproximately(1f, 0.0001f);
					destination[2].Should().BeApproximately(1f, 0.0001f);
					destination[3].Should().BeApproximately(1f, 0.0001f);
					destination[4].Should().BeApproximately(1f, 0.0001f);
				}
			}
		}

		public class DisposeMethod
		{
			[Fact]
			public void IsIdempotent()
			{
				var subject = new ParticleBuffer(100);
				subject.Dispose();
				subject.Dispose();
			}
		}
	}
}