using System.Diagnostics.CodeAnalysis;

namespace JAngine.Entities;

public sealed class PropertyCollection
{
    private readonly Dictionary<string, object> _values;

    public PropertyCollection()
    {
        _values = new Dictionary<string, object>();
    }

    public PropertyCollection(PropertyCollection original)
    {
        _values = original._values.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public PropertyCollection SetProperty(string name, object value)
    {
        _values[name] = value;
        return this;
    }

    public bool Contains(string name)
    {
        return _values.ContainsKey(name);
    }


    public bool TryGetProperty<T>(string name, [NotNullWhen(true)] out T? value)
    {
        if (_values.TryGetValue(name, out object? objectValue))
        {
            if (objectValue is T typeValue)
            {
                value = typeValue;
                return true;
            }
            // TODO: Perform conversion here if possible.
        }
        value = default;
        return false;
    }

    public bool TryGetProperty(Type type, string name, [NotNullWhen(true)] out object? value)
    {
        if (_values.TryGetValue(name, out value))
        {
            if (value.GetType().IsAssignableTo(type))
            {
                return true;
            }
            // TODO: Perform conversion here if possible.
        }
        value = null;
        return false;
    }

    // TODO: Provide a UseProperty method?

    public bool TryGetProperty(string name, [NotNullWhen(true)] out object? value)
    {
        return _values.TryGetValue(name, out value);
    }
}