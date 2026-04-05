
using UnityEngine;


/// <summary>
/// 一个四叉树的根节点
/// </summary>
/// <remarks>
/// 和普通的节点没有任何区别
/// </remarks>
internal class QuadTreeRootNode : QuadTreeNode
{
    internal QuadTreeRootNode(Rect _rect, float _childLooseRate)
        : base(_rect, _childLooseRate)
    {
    }
}
