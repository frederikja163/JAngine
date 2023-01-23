using System.Reflection;

namespace JAngine.Extensions.Reflection;

/// <summary>
/// Contains a list of all assemblies.
/// </summary>
public static class Assemblies
{
    private static readonly List<Assembly> AssemblyList = new List<Assembly>();
    private static event Action<Assembly>? AddedAssembly;
    
    /// <summary>
    /// Called when a new assembly is added,
    /// when you add this event the event will be called for all assemblies already added.
    /// </summary>
    public static event Action<Assembly> OnAddAssembly
    {
        add
        {
            lock (AssemblyList)
            {
                AddedAssembly += value;
                foreach (Assembly assembly in AssemblyList)
                {
                    value.Invoke(assembly);
                }
            }
        }
        remove => AddedAssembly -= value;
    }

    /// <summary>
    /// Adds an Assembly to the list of all assemblies.
    /// </summary>
    /// <param name="assembly">The assembly to add.</param>
    public static void Add(Assembly assembly)
    {
        lock (AssemblyList)
        {
            AssemblyList.Add(assembly);
            AddedAssembly?.Invoke(assembly);
        }
    }
}