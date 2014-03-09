namespace Mercury.ParticleEngine.Modifiers
{
    using System;
    using FluentAssertions;

    internal class AssertionModifier : Modifier
    {
        readonly Func<Particle, int, bool> _predicate;

        public AssertionModifier(Func<Particle, int, bool> predicate)
        {
            _predicate = predicate;
        }

        protected internal override unsafe void Update(float elapsedSeconds, ref Particle particle, int count)
        {
	        var i = 0;
            while (count-- > 0)
            {
                _predicate(particle, i).Should().BeTrue();

                i++;
            }
        }
    }
}