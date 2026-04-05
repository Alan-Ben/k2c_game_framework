using UnityEngine;


/// <summary>
/// 四叉树的工具库
/// </summary>
internal static class QuadTreeUtility
{
    /// <summary>
    /// 判断Rect是否相交或重叠
    /// </summary>
    internal static bool intersects(this Rect rect, Rect target)
    {
        if (rect.yMin > target.yMax ||
            rect.yMax < target.yMin ||
            rect.xMax < target.xMin ||
            rect.xMin > target.xMax)
        {
            return false;
        }

        return true;
    }
    /// <summary>
    /// 判断目标Rect是否在自身Rect内部
    /// </summary>
    internal static bool contains(this Rect _rect, Rect _target)
    {
        if (_rect.yMax > _target.yMax &&
            _rect.yMin < _target.yMin &&
            _rect.xMax > _target.xMax &&
            _rect.xMin < _target.xMin)
        {
            return true;
        }

        return false;
    }
    /// <summary>
    /// 把格子坐标转换成枚举类型
    /// </summary>
    public static EQuadTreeNodePos intPos2PosEnum(int _row, int _col)
    {
        if (_row <= 0)
        {
            if (_col <= 0)
                return EQuadTreeNodePos.LEFT_BOTTOM;

            return EQuadTreeNodePos.RIGHT_BOTTOM;
        }

        if (_col <= 0)
            return EQuadTreeNodePos.LEFT_TOP;

        return EQuadTreeNodePos.RIGHT_TOP;
    }
}
