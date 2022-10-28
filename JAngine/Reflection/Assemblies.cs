using System.Reflection;

namespace JAngine.Reflection;

public static class Assemblies
{
    private static readonly List<Assembly> AllAssemblies = new List<Assembly>();
    private static readonly List<Type> AdditionalTypes = new List<Type>();

    public static void AddAssemblies(params Assembly?[] assemblies)
    {
        AllAssemblies.AddRange(
            assemblies.Where(a => a is not null)
            .Cast<Assembly>());
    }

    public static void AddType<Type>()
    {
        AdditionalTypes.Add(typeof(Type));
    }

    public static IEnumerable<Type> GetAllTypes()
    {
        return AllAssemblies
            .SelectMany(a => a.GetTypes())
            .Concat(AdditionalTypes);
    }
}