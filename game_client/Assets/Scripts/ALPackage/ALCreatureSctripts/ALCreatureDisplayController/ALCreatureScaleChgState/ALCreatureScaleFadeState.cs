using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

#if AL_CREATURE_SYS
/***************
 * 对象比例渐变状态对象
 **/
public class ALCreatureScaleFadeState : _IALCreatureScaleBasicState
{
    /** 结束事件 */
    private float _m_fFinishTime;
    /** 过渡状态的数据处理对象 */
    private ALFloatFadeController[] _m_arrFloatFadeControlArr = new ALFloatFadeController[3];

    public ALCreatureScaleFadeState(Vector3 _startScale, Vector3 _targetScale, Vector3 _scaleSpeed, float _totalTime, float _firstAccTime)
    {
        _m_fFinishTime = Time.time + _totalTime;

        _m_arrFloatFadeControlArr[0] = new ALFloatFadeController(_startScale.x, _targetScale.x, _scaleSpeed.x, _totalTime, _firstAccTime);
        _m_arrFloatFadeControlArr[1] = new ALFloatFadeController(_startScale.y, _targetScale.y, _scaleSpeed.y, _totalTime, _firstAccTime);
        _m_arrFloatFadeControlArr[2] = new ALFloatFadeController(_startScale.z, _targetScale.z, _scaleSpeed.z, _totalTime, _firstAccTime);
    }

    /** 是否需要设置尺寸 */
    public bool needChgScale()
    {
        return true;
    }

    /** 获取当前比例信息 */
    public Vector3 getCurScale()
    {
        return new Vector3(_m_arrFloatFadeControlArr[0].curValue
                            , _m_arrFloatFadeControlArr[1].curValue
                            , _m_arrFloatFadeControlArr[2].curValue);
    }
    /** 获取目标比例 */
    public Vector3 getTargetScale()
    {
        return new Vector3(_m_arrFloatFadeControlArr[0].targetValue
                            , _m_arrFloatFadeControlArr[1].targetValue
                            , _m_arrFloatFadeControlArr[2].targetValue);
    }

    /** 获取当前比例速度 */
    public Vector3 getCurScaleSpeed()
    {
        return new Vector3(_m_arrFloatFadeControlArr[0].curFadeSpeed
                            , _m_arrFloatFadeControlArr[1].curFadeSpeed
                            , _m_arrFloatFadeControlArr[2].curFadeSpeed);
    }

    /** 判断状态是否有效，状态无效将切换到无效状态，避免无谓运算 */
    public bool enable()
    {
        return (Time.time <= _m_fFinishTime);
    }
}
#endif
