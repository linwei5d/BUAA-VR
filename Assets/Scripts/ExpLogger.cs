using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ExpLogger : MonoBehaviour
{
    public float time;
    public float timeSinceLastEvent;
    private string log;
    private string filePath;

    private void Start()
    {
        // Button[] buttons = FindObjectsOfType<Button>();
        // foreach (Button button in buttons)
        // {
        //     Debug.Log("Find button " + button.name);
        // }
        time = Time.time;
        filePath = Application.persistentDataPath + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
    }

    public void LogButtonEvent(string buttonName)
    {
        float currentTime = Time.time;
        timeSinceLastEvent = currentTime - time;
        time = currentTime;

        string currentLog = "Click"+ ", " + buttonName + ", " + timeSinceLastEvent.ToString() + "\n";
        log += currentLog;
        File.WriteAllText(filePath, log);
        Debug.Log("Log saved to: " + filePath);
    }

}
