using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 使用一个点和法线确定一个 2D 逻辑平面，其中 x 的方向也可以自定义, 得到三轴坐标逻辑方向与世界坐标系方向一致
    /// </summary>
    public class LogicPlane2DPosGetterAdaptiveWorldPos : _ALogicPlane2DPosGetter
    {
        // 一个点一个法线确定这个平面
    private readonly Vector3 _m_panelPoint;
    private readonly Vector3 _m_panelNormal;
    // 切线决定 logicX 在这个平面的方向，_m_panelUp 决定 logicY 在这个平面的方向
    private readonly Vector3 _m_panelTangent;
    private readonly Vector3 _m_panelUp;

    public LogicPlane2DPosGetterAdaptiveWorldPos(Vector3 _point, Vector3 _normal)
    {
        _m_panelPoint = _point;
        _m_panelNormal = _normal.sqrMagnitude > 0.000001f ? _normal.normalized : Vector3.forward;

        _buildBasis(_m_panelNormal, out _m_panelTangent, out _m_panelUp);
    }

    public LogicPlane2DPosGetterAdaptiveWorldPos(Vector3 _point, Vector3 _normal, Vector3 _tangent)
    {
        _m_panelPoint = _point;
        _m_panelNormal = _normal.sqrMagnitude > 0.000001f ? _normal.normalized : Vector3.forward;

        _buildBasis(_m_panelNormal, out Vector3 defaultTangent, out Vector3 defaultUp);

        Vector3 tangent = _tangent;
        if (tangent.sqrMagnitude > 0.000001f)
            tangent = Vector3.ProjectOnPlane(tangent, _m_panelNormal);

        if (tangent.sqrMagnitude <= 0.000001f)
            tangent = defaultTangent;
        else
            tangent.Normalize();

        _m_panelTangent = tangent;
        _m_panelUp = Vector3.Cross(_m_panelNormal, _m_panelTangent).normalized;
    }

    private static void _buildBasis(Vector3 _normal, out Vector3 _tangent, out Vector3 _up)
    {
        // 以法向量作为局部z+，构建与世界坐标同手性的局部x+/y+
        // 优先使用世界right在平面上的投影作为局部x+
        Vector3 tangent = Vector3.ProjectOnPlane(Vector3.right, _normal);
        if (tangent.sqrMagnitude <= 0.000001f)
            tangent = Vector3.ProjectOnPlane(Vector3.up, _normal);
        if (tangent.sqrMagnitude <= 0.000001f)
            tangent = Vector3.ProjectOnPlane(Vector3.forward, _normal);

        _tangent = tangent.normalized;
        // 使用 z×x=y，保证与世界坐标轴方向关系一致
        _up = Vector3.Cross(_normal, _tangent).normalized;
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
}