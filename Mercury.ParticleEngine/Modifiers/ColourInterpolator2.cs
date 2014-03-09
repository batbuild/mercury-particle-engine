namespace Mercury.ParticleEngine.Modifiers
{
    /// <summary>
    /// Defines a modifier which interpolates the colour of a particle over the course of its lifetime.
    /// </summary>
    public sealed class ColourInterpolator2 : Modifier
    {
        /// <summary>
        /// Gets or sets the initial colour of particles when they are released.
        /// </summary>
        public Colour InitialColour;

        /// <summary>
        /// Gets or sets the final colour of particles when they are retired.
        /// </summary>
        public Colour FinalColour;

        protected internal override unsafe void Update(float elapsedseconds, ref Particle particle, int count)
        {
            var delta = new Colour(FinalColour.H - InitialColour.H,
                                   FinalColour.S - InitialColour.S,
                                   FinalColour.L - InitialColour.L);

	        var i = 0;
	        unchecked
	        {
		        while (count-- > 0)
		        {
			        var alpha = 1 - particle.Age[i];
			        particle.R[i] = (InitialColour.H + (delta.H * particle.Age[i])) * alpha;
					particle.G[i] = (InitialColour.S + (delta.S * particle.Age[i])) * alpha;
					particle.B[i] = (InitialColour.L + (delta.L * particle.Age[i])) * alpha;

			        i++;
		        }
	        }
        }
    }
}