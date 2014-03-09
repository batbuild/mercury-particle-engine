namespace Mercury.ParticleEngine
{
    using System.Collections.Generic;
    using Mercury.ParticleEngine.Modifiers;

    public abstract class ModifierExecutionStrategy
    {
        internal abstract unsafe void ExecuteModifiers(ModifierCollection modifiers, float elapsedSeconds, ref Particle particle, int index, int count);

        static public ModifierExecutionStrategy Serial = new SerialModifierExecutionStrategy();
        static public ModifierExecutionStrategy Parallel = new ParallelModifierExecutionStrategy();

        internal class SerialModifierExecutionStrategy : ModifierExecutionStrategy
        {
            internal override unsafe void ExecuteModifiers(ModifierCollection modifiers, float elapsedSeconds, ref Particle particle, int index, int count)
            {
                foreach (var slot in modifiers.Slots)
                {
                    slot.Update(elapsedSeconds, ref particle, index, count);
                }
            }
        }

        internal class ParallelModifierExecutionStrategy : ModifierExecutionStrategy
        {
            internal override unsafe void ExecuteModifiers(ModifierCollection modifiers, float elapsedSeconds, ref Particle particle, int index, int count)
            {
	            var p = particle;
                System.Threading.Tasks.Parallel.ForEach(modifiers.Slots, slot => slot.Update(elapsedSeconds, ref p, index, count));
            }
        }
    }
}