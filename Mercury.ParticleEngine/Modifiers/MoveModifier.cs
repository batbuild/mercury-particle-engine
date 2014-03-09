using System;

namespace Mercury.ParticleEngine.Modifiers
{
    public class MoveModifier : Modifier
    {
        protected internal override unsafe void Update(float elapsedSeconds, ref Particle particle, int count)
        {
			var velXIntegral = new float[YepppConstants.BufferLength];
			var velYIntegral = new float[YepppConstants.BufferLength];

			var i = 0;
			while (i < count)
			{
				var blockLength = Math.Min(YepppConstants.BufferLength, count - i);
			
				Yeppp.Core.Multiply_V32fS32f_V32f(particle.VX, i, elapsedSeconds, velXIntegral, 0, blockLength);
				Yeppp.Core.Multiply_V32fS32f_V32f(particle.VY, i, elapsedSeconds, velYIntegral, 0, blockLength);

				Yeppp.Core.Add_IV32fV32f_IV32f(particle.X, i, velXIntegral, 0, blockLength);
				Yeppp.Core.Add_IV32fV32f_IV32f(particle.Y, i, velYIntegral, 0, blockLength);
			
				i += YepppConstants.BufferLength;
			}

//	        var i = index;
//		    while (count-- > 0)
//		    {
//			    particle.X[i] += particle.VX[i] * elapsedSeconds;
//			    particle.Y[i] += particle.VY[i] * elapsedSeconds;
//
//			    i++;
//		    }
        }
    }
}