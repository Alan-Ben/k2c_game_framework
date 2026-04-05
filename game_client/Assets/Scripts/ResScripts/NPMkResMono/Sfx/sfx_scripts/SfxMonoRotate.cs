using System.Collections.Generic;
using UnityEngine;
using System;

public class SfxMonoRotate : MonoBehaviour
{
    [Serializable]
    public class RotateInfo
    {
        [Header("特效物体")] public Transform sfxMonoGo;
        [Header("旋转速度")] public float rotateSpeed;
        [Header("旋转方向")] public Vector3 rotateDirection;
        [Header("无视TimeScale")] public bool ignoreTimeScale;
    }

    public List<RotateInfo> sfxMonoList = new List<RotateInfo>();

    void Update()
    {
        UpdateTime();
    }

    public void UpdateTime()
    {
        if(null == sfxMonoList || sfxMonoList.Count == 0)
            return;
        
        for (int i = 0; i < sfxMonoList.Count; i++)
        {
            RotateInfo rotateInfo = sfxMonoList[i];
            if (null != rotateInfo && null != rotateInfo.sfxMonoGo)
            {
                rotateInfo.sfxMonoGo.Rotate(rotateInfo.rotateDirection * (rotateInfo.ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime) * rotateInfo.rotateSpeed,
                    Space.Self);
            }
        }
    }
}