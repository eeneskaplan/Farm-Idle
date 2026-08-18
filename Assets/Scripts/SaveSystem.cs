using System.Runtime.InteropServices;
using UnityEngine;

public static class SaveSystem
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void WebGLSave_Init();

    [DllImport("__Internal")]
    private static extern void WebGLSave_SetInt(string key, int val);

    [DllImport("__Internal")]
    private static extern int WebGLSave_GetInt(string key, int defaultVal);

    [DllImport("__Internal")]
    private static extern void WebGLSave_SetFloat(string key, float val);

    [DllImport("__Internal")]
    private static extern float WebGLSave_GetFloat(string key, float defaultVal);

    [DllImport("__Internal")]
    private static extern int WebGLSave_HasKey(string key);

    [DllImport("__Internal")]
    private static extern void WebGLSave_DeleteAll();

    [DllImport("__Internal")]
    private static extern void WebGLSave_Sync();
#endif

    private static bool initialized = false;

    public static void Init()
    {
        if (initialized) return;
        initialized = true;
#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLSave_Init();
#endif
    }

    public static void SetInt(string key, int value)
    {
        Init();
        PlayerPrefs.SetInt(key, value);
#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLSave_SetInt(key, value);
#endif
    }

    public static int GetInt(string key, int defaultValue = 0)
    {
        Init();
#if UNITY_WEBGL && !UNITY_EDITOR
        if (WebGLSave_HasKey(key) == 1)
        {
            return WebGLSave_GetInt(key, defaultValue);
        }
#endif
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public static void SetFloat(string key, float value)
    {
        Init();
        PlayerPrefs.SetFloat(key, value);
#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLSave_SetFloat(key, value);
#endif
    }

    public static float GetFloat(string key, float defaultValue = 0f)
    {
        Init();
#if UNITY_WEBGL && !UNITY_EDITOR
        if (WebGLSave_HasKey(key) == 1)
        {
            return WebGLSave_GetFloat(key, defaultValue);
        }
#endif
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    public static bool HasKey(string key)
    {
        Init();
#if UNITY_WEBGL && !UNITY_EDITOR
        if (WebGLSave_HasKey(key) == 1) return true;
#endif
        return PlayerPrefs.HasKey(key);
    }

    public static void Save()
    {
        Init();
        PlayerPrefs.Save();
#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLSave_Sync();
#endif
    }

    public static void DeleteAll()
    {
        Init();
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLSave_DeleteAll();
        WebGLSave_Sync();
#endif
    }
}
