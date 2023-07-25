using JAngine.Core;
using NUnit.Framework;

namespace JAngine.Tests;

public class ResourceTests
{
    public class IntResourceLoader : IResourceLoader<int>
    {
        public int Load(Stream stream)
        {
            StreamReader sr = new StreamReader(stream);
            return int.Parse(sr.ReadToEnd());
        }
    }
    
    [TestCase("JAngine.Tests.Resource1.txt", 540)]
    [TestCase("JAngine/Tests/Resource1.txt", 540)]
    [TestCase("Resource2.txt", 43179)]
    public void TestCustomResourceLoader(string path, int expected)
    {
        int loadedValue = Resource<int>.Load(path);
        Assert.That(loadedValue, Is.EqualTo(expected));
    }
}