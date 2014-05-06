using System.Runtime.InteropServices;

namespace Mercury.ParticleEngine
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Particle
	{
		public float[] X;
		public float[] Y;
		public float[] VX;
		public float[] VY;
		public float[] Inception;
		public float[] Age;
		public float[] R;
		public float[] G;
		public float[] B;
		public float[] Opacity;
		public float[] Scale;
		public float[] Rotation;
		public float[] Mass;

		static public readonly int SizeInBytes = 44;
	}
}