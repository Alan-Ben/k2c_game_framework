
using UnityEngine;

public abstract class _ALogicPlane2DPosGetter
{
    /// <summary>
    /// 获取这个世界坐标在逻辑平面上的 x 值
    /// </summary>
    public abstract float getLogicXPos(Vector3 _pos);
    /// <summary>
    /// 获取这个世界坐标在逻辑平面上的 y 值
    /// </summary>
    public abstract float getLogicYPos(Vector3 _pos);
    /// <summary>
    /// 把世界坐标的坐标移动到逻辑平面上的 x 轴为 _x 的位置
    /// </summary>
    public abstract void setLogicXPos(ref Vector3 _pos, float _x);
    /// <summary>
    /// 把世界坐标的坐标移动到逻辑平面上的 y 轴为 _y 的位置
    /// </summary>
    public abstract void setLogicYPos(ref Vector3 _pos, float _y);

    /// <summary>
    /// 获取这个世界坐标在逻辑平面上的坐标
    /// </summary>
    public Vector2 getLogicPos(Vector3 _pos)
    {
        return new Vector2(getLogicXPos(_pos), getLogicYPos(_pos));
    }

    /// <summary>
    /// 获取这个平面上某一逻辑坐标在世界坐标上的位置
    /// </summary>
    /// <param name="_logicPos"></param>
    /// <returns></returns>
    public abstract Vector3 getWorldPosByLogicPos(Vector2 _logicPos);
    
    /// <summary>
    /// 获取世界坐标下的射线和逻辑平面的交点
    /// </summary>
    public abstract bool intersectionWorldPointWithWorldRay(Ray _ray, out Vector3 _intersectionPos);
}
