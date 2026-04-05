package NPGameRes.GameObjs.Battle.AStar;

import NPGameRes.GameObjs.Battle.DLMapPos;
import NPGameRes.GameObjs.Battle.DLPathLineCheckCommonFun;
import NPGameRes.GameObjs.Battle.WCGFloatValue;
import NPGameRes.GameObjs.Battle._IDLBasicMapData;
import ResCommon.Allocator._IResAllocator;
import WCGCommon.Enum.NPEnum.EWCGMoveType;

import java.util.ArrayList;
import java.util.List;

public class DLAStar
{
    //本算法处理对象中用到的所有节点信息的缓存池
    private DLAStarNodeCache _m_ncNodeCache = new DLAStarNodeCache();
    private DLAStarResCache _m_astartResCache = new DLAStarResCache();
    //缓存对应X所有节点队列的处理
    private DLAStarNodeXContainerCache _m_astarXContainerCache = new DLAStarNodeXContainerCache();
    //临时对象 - 已经经过处理的点容器列表
    private ArrayList<DLAStarNodeXContainer> _m_tempNodeMap = new ArrayList<>();
    //临时对象 - 需要处理的节点列表
    private DLAStarNodeListNode.DLAStarNodeListInfo _m_tempDealNodeList = new DLAStarNodeListNode.DLAStarNodeListInfo();
    //临时对象 - 所有处理过的节点列表
    private List<DLAStarNode> _m_tempDealAllNodeList = new ArrayList<DLAStarNode>();

    private DLMapPos _m_pCalStartPos = new DLMapPos();

    /*****************
     * 将节点数据放回缓冲区
     **/
    protected void _resetTemp()
    {
        //回收所有点
        for (int i = 0; i < _m_tempDealAllNodeList.size(); i++)
        {
            _m_ncNodeCache.pushBackCacheItem(_m_tempDealAllNodeList.get(i));
        }
        //清空数据集
        _m_tempDealAllNodeList.clear();
        _m_tempDealNodeList.clear();

        for (int i = 0; i < _m_tempNodeMap.size(); i++)
        {
            _m_astarXContainerCache.pushBackCacheItem(_m_tempNodeMap.get(i));
        }
        _m_tempNodeMap.clear();
    }

    /*******************
     * 取出一个路径缓存对象
     **/
    public DLAStarRes popAstarRes()
    {
        if (null == _m_astartResCache)
            return null;

        return _m_astartResCache.popItem();
    }

    /*******************
     * 放回a星的结果对象
     **/
    public void pushbackAstarRes(DLAStarRes _astarRes)
    {
        if (null == _astarRes)
            return;

        _m_astartResCache.pushBackCacheItem(_astarRes);
    }


    /******************
     * 查询直线的节点
     **/
    public DLAStarRes getLinePath(DLMapPos _startPoint, DLMapPos _endPoint, WCGFloatValue _reachDis, _IResAllocator _alloc)
    {
        DLAStarRes astarRes = _m_astartResCache.popItem();

        //查询
        DLPathLineCheckCommonFun.getLinePath(_startPoint, _endPoint, astarRes, _reachDis, _alloc);

        return astarRes;
    }

