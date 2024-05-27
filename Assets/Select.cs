using UnityEngine;
using UnityEngine.UI;
using Unity.XR.PXR;
public class Select : MonoBehaviour
{
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
        Targets[0,0,0] = GameObject.Find("Button-Apple");
        Targets[0,0,1] = GameObject.Find("Button-Banana");
        Targets[0,1,0] = GameObject.Find("Button-Coconut");
        Targets[0,1,1] = GameObject.Find("Button-Durian");
        Targets[1,0,0] = GameObject.Find("Button-Grape");
        Targets[1,0,1] = GameObject.Find("Button-Lemon");
        Targets[1,1,0] = GameObject.Find("Button-Strawberry");
        Targets[1,1,1] = GameObject.Find("Button-Watermelon");
        for(int i = 0;i < 2; i++)
            for(int j = 0;j < 2; j++)
                for(int k = 0;k < 2;k ++)
                    button[i, j, k] = Targets[i, j, k].GetComponent<Button>();
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
    private void Pre_Section(){
        EyeTrackingDataGetInfo info = new EyeTrackingDataGetInfo();
        info.displayTime = 0;
        info.flags = EyeTrackingDataGetFlags.PXR_EYE_DEFAULT
        |EyeTrackingDataGetFlags.PXR_EYE_POSITION
        |EyeTrackingDataGetFlags.PXR_EYE_ORIENTATION;
        EyeTrackingData eyeTrackingData = new EyeTrackingData();
        PXR_MotionTracking.GetEyeTrackingData(ref info, ref eyeTrackingData);
        // TODO: 算出注视点与面板交点的X，得到Eyegaze_X
        if(eyeTrackingData.eyeDatas[2].pose.position.x > Eye_SplitY)
            i = 0;
        else
            i = 1;
    }
    Vector3 R_LastPos = new Vector3(0, 0, 0), L_LastPos = new Vector3(0, 0, 0);
    private float R_X_Distance, L_X_Distance;
    public GameObject L_LeftButton, L_RightButton, R_LeftButton, R_RightButton;
    private void Pre_Select(){
        R_X_Distance = R_Hand.position.x - Head.position.x;
        R_Speed = Vector3.Distance(R_Hand.position, R_LastPos) / Time.deltaTime;
        R_LastPos = R_Hand.position;
        L_X_Distance = Head.position.x - L_Hand.position.x;
        L_Speed = Vector3.Distance(L_Hand.position, L_LastPos) / Time.deltaTime;
        L_LastPos = L_Hand.position;

        if(R_Speed > Response_Speed || L_Speed > Response_Speed)
            if((j == 0 && R_Speed > L_Speed+Response_Speed) || (j == 1 && R_Speed > Response_Speed)) {
                L_LeftButton.SetActive(false);
                L_RightButton.SetActive(false);
                j = 1;
                if(R_X_Distance > R_SplitX){
                    k = 1;
                    R_LeftButton.SetActive(false);
                    R_RightButton.SetActive(true);
                }
                else{
                    k = 0;
                    R_LeftButton.SetActive(true);
                    R_RightButton.SetActive(false);
                }
            }
            else if((j == 0 && L_Speed > Response_Speed) || (j == 1 && L_Speed > R_Speed + Response_Speed)){
                R_LeftButton.SetActive(false);
                R_RightButton.SetActive(false);
                j = 0;
                if(L_X_Distance > L_SplitX){
                    k = 0;
                    L_LeftButton.SetActive(true);
                    L_RightButton.SetActive(false);
                }
                else{
                    k = 1;
                    L_LeftButton.SetActive(false);
                    L_RightButton.SetActive(true);
                }
            }

        if(i != ii || j != jj || k != kk){
            button[ii, jj, kk].OnPointerExit(null);
            button[i, j, k].OnPointerEnter(null);
            ii = i; jj = j; kk = k;
        }

        L_Canvas.position = new Vector3(Head.position.x - L_SplitX, L_Hand.position.y + 0.15f, L_Hand.position.z);
        R_Canvas.position = new Vector3(Head.position.x + R_SplitX, R_Hand.position.y + 0.15f, R_Hand.position.z);
    }
    private HandAimState aimState;
    private int L_Pinch, R_Pinch;
    public void Confirm_Select(){
        if(j == 1) {
            PXR_HandTracking.GetAimState(HandType.HandRight, ref aimState);
            if(R_Pinch == 0 && aimState.touchStrengthRay > 0.16f){
                R_Pinch = 1;
                button[i, j, k].Select();
            }
            else if(aimState.touchStrengthRay < 0.1f)R_Pinch = 0;
        }
        else {
            PXR_HandTracking.GetAimState(HandType.HandLeft, ref aimState);
            if(L_Pinch == 0 && aimState.touchStrengthRay > 0.16f){
                L_Pinch = 1;
                button[i, j, k].Select();
            }
            else if(aimState.touchStrengthRay < 0.1f)L_Pinch = 0;
        }
    }
}
