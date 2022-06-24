using System.Reflection;

namespace JAngine;

public interface IBootstrap
{
    public void EngineInitialized();
}

public static class BootstrapRunner
{
    public static void OnAddAssembly(Assembly assembly)
    {
        IEnumerable<IBootstrap> bootstraps = assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IBootstrap)) && t != typeof(IBootstrap))
            .Select(t => Activator.CreateInstance(t) as IBootstrap ??
                         throw new Exception($"Bootstrap {t.FullName} doesn't have parameterless constructor."));
        
        foreach (IBootstrap bootstrap in bootstraps)
        {
            bootstrap.EngineInitialized();
        }
    }
}