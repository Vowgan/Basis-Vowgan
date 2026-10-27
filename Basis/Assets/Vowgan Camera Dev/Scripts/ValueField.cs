using System;
using System.Collections.Generic;

/// <summary>
/// A value store with an onChanged invocable callback.
/// </summary>
public class ValueField<T>
{

    public T value { get; private set; }
    public event Action<T> onChanged;

    public ValueField(T initialValue)
    {
        value = initialValue;
    }

    /// <summary>
    /// Set the value and run onValueChanged callbacks.
    /// If the value is the same, callbacks are not run, and this method returns false.
    /// </summary>
    public bool SetValue(T newValue)
    {
        if (EqualityComparer<T>.Default.Equals(value, newValue)) return false;

        value = newValue;
        onChanged?.Invoke(value);
        return true;
    }

}