    /**********************
     * 根据带入的起始位置与结束位置，以及地图数据，最大搜索逻辑距离长度（点个数），触摸距离，路径保存队列
     *
     * @author alzq.z
     * @time Jul 10, 2013 1:52:45 AM
     */
    public DLAStarRes findPath(DLMapPos _beginPos, DLMapPos _endPos, _IDLBasicMapData _map
            , int _maxSearchDis, WCGFloatValue _reachDis, int _weight, EWCGMoveType _moveType, boolean _ignoreCollide, int _judgeLineWalkCount, _IResAllocator _alloc)
    {
        //设置值
        _alloc.beginStack();
        try
        {
            _m_pCalStartPos.set(_beginPos);
            //路径是否已经查找到
            WCGFloatValue sqrReachDis = _reachDis.pow2(_alloc);
            int searchDis = _maxSearchDis;
            if (_maxSearchDis <= 0)
                searchDis = Integer.MAX_VALUE;

            //创建结果对象
            DLAStarRes resultObj = _m_astartResCache.popItem();


            //获取目标位置的地图位置
            //获取起始位置的地图位置
            int curX = _m_pCalStartPos.x;
            int curZ = _m_pCalStartPos.z;

            //清空临时对象
            _resetTemp();

            //最接近目标的节点
            DLAStarNode closestNode = null;

            //判断距离目标位置是否直接可走
            if (DLPathLineCheckCommonFun.lineWalkable(_m_pCalStartPos, _endPos, _map, _reachDis, _weight, _moveType, _ignoreCollide, _judgeLineWalkCount, _alloc))
            {
                //直接可走进行相关处理
                resultObj.pathFound = true;
                //获取行走路径
                DLPathLineCheckCommonFun.getLinePath(_m_pCalStartPos, _endPos, resultObj, _reachDis, _alloc);
                return resultObj;
            }

            //进行A星相关算法处理
            //将起始位置作为开始计算节点进行计算
            _openNode(curX, curZ, _map.getPointHeight(_m_pCalStartPos), null, _endPos, _m_tempNodeMap, _m_tempDealNodeList, searchDis);
            //获取需要处理的节点对象
            DLAStarNode dealingNode = _getDealNode(_m_tempDealNodeList);
            closestNode = dealingNode;

            int tmpX, tmpZ;
//    	        int endX = _endPos.x;
//    	        int endZ = _endPos.z;
            //取出第一个待处理节点，进行4方向的分别处理
            while (null != dealingNode)
            {
                //判断处理的节点是否最近点
                if (dealingNode.h < closestNode.h)
                    closestNode = dealingNode;

                //判断当前处理点距离目标的距离
                if ((_endPos.x == dealingNode.mapPosX && _endPos.z == dealingNode.mapPosZ)
                        || (dealingNode.h <= sqrReachDis.iV()))
                //暂时不做最后射线可视的判断
                //&& DLPathLineCheckCommonFun.lineVisable(dealingNode.mapPosX, dealingNode.mapPosZ, endX, endZ, _map, _moveType)))
                {
                    //设置最近点
                    closestNode = dealingNode;
                    //当到达目的地或者在距离内的点并与终点无阻挡
                    resultObj.pathFound = true;
                    break;
                }

                //分别判断4个方向的节点并进行相关处理
                // 1
                //4*2
                // 3
                curX = dealingNode.mapPosX;
                curZ = dealingNode.mapPosZ;
                //方向1
                if (_map.walkable(curX, curZ + 1, dealingNode.height, _weight, _moveType, _ignoreCollide))
                {
                    //可行，则处理该点
                    tmpX = curX;
                    tmpZ = curZ + 1;
                    //处理节点
                    _openNode(tmpX, tmpZ, _map.getPointHeight(tmpX, tmpZ), dealingNode, _endPos, _m_tempNodeMap, _m_tempDealNodeList, searchDis);
                }
                //方向2
                if (_map.walkable(curX + 1, curZ, dealingNode.height, _weight, _moveType, _ignoreCollide))
                {
                    //可行，则处理该点
                    tmpX = curX + 1;
                    tmpZ = curZ;
                    //处理节点
                    _openNode(tmpX, tmpZ, _map.getPointHeight(tmpX, tmpZ), dealingNode, _endPos, _m_tempNodeMap, _m_tempDealNodeList, searchDis);
                }
                //方向1
                if (_map.walkable(curX, curZ - 1, dealingNode.height, _weight, _moveType, _ignoreCollide))
                {
                    //可行，则处理该点
                    tmpX = curX;
                    tmpZ = curZ - 1;
                    //处理节点
                    _openNode(tmpX, tmpZ, _map.getPointHeight(tmpX, tmpZ), dealingNode, _endPos, _m_tempNodeMap, _m_tempDealNodeList, searchDis);
                }
                //方向1
                if (_map.walkable(curX - 1, curZ, dealingNode.height, _weight, _moveType, _ignoreCollide))
                {
                    //可行，则处理该点
                    tmpX = curX - 1;
                    tmpZ = curZ;
                    //处理节点
                    _openNode(tmpX, tmpZ, _map.getPointHeight(tmpX, tmpZ), dealingNode, _endPos, _m_tempNodeMap, _m_tempDealNodeList, searchDis);
                }

                //获取需要处理的节点对象
                dealingNode = _getDealNode(_m_tempDealNodeList);
            }//while(null != dealingNode)

            //设置最后点位置为最靠近的位置
            DLAStarNode finalNode = closestNode;

            //根据最后的点位置往回递推获取对应路径
            do
            {
                //加入队列
                resultObj.insertNewResPoint(finalNode.mapPosX, finalNode.mapPosZ);

                finalNode = finalNode.parentNode;
            }
            while (null != finalNode);

            return resultObj;
        } finally
        {
            _alloc.endStack();
        }

    }

