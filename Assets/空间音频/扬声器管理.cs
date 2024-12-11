using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class 扬声器管理 : MonoBehaviour
{
    // 定义：扬声器数量
    public int CntSpeaker = 24;
    // 定义：扬声器距离中心距离
    public float SpeakerRadius = 7f;
    // 定义：扬声器预制体
    public GameObject prefabSpeaker;
    //定义：扬声器状态 + 扬声器对象 + 扬声器材质
    List<int> states = new List<int>();
    List<GameObject> speakerList = new List<GameObject>();
    List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
    // 定义：四种颜色
    public Material White, Blue, Green, Red;
    // 定义：四种状态代码
    public const int NORMAL = 0, HILIGHTED = 1, CORRECT = 2, WRONG = 3;
    // 函数：添加扬声器
    public void AddSpeaker(GameObject gameObject) {
        states.Add(NORMAL);
        speakerList.Add(gameObject);
        meshRenderers.Add(gameObject.GetComponent<MeshRenderer>());
    }
    // 函数：更改颜色
    int speakerIndex = 0;
    public void ChangSpeakerState(GameObject gameObject, int state) {
        int index = speakerList.IndexOf(gameObject);

        if(index == -1) {
            if(states[speakerIndex] == HILIGHTED) {
                states[speakerIndex] = NORMAL;
                meshRenderers[speakerIndex].material = White;
            }
        }
        else if(index != speakerIndex) {
            if(states[speakerIndex] == HILIGHTED) {
                states[speakerIndex] = NORMAL;
                meshRenderers[speakerIndex].material = White;
            }
            switch(state)
            {
                case NORMAL:
                    states[index] = NORMAL;
                    meshRenderers[index].material = White;
                    break;
                case HILIGHTED:
                    states[index] = HILIGHTED;
                    meshRenderers[index].material = Blue;
                    break;
                case CORRECT:
                    states[index] = CORRECT;
                    meshRenderers[index].material = Green;
                    break;
                case WRONG:
                    states[index] = WRONG;
                    meshRenderers[index].material = Red;
                    break;
            }
            speakerIndex = index;
        } else {
            if(state > HILIGHTED && states[index] <= HILIGHTED) {
                switch(state)
                {
                    case CORRECT:
                        states[index] = CORRECT;
                        meshRenderers[index].material = Green;
                        break;
                    case WRONG:
                        states[index] = WRONG;
                        meshRenderers[index].material = Red;
                        break;
                }
            } else if(state == HILIGHTED && states[index] == NORMAL) {
                states[index] = HILIGHTED;
                meshRenderers[index].material = Blue;
            }
        }
    }

    void Start()
    {
        // 计算每个扬声器之间的角度间隔
        float angleStep = 360f / CntSpeaker;

        // 遍历每个扬声器的位置
        for (int i = 0; i < CntSpeaker; i++)
        {
            // 将角度转换为弧度
            float radius = (i * angleStep) * Mathf.Deg2Rad;

            // 计算扬声器的位置
            Vector3 pos = new Vector3(Mathf.Cos(radius) * SpeakerRadius, 0, Mathf.Sin(radius) * SpeakerRadius) + transform.position;

            // 实例化扬声器对象
            GameObject speaker = Instantiate(prefabSpeaker, pos, Quaternion.identity, transform);

            // 设置扬声器的旋转，使其朝向中心点
            speaker.transform.LookAt(transform.position);

            // 可选：如果你不希望扬声器在y轴上有任何偏移，可以重置y轴的旋转
            speaker.transform.eulerAngles = new Vector3(speaker.transform.eulerAngles.x, speaker.transform.eulerAngles.y, 0);
            
            // 加入列表等待操作
            AddSpeaker(speaker);
        }

        Destroy(prefabSpeaker);
    }
}
