// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using JAngine.Rendering.OpenGL;

namespace JAngine.Rendering.Gui;

internal interface IGuiElement
{
    public IGuiElement Parent { get; }
    public Matrix4x4 TransformMatrix { get; }
    
    public float Layer { get; }
    public float Width { get; }
    
    public float Height { get; }
    public float X { get; }
    
    public float Y { get; }

    public event Action PositionChanged;
}

public sealed class GuiElement : IGuiElement
{
    private static readonly Dictionary<Window, Mesh> _meshes = new Dictionary<Window, Mesh>();
    private readonly Window _window;
    private readonly Mesh _mesh;
    private readonly Instance2DRef _instanceRef;
    private readonly IGuiElement _parent;
    private Position _x;
    private Position _y;
    private Size _width;
    private Size _height;

    private GuiElement(IGuiElement parent)
    {
        if (parent is Window window)
        {
            _window = window;
            _parent = parent;
            if (!_meshes.TryGetValue(_window, out Mesh? mesh))
            {
                mesh = new Mesh(_window, "Gui");
                _meshes.Add(_window, mesh);
                mesh.AddIndices(new uint[]
                {
                    0, 1, 2, 0, 2, 3,
                });
                mesh.AddVertexAttribute<Vertex2D>();
                mesh.AddVertices<Vertex2D>(new Vertex2D[]
                {
                    new(0, 0), new(0, 1), new(1, 1), new(1, 0),
                });
                mesh.AddInstanceAttribute<Instance2D>();

                ShaderStage vertexShader = Resource.Load<ShaderStage>(window, "JAngine.Shaders.2D.vert");
                ShaderStage fragmentShader = Resource.Load<ShaderStage>(window, "JAngine.Shaders.2D.frag");
                Shader shader = new Shader(window, "Shader Program", vertexShader, fragmentShader);
                vertexShader.Dispose();
                fragmentShader.Dispose();
                mesh.BindToShader(shader);
            }
            _mesh = mesh;
        }
        else if (parent is GuiElement element)
        {
            _parent = parent;
            _window = element._window;
            _mesh = element._mesh;
        }
        else
        {
            throw new ArgumentException("Expected either a GuiElement or a window", nameof(parent));
        }
        _instanceRef = _mesh.AddInstance<Instance2D>(new Instance2D(Matrix4x4.Identity, Vector4.One));
        _parent.PositionChanged += UpdatePosition;
        _x = Position.Center();
        _y = Position.Center();
        _width = Size.Fill();
        _height = Size.Fill();
    }

    public GuiElement(Window window) : this((IGuiElement)window)
    {
    }

    public GuiElement(GuiElement parent) : this((IGuiElement)parent)
    {
    }

    public Vector4 BackgroundColor
    {
        get => _instanceRef.Color;
        set => _instanceRef.Color = value;
    }

    public Position X
    {
        get => _x;
        set
        {
            _x = value;
            UpdatePosition();
        }
    }

    public Position Y
    {
        get => _y;
        set
        {
            _y = value;
            UpdatePosition();
        }
    }

    float IGuiElement.Layer => _parent.Layer + 1;

    public Size Width
    {
        get => _width;
        set
        {
            _width = value;
            UpdatePosition();
        }
    }

    public Size Height
    {
        get => _height;
        set
        {
            _height = value;
            UpdatePosition();
        }
    }

    IGuiElement IGuiElement.Parent => _parent;

    Matrix4x4 IGuiElement.TransformMatrix => _instanceRef.Data.Transformation;
    private float _widthVal;
    private float _heightVal;
    private float _xVal;
    private float _yVal;
    float IGuiElement.Width => _widthVal;
    float IGuiElement.Height => _heightVal;
    float IGuiElement.X => _xVal;
    float IGuiElement.Y => _yVal;
    
    public event Action? PositionChanged;

    private void UpdatePosition()
    {
        Vector3 windowSize = new Vector3(_window.Width, _window.Height, 2);
        // _instanceRef.SetParentMatrixNoUpdate(_parent.TransformMatrix);
        _widthVal = Width.SizeDelegate(_parent.Width);
        _heightVal = Height.SizeDelegate(_parent.Height);
        _xVal = X.PositionDelegate(_parent.X, _parent.Width, _widthVal);
        _yVal = Y.PositionDelegate(_parent.Y, _parent.Height, _heightVal);
        Vector3 pos = new Vector3(_xVal, _yVal, _parent.Layer + 1);
        _instanceRef.SetPositionNoUpdate(pos * 2f / windowSize - Vector3.One);
        Vector3 scale = new Vector3(_widthVal, _heightVal, 0);
        _instanceRef.SetScaleNoUpdate(scale * 2 / windowSize);
        _instanceRef.Update();
        PositionChanged?.Invoke();
    }
}
