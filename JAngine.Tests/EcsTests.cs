using JAngine;
using NUnit.Framework;

namespace JAngine.Tests;

public sealed class EcsTests
{
    [TestCase(1)]
    [TestCase(1, 0.1f)]
    [TestCase(1, 0.1f, 0.2)]
    [TestCase(1, 0.1f, 0.2, 'a')]
    public void CreateEntities(params object[] components)
    {
        World world = new World();
        Entity expected = world.CreateEntity(components);
        Entity? actual = world.GetEntities().FirstOrDefault();
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void AddComponent()
    {
        World world = new World();
        Entity entity = world.CreateEntity(1);
        
        Assert.That(world.GetComponents<float>().Any(), Is.False);
        Assert.That(world.GetComponents<char>().Any(), Is.False);
        
        entity.AddComponent(0.1f);
        entity.AddComponent<char>();
        
        Assert.That(world.GetEntities().Count(), Is.EqualTo(1));
        Assert.That(world.GetEntities<int, float, char>().FirstOrDefault(), Is.EqualTo(entity));
        Assert.That(world.GetComponents<int, float, char>().FirstOrDefault(), Is.EqualTo((1, 0.1f, default(char))));
    }
    
    [Test]
    public void RemoveComponent()
    {
        World world = new World();
        Entity entity = world.CreateEntity(1, 0.1f, 'a');
        
        
        entity.RemoveComponent<float>();
        entity.RemoveComponent<char>();
        
        Assert.That(world.GetComponents<float>().Any(), Is.False);
        Assert.That(world.GetComponents<char>().Any(), Is.False);
        Assert.That(world.GetEntities().Count(), Is.EqualTo(1));
        Assert.That(world.GetEntities<int>().FirstOrDefault(), Is.EqualTo(entity));
        Assert.That(world.GetComponents<int>().FirstOrDefault(), Is.EqualTo(1));
    }

    [Test]
    public void TestSingleComponentEntity()
    {
        World world = new World();
        Entity expected = world.CreateEntity(1);
        Entity? actual = world.GetEntities<int>().FirstOrDefault();
        int component = world.GetComponents<int>().FirstOrDefault();
        
        Assert.IsNotNull(expected);
        Assert.IsNotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(component, Is.EqualTo(1));
    }
    
    [Test]
    public void TestDoubleComponentEntity()
    {
        World world = new World();
        Entity expected = world.CreateEntity(1, 0.1f);
        Entity? actual = world.GetEntities<int, float>().FirstOrDefault();
        (int, float) component = world.GetComponents<int, float>().FirstOrDefault();
        
        Assert.IsNotNull(expected);
        Assert.IsNotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(component, Is.EqualTo((1, 0.1f)));
    }
    
    [Test]
    public void TestTrippleComponentEntity()
    {
        World world = new World();
        Entity expected = world.CreateEntity(1, 0.1f, 0.2);
        Entity? actual = world.GetEntities<int, float, double>().FirstOrDefault();
        (int, float, double) component = world.GetComponents<int, float, double>().FirstOrDefault();
        
        Assert.IsNotNull(expected);
        Assert.IsNotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(component, Is.EqualTo((1, 0.1f, 0.2)));
    }
    
    [Test]
    public void TestQuadroupleComponentEntity()
    {
        World world = new World();
        Entity expected = world.CreateEntity(1, 0.1f, 0.2, 'a');
        Entity? actual = world.GetEntities<int, float, double, char>().FirstOrDefault();
        (int, float, double, char) component = world.GetComponents<int, float, double, char>().FirstOrDefault();
        
        Assert.IsNotNull(expected);
        Assert.IsNotNull(actual);
        Assert.That(actual, Is.EqualTo(expected));
        Assert.That(component, Is.EqualTo((1, 0.1f, 0.2, 'a')));
    }
}