using System.Reflection;

namespace JAngine.Entities;

public abstract class ComponentBase : ICloneable
{
    private readonly IReadOnlyDictionary<string, PropertyInfo> _properties;
    public Type Type { get; }

    public ComponentBase(PropertyCollection properties)
    {
        Type = GetType();
        _properties = Type.GetProperties()
            .Where(p => p.CanWrite && (p.PropertyType.IsValueType || p.PropertyType == typeof(string)))
            .ToDictionary(p => p.Name, p => p);

        SetPropertyValues(properties);
    }

    public bool TrySetPropertyValue(string name, object value)
    {
        if (!_properties.TryGetValue(name, out PropertyInfo? property)
            || property.PropertyType != value.GetType())
        {
            return false;
        }

        property.SetValue(this, value);
        return true;
    }

    public void SetPropertyValues(PropertyCollection properties)
    {
        foreach (var (propertyName, property) in _properties)
        {
            if (properties.TryGetProperty(property.PropertyType, propertyName, out object? value))
            {
                if (value is not null)
                {
                    property.SetValue(this, value);
                }
            }
        }
    }

    internal ComponentBase Clone(PropertyCollection? overrideProperties = null)
    {
        PropertyCollection properties = overrideProperties is null ? new PropertyCollection() : new PropertyCollection(overrideProperties);
        foreach (var (propertyName, property) in _properties)
        {
            if (!properties.Contains(propertyName))
            {
                object? value = property.GetValue(this);
                if (value is not null)
                {
                    properties.SetProperty(propertyName, value);
                }
            }
        }

        ComponentBase? obj = (ComponentBase?)Activator.CreateInstance(Type, properties);
        if (obj == null)
            throw new InvalidOperationException($"Component does not have a constructor taking a single 'PropertyColection'! {Type.FullName}");

        return obj;
    }

    internal static ComponentBase? CreateComponent(Type type, PropertyCollection properties)
    {
        return Activator.CreateInstance(type, properties) as ComponentBase;
    }

    public object Clone()
    {
        return Clone(null);
    }
}
