namespace Mercury.ParticleEngine.Modifiers
{
    public unsafe sealed class OpacityFastFadeModifier : Modifier
    {
        protected internal override void Update(float elapsedSeconds, ref Particle particle, int count)
        {
	        var i = 0;
	        unchecked
	        {
		        while (count-- > 0)
		        {
			        var alpha = 1.0f - particle.Age[i];
			        particle.Opacity[i] = alpha;
			        particle.R[i] *= alpha;
			        particle.G[i] *= alpha;
			        particle.B[i] *= alpha;

			        i++;
		        }
	        }
        }
    }
}