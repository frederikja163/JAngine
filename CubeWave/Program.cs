using System;
using System.Diagnostics;
using System.Reflection;
using JAngine;
using JAngine.Rendering;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Window = JAngine.Windowing.Window;

namespace CubeWave
{
    class Program
    {
        private static readonly Color4 _color = new Color4(1, 1, 1, 1);
        private static readonly float _size = 1f;
        
        static void Main(string[] args)
        {
            Log.AddLogger(new ConsoleLogger());

            Assets.AddAssets(Assembly.GetExecutingAssembly(), "Assets");

            var window = new Window(800, 600, "CubeWave");
            window.MakeCurrent();
            GL.LoadBindings(new GLFWBindingsContext());
            
            GL.Enable(EnableCap.DepthTest);
            
            var shader = new Shader(Assets.GetAsset<VertexShader>("shader.vert"),
                Assets.GetAsset<FragmentShader>("shader.frag"));
            
            var positionBuffer = new Buffer<float>(new float[]{
                -_size, -_size, -_size, //0
                -_size, -_size,  _size, //1
                -_size,  _size, -_size, //2
                -_size,  _size,  _size, //3
                 _size, -_size, -_size, //4
                 _size, -_size,  _size, //5
                 _size,  _size, -_size, //6
                 _size,  _size,  _size, //7
                });
            var ebo = new Buffer<uint>(new uint[]{
                    0, 1, 2, 1, 2, 3, //Left
                    4, 5, 6, 5, 6, 7, //Right
                    0, 1, 4, 1, 4, 5, //Bottom
                    2, 3, 6, 3, 6, 7, //Top
                    0, 2, 4, 2, 4, 6, //Front
                    1, 3, 5, 3, 5, 7, //Back
                });
            var vao = new VertexArray();
            vao.SetElementBuffer(ebo);
            vao.AddVertexBuffer(positionBuffer, 0, sizeof(float) * 3, 
                new VertexArray.Attribute(0, 3, VertexAttribType.Float, 0));

            var perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(65), window.Width / (float)window.Height, 0.001f,
                1000f);
            var viewLoc = shader.GetUniformLocation("uView");
            var timeLoc = shader.GetUniformLocation("uTime");
            shader.Bind();
            shader.SetUniform(shader.GetUniformLocation("uPerspective"), ref perspective);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            vao.Bind();
            while (window.IsRunning)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                
                var t = stopWatch.ElapsedMilliseconds / 5000f;
                var view = Matrix4.LookAt(new Vector3(MathF.Cos(t) * 5, 5f, MathF.Sin(t) * 5), Vector3.Zero, Vector3.UnitY);
                shader.SetUniform(viewLoc, ref view);
                shader.SetUniform(timeLoc, ref t);
                GL.DrawElementsInstanced(PrimitiveType.Triangles, 36, DrawElementsType.UnsignedInt, IntPtr.Zero, 10000);
                
                window.SwapBuffers();
                
                window.PollEvents();
            }
            
            window.Dispose();
        }
    }
}