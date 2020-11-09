using System;
using UnityEngine;

public class HighscorePlugin : MonoBehaviourSingleton<HighscorePlugin>
{
    const string pluginName = "com.djdm.unity.MyPlugin";

    class AlertViewCallback : AndroidJavaProxy
    {
        Action<int> alertHandler;

        public AlertViewCallback(Action<int> _alertHandler) : base (pluginName + "$AlertViewCallback")
        {
            alertHandler = _alertHandler;
        }

        public void OnButtonTapped(int index)
        {
            Debug.Log("Button tapped: " + index);

            alertHandler?.Invoke(index);
        }
    }

    static AndroidJavaClass pluginClass;
    static AndroidJavaObject pluginInstance;

    public static AndroidJavaClass PluginClass
    {
        get
        {
            if (pluginClass == null)
            {
                pluginClass = new AndroidJavaClass(pluginName);
                AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
                pluginClass.SetStatic("mainActivity", activity);
            }

            return pluginClass;
        }
    }

    public static AndroidJavaObject PluginInstance
    {
        get
        {
            if (pluginInstance == null)
                pluginInstance = PluginClass.CallStatic<AndroidJavaObject>("getInstance");

            return pluginInstance;
        }
    }

    void ShowAlertDialog(string[] strings, Action<int> handler = null)
    {
        if (strings.Length < 3)
        {
            Debug.LogError("At least 3 strings needed, got " + strings.Length);
            return;
        }

        if (Application.platform == RuntimePlatform.Android)
            PluginInstance.Call("showAlertView", new object[] { strings, new AlertViewCallback(handler) });
        else
            Debug.LogError("Application platform must be Android");
    }

    void ShowDeleteFileDialog()
    {
        string[] strings = new string[] { "Borrado de archivo", "¿Borrar el puntaje más alto?", "No", "Sí" };
        Action<int> handler = (int obj) => { if (obj == -2) DeleteHighscore(); Debug.Log("Local handler called: " + obj); };
        ShowAlertDialog(strings, handler);
    }

    public void ShowHighscoreDialog()
    {
        float highscore = GetHighscore();
        string[] strings = new string[] { "Puntaje más alto", highscore + " puntos", "Cerrar", "Borrar puntaje" };
        Action<int> handler = (int obj) => { if (obj == -2) ShowDeleteFileDialog(); Debug.Log("Local handler called: " + obj); };
        ShowAlertDialog(strings, handler);
    }

    public void SetHighscore(float score)
    {
        AndroidJavaObject activity = PluginClass.GetStatic<AndroidJavaObject>("mainActivity");
        PluginInstance.Call("writeHighscore", new object[] { score, activity });
    }

    public float GetHighscore()
    {
        AndroidJavaObject activity = PluginClass.GetStatic<AndroidJavaObject>("mainActivity");
        float highscore = PluginInstance.Call<float>("readHighscore", activity);

        return highscore;
    }

    void DeleteHighscore()
    {
        AndroidJavaObject activity = PluginClass.GetStatic<AndroidJavaObject>("mainActivity");
        PluginInstance.Call("deleteHighscore", activity);
    }
}