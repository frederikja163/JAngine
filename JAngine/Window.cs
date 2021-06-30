using System;
using System.Collections.Generic;
using System.Threading;
using JAngine.Rendering;
using JAngine.Rendering.LowLevel;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GlfwWindow = OpenTK.Windowing.GraphicsLibraryFramework.Window;

namespace JAngine
{
    internal static class GlfwTracker
    {
        private static int _glfwReferences;
        
        public static void StartTracking()
        {
            if (_glfwReferences++ <= 0)
            {
                if (!GLFW.Init())
                {
                    throw new Exception("Glfw failed to init");
                }
                GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
                GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 6);
                GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlApi);
                GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
            }
        }

        public static void StopTracking()
        {
            if (--_glfwReferences <= 0)
            {
                GLFW.Terminate();
            }
        }
    }
    
    // TODO: Create better windowing.
    public sealed unsafe class Window : IDisposable
    {
        private readonly IContainer<Window> _container;
        internal readonly GlfwWindow* Handle;
        private readonly Queue<Action> _queue = new();
        private readonly Thread _thread;
        private readonly List<IDrawable> _drawables = new ();
        private readonly Matrix4 CameraMatrix;
        
        public void Queue(Action command)
        {
            _queue.Enqueue(command);
        }
        
        public Window(IContainer<Window> container, int width, int height, string title)
        {
            Log.Info($"Creating window {title} with size ({width}, {height}).");
            
            Width = width;
            Height = height;
            CameraMatrix = Matrix4.LookAt(Vector3.UnitZ, Vector3.Zero, Vector3.UnitY) *
                Matrix4.CreateOrthographic(Width, Height, 0.001f, 1000f);
            
            _container = container;
            Handle = GLFW.CreateWindow(width, height, title, null, null);
            _container.Add(this);

            _thread = new Thread(Run);
            _thread.Start();
        }

        private void Run()
        {
            try
            {
                GLFW.MakeContextCurrent(Handle);
                GLLoader.LoadBindings(new GLFWBindingsContext());
                
                Log.Info("--[OpenGL context]--");
                Log.Info($"\tVendor: \t{GL.GetString(StringName.Vendor)}");
                Log.Info($"\tRenderer: \t{GL.GetString(StringName.Renderer)}");
                Log.Info($"\tGl version: \t{GL.GetString(StringName.Version)}");
                Log.Info($"\tGlSl version: \t{GL.GetString(StringName.ShadingLanguageVersion)}");
                // Log.Info($"Extensions: {GL.GetString(StringName.Extensions)}");
                Log.Info("--[OpenGL context]--");
                
                while (IsOpen)
                {
                    while (_queue.TryDequeue(out Action? command))
                    {
                        command();
                    }
                    
                    GL.Clear(ClearBufferMask.ColorBufferBit);

                    foreach (IDrawable drawable in _drawables)
                    {
                        GL.BindVertexArray(drawable.VertexArray.Handle);
                        GL.UseProgram(drawable.Shader.Handle);

                        int texLocation = GL.GetUniformLocation(drawable.Shader.Handle, "uTexture[0]");
                        GL.BindTextures(0, drawable.Textures.TextureHandles);
                        for (int i = 0; i < 32; i++)
                        {
                            GL.ProgramUniform1i(drawable.Shader.Handle, texLocation + i, i);
                        }

                        int camLocation = GL.GetUniformLocation(drawable.Shader.Handle, "uCamera");
                        GL.ProgramUniformMatrix4f(drawable.Shader.Handle, camLocation, false, CameraMatrix.Row0.X);
                        
                        GL.DrawElementsInstanced(PrimitiveType.Triangles, drawable.VertexArray.ElementBuffer.Size,
                            DrawElementsType.UnsignedInt, 0, drawable.InstanceCount);
                    }

                    GLFW.SwapBuffers(Handle);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public int Width { get; }
        public int Height { get; }
        
        internal void AddDrawable(IDrawable drawable)
        {
            _drawables.Add(drawable);
        }

        internal void RemoveDrawable(IDrawable drawable)
        {
            _drawables.Remove(drawable);
        }

        public bool IsOpen => !GLFW.WindowShouldClose(Handle);

        public void Dispose()
        {
            GLFW.DestroyWindow(Handle);
            _container.Remove(this);
        }
    }
}