using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PersistentParameter
{
    public static class ParameterRegistry
    {
        private static readonly string DefaultFilePath = "parameters.json";
        public static ParameterEditorWindow ParameterWindow;

        private static Dictionary<string, object> _parameters = new();
        private static Dictionary<string, object> _storage = new(); // simulates persistence
        private static bool initialized;


        public static IEnumerable<IParameter> GetAllParameters()
        {
            var seenNames = new HashSet<string>();
            foreach (var param in _parameters.Values)
            {
            if (param is IParameter iparam && seenNames.Add(iparam.Name))
                yield return iparam;
            }
        }

        public static void LoadFromFile()
        {
            var loaded = Serializer.ReadFromFile(DefaultFilePath);
            if (loaded != null)
                _parameters = loaded;
        }

        public static Parameter<T> GetParameter<T>(string name, T defaultValue, T min, T max)
        {
            if(_parameters.ContainsKey(name))
            {
                var existing = _parameters[name];
                if (existing is Parameter<T> existingParam)
                {
                    return existingParam;
                }
                else 
                {
                    // Attempt to cast the underlying value to T
                    try
                    {
                        var castedParam = new Parameter<T>(name, (T)Convert.ChangeType(existing, typeof(T)), min, max);
                        _parameters[name] = castedParam;
                        ParameterWindow?.Update();
                        return castedParam;
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidCastException($"Parameter '{name}' exists but cannot be cast to type {typeof(T)}. Actual type: {existing.GetType()}", ex);
                    }
                }

            }

            var param = new Parameter<T>(name, defaultValue, min, max);
            _parameters[name] = param;
            ParameterWindow?.Update();
            return param;
        }

        public static Parameter<float> GetFloatParameter(string name, float defaultValue = 0f, float min = float.MinValue, float max = float.MaxValue)
        {
            if (initialized == false) initialize();
            return GetParameter(name, defaultValue, min, max);
        }

        private static void initialize()
        {
            if (initialized) return;

            // Load parameters from file or other sources
            LoadFromFile();

            initialized = true;
        }


        public static Parameter<int> GetIntParameter(string name, int defaultValue = 0, int min = int.MinValue, int max = int.MaxValue)
        {
            return GetParameter(name, defaultValue, min, max);
        }

        public static Parameter<bool> GetBoolParameter(string name, bool defaultValue = false)
        {
            // min/max not meaningful for bool, so just pass defaultValue for both
            return GetParameter(name, defaultValue, defaultValue, defaultValue);
        }

        public static Parameter<string> GetStringParameter(string name, string defaultValue = "")
        {
            // min/max not meaningful for string, so just pass defaultValue for both
            return GetParameter(name, defaultValue, defaultValue, defaultValue);
        }

        public static void Register<T>(Parameter<T> param)
        {
            if (!_parameters.ContainsKey(param.Name))
            {
                _parameters[param.Name] = param;
                ParameterWindow?.Update();
            }
        }

        public static bool TryLoad<T>(string name, out T value)
        {
                // Try to load from file using Serializer's generic loader
                value = Serializer.ReadParameter(DefaultFilePath, name, default(T));
                if (!EqualityComparer<T>.Default.Equals(value, default(T)))
                {
                    GD.Print($"loading: successfully loaded {name} = {value} from file");
                    return true;
                }
                value = default!;
                return false;
        }

        public static void Store<T>(string name, T value)
        {
            _storage[name] = value;
        }

        public static Parameter<T> Get<T>(string name) =>
            _parameters.TryGetValue(name, out var param) && param is Parameter<T> typedParam
                ? typedParam
                : null;



        public static void LoadAll<T>(Dictionary<string, T> loadedValues)
        {
            foreach (var kvp in loadedValues)
            {
                var param = Get<T>(kvp.Key);
                if (param != null)
                    param.Value = kvp.Value;
                else
                    _storage[kvp.Key] = kvp.Value;
            }
        }

        public static void SaveToFile(string filePath)
        {
            Serializer.WriteToFile(filePath, _storage);
        }

        public static void StoreAndPersist<T>(string name, T value)
        {
            Store(name, value);
            SaveToFile(DefaultFilePath);
        }
    }
}


