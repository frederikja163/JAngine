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

    public static IEnumerable<T> CreateInstances<T>()
    {
        return Assemblies.AllTypes()
            .Where(t => t.IsAssignableTo(typeof(T)))
            .Select(CreateInstanceOrNull)
            .OfType<T>();

        static T? CreateInstanceOrNull(TypeInfo typeInfo)
        {
            try
            {
                return (T?)Activator.CreateInstance(typeInfo);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}