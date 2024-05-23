using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    private GameObject[,,] Targets = new GameObject[2, 2, 2];
    private Button[,,] button = new Button[2, 2, 2];
    // Start is called before the first frame update
    void Start()
    {
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
    }
    // Update is called once per frame
    public Rigidbody L_RB, R_RB;
    public Transform L_Hand, R_Hand;
    public float L_SplitX, R_SplitX, Response_Speed;
    private float L_Speed, R_Speed;
    private int i = 0, j = 0, k = 0, ii = 0, jj = 0, kk = 0;
    private float Eyegaze_Height = 20f, Mid_Height = (120f - 81f)/2;
    void Update()
    {
        L_Speed = L_RB.velocity.magnitude;
        R_Speed = R_RB.velocity.magnitude;
        Eyegaze_Height = PicoEyeHintHeight();
        // 眼睛注视区域
        if(Eyegaze_Height > Mid_Height)
            i = 0;
        else
            i = 1;
        // 手势选择
        if(L_Speed > Response_Speed|| R_Speed > Response_Speed){
            if(L_Speed > R_Speed){
                j = 0;
                if(L_Hand.position.x > L_SplitX)
                    k = 0;
                else
                    k = 1;
            }
            else{
                j = 1;
                if(R_Hand.position.x > R_SplitX)
                    k = 1;
                else
                    k = 0;
            }
        }
        // 确定
        if(i != ii || j != jj || k != kk){
            Pre_Select();
            ii = i; jj = j; kk = k;
        }
    }
    private float PicoEyeHintHeight(){

        return 100f;
    }
    
    private void Pre_Select(){
        button[ii, jj, kk].OnPointerExit(null);
        button[i, j, k].OnPointerEnter(null);
    }
    public void Confirm_Select(){
        button[i, j, k].Select();
    }
}
