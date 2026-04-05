package NPGameRes.GameObjs.Battle.AStar;

import NPCommon.Log.CommLog;
import NPGameRes.CacheSys._AALUnsafeCacheController;


/*******************
 * A星寻路处理的列表和节点信息
 **/
public class DLAStarNodeListNode
{
    /******************
     * 节点存储cache对象，避免创建过多对象
     **/
    public static class DLAStarNodeListCache extends _AALUnsafeCacheController<DLAStarNodeListNode, DLAStarNodeListNode>
    {
        public DLAStarNodeListCache()
        {
            super(10);
            init(new DLAStarNodeListNode());
        }

        @Override
        protected DLAStarNodeListNode _createItem(DLAStarNodeListNode _template)
        {
            return new DLAStarNodeListNode();
        }

        @Override
        protected void _discardItem(DLAStarNodeListNode _item)
        {
            _item.reset();
            return;
        }

        @Override
        protected void _onInit(DLAStarNodeListNode _template)
        {
        }

        @Override
        protected void _resetItem(DLAStarNodeListNode _item)
        {
            _item.reset();
        }
    }

    public static class DLAStarNodeListInfo
    {
        /**
         * 本队列内的缓存处理对象
         */
        private DLAStarNodeListCache _m_lcListCache;

        /**
         * 第一个节点对象
         */
        private DLAStarNodeListNode _m_nFirstNode;
        /**
         * 最后一个节点对象
         */
        private DLAStarNodeListNode _m_nLastNode;

        /**
         * 节点对象
         */
        private DLAStarNode _m_tempValue;
        private DLAStarNodeListNode _m_tempNode;

        public DLAStarNodeListInfo()
        {
            _m_lcListCache = new DLAStarNodeListCache();

            _m_nFirstNode = null;
            _m_nLastNode = null;
        }

        public DLAStarNodeListCache getNodeCache()
        {
            return _m_lcListCache;
        }

        public DLAStarNodeListNode firstNode()
        {
            return _m_nFirstNode;
        }

        public DLAStarNodeListNode lastNode()
        {
            return _m_nLastNode;
        }

        public boolean isEmpty()
        {
            return _m_nFirstNode == null;
        }

        /***********
         * 添加第一个节点
         **/
        public void addFirst(DLAStarNode _node)
        {
            DLAStarNodeListNode newNode = _m_lcListCache.popItem();
            if (null == newNode)
                return;

            //设置节点信息和下一个信息
            newNode._m_nNxtNode = _m_nFirstNode;
            newNode._m_nNode = _node;
            newNode._m_nPreNode = null;

            if (null != _m_nFirstNode)
                _m_nFirstNode._m_nPreNode = newNode;
            //设置队列信息
            newNode._m_liListInfo = this;

            //设置第一个节点
            _m_nFirstNode = newNode;
            if (null == _m_nLastNode)
                _m_nLastNode = newNode;
        }

        /***********
         * 添加最后一个节点
         **/
        public void addLast(DLAStarNode _node)
        {
            DLAStarNodeListNode newNode = _m_lcListCache.popItem();
            if (null == newNode)
                return;

            //设置节点信息和下一个信息
            newNode._m_nNxtNode = null;
            newNode._m_nNode = _node;
            newNode._m_nPreNode = _m_nLastNode;

            if (null != _m_nLastNode)
                _m_nLastNode._m_nNxtNode = newNode;
            //设置队列信息
            newNode._m_liListInfo = this;

            //设置第一个节点
            _m_nLastNode = newNode;
            if (null == _m_nFirstNode)
                _m_nFirstNode = newNode;
        }

        /***************
         * 取出第一个对象
         **/
        public DLAStarNode popFirst()
        {
            if (null == _m_nFirstNode)
                return null;

            _m_tempValue = _m_nFirstNode.nodeValue();
            //取出第一个对象
            _m_nFirstNode.popThis();

            return _m_tempValue;
        }

        /***************
         * 取出最后一个对象
         **/
        public DLAStarNode popLast()
        {
            if (null == _m_nLastNode)
                return null;

            _m_tempValue = _m_nLastNode.nodeValue();
            //取出第一个对象
            _m_nLastNode.popThis();

            return _m_tempValue;
        }

