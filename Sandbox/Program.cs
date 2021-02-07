using System.Drawing;
using System.IO;
using System.Reflection;
using JAngine;
using JAngine.Rendering;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Window = JAngine.Window;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.AddLogger(new FileLogger("log.txt"));
            Log.AddLogger(new ConsoleLogger());
            
            Assets.AddAssets(Assembly.GetExecutingAssembly(), "");
            
            var window = new Window(800, 600, "Sandbox");
            window.MakeCurrent();
            GL.LoadBindings(new GLFWBindingsContext());

            var vbo = new Buffer<float>(new []{0f, 0f, 0f, 1f, 1f, 1f, 1f, 0f});
            var ebo = new Buffer<uint>(new uint[]{0, 1, 2, 0, 2, 3});
            var vao = new VertexArray();
            vao.AddVertexBuffer(vbo, 0, sizeof(float) * 2, 
                new VertexArray.Attribute(0, 2, VertexAttribType.Float, sizeof(float) * 2));
            vao.SetElementBuffer(ebo);
            var shader = new Shader(Assets.GetAsset<VertexShader>("shader.vert"), Assets.GetAsset<FragmentShader>("shader.frag"));
            shader.Bind();
            shader.SetUniform(shader.GetUniformLocation("uTest"), Vector4.One);
            
            GL.ClearColor(Color4.Aqua);

            while (window.IsRunning)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);
                
                vao.Draw();
                
                window.SwapBuffers();
                
                window.PollEvents();
            }
            
            vbo.Dispose();
            ebo.Dispose();
            vao.Dispose();
            shader.Dispose();
            window.Dispose();
        }
    }
}