using UnityEngine;
using UnityEngine.UI;
using System;
using Pico;
using Unity.XR.PXR;
using UnityEngine.XR;
using System.Runtime.CompilerServices;

public class Select : MonoBehaviour
{
    private GameObject[, ,] Targets = new GameObject[5, 2, 2];
    private Button[, ,] button = new Button[5, 2, 2];
    private Scrollbar Scrollbar;

    //
    //public LineRenderer lineRenderer;
    private Vector3 start = new Vector3(0, 0, -800);
    private Vector3 direction = new Vector3(0, 0, 1);
    public float length = 100f;

    public Transform spherePrefab;
    public float spheredistance;

    private int state = 0;

    public RectTransform pic1, pic2, pic3, pic4, pic5;

    // Start is called before the first frame update
    void Start()
    {
        TrackingStateCode trackingState = (TrackingStateCode)PXR_MotionTracking.WantEyeTrackingService();
        EyeTrackingStartInfo info = new EyeTrackingStartInfo();
        info.needCalibration = 1;
        info.mode = EyeTrackingMode.PXR_ETM_BOTH;
        trackingState = (TrackingStateCode)PXR_MotionTracking.StartEyeTracking(ref info);
        Scrollbar = GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>();
        for (int i = 0; i < 5; i++)
            for (int j = 0; j < 2; j++)
                for(int k = 0; k < 2; k++)
                {
                    Targets[i, j, k] = GameObject.Find(String.Format("Button{0}", i * 4 + j * 2 + k + 1));
                    button[i, j, k] = Targets[i, j, k].GetComponent<Button>();
                }
        button[0, 0, 0].OnPointerEnter(null);
        
        
    }

    //
    /*void UpdateLine()
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, start + direction * length);
    }*/

    
    // Update is called once per frame    
    void Update()
    {
        // 图标
        Select_Icon();
        // 预备上下行切换
        Pre_ChangeLine();
        // 眼睛注视区域
        Pre_Section();
        // 手势选择
        Pre_Item();
        // 确定
        Pre_Select();

        ChangeLine();
        Confirm_Select();
    }
    public Transform L_Hand, R_Hand, Head, L_Canvas, R_Canvas;
    // TODO: 试出合适的左手分割线高度、右手分割线的X坐标、及右手响应速度
    public float L_SplitY, R_SplitX, Response_Speed, L_SplitX;
    private float R_Speed, L_Speed;
    private int i = 0, j = 0, k = 0, ii = 0, jj = 0, kk = 0;
    private float Eyegaze_X = 0f, Mid_X = 650f;
    private void Select_Icon(){
        //L_Canvas.position = new Vector3(L_Hand.position.x, Head.position.y-L_SplitY, L_Hand.position.z);
        //R_Canvas.position = new Vector3(Head.position.x + R_SplitX, R_Hand.position.y + 0.1f, L_Hand.position.z);
    }

    public Transform q1, q2, q3;
    private double gaze_y;
    private double gaze_sum = 0;
    private Vector3 gaze;

    private int eye_cnt = 0;

    private void Pre_Section()// 眼睛注视区域
    {
        EyeTrackingDataGetInfo info = new EyeTrackingDataGetInfo();
        info.displayTime = 0;
        info.flags = EyeTrackingDataGetFlags.PXR_EYE_DEFAULT
        |EyeTrackingDataGetFlags.PXR_EYE_POSITION
        |EyeTrackingDataGetFlags.PXR_EYE_ORIENTATION;
        EyeTrackingData eyeTrackingData = new EyeTrackingData();
        PXR_MotionTracking.GetEyeTrackingData(ref info, ref eyeTrackingData);


        //更新眼睛注视方向
        direction.x = eyeTrackingData.eyeDatas[2].pose.orientation.x;
        direction.y = eyeTrackingData.eyeDatas[2].pose.orientation.y;
        direction.z = eyeTrackingData.eyeDatas[2].pose.orientation.z;

        gaze = direction.normalized;

        //更新眼睛位置
        start.x = eyeTrackingData.eyeDatas[2].pose.position.x;
        start.y = eyeTrackingData.eyeDatas[2].pose.position.y;
        start.z = eyeTrackingData.eyeDatas[2].pose.position.z;

        //spherePrefab.position = new Vector3(start.x + direction.x * length, start.y + direction.y * length, start.z + direction.z * length );

        eye_cnt++;
        gaze_sum += Math.Tan(start.x) * 2600;
        if ( eye_cnt >= 20 ) 
        {
            gaze_y = gaze_sum / 20;
            eye_cnt = 0;
            gaze_sum = 0;
        }
        

        //
        //q1.position = new Vector3(q1.position.x, (float)gaze_y, 0);
        //q2.position = new Vector3(q2.position.x, eyeTrackingData.eyeDatas[2].pose.orientation.y * 20, q2.position.z);
        //q3.position = new Vector3(q3.position.x, q3.position.y, eyeTrackingData.eyeDatas[2].pose.orientation.z * 20);
        //
        
        

        if (gaze_y < pic1.transform.position.y && gaze_y > pic1.transform.position.y - pic1.sizeDelta.y + 200)
        {
            i = 0;
        }
        else if(gaze_y < pic2.transform.position.y + 200 && gaze_y > pic2.transform.position.y - pic2.sizeDelta.y + 200)
        {
            i = 1;
        }
        else if (gaze_y < pic3.transform.position.y + 200 && gaze_y > pic3.transform.position.y - pic3.sizeDelta.y + 200)
        {
            i = 2;
        }
        else if (gaze_y < pic4.transform.position.y + 200 && gaze_y > pic4.transform.position.y - pic4.sizeDelta.y + 200)
        {
            i = 3;
        }
        else if (gaze_y < pic5.transform.position.y + 200 && gaze_y > pic5.transform.position.y - pic5.sizeDelta.y)
        {
            i = 4;
        }


        // TODO: 算出注视点与面板交点的X，得到Eyegaze_X
            
    }
    Vector3 R_LastPos = new Vector3(0, 0, 0);
    Vector3 L_LastPos = new Vector3(0, 0, 0);
    private float R_X_Distance, R_Y_Distance, L_Y_Distance, L_X_Distance;
    private int ChangeLineMod = 0, ChangeItemMod = 0;

