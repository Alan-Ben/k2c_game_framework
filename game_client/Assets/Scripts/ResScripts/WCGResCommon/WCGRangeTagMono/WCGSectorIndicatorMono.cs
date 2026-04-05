using System;
using System.Collections.Generic;

using UnityEngine;

public class WCGSectorIndicatorMono : _AWCGBasicRangeTagMono
{
    //范围投影对象
    public Projector projector;
    //默认半径
    private float _m_fInitRadius;
    //扇形角度
    private float _m_fAngle;
    //transform of go
    private Transform _m_tTrans;
    //limit模式，显示的半径或者长度为相对位置的距离(setTargetRelativePos)
    private bool _m_bLimit;
    //最大长度
    private float _m_fMaxLength;

    public void Awake()
    {
        if (null == projector)
            return;

        //获取初始半径
        _m_fInitRadius = projector.orthographicSize / _g_fUnitSize;
        _m_tTrans = transform;
        _m_bLimit = false;
        _m_fMaxLength = -1f;
    }

    //设置对象位置
    public override void setPos(Vector3 _pos)
    {
        if (null == _m_tTrans)
            return;

        _m_tTrans.position = _pos;
    }
    //设置对象宽度或半径
    public override void setWidth(float _width)
    {
        if (null == projector)
            return;

        if (_m_fMaxLength > 0f && _width >= _m_fMaxLength)
        {
            _width = _m_fMaxLength;
        }

        //设置投影宽度
        projector.orthographicSize = _m_fInitRadius * _width;
    }
    //设置扇形角度
    public override void setAngle(float _angle)
    {
        if (null == projector || null == projector.material)
            return;
        _m_fAngle = _angle;
        projector.material.SetFloat("_Angle", _m_fAngle);
    }

    public override void setForward(Vector3 _forward)
    {
        if (null == _m_tTrans)
            return;
        _forward.y = 0f;
        _m_tTrans.rotation = Quaternion.LookRotation(_forward);
    }
    //设置目标相对位置
    public override void setTargetRelativePos(Vector3 _pos, float _minDis)
    {
        if (_m_bLimit)
        {
            setWidth(_pos.magnitude);
        }
    }
    //设置Limit模式，显示的半径或者长度为相对位置的距离(setTargetRelativePos)
    public override void setLimitMode(bool limitMode)
    {
        _m_bLimit = limitMode;
    }
    //作为缓存对象重置接口
    public override void reset()
    {
        _m_bLimit = false;
        _m_fMaxLength = -1f;
    }
    //设置最大宽度或半径
    public override void setMaxWidth(float _width)
    {
        _m_fMaxLength = _width;
    }
}
