
using System;

public interface ISortable : IComparable<ISortable>
{
    public string name { get; }
    public int order { get; }

    int IComparable<ISortable>.CompareTo(ISortable other)
    {
        if (other == null) return 1;
        if (order < other.order) return -1;
        if (order > other.order) return 1;
        return string.CompareOrdinal(name, other.name);
    }
}
