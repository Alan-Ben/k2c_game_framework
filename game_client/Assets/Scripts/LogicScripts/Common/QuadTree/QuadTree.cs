
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


/// <summary>
/// 松散四叉树
/// </summary>
/// <remarks>
/// <para>关于松散四叉树的知识可以参考<see href="https://www.cnblogs.com/KillerAery/p/10878367.html"/></para>
/// <para>主要使用方法为：为想要存放进四叉树的item都实现一个继承自<see cref="_AQuadTreeItem"/>的对象</para>
/// <para>接着可以使用<b>addOrUpdateItem</b>,<b>removeItem</b>,<b>getItems</b>,<b>dealWithItems</b>等主要方法来处理你需要管理的item</para>
/// </remarks>
public class QuadTree
{
    // 这个树分割的空间范围
    private readonly Rect _m_rectWorld;
    // 这个树分割的深度
    private readonly int _m_iMaxDepth;
    // 树的根节点
    [NotNull] private readonly QuadTreeRootNode _m_nodeRoot;
    // 这个树每一层的分割范围大小
    [NotNull] private readonly Vector2[] _m_arrayGridSizes;
    // 用来代替递归的处理队列
    [NotNull] private readonly Queue<QuadTreeNode> _m_queueRecursiveInstead;
    // 是否建立四叉树失败
    private bool _m_degenerate;
    // 用于临时处理的列表
    [NotNull] private readonly List<_AQuadTreeItem> _m_tempItems;

    public QuadTree(Rect _worldRect, int _maxDepth)
    {
        // 先标记创建成功
        _m_degenerate = false;

        _m_rectWorld = _worldRect;
        _m_iMaxDepth = _maxDepth;
        if (_m_rectWorld.width <= 0 ||
            _m_rectWorld.height <= 0 ||
            _m_iMaxDepth < 0)
        {
            _m_degenerate = true;
            _m_rectWorld.size = Vector2.zero;
            _m_iMaxDepth = -1;
        }

        _m_arrayGridSizes = new Vector2[_m_iMaxDepth + 1];
        for (int i = 0; i <= _m_iMaxDepth; i++)
        {
            float width = _m_rectWorld.width / Mathf.Pow(2, i);
            float height = _m_rectWorld.height / Mathf.Pow(2, i);
            _m_arrayGridSizes[i] = new Vector2(width, height);
        }

        _m_nodeRoot = new QuadTreeRootNode(_m_rectWorld, 2);
        _m_queueRecursiveInstead = new Queue<QuadTreeNode>();
        _m_tempItems = new List<_AQuadTreeItem>();
    }

    /// <summary>
    /// 这个树所分割的空间范围
    /// </summary>
    public Rect worldRect { get { return _m_rectWorld; } }
    /// <summary>
    /// 这个树的最大深度
    /// </summary>
    public int maxDepth { get { return _m_iMaxDepth; } }

