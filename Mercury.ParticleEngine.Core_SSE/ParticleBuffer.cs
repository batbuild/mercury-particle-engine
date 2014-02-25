using System;
using System.Runtime.InteropServices;

namespace Mercury.ParticleEngine
{
	public unsafe class ParticleBuffer : IDisposable
	{
		private int _tail;
		private bool _disposed;
		
		public readonly int Size;
		public readonly Particle Particles;
		public int Index;

		public ParticleBuffer(int size)
		{
			Size = size;

			Particles = new Particle
			{
				X = new float[Size],
				Y = new float[Size],
				VX = new float[Size],
				VY = new float[Size],
				Inception = new float[Size],
				Age = new float[Size],
				R = new float[Size],
				G = new float[Size],
				B = new float[Size],
				Opacity = new float[Size],
				Scale = new float[Size],
				Rotation = new float[Size],
				Mass = new float[Size],
			};
		}

		public int Available
		{
			get { return Size - _tail; }
		}

		public int Count
		{
			get { return _tail; }
		}

		public int SizeInBytes
		{
			get { return Particle.SizeInBytes * Size; }
		}

		public int Release(int releaseQuantity)
		{
			var numToRelease = Math.Min(releaseQuantity, Available);

			var oldTail = _tail;

			_tail += numToRelease;

			Index = oldTail;

			return numToRelease;
		}

		public void Reclaim(int number)
		{
			_tail -= number;

			Reclaim(number, Particles.X);
			Reclaim(number, Particles.Y);
			Reclaim(number, Particles.VX);
			Reclaim(number, Particles.VY);
			Reclaim(number, Particles.Inception);
			Reclaim(number, Particles.Age);
			Reclaim(number, Particles.R);
			Reclaim(number, Particles.G);
			Reclaim(number, Particles.B);
			Reclaim(number, Particles.Opacity);
			Reclaim(number, Particles.Scale);
			Reclaim(number, Particles.Rotation);
			Reclaim(number, Particles.Mass);
		}

		private void Reclaim(int number, float[] array)
		{
			Array.Copy(array, number, array, 0, _tail);
		}

		public void CopyTo(IntPtr destination)
		{
			var rowSize = Size * sizeof(float);

			fixed (float* data = Particles.Age)
				memcpy(destination, (IntPtr)data, rowSize);

			fixed (float* data = Particles.X)
				memcpy(IntPtr.Add(destination, rowSize), (IntPtr)data, rowSize);

			fixed (float* data = Particles.Y)
				memcpy(IntPtr.Add(destination, rowSize * 2), (IntPtr)data, rowSize);

			fixed (float* data = Particles.R)
				memcpy(IntPtr.Add(destination, rowSize * 3), (IntPtr)data, rowSize);

			fixed (float* data = Particles.G)
				memcpy(IntPtr.Add(destination, rowSize * 4), (IntPtr)data, rowSize);

			fixed (float* data = Particles.B)
				memcpy(IntPtr.Add(destination, rowSize * 5), (IntPtr)data, rowSize);

			fixed (float* data = Particles.Opacity)
				memcpy(IntPtr.Add(destination, rowSize * 6), (IntPtr)data, rowSize);

			fixed (float* data = Particles.Scale)
				memcpy(IntPtr.Add(destination, rowSize * 7), (IntPtr)data, rowSize);

			fixed (float* data = Particles.Rotation)
				memcpy(IntPtr.Add(destination, rowSize * 8), (IntPtr)data, rowSize);
		}

		[DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
		public static extern void memcpy(IntPtr dest, IntPtr src, int count);

		public void Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;
			}

			GC.SuppressFinalize(this);
		}

		~ParticleBuffer()
		{
			Dispose();
		}
	}
}