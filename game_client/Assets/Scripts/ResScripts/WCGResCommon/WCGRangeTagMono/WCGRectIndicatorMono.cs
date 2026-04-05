using System;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// 方向型指示器
/// </summary>
public class WCGRectIndicatorMono : _AWCGBasicRangeTagMono
{
    public Projector projector;//范围投影对象
    private float _m_fInitWidth;//初始宽度
    private float _m_fInitlength;//初始长度

    private float _m_fCurrentWidth;//当前宽度
    private float _m_fCurrentlength;//当前长度

    private Transform _m_tTrans;
    private Vector3 _m_vProjectorLocalPos;
    //limit模式，显示的半径或者长度为相对位置的距离(setTargetRelativePos)
    private bool _m_bLimit;
    //最大长度
    private float _m_fMaxLength;

    public void Awake()
    {
        if (null == projector)
            return;

        _m_fInitWidth = projector.orthographicSize / _g_fUnitSize;
        _m_fCurrentWidth = _m_fInitWidth;
        _m_fInitlength = _m_fInitWidth / projector.aspectRatio;
        _m_fCurrentlength = _m_fInitlength;

        _m_tTrans = transform;
        _m_vProjectorLocalPos = projector.transform.localPosition;

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

        _m_fCurrentWidth = (_width / 2f) * _m_fInitWidth;
        projector.aspectRatio = _m_fCurrentWidth / _m_fCurrentlength;
    }
    //设置对象长度
    public override void setLength(float _length)
    {
        if (null == projector)
            return;

        if (_m_fMaxLength > 0f && _length >= _m_fMaxLength)
        {
            _length = _m_fMaxLength;
        }

        _m_fCurrentlength = (_length / 2f) * _m_fInitlength;
        projector.orthographicSize = _m_fCurrentlength;
        projector.aspectRatio = _m_fCurrentWidth / _m_fCurrentlength;
        projector.transform.localPosition = new Vector3(_m_vProjectorLocalPos.x, 
            _m_vProjectorLocalPos.y, _m_vProjectorLocalPos.z + _m_fCurrentlength);
    }
    //设置朝向
    public override void setForward(Vector3 _forward)
    {
        if (null == _m_tTrans)
            return;
        _forward.y = 0;
        _m_tTrans.rotation = Quaternion.LookRotation(_forward);
    }
    //设置目标相对位置
    public override void setTargetRelativePos(Vector3 _pos, float _minDis)
    {
        if (_m_bLimit)
        {
            setLength(_pos.magnitude);
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
