using UnityEngine;
using UnityEngine.UI;
using Unity.XR.PXR;
using System;
public class Select : MonoBehaviour
{
    private string[,,] ButtonName = new string[2, 2, 2];
    private GameObject[,,] Targets = new GameObject[2, 2, 2];
    private Button[,,] button = new Button[2, 2, 2];
    // Start is called before the first frame update
    void Start()
    {
        TrackingStateCode trackingState = (TrackingStateCode)PXR_MotionTracking.WantEyeTrackingService();
        EyeTrackingStartInfo info = new EyeTrackingStartInfo();
        info.needCalibration = 1;
        info.mode = EyeTrackingMode.PXR_ETM_BOTH;
        trackingState = (TrackingStateCode)PXR_MotionTracking.StartEyeTracking(ref info);
        ButtonName[0, 0, 0] = "Button-Apple";
        ButtonName[0, 0, 1] = "Button-Banana";
        ButtonName[0, 1, 0] = "Button-Coconut";
        ButtonName[0, 1, 1] = "Button-Durian";
        ButtonName[1, 0, 0] = "Button-Grape";
        ButtonName[1, 0, 1] = "Button-Lemon";
        ButtonName[1, 1, 0] = "Button-Strawberry";
        ButtonName[1, 1, 1] = "Button-Watermelon";
        for(int i = 0;i < 2; i++)
            for(int j = 0;j < 2; j++)
                for(int k = 0;k < 2;k ++) {
                    Targets[i, j, k] = GameObject.Find(ButtonName[i, j, k]);
                    button[i, j, k] = Targets[i, j, k].GetComponent<Button>();
                }
        button[0, 0, 0].OnPointerEnter(null);
    }
    // Update is called once per frame    
    void Update()
    {
        // 眼睛注视区域
        Pre_Section();
        // 手势选择
        Pre_Select();
        // 确定
        Confirm_Select();
    }
    public Transform L_Hand, R_Hand, Head, L_Canvas, R_Canvas;
    // TODO: 试出合适的左手分割线高度、右手分割线的X坐标、及右手响应速度
    public float L_SplitX, R_SplitX, Response_Speed, Eye_SplitY;
    private float L_Speed, R_Speed;
    private int i = 0, j = 0, k = 0, ii = 0, jj = 0, kk = 0;
    private int eye_cnt = 0;
    private double gaze_y, gaze_sum = 0;
    private void Pre_Section(){
        EyeTrackingDataGetInfo info = new EyeTrackingDataGetInfo();
        info.displayTime = 0;
        info.flags = EyeTrackingDataGetFlags.PXR_EYE_DEFAULT
        |EyeTrackingDataGetFlags.PXR_EYE_POSITION
        |EyeTrackingDataGetFlags.PXR_EYE_ORIENTATION;
        EyeTrackingData eyeTrackingData = new EyeTrackingData();
        PXR_MotionTracking.GetEyeTrackingData(ref info, ref eyeTrackingData);
        // TODO: 算出注视点与面板交点的X，得到Eyegaze_X
        eye_cnt++;
        gaze_sum += Math.Tan(eyeTrackingData.eyeDatas[2].pose.position.x) * 2600;
        if ( eye_cnt >= 20 ) 
        {
            gaze_y = gaze_sum / 20;
            eye_cnt = 0;
            gaze_sum = 0;
        }
        if(gaze_y > Eye_SplitY)
            i = 0;
        else
            i = 1;
    }
    Vector3 R_LastPos = new Vector3(0, 0, 0);
    Vector3 L_LastPos = new Vector3(0, 0, 0);
    private float R_X_Distance, L_X_Distance;

    private int get_flag = 0, change_flag = 0, state = 0;

