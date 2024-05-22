using System.Collections.Generic;
using UnityEngine;

public class TrajectoryFollower : MonoBehaviour
{
    public LineRenderer lineRenderer; // 在Inspector中设置  
    private List<Vector3> positions = new List<Vector3>();
    private const int MAX_POSITIONS = 100; // 轨迹上最多保留的位置数量  

    void Update()
    {
        // 假设transform.position是物体的当前位置  
        positions.Add(transform.position);

        // 如果位置数量超过最大值，则移除最旧的位置  
        if (positions.Count > MAX_POSITIONS)
        {
            positions.RemoveAt(0);
        }

        // 更新LineRenderer的顶点  
        lineRenderer.positionCount = positions.Count;
        for (int i = 0; i < positions.Count; i++)
        {
            lineRenderer.SetPosition(i, positions[i]);
        }
    }
}
