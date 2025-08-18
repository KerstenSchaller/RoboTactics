using Godot;
using System;
using System.Collections.Generic;

namespace PersistentParameter;

public class Parameter<T>
{
    public string Name { get; }
    private T _value;

    public Parameter(string name, T defaultValue = default!)
    {
        Name = name;

        // Register first
        ParameterRegistry.Register(this);

        // Load stored value if exists, otherwise use default
        if (ParameterRegistry.TryLoad(Name, out T storedValue))
        {
            _value = storedValue;
            GD.Print($"[LOAD] {Name} = {_value}");
        }
        else
        {
            _value = defaultValue;
            Save(); // Save initial value
        }
    }

    public T Value
    {
        get => _value;
        set
        {
            if (!EqualityComparer<T>.Default.Equals(_value, value))
            {
                _value = value;
                Save();
            }
        }
    }

    private void Save()
    {
        ParameterRegistry.StoreAndPersist(Name, _value);
        GD.Print($"[SAVE] {Name} = {_value}");
    }

    public override string ToString() => $"{Name}={_value}";

    // Implicit conversion to underlying type
    public static implicit operator T(Parameter<T> param) => param._value;

    // Optional: only for numeric types, we can define operator overloads
    public static Parameter<T> operator +(Parameter<T> param, T value)
    {
        dynamic current = param._value;
        param.Value = current + (dynamic)value;
        return param;
    }

    public static Parameter<T> operator -(Parameter<T> param, T value)
    {
        dynamic current = param._value;
        param.Value = current - (dynamic)value;
        return param;
    }

    public static Parameter<T> operator *(Parameter<T> param, T value)
    {
        dynamic current = param._value;
        param.Value = current * (dynamic)value;
        return param;
    }

    public static Parameter<T> operator /(Parameter<T> param, T value)
    {
        dynamic current = param._value;
        param.Value = current / (dynamic)value;
        return param;
    }
}