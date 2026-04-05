using System;
using System.Collections.Generic;
using UnityEngine;

/***************
 * 范围提示对象控制脚本
 **/
public abstract class _AWCGBasicRangeTagMono : MonoBehaviour
{
    //对象的单位尺寸
    protected const float _g_fUnitSize = 0.5f;
    //设置对象位置
    public abstract void setPos(Vector3 _pos);
    //设置目标相对位置
    public virtual void setTargetRelativePos(Vector3 _pos,float _minDis = 0f) { }
    //设置对象位置
    public virtual void setForward(Vector3 _forward) { }
    //设置对象宽度或半径
    public virtual void setWidth(float _width) { }
    //设置对象长度
    public virtual void setLength(float _length) { }
    //设置扇形角度
    public virtual void setAngle(float _angle) { }
    //设置Limit模式，显示的半径或者长度为相对位置的距离(setTargetRelativePos)
    public virtual void setLimitMode(bool limitMode) { }
    //作为缓存对象重置接口
    public virtual void reset() { }
    //设置最大宽度或半径
    public virtual void setMaxWidth(float _width) { }
}
