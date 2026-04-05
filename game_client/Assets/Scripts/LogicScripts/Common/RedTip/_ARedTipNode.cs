using System;
using ALPackage;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 红点节点基类
    /// </summary>
    public abstract class _ARedTipNode
    {
        private long _m_lId;//自己的ID
        [NotNull] private List<_ARedTipNode> _m_lParentList; //父节点列表
        [NotNull] private List<_ARedTipNode> _m_lChildList;//子节点列表

        private long _m_lSelfCount;//自己的提示数量
        private long _m_lChildTotalCount;//子节点的总数量统计
        
        private string _m_sSaveKey;     // 保存为本地文件的时的key
    
        //用于优化重新计算性能的重新计算标记
        private long _m_lRepeatCalJudgeIndexId;
        //用于检查是否死循环的实例Id
        private long _m_lLoopCheckInstanceId;

        public string saveKey { get { return _m_sSaveKey; } }
        
        /// <summary>
        /// 自身的计数
        /// </summary>
        protected long selfCount { get { return _m_lSelfCount; } }
        /// <summary>
        /// 子节点的总计数
        /// </summary>
        protected long childCount { get { return _m_lChildTotalCount; } }
        
        protected _ARedTipNode(string _saveKey)
        {
            _m_lId = ALSerializeOpMgr.next();
            _m_sSaveKey = _saveKey;

            _m_lChildTotalCount = 0;

            _m_lParentList = new List<_ARedTipNode>();
            _m_lChildList = new List<_ARedTipNode>();

            _m_lRepeatCalJudgeIndexId = 0;
            _m_lLoopCheckInstanceId = 0;

            // 从文件中读取出上次的SelfCount和自己的chgTime
            _load();
        }

        public long Id { get { return _m_lId; } }

        /// <summary>
        /// 设置节点次数
        /// </summary>
        /// <param name="_count">可领取的奖励数量</param>
        public bool setCount(long _count)
        {
            if (_m_lSelfCount == _count)
                return false;
            
            _m_lSelfCount = _count;

            // 保存SelfCount和ChgTime到本地
            _save();
            //加入需要重新计算队列，刷新ChildCount，如果childCount改变，则发出刷新通知
            RedTipMgr.instance.addNeedRecalNodeList(this);
            // 自己的数量变了，通知刷新（但是一刷会刷所有，还有可能和上面的任务重复，待优化？）
            WinMsg.SendMsg(WinMsgType.ON_RED_TIP_CHANGE);

            return true;
        }

        /// <summary>
        /// 获取count次数(自身次数+子节点次数)
        /// </summary>
        public virtual long getCount()
        {
            return _m_lSelfCount + _m_lChildTotalCount;
        }

        /// <summary>
        /// 添加父节点
        /// </summary>
        /// <param name="_node"></param>
        protected void _addParent(_ARedTipNode _node)
        {
            if(null == _node)
                return;

            //已有则不添加
            if(_m_lParentList.Contains(_node))
                return;

            _m_lParentList.Add(_node);
            //当增加父节点意味着父节点的子对象变化了，得刷新父节点
            RedTipMgr.instance.addNeedRecalNodeList(_node);
        }

        /// <summary>
        /// 删除父节点
        /// </summary>
        /// <param name="_node"></param>
        protected void _rmvParent(_ARedTipNode _node)
        {
            if(null == _node)
                return;

            //已有则不添加
            _m_lParentList.Remove(_node);

            //当移除父节点意味着父节点的子对象变化了，得刷新父节点
            RedTipMgr.instance.addNeedRecalNodeList(_node);
        }

        /// <summary>
        /// 添加一个节点作为子节点
        /// </summary>
        /// <param name="_node"></param>
        protected void _addChild(_ARedTipNode _node)
        {
            if(null == _node)
                return;

            //已有则不添加
            if(_m_lChildList.Contains(_node))
                return;

            _m_lChildList.Add(_node);

            //刷新子节点统计数量
            //if(_node.getCount() > 0) 加判断可能会有问题，因为修改count的刷新计算是注册任务，如果代码在修改子节点的count为0之后立即调用加入，刷新数量就错了
            RedTipMgr.instance.addNeedRecalNodeList(this);
        }

        /// <summary>
        /// 添加一个节点作为子节点
        /// </summary>
        /// <param name="_node"></param>
        protected void _rmvChild(_ARedTipNode _node)
        {
            if(null == _node)
                return;

            //删除失败则直接返回
            if(!_m_lChildList.Remove(_node))
                return;

            //刷新子节点统计数量
            //if(_node.getCount() > 0) 加判断可能会有问题，因为修改count的刷新计算是注册任务，如果代码在修改子节点的count为0之后立即调用移除，刷新数量就错了
            RedTipMgr.instance.addNeedRecalNodeList(this);
        }

        /// <summary>
        /// 重新计算所有父节点的数量统计，返回红点是否变化
        /// </summary>
        protected bool _recalculateAllParentCount(long _instanceId, long _repeatId)
        {
            // 红点数据是否发生变化，只要有一个发生变化，就设置为true，最后在最外层循环触发更新事件
            bool isRedTipChg = false;

            _ARedTipNode tmpNode = null;
            for(int i = 0; i < _m_lParentList.Count; i++)
            {
                tmpNode = _m_lParentList[i];
                if(null == tmpNode)
                    continue;

                //检测本地检测变量是否一致，是则死循环
                if(tmpNode._m_lLoopCheckInstanceId == _instanceId)
                {
                    ALLog.Crush("红点计算，此处计算死循环！ " + tmpNode.ToString());
                    continue;
                }

                //计算父节点，然后计算父节点之上的父节点
                if (tmpNode._refreshChildCount())
                    isRedTipChg = true;

                //重新计算父节点
                if (tmpNode._recalculateAllParentCount(_instanceId, _repeatId))
                    isRedTipChg = true;
            }

            return isRedTipChg;
        }

        /// <summary>
        /// 刷新子节点的统计数量，返回红点是否变化
        /// </summary>
        protected bool _refreshChildCount()
        {
            //记录当前值
            long preCount = _m_lChildTotalCount;

            _m_lChildTotalCount = 0;

            //遍历子对象处理
            _ARedTipNode tmpNode = null;
            for(int i = 0; i < _m_lChildList.Count; i++)
            {
                tmpNode = _m_lChildList[i];
                if(null == tmpNode)
                    continue;

                _m_lChildTotalCount += tmpNode.getCount();
            }

            return preCount != _m_lChildTotalCount;
        }

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            builder.Append("{");
            builder.Append("nodeId").Append(":").Append(Id);
            builder.Append("}");
            return builder.ToString();
        }

        /// <summary>
        /// 当前红点是否需要显示
        /// </summary>
        /// <returns></returns>
        public bool needShow()
        {
            return _needShow();
        }

        /// <summary>
        /// 是否需要显示这个红点节点
        /// </summary>
        protected abstract bool _needShow();

        // 保存selfCount和增量产生的时间
        private void _save()
        {
            RedTipMgr.instance.saveNodeData(_m_sSaveKey, _m_lSelfCount);
        }

        // 从文件中读出selfCount和增量产生的时间
        private void _load()
        {
            RedTipMgr.instance.getNodeData(_m_sSaveKey, out _m_lSelfCount);
        }

        public string getNodeTreeString(int _tabCount = 0)
        {
            //增加funcUnlock和simpleUnlock显示
            bool isFuncUnlock = false;
            bool isSimpleUnlock = false;
            NPRedTipRefObj redTipRef = null;
            RedMonitorRefObj redMonitorRef = null;
            if (long.TryParse(_m_sSaveKey, out long redTipId))
            {
                redTipRef = GRefdataCoreMgr.instance.redTipRefCore.getRef(redTipId);
                redMonitorRef = GRefdataCoreMgr.instance.redMonitorRefCore.getRef(redTipId);
                isFuncUnlock = redTipRef != null && redTipRef.function_type != ENPFunctionType.NONE;
                isSimpleUnlock = redMonitorRef != null && redMonitorRef.simple_unlock_id > 0;
            }

            StringBuilder stringBuilder = new StringBuilder();
            // 加入tab键
            for (int i = 0; i < _tabCount; i++)
                stringBuilder.Append('\t');
            // 输出自己节点的信息
            stringBuilder.Append(
                $"{nameof(saveKey)}: {saveKey}, {nameof(selfCount)}: {selfCount}, {nameof(childCount)}: {childCount}" +
                $"{(isFuncUnlock ? ($"        FUNC: {GCommon.isFuncUnlock(redTipRef.function_type)} ") : "        ")}{(isSimpleUnlock ? ($"SIM: {GCommon.isSimpleUnlock(redMonitorRef.simple_unlock_id)}") : "")}");

            // 如果有子节点，递归打印出子节点信息
            if (_m_lChildList.Count > 0)
            {
                // 先回车准备打印子节点
                stringBuilder.Append('\n');
                // 加入tab键
                for (int i = 0; i < _tabCount; i++)
                    stringBuilder.Append('\t');
                stringBuilder.Append($"childList(count:{_m_lChildList.Count}):\n");
                // 回车后逐行加一个tab键显示子节点信息
                for (int i = 0; i < _m_lChildList.Count; i++)
                {
                    _ARedTipNode childNode = _m_lChildList[i];
                    stringBuilder.Append(childNode.getNodeTreeString(_tabCount + 1));
                    // 如果不是最后一个就加一个回车来显示下一个子节点
                    if (i != _m_lChildList.Count - 1)
                        stringBuilder.Append('\n');
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 递归查找对应的红点节点
        /// </summary>
        public _ARedTipNode getNodeBySaveKeyRecursive(string _saveKey)
        {
            if (saveKey == _saveKey)
                return this;

            foreach (_ARedTipNode node in _m_lChildList)
            {
                _ARedTipNode result = node?.getNodeBySaveKeyRecursive(_saveKey);
                if (result != null)
                    return result;
            }
            return null;
        }

        /// <summary>
        /// 递归对这个节点下的所有节点做处理
        /// </summary>
        public void doSomethingForAllNodeSync(Action<_ARedTipNode> _action)
        {
            if (_action == null)
                return;
            
            _action(this);

            foreach (_ARedTipNode node in _m_lChildList)
            {
                node?.doSomethingForAllNodeSync(_action);
            }
        }

        /// <summary>
        /// 红点管理器，为了不开放部分接口，将管理器类作为本结构体的子集
        /// </summary>
        public abstract class _ABaseRedTipMgr
        {
            //存储setting
            private RedTipTimeSaver _m_sRedTipTimeSaver;

            //注册到本管理器的所有节点容器，根据实例Id索引
            [NotNull] private Dictionary<long, _ARedTipNode> _m_dRedTipDic;

            //根节点的队列
            [NotNull] private List<_ARedTipNode> _m_lRootNodeList;

            //需要重新计算通知数量的节点队列
            [NotNull] private List<_ARedTipNode> _m_lNeedRecalNodeList;
            //重新计算的状态标记，判断是否需要计算，避免重复注册任务
            private bool _m_bRecalState;
            //是否初始化了
            protected bool _m_bIsInited;

            protected _ABaseRedTipMgr()
            {
                _m_dRedTipDic = new Dictionary<long, _ARedTipNode>();
                _m_lRootNodeList = new List<_ARedTipNode>();
                _m_lNeedRecalNodeList = new List<_ARedTipNode>();
                _m_bRecalState = false;

            }

            public void init()
            {
                if (_m_bIsInited)
                    return;

                _m_bIsInited = true;
                _m_dRedTipDic.Clear();
                _m_lRootNodeList.Clear();
                _m_lNeedRecalNodeList.Clear();

                _m_sRedTipTimeSaver = new RedTipTimeSaver();
                _m_sRedTipTimeSaver.init();
                
                _onInit();
            }

            public void discard()
            {
                _m_bIsInited = false;
                if (_m_dRedTipDic != null)
                    _m_dRedTipDic.Clear();
                if (_m_lRootNodeList != null)
                    _m_lRootNodeList.Clear();
                if (_m_lNeedRecalNodeList != null)
                    _m_lNeedRecalNodeList.Clear();

                _m_sRedTipTimeSaver = new RedTipTimeSaver();
                _m_sRedTipTimeSaver.init();
                
                _onDiscard();
            }

            /// <summary>
            /// 根据id获取对应红点Node
            /// </summary>
            /// <param name="_id"></param>
            /// <returns></returns>
            public _ARedTipNode getNodeByInstanceId(long _id)
            {
                _ARedTipNode node;
                if(_m_dRedTipDic.TryGetValue(_id, out node))
                    return node;

                return null;
            }

            /// <summary>
            /// 添加一个根节点对象
            /// </summary>
            /// <param name="_node"></param>
            /// <param name="_parentList"></param>
            public void addRootRedTipNode(_ARedTipNode _node)
            {
                if(_node == null)
                    return;

                //将该红点加入红点字典中
                _m_dRedTipDic[_node.Id] = _node;

                if(_m_lRootNodeList.Contains(_node))
                    return;

                _m_lRootNodeList.Add(_node);
            }
            
            /// <summary>
            /// 动态/静态单个红点注册函数
            /// 带上父节点
            /// </summary>
            /// <param name="_node"></param>
            public void addRedTipNodeWithParent(_ARedTipNode _node, List<_ARedTipNode> _parentList)
            {
                if(_node == null)
                    return;

                //将该红点加入红点字典中
                _m_dRedTipDic[_node.Id] = _node;

                bool isRoot = true;
                if(null != _parentList)
                {
                    // 遍历其父节点
                    foreach(_ARedTipNode parentNode in _parentList)
                    {
                        if(null == parentNode)
                            continue;

                        // 判断父节点是否已经存在红点字典中，不在则作为根节点加入
                        if(!_m_dRedTipDic.ContainsKey(parentNode.Id))
                        {
                            addRootRedTipNode(parentNode);
                        }

                        //加入的节点需要从根节点列表删除
                        _m_lRootNodeList.Remove(_node);

                        //设置非根节点
                        isRoot = false;
                        //将父节点添加到节点下
                        _node._addParent(parentNode);
                        //将节点加入父节点之下
                        parentNode._addChild(_node);
                    }
                }

                //如果是根节点则加入根节点队列
                if(isRoot)
                    addRootRedTipNode(_node);
            }

            /// <summary>
            /// 动态/静态单个红点注册函数
            /// 带上父节点
            /// </summary>
            /// <param name="_node"></param>
            public void addRedTipNodeWithParent(_ARedTipNode _node, _ARedTipNode _parentNode)
            {
                if(_node == null)
                    return;

                //将该红点加入红点字典中
                _m_dRedTipDic[_node.Id] = _node;

                bool isRoot = true;
                if(null != _parentNode)
                {
                    // 判断父节点是否已经存在红点字典中，不在则作为根节点加入
                    if(!_m_dRedTipDic.ContainsKey(_parentNode.Id))
                    {
                        addRootRedTipNode(_parentNode);
                    }

                    //加入的节点需要从根节点列表删除
                    _m_lRootNodeList.Remove(_node);

                    //设置非根节点
                    isRoot = false;
                    //将父节点添加到节点下
                    _node._addParent(_parentNode);
                    //将节点加入父节点之下
                    _parentNode._addChild(_node);
                }

                //如果是根节点则加入根节点队列
                if(isRoot)
                    addRootRedTipNode(_node);
            }
            
            /// <summary>
            /// 动态/静态单个红点注册函数
            /// 带上子节点
            /// </summary>
            /// <param name="_node"></param>
            public void addRedTipNodeWithSub(_ARedTipNode _node, List<_ARedTipNode> _subList)
            {
                if(_node == null)
                    return;
                
                // 判断节点是否已经存在红点字典中，不在则作为根节点加入
                if(!_m_dRedTipDic.ContainsKey(_node.Id))
                {
                    addRootRedTipNode(_node);
                }

                if (null != _subList && _subList.Count > 0)
                {
                    foreach (_ARedTipNode subTipNode in _subList)
                    {
                        if(null == subTipNode)
                            continue;
                        
                        //没有注册过添加一下
                        if(!_m_dRedTipDic.ContainsKey(subTipNode.Id))
                        {
                            _m_dRedTipDic.Add(subTipNode.Id, subTipNode);
                        }

                        //将父节点添加到节点下
                        _node._addChild(subTipNode);
                        //将节点加入父节点之下
                        subTipNode._addParent(_node);
                    }
                }
            }
            
            /// <summary>
            /// 从管理器中移除某个节点
            /// </summary>
            /// <param name="_id"></param>
            public void rmvRedTipNode(_ARedTipNode _node)
            {
                if(null == _node)
                    return;

                //判断节点是否在管理器内注册
                if(!_m_dRedTipDic.Remove(_node.Id))
                    return;

                //从根节点列表删除
                _m_lRootNodeList.Remove(_node);

                //将节点从父节点删除
                _ARedTipNode parentNode = null;
                for(int i = 0; i < _node._m_lParentList.Count; i++)
                {
                    parentNode = _node._m_lParentList[i];
                    if(null == parentNode)
                        continue;

                    //父节点删除子节点对象
                    parentNode._rmvChild(_node);
                }
                //清理父节点
                _node._m_lParentList.Clear();

                //将所有子节点都删除
                _ARedTipNode childNode = null;
                for(int i = 0; i < _node._m_lChildList.Count; i++)
                {
                    childNode = _node._m_lChildList[i];
                    if(null == childNode)
                        continue;

                    //父节点删除子节点对象
                    childNode._rmvParent(_node);

                    //当子节点的所有父节点空了时则从管理器清除
                    if(childNode._m_lParentList.Count <= 0)
                    {
                        //删除子节点
                        rmvRedTipNode(childNode);
                    }
                }
            }

            /// <summary>
            /// 添加需要重新计算的节点；
            /// 注意：这里面运算的所有节点只会运算子节点的变化带来的结果，自己的变化要在父节点里反应出来，如果自己就是根节点，则没有任何效果；
            /// </summary>
            public void addNeedRecalNodeList(_ARedTipNode _node)
            {
                if(null == _node)
                    return;

                //判断是否注册
                if(!_m_dRedTipDic.ContainsKey(_node.Id))
                {
                    ALLog.Error($"Try Recalculate a red tip node which is no registed! nodeId: {_node.Id}\t\tsaveKey:   {_node.saveKey}");
                    return;
                }

                //判断是否重复，是则不重复注册
                if(_m_lNeedRecalNodeList.Contains(_node))
                    return;

                _m_lNeedRecalNodeList.Add(_node);
                if(!_m_bRecalState)
                {
                    _m_bRecalState = true;
                    //注册重新计算函数
                    ALCommonTaskController.CommonActionAddLaterMonoTask(_recalculateNodeCount);
                }
            }
            
            /// <summary>
            /// 重新计算所有注册的需要重新计算的节点
            /// </summary>
            protected void _recalculateNodeCount()
            {
                _m_bRecalState = false;
                long recalIndex = ALSerializeOpMgr.next();
                bool isRedTipChg = false;

                //将队列节点取出，逐个计算
                _ARedTipNode tmpNode = null;
                for(int i = 0; i < _m_lNeedRecalNodeList.Count; i++)
                {
                    tmpNode = _m_lNeedRecalNodeList[i];
                    if(null == tmpNode)
                        continue;

                    //判断节点是否被重新计算过，是则不计算
                    if(tmpNode._m_lRepeatCalJudgeIndexId == recalIndex)
                        continue;

                    //判断是否注册，未注册则不处理
                    if(!_m_dRedTipDic.ContainsKey(tmpNode.Id))
                        continue;

                    if (tmpNode._refreshChildCount())
                        isRedTipChg = true;

                    //重新计算，并且返回true时，标记成红点有发生变动
                    if (tmpNode._recalculateAllParentCount(ALSerializeOpMgr.next(), recalIndex))
                        isRedTipChg = true;
                }

                if (isRedTipChg)
                    WinMsg.SendMsg(WinMsgType.ON_RED_TIP_CHANGE);

                //清空队列
                _m_lNeedRecalNodeList.Clear();
            }

            /// <summary>
            /// 获取整个红点树字符串
            /// </summary>
            /// <returns></returns>
            public string getNodeTreeString()
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (_ARedTipNode node in _m_lRootNodeList)
                {
                    stringBuilder.Append(node.getNodeTreeString());
                    stringBuilder.Append('\n');
                }
                return stringBuilder.ToString();
            }

            /// <summary>
            /// 递归查找对应的红点节点
            /// </summary>
            public _ARedTipNode getNodeBySaveKeyRecursive(string _saveKey)
            {
                foreach (_ARedTipNode node in _m_lRootNodeList)
                {
                    _ARedTipNode result = node?.getNodeBySaveKeyRecursive(_saveKey);
                    if (result != null)
                        return result;
                }
                return null;
            }
            
            /// <summary>
            /// 获取指定红点根节点
            /// </summary>
            /// <param name="_saveKey"></param>
            /// <returns></returns>
            public _ARedTipNode getRootNodeBySaveKey(string _saveKey)
            {
                foreach (_ARedTipNode node in _m_lRootNodeList)
                {
                    if (node != null && node.saveKey == _saveKey)
                        return node;
                }
                return null;
            }

            /// <summary>
            /// 保存红点的信息
            /// </summary>
            public void saveNodeData(string _saveKey, long _selfCount)
            {
                if(null == _m_sRedTipTimeSaver)
                    return;
                
                _m_sRedTipTimeSaver.saveNodeData(_saveKey, _selfCount);
            }
            
            /// <summary>
            /// 根据红点的saveKey获取对应的数据
            /// </summary>
            public void getNodeData(string _saveKey, out long _selfCount)
            {
                if (null == _m_sRedTipTimeSaver)
                {
                    _selfCount = 0;
                    return;
                }
                
                _m_sRedTipTimeSaver.getNodeData(_saveKey, out _selfCount);
            }

            //当初始化
            protected abstract void _onInit();
            
            //当销毁
            protected abstract void _onDiscard();
        }
    }
}