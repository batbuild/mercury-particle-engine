namespace Mercury.ParticleEngine.Modifiers
{
    public abstract class Modifier
    {
        protected internal abstract unsafe void Update(float elapsedSeconds, ref Particle particle, int index, int count);
    }
}