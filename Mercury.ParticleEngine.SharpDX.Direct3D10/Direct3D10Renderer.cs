namespace Mercury.ParticleEngine.Renderers
{
    using SharpDX;
    using SharpDX.Direct3D10;
    using System.Runtime.InteropServices;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D;

    using Format = SharpDX.DXGI.Format;
    using Exception = System.Exception;

    public class Direct3D10Renderer
    {
        private readonly Device _device;
        private readonly int _size;
        private readonly Buffer _vertexBuffer;
        private readonly VertexBufferBinding _vertexBufferBinding;
        private readonly Effect _effect;
        private readonly GeometryShader _geometryShader;
        private readonly InputLayout _inputLayout;

        public Direct3D10Renderer(Device device, int size)
        {
            _device = device;
            _size = size;
            _vertexBuffer = new Buffer(_device, _size * Particle.SizeInBytes, ResourceUsage.Dynamic, BindFlags.VertexBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None);

            _vertexBufferBinding = new VertexBufferBinding(_vertexBuffer, Particle.SizeInBytes, 0);

            var geometryShaderBytecode = ShaderBytecode.CompileFromFile("GeometryShader.fx", "GS");
            _geometryShader = new GeometryShader(_device, geometryShaderBytecode);

            _inputLayout = new InputLayout(_device, geometryShaderBytecode, new[]
            {
                new InputElement("COLOR",    1, Format.R32_Float,       (int)Marshal.OffsetOf(typeof(Particle), "Age"),      0, InputClassification.PerVertexData, 0),
                new InputElement("POSITION", 0, Format.R32G32_Float,    (int)Marshal.OffsetOf(typeof(Particle), "Position"), 0, InputClassification.PerVertexData, 0),
                new InputElement("COLOR",    0, Format.R32G32B32_Float, (int)Marshal.OffsetOf(typeof(Particle), "Colour"),   0, InputClassification.PerVertexData, 0),
                new InputElement("COLOR",    2, Format.R32_Float,       (int)Marshal.OffsetOf(typeof(Particle), "Scale"),    0, InputClassification.PerVertexData, 0),
                new InputElement("COLOR",    3, Format.R32_Float,       (int)Marshal.OffsetOf(typeof(Particle), "Rotation"), 0, InputClassification.PerVertexData, 0),
            });
        }

        public void Render(Emitter emitter, Matrix worldViewProjection, Texture2D texture)
        {
            if (emitter.ActiveParticles == 0)
                return;

            if (emitter.ActiveParticles > _size)
                throw new Exception("Cannot render this emitter, vertex buffer not big enough.");

            var technique = _effect.GetTechniqueByIndex(0);

            _effect.GetVariableByName("WVPMatrix").AsMatrix().SetMatrix(worldViewProjection);

            using (var dataStream = _vertexBuffer.Map(MapMode.WriteDiscard, MapFlags.None))
            {
                emitter.Buffer.CopyTo(dataStream.DataPointer);
            }

            _device.InputAssembler.InputLayout = _inputLayout;
            _device.InputAssembler.PrimitiveTopology = PrimitiveTopology.PointList;
            _device.InputAssembler.SetVertexBuffers(0, _vertexBufferBinding);
            _device.GeometryShader.Set(_geometryShader);

            _device.Draw(emitter.ActiveParticles, 0);
        }
    }
}