using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class EnvLoader
{
    private static Dictionary<string, string> envVars = new Dictionary<string, string>();
    private static bool isLoaded = false;

    public static void Load()
    {
        if (isLoaded) return;

        string path = Path.Combine(Application.dataPath, "../.env");

        if (!File.Exists(path))
        {
            Debug.LogWarning(".env file not found at: " + path);
            return;
        }

        string[] lines = File.ReadAllLines(path);
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            string[] parts = line.Split('=', 2);
            if (parts.Length == 2)
                envVars[parts[0].Trim()] = parts[1].Trim();
        }

        isLoaded = true;
    }

    public static string Get(string key)
    {
        if (!isLoaded) Load();

        if (envVars.ContainsKey(key))
            return envVars[key];

        Debug.LogWarning("Key not found in .env: " + key);
        return null;
    }
}