    private int get_flag = 0, change_flag = 0;

    private float R_start_pos, L_start_pos, Mid_start_pos;

    private float Mid_pos;

    private int y_flag = 2;

    private void Pre_Item()// 手势选择
    {
        R_X_Distance = R_Hand.position.x - Head.position.x;
        R_Speed = Vector3.Distance(R_Hand.position, R_LastPos) / Time.deltaTime;
        R_LastPos = R_Hand.position;
        L_X_Distance = Head.position.x - L_Hand.position.x;
        L_Speed = Vector3.Distance(L_Hand.position, L_LastPos) / Time.deltaTime;
        L_LastPos = L_Hand.position;
        L_Y_Distance = Head.position.y - L_Hand.position.y;
        R_Y_Distance = Head.position.y - R_Hand.position.y;
        Mid_pos = (R_Y_Distance + L_Y_Distance) / 2;

        //if (R_Speed > Response_Speed || L_Speed > Response_Speed)
        //mealy状态机
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
                    Mid_start_pos = Mid_pos;
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
                    Mid_start_pos = Mid_pos;
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
                    if(Mid_pos > Mid_start_pos)//向下
                    {
                        y_flag = 0;
                    }
                    else//向上
                    {
                        y_flag = 1;
                    }
                    state = 3;
                    change_flag = 1;//
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
        
    }
    //public GameObject UpButton, DownButton, LeftButton, RightButton;
    private float midPos;
    private void Pre_ChangeLine()// 预备上下行切换
    {
        L_Y_Distance = Head.position.y - L_Hand.position.y;
        R_Y_Distance = Head.position.y - R_Hand.position.y;
        midPos = (L_Hand.position.y + R_Hand.position.y) / 2;

        if (y_flag == 0) {
            if(ChangeLineMod != 1 ){
                ChangeLineMod = 1;
                //UpButton.SetActive(true);
                //DownButton.SetActive(false);
            }
        }
        else if(y_flag == 1){
            if(ChangeLineMod != 0 ){
                ChangeLineMod = 0;
                //UpButton.SetActive(false);
                //DownButton.SetActive(true);
            }
        }
    }
    public float index;
    private void Pre_Select()// 确定
    {
        if(i != ii || j != jj || k != kk){
            button[ii, jj, kk].OnPointerExit(null);
            button[i, j, k].OnPointerEnter(null);
            ii = i; jj = j; kk = k;
        }
    }
    private float[] ScrollWeight = new float[5]{1f, 0.78f, 0.5f, 0.22f , 0f};
    public float Response_Distance;
    public void UpLine(){
        if(Scrollbar.value <= 1 && Mid_pos - Mid_start_pos >= Response_Distance) {
            Scrollbar.value += (Mid_pos - Mid_start_pos) * index;
        }
    }
    public void DownLine(){
        if(Scrollbar.value >= 0 && Mid_start_pos - Mid_pos >= Response_Distance) {
            Scrollbar.value -= (Mid_start_pos - Mid_pos) * index;
        }
    }
    private HandAimState aimStateR, aimStateL;
    private int L_Pinch, R_Pinch;
    public void ChangeLine() 
    {
        midPos = (L_Hand.position.y + R_Hand.position.y) / 2;
        if (change_flag == 1){
            change_flag = 0;
            if(y_flag == 0)
                UpLine();
            else if(y_flag == 1)
                DownLine();
        }
    }

    public GameObject Log;
    public void Confirm_Select(){

        PXR_HandTracking.GetAimState(HandType.HandRight, ref aimStateR);
        if (aimStateR.touchStrengthRay > 0.3f)
            R_Pinch = 1;
        else if (aimStateR.touchStrengthRay < 0.2f) R_Pinch = 0;
        PXR_HandTracking.GetAimState(HandType.HandLeft, ref aimStateL);
        if (aimStateL.touchStrengthRay > 0.3f)
            L_Pinch = 1;
        else if (aimStateL.touchStrengthRay < 0.2f) L_Pinch = 0;
        if (j == 1)
        {
            if(aimStateR.touchStrengthRay < 0.2f && get_flag == 1)
            {
                get_flag = 0;
                button[i, j, k].Select();
                Log.GetComponent<ExpLogger>().LogButtonEvent(String.Format("Button{0}", i * 4 + j * 2 + k + 1));
            }
        }
        else
        {
            if (aimStateL.touchStrengthRay < 0.2f && get_flag == 1)
            {
                get_flag = 0;
                button[i, j, k].Select();
                Log.GetComponent<ExpLogger>().LogButtonEvent(String.Format("Button{0}", i * 4 + j * 2 + k + 1));
            }
        }
    }
}
