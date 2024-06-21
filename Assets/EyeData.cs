using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.PXR;

public class EyeData : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TMP_Text Eye_direction;
    void Start()
    {
        TrackingStateCode trackingState = (TrackingStateCode)PXR_MotionTracking.WantEyeTrackingService();
        EyeTrackingStartInfo info = new EyeTrackingStartInfo();
        info.needCalibration = 1;
        info.mode = EyeTrackingMode.PXR_ETM_BOTH;
        trackingState = (TrackingStateCode)PXR_MotionTracking.StartEyeTracking(ref info);
    }

    // Update is called once per frame
    void Update()
    {
        EyeTrackingDataGetInfo info = new EyeTrackingDataGetInfo();
        info.displayTime = 0;
        info.flags = EyeTrackingDataGetFlags.PXR_EYE_DEFAULT
        | EyeTrackingDataGetFlags.PXR_EYE_POSITION
        | EyeTrackingDataGetFlags.PXR_EYE_ORIENTATION;
        EyeTrackingData eyeTrackingData = new EyeTrackingData();
        PXR_MotionTracking.GetEyeTrackingData(ref info, ref eyeTrackingData);
        Eye_direction.text = String.Format("{0}, {1}, {2}", eyeTrackingData.eyeDatas[1].pose.position.x, eyeTrackingData.eyeDatas[1].pose.position.y, eyeTrackingData.eyeDatas[1].pose.position.z);
    }
}
