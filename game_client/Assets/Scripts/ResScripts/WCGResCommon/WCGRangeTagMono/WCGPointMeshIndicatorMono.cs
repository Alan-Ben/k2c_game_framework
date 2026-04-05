using System;
using System.Collections.Generic;

using UnityEngine;

//基于mesh实现的单体技能范围指示器
public class WCGPointMeshIndicatorMono : _AWCGBasicRangeTagMono
{
    //显示的mesh
    public Transform meshIndicator;
    //初始半径缩放比例
    private float _m_fInitRadiusScale;

    public void Awake()
    {
        if (null == meshIndicator)
            return;

        //获取初始半径
        _m_fInitRadiusScale = meshIndicator.localScale.z;
    }

    //设置对象位置
    public override void setPos(Vector3 _pos)
    {
        transform.position = _pos;
    }

    //设置目标相对位置
    public override void setTargetRelativePos(Vector3 _pos, float _minDis = 0f)
    {
        if (meshIndicator == null)
            return;

        //如果用InverseTransformVector会受到TranFrom的Scale的影响  
        Vector3 vec = transform.InverseTransformDirection(_pos);

        //通过上面的判定只能限制最大值，此处做最小距离限定
        if (vec.z < _minDis)
            vec.z = _minDis;

        meshIndicator.localPosition = vec;
    }
    //设置对象宽度或半径
    public override void setWidth(float _width)
    {
        if (null == meshIndicator)
            return;
        //设置显示的mesh的缩放
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
