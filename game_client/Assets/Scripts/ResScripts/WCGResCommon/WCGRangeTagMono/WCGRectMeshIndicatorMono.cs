using System;
using System.Collections.Generic;

using UnityEngine;

//基于mesh实现的指向型技能指示器
public class WCGRectMeshIndicatorMono : _AWCGBasicRangeTagMono
{
    public Transform meshIndicator;//显示的mesh
    private float _m_fInitWidthScale;//初始宽度
    private float _m_fInitlengthScale;//初始长度
    //limit模式，显示的半径或者长度为相对位置的距离(setTargetRelativePos)
    private bool _m_bLimit;
    //最大长度
    private float _m_fMaxLength;
    //初始MeshIndicator的本地位置
    private Vector3 _m_vMeshIndicatorLocalPos;

    public void Awake()
    {
        if (null == meshIndicator)
            return;

        _m_fInitWidthScale = meshIndicator.localScale.x;
        _m_fInitlengthScale = meshIndicator.localScale.z;

        _m_bLimit = false;
        _m_fMaxLength = -1f;
        _m_vMeshIndicatorLocalPos = meshIndicator.localPosition;
    }

    //设置对象位置
    public override void setPos(Vector3 _pos)
    {
        transform.position = _pos;
    }
    //设置对象宽度或半径
    public override void setWidth(float _width)
    {
        if (null == meshIndicator)
            return;

        Vector3 scale = meshIndicator.localScale;
        scale.x = _m_fInitWidthScale * _width;
        meshIndicator.localScale = scale;
    }
    //设置对象长度
    public override void setLength(float _length)
    {
        if (null == meshIndicator)
            return;

        if (_m_fMaxLength > 0f && _length >= _m_fMaxLength)
        {
            _length = _m_fMaxLength;
        }

        Vector3 scale = meshIndicator.localScale;
        scale.z = _m_fInitlengthScale * _length;
        meshIndicator.localScale = scale;

        meshIndicator.transform.localPosition = new Vector3(_m_vMeshIndicatorLocalPos.x,
            _m_vMeshIndicatorLocalPos.y, _m_vMeshIndicatorLocalPos.z + scale.z / 2f);
    }
    //设置朝向
    public override void setForward(Vector3 _forward)
    {
        _forward.y = 0;
        transform.rotation = Quaternion.LookRotation(_forward);
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
