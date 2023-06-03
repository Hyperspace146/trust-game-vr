using UnityEngine;
using System.Collections;
using TMPro;

public class DebugGUI : MonoBehaviour
{
    public uint qsize = 100;  // number of messages to keep
    Queue myLogQueue = new Queue();

    public TextMeshProUGUI debugTextUI;

    void Start()
    {
        Debug.Log("Started up logging.");
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        myLogQueue.Enqueue("[" + type + "] : " + logString);
        if (type == LogType.Exception)
            myLogQueue.Enqueue(stackTrace);
        while (myLogQueue.Count > qsize)
            myLogQueue.Dequeue();

        debugTextUI.text = string.Join("\n", myLogQueue.ToArray());
    }

    //void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(Screen.width - 400, 0, 400, Screen.height));
    //    GUILayout.Label("\n" + string.Join("\n", myLogQueue.ToArray()));
    //    GUILayout.EndArea();
    //}
}