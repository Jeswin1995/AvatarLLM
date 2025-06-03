using UnityEngine;
using System.Reflection;
using GoogleTextToSpeech.Scripts;
using GoogleSpeechToText.Scripts;

public class ApiKeyInitializer : MonoBehaviour
{
    [SerializeField] private GameObject geminiManager;
    [SerializeField] private GameObject textToSpeech;
    [SerializeField] private GameObject speechToText;

    private void Start()
    {
        Debug.Log($"Loaded API Keys - Regular: {ApiManager.ApiKey}, Gemini: {ApiManager.GeminiApiKey}");
        InitializeApiKeys();
    }

    private void InitializeApiKeys()
    {
        // Set Gemini API Key
        if (geminiManager != null)
        {
            var geminiScript = geminiManager.GetComponent<UnityAndGeminiV3>();
            if (geminiScript != null)
            {
                // apiKey is public in UnityAndGeminiV3
                geminiScript.apiKey = ApiManager.GeminiApiKey;
                Debug.Log($"Gemini API Key initialized successfully: {ApiManager.GeminiApiKey}");
            }
            else
            {
                Debug.LogError("UnityAndGeminiV3 component not found on GeminiManager");
            }
        }
        else
        {
            Debug.LogError("GeminiManager GameObject reference is missing");
        }

        // Set Text-to-Speech API Key
        if (textToSpeech != null)
        {
            var textToSpeechComponent = textToSpeech.GetComponent<TextToSpeech>();
            if (textToSpeechComponent != null)
            {
                SetPrivateField(textToSpeechComponent, "apiKey", ApiManager.ApiKey);
                Debug.Log($"Text-to-Speech API Key initialized successfully: {ApiManager.ApiKey}");
            }
            else
            {
                Debug.LogError("TextToSpeech component not found on TEXT_TO_SPEECH GameObject");
            }
        }
        else
        {
            Debug.LogError("TEXT_TO_SPEECH GameObject reference is missing");
        }

        // Set Speech-to-Text API Key
        if (speechToText != null)
        {
            var sttScript = speechToText.GetComponent<SpeechToTextManager>();
            if (sttScript != null)
            {
                // Speech-to-Text needs the regular API key, not the Gemini key
                SetPrivateField(sttScript, "apiKey", ApiManager.ApiKey);
                Debug.Log($"Speech-to-Text API Key initialized successfully: {ApiManager.ApiKey}");
            }
            else
            {
                Debug.LogError("SpeechToTextManager component not found on GoogleServices GameObject");
            }
        }
        else
        {
            Debug.LogError("GoogleServices GameObject reference is missing");
        }
    }

    private void SetPrivateField(object target, string fieldName, string value)
    {
        var field = target.GetType().GetField(fieldName, 
            BindingFlags.NonPublic | BindingFlags.Instance);
        
        if (field != null)
        {
            field.SetValue(target, value);
        }
        else
        {
            Debug.LogError($"Field '{fieldName}' not found in {target.GetType().Name}");
        }
    }
} 