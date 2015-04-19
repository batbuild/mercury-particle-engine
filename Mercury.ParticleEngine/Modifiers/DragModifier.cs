namespace Mercury.ParticleEngine.Modifiers
{
    public class DragModifier : Modifier
    {
        public float DragCoefficient = 0.47f;
        public float Density = .5f;

        protected internal unsafe override void Update(float elapsedSeconds, ref Particle particle, int count)
        {
			fixed (float* massPtr = particle.Mass)
			fixed (float* vxPtr = particle.VX)
			fixed (float* vyPtr = particle.VY)
			{
				var mass = massPtr;
				var vxDataPtr = vxPtr;
				var vyDataPtr = vyPtr;

				for (var j = 0; j < count; j++)
				{
					var drag = -DragCoefficient * Density * *(mass + j) * elapsedSeconds;

					*(vxDataPtr + j) += (*(vxDataPtr + j) * drag);
					*(vyDataPtr + j) += (*(vyDataPtr + j) * drag);
				}
			}
        }
    }
}