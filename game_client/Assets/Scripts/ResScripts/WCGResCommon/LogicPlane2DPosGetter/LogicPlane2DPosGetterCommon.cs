
using GOE;
using UnityEngine;

/// <summary>
/// 使用一个点和法线确定一个 2D 逻辑平面，其中 x 的方向也可以自定义，默认的 x 方向会和
/// </summary>
public class LogicPlane2DPosGetterCommon : _ALogicPlane2DPosGetter
{
    // 一个点一个法线确定这个平面
    private readonly Vector3 _m_panelPoint;
    private readonly Vector3 _m_panelNormal;
    // 切线决定 logicX 在这个平面的方向，_m_panelUp 决定 logicY 在这个平面的方向
    private readonly Vector3 _m_panelTangent;
    private readonly Vector3 _m_panelUp;

    public LogicPlane2DPosGetterCommon(Vector3 _point, Vector3 _normal)
    {
        _m_panelPoint = _point;
        _m_panelNormal = _normal.normalized;
        // 如果法线是上下的话，切线就是右边
        if (_m_panelNormal is { x: 0, z: 0 })
            _m_panelTangent = Vector3.right;
        else
            _m_panelTangent = Vector3.Cross(_m_panelNormal, Vector3.up);
        _m_panelUp = Vector3.Cross(_m_panelTangent, _m_panelNormal);
    }

    public LogicPlane2DPosGetterCommon(Vector3 _point, Vector3 _normal, Vector3 _tangent)
    {
        _m_panelPoint = _point;
        _m_panelNormal = _normal.normalized;
        _m_panelTangent = _tangent.normalized;
        // 检查切线是否和法线垂直
        float perspectiveValue = Vector3.Dot(_m_panelTangent, _m_panelNormal);
        if (perspectiveValue > 0.0001f)
        {
            Vector3 perspectiveVector = Vector3.Dot(_m_panelTangent, _m_panelNormal) * _m_panelNormal;
            _m_panelTangent = (_m_panelTangent - perspectiveVector).normalized;
        }

        _m_panelUp = Vector3.Cross(_m_panelTangent, _m_panelNormal);
    }

    public override float getLogicXPos(Vector3 _pos)
    {
        return Vector3.Dot(_pos - _m_panelPoint, _m_panelTangent);
    }

    public override float getLogicYPos(Vector3 _pos)
    {
        return Vector3.Dot(_pos - _m_panelPoint, _m_panelUp);
    }

    public override void setLogicXPos(ref Vector3 _pos, float _x)
    {
        _pos = _m_panelPoint + 
               _m_panelTangent * _x +
               _m_panelUp * getLogicYPos(_pos) + 
               _m_panelNormal * Vector3.Dot(_pos - _m_panelPoint, _m_panelNormal);
    }

    public override void setLogicYPos(ref Vector3 _pos, float _y)
    {
        _pos = _m_panelPoint + 
               _m_panelTangent * getLogicXPos(_pos) + 
               _m_panelUp * _y + 
               _m_panelNormal * Vector3.Dot(_pos - _m_panelPoint, _m_panelNormal);
    }

    public override Vector3 getWorldPosByLogicPos(Vector2 _logicPos)
    {
        return _m_panelPoint + _m_panelTangent * _logicPos.x + _m_panelUp * _logicPos.y;
    }

    public override bool intersectionWorldPointWithWorldRay(Ray _ray, out Vector3 _intersectionPos)
    {
        return NPGameUtility.rayToPanelCollision(_ray.origin, _ray.direction, _m_panelPoint, _m_panelNormal, out _intersectionPos, out float t) && t >= 0;
    }
}