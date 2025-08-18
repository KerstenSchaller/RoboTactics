using System.IO;
using System.Collections.Generic;


public static class Serializer
{

    public static void WriteToFile(string filePath, Dictionary<string, object> dict)
    {
        using (var writer = new StreamWriter(filePath, false))
        {
            foreach (var kvp in dict)
            {
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