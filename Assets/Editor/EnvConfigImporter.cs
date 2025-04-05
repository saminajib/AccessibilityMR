#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class EnvConfigImporter : EditorWindow
{
    private static string envFilePath = Path.Combine(Application.dataPath, "../.env");

    [MenuItem("Tools/Import .env to EnvConfig")]
    public static void ImportEnvConfig()
    {
        if (!File.Exists(envFilePath))
        {
            Debug.LogWarning(".env file not found at: " + envFilePath);
            return;
        }

        string[] lines = File.ReadAllLines(envFilePath);
        string assetPath = "Assets/EnvConfig.asset";

        EnvConfig config = AssetDatabase.LoadAssetAtPath<EnvConfig>(assetPath);
        if (config == null)
        {
            config = ScriptableObject.CreateInstance<EnvConfig>();
            AssetDatabase.CreateAsset(config, assetPath);
        }

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            string[] parts = line.Split('=', 2);
            if (parts.Length == 2)
            {
                string key = parts[0].Trim();
                string value = parts[1].Trim();

                if (key == "API_KEY")
                    config.apiKey = value;
                else if (key == "ENVIRONMENT")
                    config.environment = value;
            }
        }

        EditorUtility.SetDirty(config);
        AssetDatabase.SaveAssets();
        Debug.Log("✅ EnvConfig.asset created/updated!");
    }
}
#endif
