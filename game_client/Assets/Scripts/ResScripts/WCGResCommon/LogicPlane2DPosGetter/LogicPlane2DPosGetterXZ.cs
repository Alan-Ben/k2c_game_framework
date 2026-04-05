using GOE;
using UnityEngine;

/// <summary>
/// 一个以传入的坐标为原点，世界坐标下 XZ 方向为 xy 方向的 2d 平面
/// </summary>
public class LogicPlane2DPosGetterXZ : _ALogicPlane2DPosGetter
{
    private readonly Vector3 _m_originPos;
    
    public LogicPlane2DPosGetterXZ(Vector3 _originPos)
    {
        _m_originPos = _originPos;
    }
    
    public override float getLogicXPos(Vector3 _pos)
    {
        Vector3 localPos = _pos - _m_originPos;
        return localPos.x;
    }
    public override float getLogicYPos(Vector3 _pos)
    {
        Vector3 localPos = _pos - _m_originPos;
        return localPos.z;
    }
    public override void setLogicXPos(ref Vector3 _pos, float _x)
    {
        _pos.x = _m_originPos.x + _x;
    }
    public override void setLogicYPos(ref Vector3 _pos, float _y)
    {
        _pos.z = _m_originPos.z + _y;
    }

    public override Vector3 getWorldPosByLogicPos(Vector2 _logicPos)
    {
        return new Vector3(_logicPos.x + _m_originPos.x, _m_originPos.y, _logicPos.y + _m_originPos.z);
    }
    
    public override bool intersectionWorldPointWithWorldRay(Ray _ray, out Vector3 _intersectionPos)
    {
        return NPGameUtility.rayToPanelCollision(_ray.origin, _ray.direction, _m_originPos, new Vector3(0, 1, 0), out _intersectionPos, out float t) && t >= 0;
    }
}
