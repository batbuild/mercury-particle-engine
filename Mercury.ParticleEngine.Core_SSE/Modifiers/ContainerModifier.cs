using Yeppp;

namespace Mercury.ParticleEngine.Modifiers
{
    using System;

    public sealed unsafe class ContainerModifier : Modifier
    {
        public Coordinate Position;
        public int Width;
        public int Height;
        public float RestitutionCoefficient;

        protected internal override void Update(float elapsedSeconds, ref Particle particle, int index, int count)
        {
            var left = Width * -0.5f;
            var right = Width * 0.5f;
            var top = Height * -0.5f;
            var bottom = Height * 0.5f;

			var leftValues = new float[YepppConstants.BufferLength];
			var rightValues = new float[YepppConstants.BufferLength];
			var topValues = new float[YepppConstants.BufferLength];
			var bottomValues = new float[YepppConstants.BufferLength];
			var newXVelocities = new float[YepppConstants.BufferLength];
			var newYVelocities = new float[YepppConstants.BufferLength];


	        var i = 0;
//	        while (i < count)
//	        {
//		        var blockLength = Math.Min(YepppConstants.BufferLength, count - i);

//				Yeppp.Core.Subtract();

//		        i += YepppConstants.BufferLength;
//	        }
	        
	        unchecked
	        {
		        while (count-- > 0)
		        {
			        if ((int)particle.X[i] < left)
			        {
				        particle.X[i] = left + (left - particle.X[i]);
				        particle.VX[i] = -particle.VX[i] * RestitutionCoefficient;
			        }
			        else if ((int)particle.X[i] > right)
			        {
				        particle.X[i] = right - (particle.X[i] - right);
				        particle.VX[i] = -particle.VX[i] * RestitutionCoefficient;
			        }

			        if ((int)particle.Y[i] < top)
			        {
				        particle.Y[i] = top + (top - particle.Y[i]);
				        particle.VY[i] = -particle.VY[i] * RestitutionCoefficient;
			        }
			        else if ((int)particle.Y[i] > bottom)
			        {
				        particle.Y[i] = bottom - (particle.Y[i] - bottom);
				        particle.VY[i] = -particle.VY[i] * RestitutionCoefficient;
			        }
		        }
	        }
        }
    }
}