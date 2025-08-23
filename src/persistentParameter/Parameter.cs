using Godot;
using System;
using System.Collections.Generic;

namespace PersistentParameter
{
    public interface IParameter
    {
        string Name { get; }
        Type ValueType { get; }
        object GetValue();
        void SetValue(object value);
    }

    public class Parameter<T> : IParameter
    {
        public T Min { get; }
        public T Max { get; }
        public string Name { get; }
        private T _value;
        public Type ValueType => typeof(T);

        public object GetValue() => _value!;

        public void SetValue(object value)
        {
            _value = (T)Convert.ChangeType(value, typeof(T));
            Save();
        }

        // Make constructor internal so only registry can call it
        internal Parameter(string name, T defaultValue, T min, T max)
        {
            Name = name;
            Min = min;
            Max = max;

            if (ParameterRegistry.TryLoad(Name, out T storedValue))
            {
                _value = storedValue;
                GD.Print($"[LOAD] {Name} = {_value}");
            }
            else
            {
                _value = defaultValue;
                GD.Print($"[DEFAULT] {Name} = {_value}");
                Save();
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
                    GD.Print($"[EXTSAVE] {Name} = {_value}");
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

        public static implicit operator T(Parameter<T> param) => param._value;

        // Operator overloads remain unchanged...
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
}