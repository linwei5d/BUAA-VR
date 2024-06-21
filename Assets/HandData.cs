using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.PXR;
public class HandData : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform L, R;
    public TMPro.TMP_Text T_L_HandH, T_R_HandX;
    void Start()
    {
        // T_L_HandH = L_HandH.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        T_L_HandH.text = String.Format("L_Hand_Height {0}", L.position.y);
        T_R_HandX.text = String.Format("R_Hand_X {0}", R.position.x);
    }
}
