using UnityEngine;
using System.IO;

[System.Serializable]
public class ApiConfig
{
    public string apiKey;
    public string geminiApikey;
}

public class ApiManager : MonoBehaviour
{
    public static string ApiKey { get; private set; }
    public static string GeminiApiKey { get; private set; }

    private void Awake()
    {
        LoadApiKey();
    }

    void LoadApiKey()
    {
        string path = Path.Combine(Application.dataPath, "../config.json");
        Debug.Log("Attempting to read config file from: " + path);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log("Config file contents: " + json);
            
            ApiConfig config = JsonUtility.FromJson<ApiConfig>(json);
            if (config != null)
            {
                ApiKey = config.apiKey;
                GeminiApiKey = config.geminiApikey;
                Debug.Log("API Keys loaded - Regular: [" + ApiKey + "], Gemini: [" + GeminiApiKey + "]");
            }
            else
            {
                Debug.LogError("Failed to parse config.json. Check if the JSON format is correct.");
            }
        }
        else
        {
            Debug.LogError("config.json not found at: " + path);
        }
    }
}
