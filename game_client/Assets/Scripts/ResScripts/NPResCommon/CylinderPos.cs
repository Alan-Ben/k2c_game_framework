
using UnityEngine;

/// <summary>
/// 圆柱坐标
/// </summary>
public struct CylinderPos
{
    // 到轴心的距离
    private float _m_radius;
    // 沿着轴心方向到中心点的距离
    private float _m_height;
    // 这个点的圆柱上的欧拉角
    private float _m_angleEuler;
    // 这个点的圆柱上的弧度
    private float _m_angle;

    public CylinderPos(float _radius, float _height, float _angle)
    {
        _m_radius = _radius;
        _m_height = _height;
        _m_angle = _angle;
        _m_angleEuler = _m_angle * Mathf.Rad2Deg;
    }

    /// <summary>
    /// 到轴心的距离
    /// </summary>
    public float radius { get { return _m_radius; } set { _m_radius = value; if (_m_radius < 0) _m_radius = 0; } }
    /// <summary>
    /// 沿着轴心方向到中心点的距离
    /// </summary>
    public float height { get { return _m_height; } set { _m_height = value; } }
    /// <summary>
    /// 这个点的圆柱上的欧拉角
    /// </summary>
    public float angleEuler { get { return _m_angleEuler; } set { _m_angleEuler = value; _m_angle = value * Mathf.Deg2Rad; } }
    /// <summary>
    /// 这个点的圆柱上的弧度
    /// </summary>
    public float angle { get { return _m_angle; } set { _m_angle = value; _m_angleEuler = value * Mathf.Rad2Deg; } }
    
    /// <summary>
    /// 贴着圆柱表面移动一定距离（保持 radius 和 height 不变），移动一定距离
    /// </summary>
    public CylinderPos moveOnSurface(float _distance)
    {
        if (_distance == 0 || radius == 0)
            return this;

        angle += _distance / radius;
        return this;
    }
}
