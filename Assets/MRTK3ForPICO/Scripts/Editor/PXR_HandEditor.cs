#if MRTK3_INSTALL
using UnityEngine;
using UnityEditor;
using Unity.XR.PXR;
using PicoMRTK3Support.Runtime;

[CustomEditor(typeof(PicoMRTKHandVisualizer))]
public class PXR_HandEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();

        PicoMRTKHandVisualizer hand = (PicoMRTKHandVisualizer)target;

        EditorGUILayout.LabelField("Hand Joints", EditorStyles.boldLabel);

        for (int i = 0; i < (int)HandJoint.JointMax; i++)
        {
            string jointName = ((HandJoint)i).ToString();
            hand.riggedVisualJointsArray[i] = (Transform)EditorGUILayout.ObjectField(jointName, hand.riggedVisualJointsArray[i], typeof(Transform), true);
        }
    }
}
#endif
