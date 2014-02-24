namespace Mercury.ParticleEngine.Modifiers
{
    using Xunit;
    using FluentAssertions;

    public class ColourInterpolator2Tests
    {
        public class UpdateMethod
        {
            [Fact]
            public void WhenParticleLifeIsZero_SetsInitialColour()
            {
                var particle = new Particle
                {
					Age = new[] { 0.0f },
					R = new float[1],
					G = new float[1],
					B = new float[1]
                };

                var subject = new ColourInterpolator2
                {
                    InitialColour = new Colour(1f, 0f, 0f),
                    FinalColour = new Colour(0f, 0f, 1f)
                };

                unsafe
                {
                    subject.Update(0.01666f, ref particle, 1);

                    particle.R[0].Should().BeApproximately(1f, 0.000001f);
                    particle.G[0].Should().BeApproximately(0f, 0.000001f);
                    particle.B[0].Should().BeApproximately(0f, 0.000001f);
                }
            }

            [Fact]
            public void WhenParticleLifeIsOne_SetsFinalColour()
            {
                var particle = new Particle
                {
					Age = new []{1f},
					R = new float[1],
					G = new float[1],
					B = new float[1]
                };

                var subject = new ColourInterpolator2
                {
                    InitialColour = new Colour(1f, 0f, 0f),
                    FinalColour = new Colour(0f, 0f, 1f)
                };

	            subject.Update(0.01666f, ref particle, 1);

	            particle.R[0].Should().BeApproximately(0f, 0.000001f);
	            particle.G[0].Should().BeApproximately(0f, 0.000001f);
	            particle.B[0].Should().BeApproximately(1f, 0.000001f);
            }

            [Fact]
            public void WhenParticleLifeIsPointFive_SetsColourBetweenInitialAndFinal()
            {
                var particle = new Particle
                {
                    Age = new []{ 0.5f },
					R = new float[1],
					G = new float[1],
					B = new float[1]
                };

                var subject = new ColourInterpolator2
                {
                    InitialColour = new Colour(1f, 0f, 0f),
                    FinalColour = new Colour(0f, 0f, 1f)
                };

                unsafe
                {
                    subject.Update(0.01666f, ref particle, 1);

					particle.R[0].Should().BeApproximately(0.5f, 0.000001f);
					particle.G[0].Should().BeApproximately(0f, 0.000001f);
					particle.B[0].Should().BeApproximately(0.5f, 0.000001f);
                }
            }

            [Fact]
            public void IteratesOverEachParticle()
            {
	            var buffer = new Particle
	            {
					Age = new float[100],
					R = new float[100],
					G = new float[100],
					B = new float[100]
	            };
                
                for (var i = 0; i < buffer.Age.Length; i++)
                    buffer.Age[i] = 1.0f;

                var subject = new ColourInterpolator2
                {
                    InitialColour = new Colour(1f, 0f, 0f),
                    FinalColour = new Colour(0f, 0f, 1f)
                };

	            subject.Update(0.1666666f, ref buffer, buffer.Age.Length);

	            for (int i = 0; i < buffer.Age.Length; i++)
	            {
		            buffer.B[0].Should().BeApproximately(1f, 0.000001f);
	            }
            }
        }
    }
}