    /// <summary>
    /// 往四叉树中添加或者刷新一个东西
    /// </summary>
    public void addOrUpdateItem(_AQuadTreeItem _item)
    {
        if (_item == null)
            return;

        // 如果四叉树创建失败了，要特殊处理
        if (_m_degenerate)
        {
            // 如果之前没有在树内，就直接添加到根节点中
            if (!_item.isInQuadTree)
            {
                _m_nodeRoot.addItem(_item);
            }
            return;
        }

        // 用来标识这个节点是否需要从之前的节点中移除
        bool isRemoved = false;
        QuadTreeNode lastBelongNode = null;
        // 如果item已经在树中则做是否需要变动的判断
        if (_item.isInQuadTree)
        {
            // 上面的判断通过了之后item就一定有所属的node
            // ReSharper disable once PossibleNullReferenceException
            if (_item.belongNode.contains(_item.rect))
                // 如果item还在自己的父节点的范围内，就不做变化
                return;
            else
            {
                // 如果item不在自己父节点的松散范围内，就进行移除并标识起来，稍后在做调整树结构的处理
                lastBelongNode = _item.belongNode;
                _item.belongNode.removeItem(_item);
                isRemoved = true;
            }
        }

        // 判断这个物体要存在第几层
        int depth = getDepth(_item.rect.size);
        // 如果在根节点，就直接加入
        if (depth == 0)
        {
            // 在第0层就直接加到根节点里
            _m_nodeRoot.addItem(_item);
        }
        else
        {
            // 获取这一层的格子尺寸
            Vector2 gridSize = _m_arrayGridSizes[depth];
            int maxIndexInDepth = (int)Mathf.Pow(2, depth) - 1;
            // 判断在这一层构成的网格中的第几行第几列
            int row = Mathf.FloorToInt((_item.rect.center.y - _m_rectWorld.yMin) / gridSize.y);
            int column = Mathf.FloorToInt((_item.rect.center.x - _m_rectWorld.xMin) / gridSize.x);
            // 将坐标限制在合理范围内
            row = Mathf.Clamp(row, 0, maxIndexInDepth);
            column = Mathf.Clamp(column, 0, maxIndexInDepth);

            // 从根节点开始，找到item应该放入的节点
            QuadTreeNode curNode = _m_nodeRoot;
            for (int i = 0; i < depth; i++)
            {
                // 从第一层开始，计算这个item所在的节点分别位于每一层的哪个位置。
                // 假设有N层，那么第N层的坐标范围为 [0, 2^N - 1]
                // 假设N=4，那么坐标范围为[0, 15]
                // 假设row=15，col=3，左下角为原点。
                // 不难算出，在每一层的坐标为：
                // L1 row=15/(2^3) col=3/(2^3) 下取整后为row=1 col=0
                // L2 row=15%(2^3)/(2^2) col=3%(2^3)/(2^2) => row=7/(2^2) col=3/(2^2) 下取整后为row=1 col=0
                // L3 row=7%(2^2)/(2^1) col=3%(2^2)/(2^1) => row=3/(2^1) col=3/(2^1) 下取整后为row=1 col=1
                // L4 row=3%(2^1)/(2^0) col=3%(2^1)/(2^0) => row=1 col=1
                // 很容易总结出算法规律，下面为上面算法的实现

                float div = Mathf.Pow(2, depth - i - 1);
                int intDiv = (int)div;
                int rowInThisDepth = Mathf.FloorToInt(row / div);
                int colInThisDepth = Mathf.FloorToInt(column / div);
                row %= intDiv;
                column %= intDiv;
                // 获取或创建这一层的节点
                curNode = curNode.createOrGetChildNode(QuadTreeUtility.intPos2PosEnum(rowInThisDepth, colInThisDepth));
            }

            // 添加到目标节点中
            curNode.addItem(_item);
        }

        // 最后再进行调整树结构的操作，如果在前面就调整的话，有可能会导致一大串节点被删除然后又被添加回来
        if (isRemoved)
            _clearBranchFromChild(lastBelongNode as QuadTreeChildNode);
    }
    /// <summary>
    /// 在四叉树中移除一个东西
    /// </summary>
    public void removeItem(_AQuadTreeItem _item)
    {
        if (_item == null || !_item.isInQuadTree)
            return;

        // ReSharper disable once PossibleNullReferenceException
        _item.belongNode.removeItem(_item);
        _clearBranchFromChild(_item.belongNode as QuadTreeChildNode);
    }
    /// <summary>
    /// 指定一片区域，获取这片区域中的所有item
    /// </summary>
    public List<_AQuadTreeItem> getItems(Rect _range)
    {
        List<_AQuadTreeItem> result = new List<_AQuadTreeItem>();
        getItemsNonAlloc(_range, result);
        return result;
    }
    /// <summary>
    /// 指定一片区域，获取这片区域中的所有item
    /// </summary>
    public void getItemsNonAlloc(Rect _range, List<_AQuadTreeItem> _result)
    {
        if (_result == null)
            return;

        _dealItemsFromNode(_m_nodeRoot, _range, _result.Add);
    }
    /// <summary>
    /// 对指定范围内的item直接进行操作
    /// </summary>
    public void dealWithItems(Rect _range, Action<_AQuadTreeItem> _action)
    {
        if (_action == null)
            return;

        _dealItemsFromNode(_m_nodeRoot, _range, _action);
    }
    /// <summary>
    /// 判断对于size的物品需要存放在深度为多少的节点中
    /// </summary>
    public int getDepth(Vector2 size)
    {
        for (int i = _m_arrayGridSizes.Length - 1; i >= 0; i--)
        {
            if (size.x <= _m_arrayGridSizes[i].x && size.y <= _m_arrayGridSizes[i].y)
            {
                return i;
            }
        }

        return 0;
    }
    // 获取从某个节点对范围内所有item做操作
    private void _dealItemsFromNode(QuadTreeNode _checkNode, Rect _range, Action<_AQuadTreeItem> _action)
    {
        if (_action == null || _checkNode == null)
            return;

        // 如果是失败的四叉树，直接全部遍历，不做求交
        if (_m_degenerate)
        {
            _m_tempItems.Clear();
            _m_tempItems.AddRange(_checkNode.getItems());
            foreach (_AQuadTreeItem item in _m_tempItems)
            {
                // 如果item为空（虽然不可能）就跳过
                if (item == null)
                    continue;

                _action.Invoke(item);
            }

            return;
        }

        // 清空处理队列，准备用这个队列来代替递归
        _m_queueRecursiveInstead.Clear();
        // 从要处理的节点开始，判断并入列
        _checkAndAddNodeToRecursiveInsteadQueue(_checkNode, _range);
        // 用循环的方式代替递归
        while (_m_queueRecursiveInstead.Count > 0)
        {
            // 一个一个进行处理
            QuadTreeNode dealNode = _m_queueRecursiveInstead.Dequeue();
            // 如果为空（虽然不可能）就跳过
            if (dealNode == null)
                continue;

            // 对内部的物品再进行一次检查
            _m_tempItems.Clear();
            _m_tempItems.AddRange(dealNode.getItems());
            foreach (_AQuadTreeItem item in _m_tempItems)
            {
                // 如果item为空（虽然不可能）就跳过
                if (item == null)
                    continue;

                // item的范围再与检查区域进行求交，成立的就加入到列表中
                if (item.rect.intersects(_range))
                    _action.Invoke(item);
            }

            // 对这个节点的四个子节点进行处理，它也不一定会有子节点
            _checkAndAddNodeToRecursiveInsteadQueue(dealNode[EQuadTreeNodePos.LEFT_BOTTOM], _range);
            _checkAndAddNodeToRecursiveInsteadQueue(dealNode[EQuadTreeNodePos.RIGHT_BOTTOM], _range);
            _checkAndAddNodeToRecursiveInsteadQueue(dealNode[EQuadTreeNodePos.LEFT_TOP], _range);
            _checkAndAddNodeToRecursiveInsteadQueue(dealNode[EQuadTreeNodePos.RIGHT_TOP], _range);
        }
    }
    // 判断求交并放入处理队列中
    private void _checkAndAddNodeToRecursiveInsteadQueue(QuadTreeNode _checkNode, Rect _checkRange)
    {
        // 如果需要检查的node不为null，并且和检查的范围相交就放入处理队列中
        if (_checkNode != null && _checkNode.intersects(_checkRange))
        {
            _m_queueRecursiveInstead.Enqueue(_checkNode);
        }
    }
    // 从一个子节点开始，判断子节点是否可以移除，并沿着子节点往根节点判断是否可以移除，如果可以移除就干掉
    private static void _clearBranchFromChild(QuadTreeChildNode _beginWith)
    {
        // 当前要处理的节点
        QuadTreeChildNode dealNode = _beginWith;
        // 用循环的方式代替递归，如果处理的节点为空，就做移除处理，并把处理转到其父对象
        while (dealNode != null && dealNode.isEmpty)
        {
            QuadTreeChildNode parent = dealNode.parentNode as QuadTreeChildNode;
            dealNode.parentNode.removeChildNode(dealNode);
            dealNode = parent;
        }
    }
}
