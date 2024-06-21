using System.Collections;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class taskmonitor : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI textcomponent;
    // 获得按钮
    // private GameObject apple_btn = GameObject.Find('Button-Apple').GetComponent<Button>();
    private string[] btn_list = {"Apple", "Banana", "Coconut", "Durian", "Grape", "Lemon", "Strawberry", "Watermelon",
    "Fig.1", "Fig.2", "Fig.3", "Fig.4", "Fig.5", "Fig.6", "Fig.7", "Fig.8", "Fig.9", "Fig.10", "Fig.11", "Fig.12", "Fig.13", "Fig.14", "Fig.15","Fig.16", "Fig.17", "Fig.18", "Fig.19", "Fig.20"};
    private string now_btn_monitor = "Now Selection\nStart";
    private string pre_btn_selected = "";
    private string arrow =  "\u2192";

    void Start()
    {
        textcomponent = GetComponent<TextMeshProUGUI>();
        if (textcomponent == null)
        {
            Debug.LogError("Not find the task monitor text!");
        }
        Debug.Log("TaskMonitor running");
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.name != pre_btn_selected)
        {
            Debug.Log(EventSystem.current.currentSelectedGameObject.name);
            string now_section = EventSystem.current.currentSelectedGameObject.name;
            pre_btn_selected = now_section;
            now_section = normalize_name(now_section);
            Debug.Log(now_section);
            if (btn_list.Contains(now_section))
            {
                now_btn_monitor = now_btn_monitor + arrow + now_section;
                UpdateText(now_btn_monitor);
            }
        }
    }

    public void UpdateText(string newText)
    {
        textcomponent.text = newText;
    }

    public string normalize_name(string name)
    {
        if (name.Contains("-"))
        {
            name = name.Remove(0, 7);
        }
        else
        {
            name = name.Remove(0, 6);
            name = "Fig." + name;
        }
        return name;
    }
}
