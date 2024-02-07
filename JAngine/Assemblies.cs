using System.Diagnostics.Contracts;
using System.Reflection;
using JAngine.Extensions;

namespace JAngine;

internal static class Assemblies
{
    private static readonly List<Assembly> s_assemblies;
    
    static Assemblies()
    {
        // TODO: Watch for new .dll files here as well.
        s_assemblies = Directory.GetFiles("./", "*.dll", SearchOption.AllDirectories)
            .Select(Assembly.LoadFrom)
            .CatchExceptions()
            .ToList();
    }

    [Pure]
    public static IEnumerable<Assembly> AllAssemblies()
    {
        foreach (Assembly assembly in s_assemblies)
        {
            yield return assembly;
        }
    }

    [Pure]
    public static IEnumerable<TypeInfo> AllTypes()
    {
        return s_assemblies.SelectMany(a => a.DefinedTypes);
    }

    public static IEnumerable<T> CreateInstances<T>()
    {
        return Assemblies.AllTypes()
            .Where(t => t.IsAssignableTo(typeof(T)))
            .Select(CreateInstanceOrNull)
            .Where(i => i is not null)
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
