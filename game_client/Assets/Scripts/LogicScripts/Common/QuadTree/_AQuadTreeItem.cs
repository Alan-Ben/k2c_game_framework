
using UnityEngine;


/// <summary>
/// 一个可以被存放在四叉树中的物品
/// </summary>
/// <remarks>
/// 其实理论上应该做成接口类，但是接口类不能声明变量，由于四叉树需要用到belongNode这个属性，而这个属性也不是子类应该关心的，所以无奈还是做成抽象类了
/// </remarks>
public abstract class _AQuadTreeItem
{
    // 这个东西所属的node
    private QuadTreeNode _m_belongNode;

    /// <summary>
    /// 这个item的范围
    /// </summary>
    public abstract Rect rect { get; }
    /// <summary>
    /// 这个item是否在四叉树中
    /// </summary>
    public bool isInQuadTree { get { return _m_belongNode != null; } }
    /// <summary>
    /// 这个item所属的四叉树node
    /// </summary>
    internal QuadTreeNode belongNode { get { return _m_belongNode; } set { _m_belongNode = value; } }
}
