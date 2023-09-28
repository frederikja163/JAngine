using JAngine.Rendering;
using NUnit.Framework;

namespace JAngine.Tests;

public class ResourceTests
{
    public class IntResourceLoader : IResourceLoader<int>
    {
        public int Load(Window window, string path, Stream stream)
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
        Window window = new Window("__test__window__", 0, 0);
        int loadedValue = Resource<int>.Load(window, path);
        Assert.That(loadedValue, Is.EqualTo(expected));
    }

    [Test]
    public void TestCannotLoadException()
    {
        Window window = new Window("__test__window__", 0, 0);
        Assert.Throws<TypeInitializationException>(() => Resource<Thread>.Load(window, "Test"));
        Assert.Throws<FileNotFoundException>(() => Resource<int>.Load(window, "Test"));
    }
}