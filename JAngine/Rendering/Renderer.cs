using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering;

internal static class Renderer
{
    private static readonly Dictionary<Material, List<Mesh>> MeshesByMaterial = new Dictionary<Material, List<Mesh>>();
    public static void Init()
    {
        GL.ClearColor(1, 0, 1, 1);

        GL.Enable(EnableCap.DepthTest);
        GL.Enable(EnableCap.AlphaTest);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
    }

    public static int MaxTextures
    {
        get
        {
            int value = 0;
            GL.GetInteger(GetPName.MaxCombinedTextureImageUnits, ref value);
            return value;
        }
    }

    public static void AddToRender(Mesh mesh, Material material)
    {
        if (!MeshesByMaterial.TryGetValue(material, out List<Mesh>? meshes))
        {
            MeshesByMaterial.Add(material, meshes = new List<Mesh>());
        }
        meshes.Add(mesh);
    }

    public static void Clear()
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    public static void RenderScene()
    {
        foreach (var (material, meshes) in MeshesByMaterial)
        {
            material.Bind();
            foreach (var mesh in meshes)
            {
                mesh.Bind();
                GL.DrawElementsInstanced(PrimitiveType.Triangles, mesh.IndexCount, DrawElementsType.UnsignedInt, 0, mesh.InstanceCount);
            }
        }
    }
}
