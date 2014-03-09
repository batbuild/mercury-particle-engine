namespace Mercury.ParticleEngine.Modifiers
{
	public class ScaleModifier : Modifier
	{
		public float StartScale { get; set; }
		public float EndScale { get; set; }

		protected internal override unsafe void Update(float elapsedSeconds, ref Particle particle, int count)
		{
			var i = 0;
			unchecked
			{
				while (count-- > 0)
				{
					particle.Scale[i] = StartScale + (EndScale - StartScale) * particle.Age[i];
					
					i++;
				}
			}
		}
	}
}