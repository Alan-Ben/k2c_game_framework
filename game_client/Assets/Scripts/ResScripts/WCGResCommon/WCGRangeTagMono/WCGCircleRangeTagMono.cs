using System;
using System.Collections.Generic;

using UnityEngine;

public class WCGCircleRangeTagMono : _AWCGBasicRangeTagMono
{
    //范围投影对象
    public Projector projector;

    //默认半径
    private float _m_fSrcRadius;

    private Transform _m_tTrans;

    public void Awake()
    {
        if (null == projector)
            return;

        //获取初始半径
        _m_fSrcRadius = projector.orthographicSize / _g_fUnitSize;
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

    }
    //设置对象宽度或半径
    public override void setWidth(float _width)
    {
        if (null == projector)
            return;

        //设置投影宽度
        projector.orthographicSize = _m_fSrcRadius * _width;
        if (null != _m_tTrans)
            _m_tTrans.localScale = new Vector3(_width, 0, _width);
    }
    //设置对象长度
    public override void setLength(float _length)
    {

    }

    public override void setForward(Vector3 _forward)
    {
        if (null == _m_tTrans)
            return;

        _m_tTrans.rotation = Quaternion.LookRotation(_forward);
    }
}