        /***************
         * 删除指定对象
         **/
        public void removeNode(DLAStarNode _node)
        {
            if (null == _node)
                return;

            _m_tempNode = _m_nFirstNode;

            while (_m_tempNode != null)
            {
                //执行操作
                if (_m_tempNode.nodeValue() == _node)
                {
                    _m_tempNode.popThis();
                    return;
                }

                //取下一个点
                _m_tempNode = _m_tempNode.nxtNode();
            }
        }


        /*************
         * 动态向前排序
         *
         * @author alzq.z
         * @time Jul 11, 2013 1:06:33 AM
         */
        public boolean reSortNode(DLAStarNode _node)
        {
            if (null == _node)
                return false;

            DLAStarNodeListNode tmpNode = _m_nFirstNode;

            while (tmpNode != null)
            {
                //执行操作
                if (tmpNode._m_nNode == _node)
                {
                    break;
                }

                //取下一个点
                tmpNode = tmpNode._m_nNxtNode;
            }

            //此时如果没检索到数据则返回失败
            if (null == tmpNode)
                return false;

            //此时从本位置向前检测，并添加到合适位置
            //使用倒序判断，计算量应该更小
            //从当前位置往前
            DLAStarNodeListNode itor = tmpNode._m_nPreNode;
            //将本节点从队列取出
            tmpNode.popThis();

            DLAStarNode tmpValue = null;
            while (null != itor)
            {
                //迭代处理
                tmpValue = itor._m_nNode;

                if (tmpValue.f <= _node.f)
                {
                    itor.addNext(_node);
                    return true;
                }

                itor = itor._m_nPreNode;
            }

            //全队列都没有比他权重大的则直接插入队列末尾
            addFirst(_node);

            return true;
        }


        /***************
         * 对每个节点的值进行操作
         **/
//        public void loopValue(Action<DLAStarNode> _action)
//        {
//            if (null == _action)
//                return;
//
//            _m_tempNode = _m_nFirstNode;
//
//            while (_m_tempNode != null)
//            {
//                //执行操作
//                _action(_m_tempNode.nodeValue());
//                _m_tempNode = _m_tempNode.nxtNode();
//            }
//        }

        /***************
         * 清空所有数据
         **/
        public void clear()
        {
            if (null == _m_nFirstNode)
                return;

            DLAStarNodeListNode firstNode = _m_nFirstNode;
            DLAStarNodeListNode tempNode = null;
            _m_nFirstNode = null;
            _m_nLastNode = null;

            do
            {
                tempNode = firstNode;
                firstNode = firstNode.nxtNode();

                //回收对象
                _m_lcListCache.pushBackCacheItem(tempNode);
            } while (firstNode != null);
        }

        /**
         * 设置最后一个节点信息
         */
        protected void _setLastNode(DLAStarNodeListNode _lastNode)
        {
            _m_nLastNode = _lastNode;
        }

        protected void _setFirstNode(DLAStarNodeListNode _firstNode)
        {
            _m_nFirstNode = _firstNode;
        }

        /**
         * 当某个节点移除时的操作
         */
        protected void _onNodeRemove(DLAStarNodeListNode _node)
        {
            if (null == _node)
                return;

            if (_node == _m_nLastNode && null != _m_nLastNode)
                _m_nLastNode = _m_nLastNode.preNode();

            if (_node == _m_nFirstNode && null != _m_nFirstNode)
                _m_nFirstNode = _m_nFirstNode.nxtNode();

            //放入缓存
            _m_lcListCache.pushBackCacheItem(_node);
        }
    }

    /**
     * 上一个节点信息
     */
    private DLAStarNodeListNode _m_nPreNode;
    /**
     * 当前节点信息
     */
    private DLAStarNode _m_nNode;
    /**
     * 下一个节点信息
     */
    private DLAStarNodeListNode _m_nNxtNode;

    /**
     * 队列信息
     */
    private DLAStarNodeListInfo _m_liListInfo;


