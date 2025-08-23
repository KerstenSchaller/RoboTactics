using Godot;
using System;
using System.Collections.Generic;

namespace PersistentParameter
{
    public static class ParameterRegistry
    {
        private static readonly string DefaultFilePath = "parameters.json";
        public static ParameterEditorWindow ParameterWindow;

        private static Dictionary<string, object> _parameters = new();
        private static Dictionary<string, object> _storage = new(); // simulates persistence

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
                _storage = loaded;
        }

        public static Parameter<T> CreateParameter<T>(string name, T defaultValue, T min, T max)
        {
            if (_parameters.TryGetValue(name, out var existing) && existing is Parameter<T> typed)
            return typed;

            var param = new Parameter<T>(name, defaultValue, min, max);
            _parameters[name] = param;
            ParameterWindow?.Update();
            return param;
        }

        public static Parameter<float> GetFloatParameter(string name, float defaultValue = 0f, float min = float.MinValue, float max = float.MaxValue)
        {
            return CreateParameter(name, defaultValue, min, max);
        }

        public static Parameter<int> GetIntParameter(string name, int defaultValue = 0, int min = int.MinValue, int max = int.MaxValue)
        {
            return CreateParameter(name, defaultValue, min, max);
        }

        public static Parameter<bool> GetBoolParameter(string name, bool defaultValue = false)
        {
            // min/max not meaningful for bool, so just pass defaultValue for both
            return CreateParameter(name, defaultValue, defaultValue, defaultValue);
        }

        public static Parameter<string> GetStringParameter(string name, string defaultValue = "")
        {
            // min/max not meaningful for string, so just pass defaultValue for both
            return CreateParameter(name, defaultValue, defaultValue, defaultValue);
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
            LoadFromFile(); // Ensure _storage is populated

            if (_storage.TryGetValue(name, out Object stored))
            {
                try
                {
                    value = (T)Convert.ChangeType(stored, typeof(T));
                    return true;
                }
                catch (Exception e)
                {
                    GD.PrintErr($"loading: failed to convert {name}, stored type: {stored.GetType()}, error: {e.Message}");
                }
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

        public static void SaveAll()
        {
            foreach (var param in _parameters.Values)
                Console.WriteLine($"[SAVE ALL] {param}");
        }

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


