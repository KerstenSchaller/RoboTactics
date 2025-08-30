
using System;
using System.IO;
using System.Collections.Generic;
using Godot;


public static class Serializer
{
    // Loads a parameter value of type T from file, returns defaultValue if not found or conversion fails
    public static T ReadParameter<T>(string filePath, string name, T defaultValue = default)
    {
        var dict = ReadFromFile(filePath);
        if (dict.TryGetValue(name, out var valueStr) && valueStr is string s)
        {
            try
            {
                return (T)Convert.ChangeType(s, typeof(T));
            }
            catch
            {
                // fallback to default if conversion fails
            }
        }
        return defaultValue;
    }

    public static void WriteToFile(string filePath, Dictionary<string, object> dict)
    {
        using (var writer = new StreamWriter(filePath, false))
        {
            GD.Print($"Writing parameters to {filePath}");
            foreach (var kvp in dict)
            {
                GD.Print($"Writing parameter: {kvp.Key} = {kvp.Value}");
                // Escape '=' and newlines if needed
                var key = kvp.Key.Replace("=", "\\=").Replace("\n", "\\n");
                var value = kvp.Value?.ToString().Replace("=", "\\=").Replace("\n", "\\n") ?? "";
                writer.WriteLine($"{key}={value}");
            }
        }
    }

    public static Dictionary<string, object> ReadFromFile(string filePath)
    {
        var dict = new Dictionary<string, object>();
        if (!File.Exists(filePath))
            return dict;

        foreach (var line in File.ReadAllLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line) || !line.Contains("="))
                continue;

            var idx = line.IndexOf('=');
            var key = line.Substring(0, idx).Replace("\\=", "=").Replace("\\n", "\n");
            var value = line.Substring(idx + 1).Replace("\\=", "=").Replace("\\n", "\n");
            dict[key] = value;
        }
        return dict;
    }
}