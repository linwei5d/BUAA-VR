using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    // 旋转速度
    public float speed = 100f;
    void Update()
    {
        // 在y轴上进行无限旋转
        transform.Rotate(0, speed * Time.deltaTime, 0, Space.Self);
    }
}
