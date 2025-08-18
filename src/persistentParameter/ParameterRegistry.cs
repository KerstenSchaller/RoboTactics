using Godot;
using System;
using System.Collections.Generic;



namespace PersistentParameter;

public static class ParameterRegistry
{

    private static readonly string DefaultFilePath = "parameters.json";
    public static void LoadFromFile()
    {
        var loaded = Serializer.ReadFromFile(DefaultFilePath);
        if (loaded != null)
            _storage = loaded;
    }
    private static Dictionary<string, object> _parameters = new();
    private static Dictionary<string, object> _storage = new(); // simulates persistence

    public static void Register<T>(Parameter<T> param)
    {
        if (!_parameters.ContainsKey(param.Name))
            _parameters[param.Name] = param;
        else
            Console.WriteLine($"[WARNING] Parameter '{param.Name}' is already registered!");
    }

    public static bool TryLoad<T>(string name, out T value)
    {
        GD.Print($"loading: {name}");

        LoadFromFile(); // Ensure _storage is populated

        if (_storage.TryGetValue(name, out Object stored))
        {
            try
            {
                // Convert stored primitive to requested type
                value = (T)Convert.ChangeType(stored, typeof(T));
                GD.Print($"loading: success {value}");
                return true;
            }
            catch (Exception e)
            {
                GD.Print($"loading: failed to convert {name}, stored type: {stored.GetType()}, error: {e.Message}");
            }
        }
        else
        {
            GD.Print($"loading: {name} not found");
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
                _storage[kvp.Key] = kvp.Value; // store for future Parameter creation
        }
    }

    public static void SaveToFile(string filePath)
    {
        Serializer.WriteToFile(filePath, _storage);
    }

    // Call this from Parameter<T>.Save()
 
    public static void StoreAndPersist<T>(string name, T value)
    {
        Store(name, value);
        SaveToFile(DefaultFilePath);
    }
}


