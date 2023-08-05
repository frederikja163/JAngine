using System.Diagnostics.Contracts;
using System.Reflection;

namespace JAngine.Core;

internal static class Assemblies
{
    private static readonly List<Assembly> _assemblies;
    
    static Assemblies()
    {
        // TODO: Watch for new .dll files here as well.
        _assemblies = Directory.GetFiles("./", "*.dll", SearchOption.AllDirectories)
            .Select(Assembly.LoadFrom)
            .CatchExceptions()
            .ToList();
    }

    [Pure]
    public static IEnumerable<Assembly> AllAssemblies()
    {
        foreach (Assembly assembly in _assemblies)
        {
            yield return assembly;
        }
    }

    [Pure]
    public static IEnumerable<TypeInfo> AllTypes()
    {
        return _assemblies.SelectMany(a => a.DefinedTypes);
    }

    [Pure]
    public static IEnumerable<T> CreateInstances<T>(params object[] args)
    {
        return Assemblies.AllTypes()
            .Where(t => t.IsAssignableTo(typeof(T)))
            .Select((t) => Activator.CreateInstance(t, args))
            .CatchExceptions()
            .OfType<T>();
    }
}