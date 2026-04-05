using System;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// 小圆点类型指示器
/// </summary>
public class WCGPointIndicatorMono : _AWCGBasicRangeTagMono
{
    public Projector projector;//范围投影对象

    private float _m_fSrcWidth;

    private Transform _m_tTrans;

    public void Awake()
    {
        if (null == projector)
            return;

        //获取初始半径
        _m_fSrcWidth = projector.orthographicSize / _g_fUnitSize;
        _m_tTrans = transform;
    }

    //设置对象位置
    public override void setPos(Vector3 _pos)
    {
        if (null == _m_tTrans)
            return;

        _m_tTrans.position = _pos;
    }
    //设置目标相对位置
    public override void setTargetRelativePos(Vector3 _pos, float _minDis)
    {
        if(projector == null)
            return;

        projector.transform.localPosition = _pos;
    }
    //设置对象宽度或半径
    public override void setWidth(float _width)
    {
        if (null == projector)
            return;

        //设置投影宽度
        projector.orthographicSize = _m_fSrcWidth * _width;
    }
    //设置对象长度
    public override void setLength(float _length)
    {

    }

    public override void setForward(Vector3 _forward)
    {
    }
}
