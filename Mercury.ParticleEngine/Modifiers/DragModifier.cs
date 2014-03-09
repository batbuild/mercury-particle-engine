namespace Mercury.ParticleEngine.Modifiers
{
    public class DragModifier : Modifier
    {
        public float DragCoefficient = 0.47f;
        public float Density = .5f;

        protected internal unsafe override void Update(float elapsedSeconds, ref Particle particle, int count)
        {
	        var i = 0;
	        unchecked
	        {
		        while (count-- > 0)
		        {
			        var drag = -DragCoefficient * Density * particle.Mass[i] * elapsedSeconds;

			        particle.VX[i] += (particle.VX[i] * drag);
			        particle.VY[i] += (particle.VY[i] * drag);

			        i++;
		        }
	        }
        }
    }
}