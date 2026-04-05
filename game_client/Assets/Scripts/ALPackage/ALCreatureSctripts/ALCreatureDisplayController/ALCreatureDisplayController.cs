using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

#if AL_CREATURE_SYS
/*******************
 * 个体对象的现实信息相关控制对象
 **/
public class ALCreatureDisplayController
{
    /** 对应的个体对象 */
    private _AALBasicCreatureControl _m_ccParentControl;

    /** 当前的缩放比例状态对象 */
    private _IALCreatureScaleBasicState _m_ssCurScaleState;

    public ALCreatureDisplayController(_AALBasicCreatureControl _creatureControl)
    {
        _m_ccParentControl = _creatureControl;

        _m_ssCurScaleState = null;
    }

    public _AALBasicCreatureControl creatureControl { get { return _m_ccParentControl; } }
    public Vector3 curTargetScale { get { return _m_ssCurScaleState.getTargetScale(); } }

    /******************
     * 每帧调用的处理函数
     **/
    public void frameCheck()
    {
        if (null != _m_ssCurScaleState && _m_ssCurScaleState.needChgScale())
        {
            //需要设置比例信息
            Vector3 newScale = _m_ssCurScaleState.getCurScale();
            //设置新比例信息
            creatureControl.scaleSize = newScale;

            //判断状态是否有效，无效则设置为默认状态
            if (!_m_ssCurScaleState.enable())
            {
                _setScaleState(new ALCreatureDefaultScaleState(_m_ssCurScaleState.getCurScale()));
            }
        }
    }

    /** 设置为当前比例的静止状态 */
    public void resetScaleState(float _scale)
    {
        _setScaleState(new ALCreatureDefaultScaleState(_scale));
    }

    /** 切换到对应的尺寸 */
    public void swapToScale(Vector3 _targetScale, float _totalTime, float _firstAccTime)
    {
        Vector3 curScaleSpeed = new Vector3(0, 0, 0);
        if(null != _m_ssCurScaleState)
            curScaleSpeed = _m_ssCurScaleState.getCurScaleSpeed();

        //设置新的尺寸切换状态
        _setScaleState(new ALCreatureScaleFadeState(creatureControl.scaleSize, _targetScale, curScaleSpeed, _totalTime, _firstAccTime));
    }

    /** 设置缩放状态对象 */
    protected void _setScaleState(_IALCreatureScaleBasicState _scaleState)
    {
        _m_ssCurScaleState = _scaleState;
    }
}
#endif
