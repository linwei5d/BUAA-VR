using System.Collections.Generic;
using UnityEngine;

public class TrajectoryFollower : MonoBehaviour
{
    public LineRenderer lineRenderer; // ��Inspector������  
    private List<Vector3> positions = new List<Vector3>();
    private const int MAX_POSITIONS = 100; // �켣����ౣ����λ������  

    void Update()
    {
        // ����transform.position������ĵ�ǰλ��  
        positions.Add(transform.position);

        // ���λ�������������ֵ�����Ƴ���ɵ�λ��  
        if (positions.Count > MAX_POSITIONS)
        {
            positions.RemoveAt(0);
        }

        // ����LineRenderer�Ķ���  
        lineRenderer.positionCount = positions.Count;
        for (int i = 0; i < positions.Count; i++)
        {
            lineRenderer.SetPosition(i, positions[i]);
        }
    }
}
