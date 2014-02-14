namespace Mercury.ParticleEngine.Renderers
{
    using System;
    using System.Runtime.InteropServices;
    using SharpDX;
    using SharpDX.Direct3D9;

    public class PointSpriteRenderer : IDisposable
    {
        private readonly Device _device;
        private readonly int _size;
        private readonly VertexBuffer _vertexBuffer;
        private readonly VertexDeclaration _vertexDeclaration;
        private readonly Effect _effect;

        private bool _enableFastFade;
        public bool EnableFastFade
        {
            get { return _enableFastFade; }
            set
            {
                if(value != _enableFastFade)
                {
                    _enableFastFade = value;
                    _effect.SetValue("FastFade", _enableFastFade);
                }
            }
        }

        public PointSpriteRenderer(Device device, int size)
        {
            if (device == null)
                throw new ArgumentNullException("device");

            _device = device;
            _size = size;
            _vertexBuffer = new VertexBuffer(_device, _size * Particle.SizeInBytes, Usage.Dynamic | Usage.Points | Usage.WriteOnly, VertexFormat.None, Pool.Default);

            var vertexElements = new[]
            {
//                new VertexElement(0, (short)Marshal.OffsetOf(typeof(Particle), "Age"),      DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.Color, 1),
//                new VertexElement(0, (short)Marshal.OffsetOf(typeof(Particle), "Position"), DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.Position, 0),
//                new VertexElement(0, (short)Marshal.OffsetOf(typeof(Particle), "Colour"),   DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Color, 0),
//                new VertexElement(0, (short)Marshal.OffsetOf(typeof(Particle), "Scale"),    DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.PointSize, 0),
//                new VertexElement(0, (short)Marshal.OffsetOf(typeof(Particle), "Rotation"), DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.Color, 2),

//				  new VertexElement(0, 0,     DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.Color, 1),
//                new VertexElement(0, 4,		DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.Position, 0),
//                new VertexElement(0, 12,	DeclarationType.Float4, DeclarationMethod.Default, DeclarationUsage.Color, 0),
//                new VertexElement(0, 28,    DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.PointSize, 0),
//                new VertexElement(0, 32,	DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.Color, 2),

					new VertexElement(0, 0,     DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.Color, 0),					// Age
					new VertexElement(1, 0,     DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.Position, 0),				// X
					new VertexElement(2, 0,		DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.Position, 1),				// Y
					new VertexElement(3, 0,		DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.Color, 1),					// R
					new VertexElement(4, 0,		DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.Color, 2),					// R
					new VertexElement(5, 0,		DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.Color, 3),					// R
					new VertexElement(6, 0,		DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.PointSize, 0),				// Size
					new VertexElement(7, 0,		DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),		// Rotation
                VertexElement.VertexDeclarationEnd
            };

//            _effect = Effect.FromString(device, Resources.PointSpriteShader, ShaderFlags.PartialPrecision);
            _effect = Effect.FromString(device, Resources.PointSpriteShader1, ShaderFlags.PartialPrecision);

            _vertexDeclaration = new VertexDeclaration(device, vertexElements);
        }

        public void Render(Emitter emitter, Matrix worldViewProjection, Texture texture)
        {
            if (emitter.ActiveParticles == 0)
                return;

            if (emitter.ActiveParticles > _size)
                throw new Exception("Cannot render this emitter, vertex buffer not big enough");

            var technique = _effect.GetTechnique(0);

            _effect.SetValue("WVPMatrix", worldViewProjection);
            _effect.SetTexture(_effect.GetParameter(null, "SpriteTexture"), texture);

            using (var dataStream = _vertexBuffer.Lock(0, emitter.ActiveParticles * Particle.SizeInBytes, LockFlags.Discard))
            {
                emitter.Buffer.CopyTo(dataStream.DataPointer);
            }

            _device.SetRenderState(RenderState.PointSpriteEnable, true);
            _device.SetRenderState(RenderState.AlphaBlendEnable, true);

            SetupBlend(emitter.BlendMode);

            _device.SetRenderState(RenderState.ZWriteEnable, false);
            
            _effect.Technique = technique;
            _effect.Begin(FX.DoNotSaveState);
            _effect.BeginPass(0);

//            _device.SetStreamSource(0, _vertexBuffer, 0, Particle.SizeInBytes);

            _device.SetStreamSource(0, _vertexBuffer, 0, sizeof(float));
            _device.SetStreamSource(1, _vertexBuffer, emitter.ActiveParticles * sizeof(float), sizeof(float));
            _device.SetStreamSource(2, _vertexBuffer, emitter.ActiveParticles * sizeof(float) * 2, sizeof(float));
            _device.SetStreamSource(3, _vertexBuffer, emitter.ActiveParticles * sizeof(float) * 3, sizeof(float));
            _device.SetStreamSource(4, _vertexBuffer, emitter.ActiveParticles * sizeof(float) * 4, sizeof(float));
            _device.SetStreamSource(5, _vertexBuffer, emitter.ActiveParticles * sizeof(float) * 5, sizeof(float));
            _device.SetStreamSource(6, _vertexBuffer, emitter.ActiveParticles * sizeof(float) * 6, sizeof(float));
            _device.SetStreamSource(7, _vertexBuffer, emitter.ActiveParticles * sizeof(float) * 7, sizeof(float));

            _device.VertexDeclaration = _vertexDeclaration;
            _device.DrawPrimitives(PrimitiveType.PointList, 0, emitter.Buffer.Count);

            _effect.EndPass();
            _effect.End();
        }

        private void SetupBlend(BlendMode blendMode)
        {
            switch (blendMode)
            {
                case BlendMode.Alpha:
                    _device.SetRenderState(RenderState.BlendOperation, BlendOperation.Add);
                    _device.SetRenderState(RenderState.BlendOperationAlpha, BlendOperation.Add);
                    _device.SetRenderState(RenderState.SourceBlendAlpha, Blend.SourceAlpha);
                    _device.SetRenderState(RenderState.DestinationBlendAlpha, Blend.InverseSourceAlpha);
                    _device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
                    _device.SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
                    return;
                case BlendMode.Add:
                    _device.SetRenderState(RenderState.BlendOperation, BlendOperation.Add);
                    _device.SetRenderState(RenderState.BlendOperationAlpha, BlendOperation.Add);
                    _device.SetRenderState(RenderState.SourceBlendAlpha, Blend.SourceAlpha);
                    _device.SetRenderState(RenderState.DestinationBlendAlpha, Blend.InverseSourceAlpha);
                    _device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
                    _device.SetRenderState(RenderState.DestinationBlend, Blend.One);
                    return;
                case BlendMode.Subtract:
                    _device.SetRenderState(RenderState.BlendOperation, BlendOperation.ReverseSubtract);
                    _device.SetRenderState(RenderState.BlendOperationAlpha, BlendOperation.Add);
                    _device.SetRenderState(RenderState.SourceBlendAlpha, Blend.SourceAlpha);
                    _device.SetRenderState(RenderState.DestinationBlendAlpha, Blend.InverseSourceAlpha);
                    _device.SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
                    _device.SetRenderState(RenderState.DestinationBlend, Blend.One);
                    return;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _vertexBuffer.Dispose();
                _effect.Dispose();
            }
        }

        ~PointSpriteRenderer()
        {
            Dispose(false);
        }
    }
}