using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardScroll : MonoBehaviour
{
    public ScrollRect scrollRect; // 指定ScrollView组件
    public float scrollSensitivity = 10f; // 滚动的灵敏度
 
    void Update()
    {
        // 检测用户是否按下上下键
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            scrollRect.verticalNormalizedPosition += Time.deltaTime * scrollSensitivity;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            scrollRect.verticalNormalizedPosition -= Time.deltaTime * scrollSensitivity;
        }
    }
}
