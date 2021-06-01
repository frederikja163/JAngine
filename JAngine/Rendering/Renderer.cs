using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JAngine.Rendering.LowLevel;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering
{
    public sealed class Renderer : IDisposable
    {
        private readonly Queue<Action> _queue = new();
        private readonly Thread _thread;
        internal bool IsRunning { get; set; }
        
        internal Renderer()
        {
            _thread = new Thread(Run);
            _thread.Start();
        }

        private void Run()
        {
            while (IsRunning)
            {
                while (_queue.TryDequeue(out Action? command))
                {
                    command();
                }
            }
        }
        
        public void Queue(Action command)
        {
            _queue.Enqueue(command);
        }
        
        public void Draw(VertexArray vao, ShaderProgram shader)
        {
            vao.Bind();
            shader.Bind();
            
            GL.DrawElements(PrimitiveType.Triangles, vao.ElementBuffer.Size, DrawElementsType.UnsignedInt, 0);
        }
        
        public void Dispose()
        {
            
        }
    }
}