    /************************
     * 尝试打开一个节点并进行相关处理
     *
     * @author alzq.z
     * @time Jul 11, 2013 12:59:32 AM
     */
    protected void _openNode(int _curX, int _curZ, short _height, DLAStarNode _parentNode, DLMapPos _destination
            , ArrayList<DLAStarNodeXContainer> _nodeMap, DLAStarNodeListNode.DLAStarNodeListInfo _dealNodeList, int _maxSearchDis)
    {
        //判断节点是否已经经过处理
        DLAStarNodeXContainer resC = null;
        int _nodeMapSize = _nodeMap.size();
        for (int i = 0; i < _nodeMapSize; i++)
        {
            if (_nodeMap.get(i).getX() == _curX)
            {
                resC = _nodeMap.get(i);
                break;
            }
        }
        //如无结果则重建一个新对象
        if (null == resC)
        {
            resC = _m_astarXContainerCache.popItem();
            resC.setX(_curX);
            //加入队列
            _nodeMap.add(resC);
        }
        //获取对应节点
        DLAStarNode dealedNode = resC.getNode(_curZ);
        if (dealedNode != null)
        {
            //已经处理过的节点使用更新操作
            _updateNode(dealedNode, _parentNode, _dealNodeList);
        } else
        {
            //未处理过的节点创建后插入
            dealedNode = _m_ncNodeCache.popItem();
            dealedNode.setNodeInfo(_curX, _curZ, _height, _parentNode, _destination);
            //添加到使用过列表
            _m_tempDealAllNodeList.add(dealedNode);

            //判断移动距离是否超出了最大检索范围
            if (dealedNode.g <= _maxSearchDis)
            {
                //插入新的待处理节点
                _insertNewNode(dealedNode, _dealNodeList);

                //将节点添加到已开启节点集合中
                resC.addNode(dealedNode);
            }
        }
    }

    /************************
     * 根据带入的信息，判断是否需要重新处理该节点，并将该节点重新放入待处理队列中
     *
     * @author alzq.z
     * @time Jul 11, 2013 1:01:29 AM
     */
    protected void _updateNode(DLAStarNode _node, DLAStarNode _newParentNode, DLAStarNodeListNode.DLAStarNodeListInfo _dealNodeList)
    {
        if (_node.tryChgParentNode(_newParentNode))
        {
            //尝试修改成功，此时进行其他操作
            if (!_node.dealed)
            {
                //从当前位置开始往前重新排序
                if (!_dealNodeList.reSortNode(_node))
                {
                    //失败则直接添加
                    _insertNewNode(_node, _dealNodeList);
                }
            } else
            {
                //设置节点未处理
                _node.setUndeal();

                //重新插入该节点
                _insertNewNode(_node, _dealNodeList);
            }
        }
    }

    /*************
     * 动态向前排序
     *
     * @author alzq.z
     * @time Jul 11, 2013 1:06:33 AM
     */
    protected static void _reSortNode(DLAStarNode _node, DLAStarNodeListNode.DLAStarNodeListInfo _dealNodeList)
    {
        //DLAStarNodeListNode itor = _dealNodeList.firstNode;

        //while (null != itor)
        //{
        //    //迭代处理
        //    _m_tempNodeValue = itor.nodeValue;

        //    if (_m_tempNodeValue.f >= _node.f)
        //    {
        //        itor.addPre(_node);
        //        return;
        //    }

        //    itor = itor.nxtNode;
        //}

        ////全队列都没有比他权重大的则直接插入队列末尾
        //_dealNodeList.addLast(_node);

        //使用倒序判断，计算量应该更小
        DLAStarNodeListNode itor = _dealNodeList.lastNode();

        DLAStarNode tmpValue = null;
        while (null != itor)
        {
            //迭代处理
            tmpValue = itor.nodeValue();

            if (tmpValue.f <= _node.f)
            {
                itor.addNext(_node);
                return;
            }

            itor = itor.preNode();
        }

        //全队列都没有比他权重大的则直接插入队列末尾
        _dealNodeList.addFirst(_node);
    }

    /*************
     * 插入一个新的节点到待处理队列中，队列中按照权重系数进行冒泡排序
     *
     * @author alzq.z
     * @time Jul 11, 2013 1:06:33 AM
     */
    protected void _insertNewNode(DLAStarNode _node, DLAStarNodeListNode.DLAStarNodeListInfo _dealNodeList)
    {
        //DLAStarNodeListNode itor = _dealNodeList.firstNode;

        //while (null != itor)
        //{
        //    //迭代处理
        //    _m_tempNodeValue = itor.nodeValue;

        //    if (_m_tempNodeValue.f >= _node.f)
        //    {
        //        itor.addPre(_node);
        //        return;
        //    }

        //    itor = itor.nxtNode;
        //}

        ////全队列都没有比他权重大的则直接插入队列末尾
        //_dealNodeList.addLast(_node);

        //使用倒序判断，计算量应该更小
        DLAStarNodeListNode itor = _dealNodeList.lastNode();
        DLAStarNode tmpValue = null;
        while (null != itor)
        {
            //迭代处理
            tmpValue = itor.nodeValue();

            if (tmpValue.f <= _node.f)
            {
                itor.addNext(_node);
                return;
            }
            itor = itor.preNode();
        }
        //全队列都没有比他权重大的则直接插入队列末尾
        _dealNodeList.addFirst(_node);
    }

    /*******************
     * 从待处理节点队列中取出第一位（权重最高的节点）
     *
     * @author alzq.z
     * @time Jul 11, 2013 1:30:07 AM
     */
    protected DLAStarNode _getDealNode(DLAStarNodeListNode.DLAStarNodeListInfo _dealNodeList)
    {
        if (_dealNodeList.isEmpty())
            return null;
        return _dealNodeList.popFirst();
    }
}
