
using UnityEngine;
using JetBrains.Annotations;
using System.Collections.Generic;


/// <summary>
/// 一个四叉树节点
/// </summary>
/// <remarks>
/// 具有自己所表示的范围，以及内含的item列表，还需有可添加移除子节点的功能
/// </remarks>
internal class QuadTreeNode
{
    // 这个节点的范围
    private readonly Rect _m_rect;
    // 四叉树的四个节点
    private QuadTreeChildNode[] _m_arrayChildNodes;
    // 这个节点里的物品列表
    [NotNull]
    private readonly List<_AQuadTreeItem> _m_listItem;
    // 子节点的松散程度
    private readonly float _m_fChildLooseRate;

    internal QuadTreeNode(Rect _rect, float _childLooseRate)
    {
        _m_rect = _rect;
        _m_fChildLooseRate = _childLooseRate;

        _m_listItem = new List<_AQuadTreeItem>();
    }
    /// <summary>
    /// 这个节点的范围
    /// </summary>
    internal Rect rect { get { return _m_rect; } }
    /// <summary>
    /// 是否完全没有数据了，是一个空节点
    /// </summary>
    public bool isEmpty { get { return _m_arrayChildNodes == null && _m_listItem.Count <= 0; } }

    /// <summary>
    /// 获取指定行列的子节点
    /// </summary>
    internal QuadTreeChildNode this[EQuadTreeNodePos _pos]
    {
        get
        {
            // 返回指定的子节点（不一定有）
            return _m_arrayChildNodes?[(int)_pos];
        }
    }
    /// <summary>
    /// 添加或获取一个子节点，如果只是想获取，使用[]索引器直接访问
    /// </summary>
    [NotNull]
    internal QuadTreeChildNode createOrGetChildNode(EQuadTreeNodePos _pos)
    {
        // 如果已经存在，就直接返回
        QuadTreeChildNode childNode = _m_arrayChildNodes?[(int)_pos];
        if (childNode != null)
            return childNode;

        // 计算这个子节点的Rect数据
        Vector2 childRectSize = _m_rect.size / 2;
        Vector2 childRectPos = _m_rect.center;
        switch (_pos)
        {
            case EQuadTreeNodePos.LEFT_BOTTOM:
                childRectPos = _m_rect.min;
                break;
            case EQuadTreeNodePos.RIGHT_BOTTOM:
                childRectPos = _m_rect.min + childRectSize.x * Vector2.right;
                break;
            case EQuadTreeNodePos.LEFT_TOP:
                childRectPos = _m_rect.min + childRectSize.y * Vector2.up;
                break;
            case EQuadTreeNodePos.RIGHT_TOP:
                childRectPos = _m_rect.center;
                break;
        }
        // 构建一个子节点
        childNode = new QuadTreeChildNode(new Rect(childRectPos, childRectSize), _m_fChildLooseRate, this, _pos);

        // 如果之前这个节点是叶子节点，就构建一个新的子节点队列
        if (_m_arrayChildNodes == null)
            _m_arrayChildNodes = new QuadTreeChildNode[4];

        // 添加到列表上
        _m_arrayChildNodes[(int)_pos] = childNode;
        // 返回给外部
        return childNode;
    }
    /// <summary>
    /// 移除一个子节点
    /// </summary>
    internal void removeChildNode(QuadTreeChildNode _node)
    {
        // 如果本身就是叶子节点，或者想要移除的节点有问题，就不处理
        if (_m_arrayChildNodes == null || _node == null)
            return;

        // 用来标记移除后是不是这个节点就变成叶子节点了
        bool empty = true;
        // 遍历所有节点，寻找想要移除的节点
        for (int i = 0; i < _m_arrayChildNodes.Length; i++)
        {
            // 找到了就移除它
            if (_m_arrayChildNodes[i] == _node)
            {
                _m_arrayChildNodes[i] = null;
            }
            // 否则判断这个位置有没有东西
            else if (_m_arrayChildNodes[i] != null && !_m_arrayChildNodes[i].isEmpty)
                // 有东西就标记这个节点并不是叶子节点
                empty = false;
        }

        // 如果变成叶子节点了，就把对应的列表释放
        if (empty)
            _m_arrayChildNodes = null;
    }
    /// <summary>
    /// 添加一个物品到列表中
    /// </summary>
    internal void addItem(_AQuadTreeItem _item)
    {
        if (_item == null)
            return;

        if (_item.belongNode != null)
        {
            Debug.LogError_EditorOnly("【QuadTree】将东西添加到四叉树中时，这个东西已经属于四叉树的某个节点了");
            return;
        }

        _m_listItem.Add(_item);
        _item.belongNode = this;
    }
    /// <summary>
    /// 从列表中移除一个物品
    /// </summary>
    internal void removeItem(_AQuadTreeItem _item)
    {
        if (_item == null)
            return;

        if (_item.belongNode != this)
        {
            Debug.LogError_EditorOnly("【QuadTree】正在试图从不包含这个东西的节点中移除这个东西");
            return;
        }

        _m_listItem.Remove(_item);
        _item.belongNode = null;
    }
    /// <summary>
    /// 获取节点内部的item列表
    /// </summary>
    [NotNull]
    internal List<_AQuadTreeItem> getItems()
    {
        return _m_listItem;
    }

    /// <summary>
    /// 判断rect是否在自己内部
    /// </summary>
    internal virtual bool contains(Rect _checkRect)
    {
        return rect.contains(_checkRect);
    }
    /// <summary>
    /// 判断rect是否与自己相交或重叠
    /// </summary>
    internal virtual bool intersects(Rect _checkRect)
    {
        return rect.intersects(_checkRect);
    }
}
