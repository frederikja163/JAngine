namespace JAngine;

internal sealed class TypeComparer : IComparer<Type>, IEqualityComparer<Type>
{
    internal static TypeComparer Default { get; } = new TypeComparer();
    
    public int Compare(Type? x, Type? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        return string.Compare(x.FullName, y.FullName, StringComparison.Ordinal);
    }

    public bool Equals(Type? x, Type? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        return x.FullName == y.FullName && x.GUID.Equals(y.GUID);
    }

    public int GetHashCode(Type obj)
    {
        return HashCode.Combine(obj.FullName, obj.GUID);
    }
}
