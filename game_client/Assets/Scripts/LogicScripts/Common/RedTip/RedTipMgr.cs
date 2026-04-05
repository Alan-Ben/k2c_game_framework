using System;
using JetBrains.Annotations;
using NPEnum;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 全局红点管理器,处理了配表红点的初始化
    /// </summary>
    public class RedTipMgr : _ARedTipNode._ABaseRedTipMgr
    {
        private static RedTipMgr _g_instance = new RedTipMgr();
        [NotNull]public static RedTipMgr instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new RedTipMgr();
                return _g_instance;
            }
        }

        // 键值为表中红点Id，值为对应的node
        [NotNull]private Dictionary<long, _ARedTipNode> _m_dRefRedTipDic = new Dictionary<long, _ARedTipNode>();
        [NotNull]private List<RefdataRedTipNode> _m_refDataTipList = new List<RefdataRedTipNode>();
        // 配置了simpleUnlock的红点刷新字典 <红点id，刷新方法>
        [NotNull]private Dictionary<long, Action> _m_dSimpleUnlockTriggerRecalDic = new Dictionary<long, Action>();
        
        public RedTipMgr() : base()
        {
            
        }

        /// <summary>
        /// 初始化红点系统，依赖配表
        /// </summary>
        protected override void _onInit()
        {
            if (!GRefdataCoreMgr.instance.isInited)
            {
                return;
            }
            
            _m_dRefRedTipDic.Clear();
            _m_refDataTipList.Clear();
            // 现将表中所有红点数据注册
            GRefdataCoreMgr.instance.redTipRefCore.dealAllRef((refObj) =>
            {
                // _ARedTipNode node;
                if (!_m_dRefRedTipDic.ContainsKey(refObj.id))
                {
                    RefdataRedTipNode node = new RefdataRedTipNode(refObj.id);
                    _m_dRefRedTipDic.Add(refObj.id, node);
                    _m_refDataTipList.Add(node);
                }
                else
                {
                    Debug.LogError($"红点id:{refObj.id}已经存在RefdataRedTipMgr管理器中");
                }
            });
        
            //子节点列表
            List<_ARedTipNode> subNodeList = new List<_ARedTipNode>();
            // 构造表中红点的树形结构
            GRefdataCoreMgr.instance.redTipRefCore.dealAllRef((refObj) =>
            {
                // 获取当前红点数据对应的node
                _ARedTipNode node = _m_dRefRedTipDic[refObj.id];

                // 否则遍历父节点Id列表，并从字典中获取父节点对应的node，存入临时父节点node列表中
                foreach (long id in refObj.sub_id_list)
                {
                    _ARedTipNode parentNode;
                    if (_m_dRefRedTipDic.TryGetValue(id, out parentNode))
                    {
                        subNodeList.Add(parentNode);
                    }
                    else
                    {
                        Debug.LogError($"[RedTipMgr:initRedTipMgr]红点{id}对应的红点节点不存在");
                    }
                }
                // 构建节点树
                addRedTipNodeWithSub(node, subNodeList);
                subNodeList.Clear();
            });

            // 添加配置了simpleUnlockId红点的刷新监听
            _regSimpleUnlockMsg();

            WinMsg.RegisterMsg(WinMsgType.ON_NEW_FUNCTION_UNLOCK_SHOW, _onFunctionUnlock);
        }
        
        //销毁
        protected override void _onDiscard()
        {
            _m_dRefRedTipDic.Clear();
            _m_refDataTipList.Clear();

            // 移除配置了simpleUnlockId红点的刷新监听
            _unRegSimpleUnlockMsg();

            WinMsg.UnregisterMsg(WinMsgType.ON_NEW_FUNCTION_UNLOCK_SHOW, _onFunctionUnlock);
        }
        
        /// <summary>
        /// 根据配表id获取红点数据
        /// </summary>
        /// <param name="_refId"></param>
        /// <returns></returns>
        public _ARedTipNode getNodeByRefRedTipId(long _refId)
        {
            if (!_m_bIsInited)
            {
                Debug.LogError($"[RedTipMgr] RedTipMgr还未初始化 _id:{_refId}");
                return null;
            }
                
            _ARedTipNode node;
            if (!_m_dRefRedTipDic.TryGetValue(_refId, out node))
            {
#if UNITY_EDITOR
                Debug.LogError($"[RedTipMgr:initRedTipMgr]红点表红点id：{_refId}对应的红点节点不存在");
#endif
            }

            return node;
        }

        /// <summary>
        /// 根据红点表的红点id设置count
        /// </summary>
        /// <param name="_refId"></param>
        /// <param name="_count"></param>
        /// <returns>设置成功返回true，不成功返回false</returns>
        public bool setCountByRefRedTipId(long _refId, long _count)
        {
            if (!_m_bIsInited)
            {
                Debug.LogError($"[RedTipMgr] RedTipMgr还未初始化 _id:{_refId}");
                return false;
            }
            
            if (_count <= 0)
                _count = 0;
            _ARedTipNode node = getNodeByRefRedTipId(_refId);
            if (node != null)
            {
                return node.setCount(_count);
            }
            else
            {
                Debug.LogError($"[RedTipMgr:setTagByRefRedTipId]设置count错误，红点表中不存在id为{_refId}的红点");
                return false;
            }
        }

        /// <summary>
        /// 根据红点表的红点id设置增加count
        /// </summary>
        /// <param name="_refId"></param>
        /// <param name="_count"></param>
        /// <returns>设置成功返回true，不成功返回false</returns>
        public bool addCountByRefRedTipId(long _refId, long _count)
        {
            if (!_m_bIsInited)
            {
                Debug.LogError($"[RedTipMgr] RedTipMgr还未初始化 _id:{_refId}");
                return false;
            }
            
            _ARedTipNode node = getNodeByRefRedTipId(_refId);
            if (node != null)
            {
                long newCount = _count + node.getCount();
                if(newCount <= 0)
                    newCount = 0;
                return node.setCount(newCount);
            }
            else
            {
                Debug.LogError($"[RedTipMgr:addCountByRefRedTipId]设置count错误，红点表中不存在id为{_refId}的红点");
                return false;
            }
        }

        /// <summary>
        /// 动态/静态单个红点注册函数
        /// 带上父节点
        /// </summary>
        public void addRedTipNodeWithParent(_ARedTipNode _node, long _parentRedRefId)
        {
            if (_m_dRefRedTipDic.TryGetValue(_parentRedRefId, out _ARedTipNode _parentNode))
            {
                addRedTipNodeWithParent(_node, _parentNode);
            }
            else
            {
                UnityEngine.Debug.LogError($"配表缺失红点id:{_parentRedRefId}");
            }
        }

        /// <summary>
        /// 重新计算带有FuncUnlock配置的红点
        /// </summary>
        public void recalculateRedTipWithFuncUnlock()
        {
            foreach (RefdataRedTipNode redTipNode in _m_refDataTipList)
            {
                if (null != redTipNode && null != redTipNode.redTipRefObj && redTipNode.redTipRefObj.function_type != ENPFunctionType.NONE)
                {
                    RedTipMgr.instance.addNeedRecalNodeList(redTipNode);
                }
            }
        }

        private void _onFunctionUnlock(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            ENPFunctionType functionType = (ENPFunctionType) _objects[0];
            
            //把对应解锁类型的红点节点加入到需要重新计算的红点节点列表中
            foreach (RefdataRedTipNode redTipNode in _m_refDataTipList)
            {
                if (null != redTipNode && null != redTipNode.redTipRefObj &&
                    redTipNode.redTipRefObj.function_type == functionType)
                {
                    RedTipMgr.instance.addNeedRecalNodeList(redTipNode);
                }
            }
        }

        /// <summary>
        /// 添加配置了simpleUnlockId红点的刷新监听
        /// </summary>
        private void _regSimpleUnlockMsg()
        {
            _m_dSimpleUnlockTriggerRecalDic.Clear();
            GRefdataCoreMgr.instance.redMonitorRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.msgTypeList != null && _ref.msgTypeList.Count > 0)
                {
                    Action msgRecAction = ()=> { _onSimpleUnlockRefresh(_ref); };

                    for (int i = 0; i < _ref.msgTypeList.Count; i++)
                    {
                        WinMsg.RegisterMsgAct(_ref.msgTypeList[i], msgRecAction);
                    }

                    _m_dSimpleUnlockTriggerRecalDic[_ref.id] = msgRecAction;
                }
            });
        }

        /// <summary>
        /// 移除配置了simpleUnlockId红点的刷新监听
        /// </summary>
        private void _unRegSimpleUnlockMsg()
        {
            if (_m_dSimpleUnlockTriggerRecalDic.Count <= 0)
                return;

            GRefdataCoreMgr.instance.redMonitorRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.msgTypeList != null && _ref.msgTypeList.Count > 0 && 
                    _m_dSimpleUnlockTriggerRecalDic.TryGetValue(_ref.id, out Action msgRecAction))
                {
                    for (int i = 0; i < _ref.msgTypeList.Count; i++)
                    {
                        WinMsg.UnregisterMsgAct(_ref.msgTypeList[i], msgRecAction);
                    }
                }
            });
            _m_dSimpleUnlockTriggerRecalDic.Clear();
        }

        /// <summary>
        /// 刷新配置了simpleUnlockId的红点
        /// </summary>
        /// <param name="_redMonitorRef"></param>
        private void _onSimpleUnlockRefresh(RedMonitorRefObj _redMonitorRef)
        {
            if (_redMonitorRef == null)
                return;

            if (_m_dRefRedTipDic.TryGetValue(_redMonitorRef.id, out _ARedTipNode redTipNode))
            {
                // 添加到需要重新计算的红点节点列表中
                RedTipMgr.instance.addNeedRecalNodeList(redTipNode);
            }
        }
    }
}