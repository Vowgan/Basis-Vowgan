using System;
using System.Collections.Generic;

public class ValueChangedField<T>
{

    public T value { get; private set; }
    public event Action<T> onValueChanged;

    public ValueChangedField(T initialValue)
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
        onValueChanged?.Invoke(value);
        return true;
    }

}
