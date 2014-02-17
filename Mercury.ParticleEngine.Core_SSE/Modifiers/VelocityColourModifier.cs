namespace Mercury.ParticleEngine.Modifiers
{
    using System;

    public class VelocityColourModifier : Modifier
    {
        public Colour StationaryColour;
        public Colour VelocityColour;
        public float VelocityThreshold;

        protected internal override unsafe void Update(float elapsedSeconds, ref Particle particle, int index, int count)
        {
            var velocityThreshold2 = VelocityThreshold * VelocityThreshold;

	        var i = index;
	        unchecked
	        {
		        while (count-- > 0)
		        {
			        var velocity2 = ((particle.VX[i] * particle.VX[i]) + (particle.VY[i] * particle.VY[i]));

			        if (velocity2 >= velocityThreshold2) 
			        {
				        particle.R[i] = VelocityColour.H;
				        particle.G[i] = VelocityColour.S;
				        particle.B[i] = VelocityColour.L;
			        }
			        else
			        {
				        var t = (float)Math.Sqrt(velocity2) / VelocityThreshold;

				        particle.R[i] = ((VelocityColour.H - StationaryColour.H) * t) + StationaryColour.H;
				        particle.G[i] = ((VelocityColour.S - StationaryColour.S) * t) + StationaryColour.S;
				        particle.B[i] = ((VelocityColour.L - StationaryColour.L) * t) + StationaryColour.L;
			        }

			        i++;
		        }
	        }
        }
    }
}