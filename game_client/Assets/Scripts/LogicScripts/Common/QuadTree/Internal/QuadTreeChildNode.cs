
using UnityEngine;
using JetBrains.Annotations;


/// <summary>
/// 一个四叉树的子节点
/// </summary>
/// <remarks>
/// 子节点和普通的节点不一样，它拥有松散范围（google松散四叉树），还拥有父节点，他重写了包含（contains）和相交（intersects）方法的逻辑，改为使用松散范围来进行判断
/// </remarks>
internal class QuadTreeChildNode : QuadTreeNode
{
    // 父节点
    [NotNull]
    private readonly QuadTreeNode _m_parentNode;
    // 这个节点的松散范围
    private readonly Rect _m_rectLoose;
    // 在父节点下的位置
    private readonly EQuadTreeNodePos _m_ePos;

    internal QuadTreeChildNode(Rect _rect, float _looseRate, [NotNull] QuadTreeNode _parent, EQuadTreeNodePos _posInParent)
        : base(_rect, _looseRate)
    {
        _m_parentNode = _parent;
        _m_ePos = _posInParent;

        // 计算松散范围的Rect
        Vector2 looseRectSize = _looseRate * rect.size;
        Vector2 looseRectPos = rect.center - rect.size * _looseRate / 2;
        _m_rectLoose = new Rect(looseRectPos, looseRectSize);
    }

    /// <summary>
    /// 这个节点的父节点
    /// </summary>
    [NotNull]
    internal QuadTreeNode parentNode { get { return _m_parentNode; } }
    /// <summary>
    /// 在父节点中的位置
    /// </summary>
    internal EQuadTreeNodePos posInParent { get { return _m_ePos; } }
    /// <summary>
    /// 这个节点的松散范围
    /// </summary>
    internal Rect looseRect { get { return _m_rectLoose; } }
    /// <inheritdoc/>
    internal override bool contains(Rect _checkRect)
    {
        return looseRect.contains(_checkRect);
    }
    /// <inheritdoc/>
    internal override bool intersects(Rect _checkRect)
    {
        return looseRect.intersects(_checkRect);
    }
}