    private float R_start_pos, L_start_pos;
    private void Pre_Select(){
        R_X_Distance = R_Hand.position.x - Head.position.x;
        R_Speed = Vector3.Distance(R_Hand.position, R_LastPos) / Time.deltaTime;
        R_LastPos = R_Hand.position;
        L_X_Distance = Head.position.x - L_Hand.position.x;
        L_Speed = Vector3.Distance(L_Hand.position, L_LastPos) / Time.deltaTime;
        L_LastPos = L_Hand.position;

        if(state == 0)//两手皆空
        {
            if(R_Pinch == 1)//捏右手//((j == 0 && R_Speed > L_Speed + Response_Speed) || (j == 1 && R_Speed > Response_Speed))
            {
                j = 1; state = 1;
                R_start_pos = R_X_Distance;
                //k = (R_X_Distance > R_SplitX) ? 1 : 0;
            }
            else if(L_Pinch == 1)//捏左手//((j == 0 && L_Speed > Response_Speed) || (j == 1 && L_Speed > R_Speed + Response_Speed))
            {
                j = 0; state = 2;
                L_start_pos = L_X_Distance;
                //k = (L_X_Distance < -L_SplitX) ? 1 : 0;
            }
        }
        else if(state == 1)//右手捏住
        {
            if(L_Pinch == 1 && R_Pinch == 1)
            {
                state = 3;
            }
            else if(R_Pinch == 0 && L_Pinch == 0)
            {
                state = 0;j = 1;
                k = (R_X_Distance > R_start_pos) ? 1 : 0;
                get_flag = 1;//
            }
            else if(R_Pinch == 1)
            {
                state = 1;j = 1;
                k = (R_X_Distance > R_start_pos) ? 1 : 0;
            }
            else
            {
                state = 0;
            }
        }
        else if(state == 2)//左手捏住
        {
            if (R_Pinch == 1 && L_Pinch == 1)
            {
                state = 3;
            }
            else if (L_Pinch == 0 && R_Pinch == 0)
            {
                state = 0; j = 0;
                k = (L_X_Distance < L_start_pos) ? 1 : 0;
                get_flag = 1;//
            }
            else if(L_Pinch == 1)
            {
                state = 2; j = 0;
                k = (L_X_Distance < L_start_pos) ? 1 : 0;
            }
            else
            {
                state = 0;
            }
        }
        else if(state == 3)//双手捏住
        {
            if(R_Pinch == 1 && L_Pinch == 1)
            {
                state = 3;
                // change_flag = 1;//
            }
            else if(R_Pinch == 0 && L_Pinch == 1)
            {
                state = 2;j = 0;
                k = (L_X_Distance < L_start_pos) ? 1 : 0;
            }
            else if(R_Pinch == 1 && L_Pinch == 0)
            {
                state = 1; j = 1;
                k = (R_X_Distance > R_start_pos) ? 1 : 0;
            }
            else
            {
                state = 0;
            }
        }
        if(i != ii || j != jj || k != kk) {
            button[ii, jj, kk].OnPointerExit(null);
            button[i, j, k].OnPointerEnter(null);
            ii = i; jj = j; kk = k;
        }
    }
    private HandAimState aimStateR, aimStateL;
    private int L_Pinch, R_Pinch;
    public GameObject Log;
    public void Confirm_Select(){
        PXR_HandTracking.GetAimState(HandType.HandRight, ref aimStateR);
        if (aimStateR.touchStrengthRay > 0.22f)
            R_Pinch = 1;
        else if (aimStateR.touchStrengthRay < 0.1f) R_Pinch = 0;
        PXR_HandTracking.GetAimState(HandType.HandLeft, ref aimStateL);
        if (aimStateL.touchStrengthRay > 0.22f)
            L_Pinch = 1;
        else if (aimStateL.touchStrengthRay < 0.1f) L_Pinch = 0;
        
        if (j == 1)
        {
            if(aimStateR.touchStrengthRay < 0.1f && get_flag == 1)
            {
                get_flag = 0;
                button[i, j, k].Select();
                Log.GetComponent<ExpLogger>().LogButtonEvent(ButtonName[i, j, k]);
            }
        }
        else
        {
            if (aimStateL.touchStrengthRay < 0.1f && get_flag == 1)
            {
                get_flag = 0;
                button[i, j, k].Select();
                Log.GetComponent<ExpLogger>().LogButtonEvent(ButtonName[i, j, k]);
            }
        }
    }
}
