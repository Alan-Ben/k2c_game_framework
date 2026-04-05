using System;
using System.Collections.Generic;
using UnityEngine;

//基于mesh实现的圆形范围指示器
public class WCGBuildingDefaultRangeMono : _AWCGBasicRangeTagMono
{
    //显示的mesh
    public Transform meshIndicator;
    //初始半径缩放比例
    private float _m_fInitRadiusScale;

    public void Awake()
    {
        if (meshIndicator == null)
        {
            return;
        }
        //获取初始半径
        _m_fInitRadiusScale = meshIndicator.localScale.z;
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
        scale.x = scale.z = _m_fInitRadiusScale * _width;
        meshIndicator.localScale = scale;
    }
    //设置朝向
    public override void setForward(Vector3 _forward)
    {
        _forward.y = 0f;
        transform.rotation = Quaternion.LookRotation(_forward);
    }
}
