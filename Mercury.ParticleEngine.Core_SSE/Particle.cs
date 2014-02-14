using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mercury.ParticleEngine
{
	// Note: Remember to try shoving the individual arrays into streams and use instancing. That way you won't have to
	// convert from the SOA version to a AOS version in order to render.
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

		static public readonly int SizeInBytes = 36;
	}
}