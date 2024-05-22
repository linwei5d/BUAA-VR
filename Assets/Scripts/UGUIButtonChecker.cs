using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class UGUIButtonChecker : MonoBehaviour
{
    void Update()
    {
        List<int> btns = ButtonExpand.btns;
        if (ButtonExpand.btns.Count != 0)
        {
            foreach (var item in btns)
            {
                ButtonExpand.btnDc[item] = false;
            }
            btns.Clear();
        }
    }
}
 
// Button 拓展类
public static class ButtonExpand
{
    public static Dictionary<int, bool> btnDc = new Dictionary<int, bool>();//字典（键：按钮的ID，值：按钮是否被点击）
    public static List<int> btns = new List<int>();
    static GameObject obj;//常驻
 
 
 
    /// <summary>
    /// 拓展方法：判断Button是否被点击
    /// </summary>
    public static bool isClick(this Button btn)
    {
        if (btn == null)
            return false;
 
        if (obj == null)
        {
            UGUIButtonChecker o = Object.FindObjectOfType<UGUIButtonChecker>();
            if (o == null)
            {
                (obj = new GameObject("UGUI Button Checker")).AddComponent<UGUIButtonChecker>();
            }
            else
            {
                obj = o.gameObject;
            }
            Object.DontDestroyOnLoad(obj);
        }
        int instanID = btn.GetInstanceID();
 
        if (btnDc.ContainsKey(instanID) == false)
        {
            btnDc.Add(instanID, false);
 
            btn.onClick.AddListener(() =>
            {
                btnDc[instanID] = true;
            });
        }
 
        bool isCli = btnDc[instanID];
        if (isCli)
        {
            if (btns.Contains(instanID) == false)
                btns.Add(instanID);
        }
        return isCli;
    }
 
    /// <summary>
    /// 拓展方法：如果Button调用“RemoveAllListeners”，之后需调用一遍此方法，以免出现未知错误！
    /// </summary>
    public static void RemoveAllListenersLater(this Button btn)
    {
        //按钮点击事件的监听被移除了，这个按钮也需要从字典中移除了
        if (btnDc.ContainsKey(btn.GetInstanceID()))
        {
            btnDc.Remove(btn.GetInstanceID());
        }
    }
}