    //protected static DLAStarNodeListNode _createListNode() { return DLAStarNodeListCache.getInstance().popItem(); }

    protected DLAStarNodeListNode()
    {
        _m_nPreNode = null;
        _m_nNode = null;
        _m_nNxtNode = null;

        _m_liListInfo = null;
    }

    public DLAStarNode nodeValue()
    {
        return _m_nNode;
    }

    public DLAStarNodeListNode preNode()
    {
        return _m_nPreNode;
    }

    public DLAStarNodeListNode nxtNode()
    {
        return _m_nNxtNode;
    }

    /***************
     * 添加上一个节点
     **/
    public DLAStarNodeListNode addPre(DLAStarNode _node)
    {
        //不在队列中则不允许进行添加操作
        //DLAStarNodeListNode newNode = _createListNode();
        if (null == _m_liListInfo)
            return null;

        //服务器采用不同的处理
        DLAStarNodeListNode newNode = _m_liListInfo.getNodeCache().popItem();
        if (null == newNode)
            return null;

        //设置节点信息和下一个信息
        newNode._m_nNxtNode = this;
        newNode._m_nNode = _node;
        newNode._m_nPreNode = _m_nPreNode;

        if (null != _m_nPreNode)
            _m_nPreNode._m_nNxtNode = newNode;
        //设置队列信息
        if (null != newNode._m_liListInfo)
        {
            CommLog.error("node has list info");
        }
        newNode._m_liListInfo = this._m_liListInfo;

        //判断是否是第一个
        if (null != _m_liListInfo && _m_liListInfo.firstNode() == this)
            _m_liListInfo._setFirstNode(newNode);

        //设置本对象的下一节点信息
        this._m_nPreNode = newNode;

        return newNode;
    }

    /***************
     * 添加下一个节点
     **/
    public DLAStarNodeListNode addNext(DLAStarNode _node)
    {
        //不在队列中则不允许进行添加操作
        //DLAStarNodeListNode newNode = _createListNode();
        if (null == _m_liListInfo)
            return null;

        //服务器采用不同的处理
        DLAStarNodeListNode newNode = _m_liListInfo.getNodeCache().popItem();
        if (null == newNode)
            return null;

        //设置节点信息和下一个信息
        newNode._m_nPreNode = this;
        newNode._m_nNode = _node;
        newNode._m_nNxtNode = _m_nNxtNode;

        if (null != _m_nNxtNode)
            _m_nNxtNode._m_nPreNode = newNode;
        //设置队列信息
        if (null != newNode._m_liListInfo)
        {
            CommLog.error("node has list info");
        }
        newNode._m_liListInfo = this._m_liListInfo;

        //判断是否是最后一个
        if (null != _m_liListInfo && _m_liListInfo.lastNode() == this)
            _m_liListInfo._setLastNode(newNode);

        //设置本对象的下一节点信息
        this._m_nNxtNode = newNode;

        return newNode;
    }

    /***************
     * 添加到最后一个节点
     **/
    public DLAStarNodeListNode addLast(DLAStarNode _node)
    {
        if (null == _m_liListInfo)
            return null;

        return _m_liListInfo.lastNode().addNext(_node);
    }

    /***************
     * 取出本对象节点，并返回剩余的对象
     **/
    public DLAStarNodeListNode popThis()
    {
        //设置本对象
        DLAStarNodeListNode tempListNode = _m_nNxtNode;
        if (null != _m_nNxtNode)
            _m_nNxtNode._m_nPreNode = this.preNode();
        if (null != _m_nPreNode)
            _m_nPreNode._m_nNxtNode = _m_nNxtNode;

        //判断是否是最后一个
        if (null != _m_liListInfo)
            _m_liListInfo._onNodeRemove(this);

        //回收本节点，服务器的代码在_onNodeRemove里进行了处理
        //DLAStarNodeListCache.getInstance().pushBackCacheItem(this);

        return tempListNode;
    }

    public void reset()
    {
        _m_nPreNode = null;
        _m_nNode = null;
        _m_nNxtNode = null;

        _m_liListInfo = null;
    }
}


