namespace Mercury.ParticleEngine.Modifiers
{
	public class MoveModifier : Modifier
	{
		protected internal override unsafe void Update(float elapsedSeconds, ref Particle particle, int count)
		{
			fixed (float* xPtr = particle.X)
			fixed (float* yPtr = particle.Y)
			fixed (float* vxPtr = particle.VX)
			fixed (float* vyPtr = particle.VY)
			{
				var xDataPtr = xPtr;
				var yDataPtr = yPtr;
				var vxDataPtr = vxPtr;
				var vyDataPtr = vyPtr;

				for (var j = 0; j < count; j++)
				{
					*(xDataPtr + j) += *(vxDataPtr + j) * elapsedSeconds;
					*(yDataPtr + j) += *(vyDataPtr + j) * elapsedSeconds;
				}
			}
		}
	}
}