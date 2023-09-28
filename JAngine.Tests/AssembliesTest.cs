using JAngine;
using NUnit.Framework;

namespace JAngine.Tests;


public class AssembliesTest
{
    internal interface IInternalInterface
    {
    
    }

    private interface IPrivateInterface
    {
        
    }

    private class PrivateClass : IInternalInterface, IPrivateInterface
    {
        
    }
    
    [TestCase("JAngine.Tests")]
    [TestCase("JAngine")]
    public void IncludesAssembly(string name)
    {
        Assert.True(Assemblies.AllAssemblies().Any(ass => ass.FullName?.StartsWith(name) ?? false));
    }
    
    [TestCase(typeof(Assemblies))]
    [TestCase(typeof(IInternalInterface))]
    [TestCase(typeof(IPrivateInterface))]
    [TestCase(typeof(AssembliesTest))]
    public void IncludesType(Type type)
    {
        Assert.True(Assemblies.AllTypes().Any(t => t == type));
    }
}