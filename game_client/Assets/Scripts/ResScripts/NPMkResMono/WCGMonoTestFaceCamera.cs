using System;
using System.Collections.Generic;
using GOE;
using UnityEngine;

public class WCGMonoTestFaceCamera : MonoBehaviour
{
    public Camera testC;

    public void Start()
    {
        if(null != testC && null != testC.transform)
            FaceCameraMgr.instance.setRotation(testC.transform.rotation);
    }
}
