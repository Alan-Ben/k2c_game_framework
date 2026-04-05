using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

/*************
 * 浮点过度效果的参数结构体
 **/
[System.Serializable]
public class ALFloatFadeTimeInfo
{
    public float srcSpeed;
    public float totalTime;
    public float firstAccSpeedTime;

    public ALFloatFadeTimeInfo()
    {
        totalTime = 0;
        firstAccSpeedTime = 0;
    }
    public ALFloatFadeTimeInfo(float _totalTime, float _firstAccSpeedTime)
    {
        totalTime = _totalTime;
        firstAccSpeedTime = _firstAccSpeedTime;
    }
}
