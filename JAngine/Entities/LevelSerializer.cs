using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using JAngine.Reflection;

namespace JAngine.Entities;

public static class LevelSerializer
{
    public static void Serialize(Level level, string path)
    {
        using Stream stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
        XmlDocument document = new XmlDocument();
        document.Load(stream);
        throw new NotImplementedException();
    }

    public static Level Deserialize(string path)
    {
        using Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        XmlDocument document = new XmlDocument();
        document.Load(stream);

        XmlNode? levelNode = document.SelectSingleNode("Level");
        if (levelNode == null)
            throw new FormatException($"{path} a <Level> tag is required to load a level.");

        Level level = new Level();
        XmlNodeList? entityLists = levelNode.SelectNodes("./Entities");
        if (entityLists != null)
            foreach (XmlElement element in entityLists)
            {
                GetEntities(element, level.Entities);
            }

        return level;
    }

    private static void GetEntities(XmlElement element, EntityCollection entities)
    {
        XmlNodeList? entityNodes = element.SelectNodes("./Entity");
        if (entityNodes == null)
            return;

        foreach (XmlElement entityElement in entityNodes)
        {
            Entity entity = CreateEntity(entityElement, entities, out string name);
            Debug.WriteLineIf(!entities.TryAddEntity(name, entity), $"Multiple prototypes found with name {name}.");
        }
    }

    private static Entity CreateEntity(XmlElement element, EntityCollection entities, out string name)
    {
        name = element.GetAttribute("name");

        string baseEntity = element.GetAttribute("base");
        Entity? entity = null;
        if (!string.IsNullOrWhiteSpace(baseEntity))
            Debug.WriteLineIf(!entities.TryGetPrototype(baseEntity, out entity), $"{baseEntity} is not a valid entity base.");
        if (entity == null)
            entity = new Entity();
        else
            entity = entity.CreateClone();

        XmlNodeList? componentNodes = element.SelectNodes("Component");
        if (componentNodes == null)
            return entity;
        foreach (XmlElement componentNode in componentNodes)
        {
            if (TryCreateComponentProperties(componentNode, out PropertyCollection? properties, out Type? compType))
            {
                if (!entity.TryGetComponent(compType, out ComponentBase? component))
                {
                    component = ComponentBase.CreateComponent(compType, properties);
                    if (component == null)
                        continue;
                    entity.TryAddComponent(component);
                }
                else
                {
                    component.SetPropertyValues(properties);
                }
            }
        }

        return entity;
    }

    private static bool TryCreateComponentProperties(XmlElement element, [NotNullWhen(true)] out PropertyCollection? component, [NotNullWhen(true)] out Type? compType)
    {
        compType = GetComponentType(element.GetAttribute("type"));
        if (compType == null)
        {
            Debug.WriteLine($"Component type not found!");
            component = null;
            return false;
        }

        component = new PropertyCollection();
        XmlNodeList propertyNodes = element.ChildNodes;
        foreach (XmlElement propertyElement in propertyNodes)
        {
            Debug.WriteLineIf(!TryAddProperty(propertyElement, compType, component),
                $"{propertyElement.InnerText} not assignable to property of type {compType}.");
        }
        return true;
    }

    private static Type? GetComponentType(string name)
    {
        return Assemblies.GetAllTypes()
            .FirstOrDefault(t => t.Name == name && t.IsAssignableTo(typeof(ComponentBase)));
    }

    private static bool TryAddProperty(XmlElement element, Type compType, PropertyCollection component)
    {
        string name = element.Name;
        if (!TryGetPropertyType(compType, name, out Type? type))
            return false;

        object? value = GetValueType(element.InnerText, type);
        if (value == null)
            return false;
        component.SetProperty(name, value);
        return true;
    }

    private static bool TryGetPropertyType(Type compType, string name, [NotNullWhen(true)] out Type? propType)
    {
        propType = compType.GetProperty(name)?.PropertyType;
        return propType != null;
    }


    private static readonly IReadOnlyDictionary<Type, Func<string, object>> TypeConverters =
        new Dictionary<Type, Func<string, object>>()
        {
            {typeof(string), s => s},
            {typeof(char), s => char.Parse(s)},
            {typeof(bool), s => bool.Parse(s)},
            {typeof(float), s => float.Parse(s)},
            {typeof(double), s => double.Parse(s)},
            {typeof(long), s => long.Parse(s)},
            {typeof(ulong), s => ulong.Parse(s)},
            {typeof(int), s => int.Parse(s)},
            {typeof(uint), s => uint.Parse(s)},
            {typeof(short), s => short.Parse(s)},
            {typeof(ushort), s => ushort.Parse(s)},
            {typeof(sbyte), s => sbyte.Parse(s)},
            {typeof(byte), s => byte.Parse(s)},
        };
    private static object? GetValueType(string str, Type type)
    {
        if (!TypeConverters.TryGetValue(type, out Func<string, object>? parser))
            return null;

        try
        {
            return parser(str);
        }
        catch
        {
            return null;
        }
    }
}
