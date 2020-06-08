using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Native Plugin Manager [Android Only]
/// </summary>
public class NativeManager : MonoBehaviour {
    private static NativeManager _instance;
    public static NativeManager instance {
        get {
            if(_instance == null) {
                _instance = FindObjectOfType<NativeManager>();
                if (_instance==null) {
                    var go = new GameObject(GAME_OBJECT_NAME);
                    _instance = go.AddComponent<NativeManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
    /// <summary>
    /// Android Native Plugin pakage name
    /// </summary>
    public const string PAKAGE_NAME = "com.bss.download.PluginManager";
    /// <summary>
    /// This GameObejct name.
    /// Required for initialization.
    /// </summary>
    public const string GAME_OBJECT_NAME = "NativeManager";

    /// <summary>
    /// Callback function name when native plugin calls send message
    /// Required for initialization.
    /// </summary>
    public const string CALLBACK_FUNTION_NAME = "onReceiveCallback";

    private void Awake() {
        if(_instance != null) {
            Destroy(gameObject);
        }
        Initialize();
    }

    /// <summary>
    /// Notification Channel Initialize (Version.Oreo support),
    /// Register to Download BroadcastReceiver.
    /// </summary>
    private void Initialize() {
#if !UNITY_EDITOR && UNITY_ANDROID
        using(var manager = new AndroidJavaObject(PAKAGE_NAME)) {
            manager.Call("Initialize",GAME_OBJECT_NAME,CALLBACK_FUNTION_NAME);
        }
#endif
    }

    /// <summary>
    /// Show Android Toast message. (LENGTH_SHORT).
    /// </summary>
    /// <param name="text">toast text</param>
    public void ShowToast(string text) {
#if !UNITY_EDITOR && UNITY_ANDROID
        using(var manager = new AndroidJavaObject(PAKAGE_NAME)) {
            manager.Call("ShowToast", text);
        }
#endif
    }

    /// <summary>
    /// Send Android Notification message.
    /// </summary>
    /// <param name="title">content title</param>
    /// <param name="text">content text</param>
    public void SendNotification(string title, string text) {
#if !UNITY_EDITOR && UNITY_ANDROID
        using(var manager = new AndroidJavaObject(PAKAGE_NAME)) {
            manager.Call("SendNotification", title,text);
        }
#endif
    }

    /// <summary>
    /// Download using Http url.
    /// When the download is finished, notification is sent.
    /// </summary>
    /// <param name="url">Download HTTP URL</param>
    /// <param name="fileName">Destination Path ( $Application.persistentDataPath/$fileName )</param>
    /// <returns></returns>
    public long DownloadData(string url, string fileName) {
        long downId = -1;
#if !UNITY_EDITOR && UNITY_ANDROID
        using(var manager = new AndroidJavaObject(PAKAGE_NAME)) {
            downId = manager.Call<long>("DownloadData", url,fileName);
        }
#endif
        return downId;
    }

    /// <summary>
    /// Check the status value returned by the download manager.
    /// </summary>
    /// <param name="downloadId">Return value of 'DownloadData' function</param>
    /// <returns>status</returns>
    public DownloadStatus GetDownloadStatus(long downloadId) {
        int statusId = -1;
#if !UNITY_EDITOR && UNITY_ANDROID
        using(var manager = new AndroidJavaObject(PAKAGE_NAME)) {
            statusId = manager.Call<int>("GetDownloadStatus", downloadId);
        }
#endif
        return (DownloadStatus)statusId;
    }


    /// <summary>
    /// Callback function that is called when the native plugin calls send message
    /// </summary>
    /// <param name="param">json parameter</param>
    private void onReceiveCallback(string param) {
        Debug.Log("Receive " + param);
    }


}
