#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using HuggingFace.API; // 👈 Ensure this matches your namespace

public class APIConfigEnvInjector
{
    private const string EnvFilePath = "../.env"; // relative to Assets/
    private const string AssetPath = "Assets/Resources/HuggingFaceAPIConfig.asset"; // update if stored elsewhere

    [MenuItem("Tools/Hugging Face/Inject API Key from .env")]
    public static void InjectApiKey()
    {
        string fullPath = Path.Combine(Application.dataPath, EnvFilePath);

        if (!File.Exists(fullPath))
        {
            Debug.LogError($".env file not found at: {fullPath}");
            return;
        }

        string[] lines = File.ReadAllLines(fullPath);
        string apiKey = null;

        foreach (string line in lines)
        {
            if (line.StartsWith("API_KEY="))
            {
                apiKey = line.Substring("API_KEY=".Length).Trim();
                break;
            }
        }

        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API_KEY not found in .env file.");
            return;
        }

        var config = AssetDatabase.LoadAssetAtPath<APIConfig>(AssetPath);
        if (config == null)
        {
            Debug.LogError($"HuggingFaceAPIConfig.asset not found at path: {AssetPath}");
            return;
        }

        config.SetAPIKey(apiKey);
        EditorUtility.SetDirty(config);
        AssetDatabase.SaveAssets();

        Debug.Log("✅ Hugging Face API Key injected into APIConfig.");
    }
}
#endif
