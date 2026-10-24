using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// A wrapper for a System.Collections.Generic.List, supporting callbacks.
/// </summary>
[Serializable]
public class CallbackList<T> : IEnumerable<T>
{
    /// <summary>
    /// (Index, Value)
    /// When a value is added, this passes the value's index and the value itself.
    /// </summary>
    public event Action<int, T> onIndexAdded;

    /// <summary>
    /// (Index, Value)
    /// When a value is removed, this passes the value's previous index and the value itself.
    /// </summary>
    public event Action<int, T> onIndexRemoved;

    /// <summary>
    /// (Index, Value)
    /// When a value is modified, this passes the value's index and the value itself.
    /// </summary>
    public event Action<int, T> onIndexChanged;

    [SerializeField] private List<T> _list;

    public int Count => _list.Count;


    public CallbackList()
    {
        _list = new();
    }

    public CallbackList(int capacity)
    {
        _list = new(capacity);
    }

    public CallbackList([NotNull] IEnumerable<T> contents)
    {
        _list = new(contents);
    }


    public T this[int index]
    {
        get => _list[index];
        set
        {
            _list[index] = value;
            onIndexChanged?.Invoke(index, value);
        }
    }


    public void Add(T value)
    {
        _list.Add(value);
        onIndexAdded?.Invoke(Count - 1, value);
    }

    public void AddRange(IEnumerable<T> values)
    {
        IEnumerable<T> items = values as T[] ?? values.ToArray();
        foreach (T item in items)
        {
            _list.Add(item);
            onIndexAdded?.Invoke(Count - 1, item);
        }
    }

    public void Remove(T value)
    {
        int index = _list.IndexOf(value);
        if (index == -1) return;

        _list.Remove(value);
        onIndexRemoved?.Invoke(index, value);
    }

    public void Clear()
    {
        List<(int, T)> items = _list.Select((t, i) => (i, t)).ToList();

        _list.Clear();

        if (onIndexRemoved != null)
            foreach ((int index, T item) in items)
                onIndexRemoved.Invoke(index, item);
    }

    public int IndexOf([CanBeNull] T value) => _list.IndexOf(value);

    public void Sort() => _list.Sort();
    public void Sort([NotNull] Comparison<T> comparison) => _list.Sort(comparison);
    public void Sort(IComparer<T> comparer) => _list.Sort(comparer);
    public void Sort(int index, int count, IComparer<T> comparer) => _list.Sort(index, count, comparer);




    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
}
