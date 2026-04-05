using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

#if AL_CREATURE_SYS
/*****************
 * 默认的无多余操作的比例状态类
 **/
public class ALCreatureDefaultScaleState : _IALCreatureScaleBasicState
{
    private bool _m_bNeedSet;
    private Vector3 _m_vScale;

    public ALCreatureDefaultScaleState()
    {
        _m_bNeedSet = true;
        _m_vScale.x = 1;
        _m_vScale.y = 1;
        _m_vScale.z = 1;
    }
    public ALCreatureDefaultScaleState(float _scale)
    {
        _m_bNeedSet = true;
        _m_vScale.x = _scale;
        _m_vScale.y = _scale;
        _m_vScale.z = _scale;
    }
    public ALCreatureDefaultScaleState(Vector3 _scale)
    {
        _m_bNeedSet = true;
        _m_vScale = _scale;
    }

    /** 是否需要设置尺寸 */
    public bool needChgScale()
    {
        if (!_m_bNeedSet)
            return false;

        _m_bNeedSet = false;
        return true;
    }

    /** 获取当前比例信息 */
    public Vector3 getCurScale()
    {
        return _m_vScale;
    }
    /** 获取目标比例 */
    public Vector3 getTargetScale()
    {
        return _m_vScale;
    }

    /** 获取当前比例速度 */
    public Vector3 getCurScaleSpeed()
    {
        return new Vector3(0, 0, 0);
    }

    /** 判断状态是否有效，状态无效将切换到无效状态，避免无谓运算 */
    public bool enable()
    {
        return true;
    }
}
#endif
