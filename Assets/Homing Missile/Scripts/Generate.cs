using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Tarodev;
using UnityEngine;

public class Generate : MonoBehaviour
{
    private GameObject missile; // 拖拽你创建的预制体到这个变量中
    public static List<Transform> Targets = new List<Transform>();
    private void Start()
    {
        missile = Resources.Load<GameObject>("Prefabs/Missile");
        Create();
    }
    // Update 函数在每一帧都会调用
    private void Update() {
        Missile_Load -= Time.deltaTime;
        Ultimate_Load -= Time.deltaTime;
    }
    private float Ultimate_Load = 15.0f;
    private float Missile_Load = 0.0f;
    public void Ultimate(){
        if(Ultimate_Load > 0.1f)return;
        Ultimate_Load = 10.0f;
        for(int i = -4;i <= 4;i++){
            for(int j = -4;j <= 4;j++){
                Vector3 pos = new Vector3(i*10, j*10,-3);
                GameObject obj = Instantiate(missile, transform.parent.position + pos, transform.parent.rotation);
                obj.GetComponent<Missile_Self>().SetOffset(transform.parent.forward);
            }
        }
    }
    // 创建物体的函数
    public void Create()
    {
        if(Missile_Load > 0.1f)return;
        Missile_Load = 1.0f;
        GameObject obj = Instantiate(missile, transform.position, transform.parent.rotation);
        obj.GetComponent<Missile_Self>().SetOffset(transform.parent.forward);
        if (Targets.Count > 1)
            obj.GetComponent<Missile_Self>().SetTarget(Targets[Random.Range(0, Targets.Count+1)]);
        else
            obj.GetComponent<Missile_Self>().SetTarget(Targets[0]);
    }
    public static void RemoveDestroyedObjects()
    {
        // 遍历容器中的每个游戏对象
        for (int i = Targets.Count - 1; i >= 0; i--)
        {
            // 如果游戏对象被销毁，从容器中移除它
            if (Targets[i] == null)
            {
                Targets.RemoveAt(i);
            }
        }
    }
}