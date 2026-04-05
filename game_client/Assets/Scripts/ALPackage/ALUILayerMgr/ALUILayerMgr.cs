﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ALPackage
{
    /*********************
     * UI系统的层级管理根对象
     * UnityEngine.Debug.LogError($"[{Time.frameCount}] Add Node: {_node.transF.name} - [{_node.goInstanceID}]");
     **/
    public class ALUILayerMgr : _IALBaseMonoTask
    {
        /// <summary>
        /// 用于检测队列排序的处理
        /// </summary>
        protected struct UILayerCheckInfo
        {
            /// <summary>
            /// 对象的层级，用于判断执行顺序，要求父节点一定在子节点之前处理
            /// </summary>
            public int layer;
            /// <summary>
            /// 执行检测的对象
            /// </summary>
            public _IALUILayerBasicNodeMonoInterface _checkMono;
        }


        private static ALUILayerMgr _g_instance = new ALUILayerMgr();
        public static ALUILayerMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ALUILayerMgr();

                return _g_instance;
            }
        }

        /// <summary>
        /// 存储所有节点对应对象的映射关系表
        /// </summary>
        private Dictionary<int, _AALUILayerBasicNode> _m_dicAllNodeDic;

        /// <summary>
        /// 根节点的对象队列
        /// </summary>
        private List<ALUILayerRootNode> _m_lRootNodeList;

        /// <summary>
        /// 每帧处理的执行检测操作的临时队列，在本帧处理完毕之前用于临时存储的队列
        /// 这样可以保证每帧处理的对象在同一个函数内处理
        /// </summary>
        private List<_IALUILayerBasicNodeMonoInterface> _m_lTickCheckInfoTmpList;
        /// <summary>
        /// 处理完毕的帧数，用帧数对比来判断当前帧是否处理完毕
        /// </summary>
        private int _m_iIsCurFrameDone;
        /// <summary>
        /// 每帧处理的执行检测操作的队列
        /// </summary>
        private List<UILayerCheckInfo> _m_lTickCheckInfoList;


        protected ALUILayerMgr()
        {
            _m_dicAllNodeDic = new Dictionary<int, _AALUILayerBasicNode>();
            _m_lRootNodeList = new List<ALUILayerRootNode>();

            _m_iIsCurFrameDone = 0;
            _m_lTickCheckInfoTmpList = new List<_IALUILayerBasicNodeMonoInterface>();

            _m_lTickCheckInfoList = new List<UILayerCheckInfo>();
        }

        /// <summary>
        /// 初始化所有UI根节点的节点对象
        /// </summary>
        public void initUIRootNodes(List<ALUILayerInfo> _uiLayerInfoList)
        {
            if(null == _uiLayerInfoList)
                return;

            //逐个注册
            ALUILayerInfo tmpObj = null;
            for(int i = 0; i < _uiLayerInfoList.Count; i++)
            {
                tmpObj = _uiLayerInfoList[i];
                if(null == tmpObj)
                    continue;

                if(null == tmpObj.layerRootGo || null == tmpObj.cavasObj)
                    continue;

                //判断是否有重复节点
                if(_m_dicAllNodeDic.ContainsKey(tmpObj.layerRootGo.GetInstanceID()))
                {
                    //有可能多个UI节点使用不同的node，所以这里跳过注册
                    continue;
                }

                //创建一个根节点对象
                ALUILayerRootNode rootNode = new ALUILayerRootNode(tmpObj);
                //添加到根节点队列
                _m_lRootNodeList.Add(rootNode);
                //添加一个节点对象
                _regUILayerNode(rootNode);
            }

            //注册本对象为每帧执行的任务
            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }

        /// <summary>
        /// 执行每帧检测需要检测状态的UI层级对象的任务
        /// </summary>
        public void deal()
        {
            try
            {
                if (_m_lTickCheckInfoList.Count <= 0)
                {
                    //添加到下一帧
                    ALMonoTaskMgr.instance.addNextFrameTask(this);
                    return;
                }

                //逐个检测需要检测的对象
                UILayerCheckInfo tmpInfo;
                for (int i = 0; i < _m_lTickCheckInfoList.Count; i++)
                {
                    tmpInfo = _m_lTickCheckInfoList[i];
                    if (null == tmpInfo._checkMono)
                        continue;

                    //UnityEngine.Debug.LogError($"[{Time.frameCount}] Check Node: {tmpInfo._checkMono.trans.name} - [{tmpInfo._checkMono.trans.GetInstanceID()}]");
                    tmpInfo._checkMono.check();
                }

                //清空队列
                _m_lTickCheckInfoList.Clear();

                //添加到下一帧
                ALMonoTaskMgr.instance.addNextFrameTask(this);
            }
            finally
            {
                //设置处理完毕的帧数
                _setDoneFrame(Time.frameCount);
            }
        }

        /// <summary>
        /// 设置处理完毕的帧数
        /// </summary>
        /// <param name="_frame"></param>
        private void _setDoneFrame(int _frame)
        {
            //设置处理完毕的帧数
            _m_iIsCurFrameDone = _frame;

            //将临时队列拷贝到正式队列，并清空临时队列
            foreach(_IALUILayerBasicNodeMonoInterface info in _m_lTickCheckInfoTmpList)
            {
                addCheckInfo(info);
            }

            _m_lTickCheckInfoTmpList.Clear();
        }

        /// <summary>
        /// 添加一个检测对象，根据检测对象对应的层级添加到管理器中
        /// </summary>
        /// <param name="_infoMono"></param>
        public void addCheckInfo(_IALUILayerBasicNodeMonoInterface _infoMono)
        {
            if (null == _infoMono)
                return;

            //如当前帧还未处理完则放入临时队列
            if (_m_iIsCurFrameDone != Time.frameCount)
            {
                _m_lTickCheckInfoTmpList.Add(_infoMono);
                return;
            }

            //获取对应的对象
            Transform tmpTrans = _infoMono.trans;
            if (null == tmpTrans)
                return;

            //循环检测对象在第几层
            int deeper = 0;
            bool isLayerChild = false;
            while(null != tmpTrans)
            {
                deeper++;

                //判断对象是否已经存在，如存在则表明是UI的子节点
                //由于如果不是UI子节点不做处理，所以这里进行判断
                if (_m_dicAllNodeDic.ContainsKey(tmpTrans.GetInstanceID()))
                    isLayerChild = true;

                tmpTrans = tmpTrans.parent;
            }

            //如果不是UI子节点则不做处理
            if (!isLayerChild)
                return;

            //构造处理对象放入队列
            UILayerCheckInfo tmpInfo = new UILayerCheckInfo();
            tmpInfo.layer = deeper;
            tmpInfo._checkMono = _infoMono;

            //依序放入队列，层级低的在前
            for (int i = 0; i < _m_lTickCheckInfoList.Count; )
            {
                if (_m_lTickCheckInfoList[i].layer > deeper)
                {
                    _m_lTickCheckInfoList.Insert(i, tmpInfo);
                    return;
                }
                else if (_m_lTickCheckInfoList[i]._checkMono.trans == _infoMono.trans)
                {
                    //如对象一致，则从当前队列删除，等待添加到同一层级末尾
                    //删除同一对象可以保证最后一次调用的对象层级一定是最新的
                    _m_lTickCheckInfoList.RemoveAt(i);
                    continue;
                }

                i++;
            }

            //如果没有找到则直接添加
            _m_lTickCheckInfoList.Add(tmpInfo);
        }

        /// <summary>
        /// 从数据中移除需要检测的某个数据
        /// </summary>
        public void removeCurTickCheckInfo(_IALUILayerBasicNodeMonoInterface _interf, List<_IALUILayerBasicNodeMonoInterface> _recInterfaceList = null)
        {
            //检索临时队列，将所有对象及子对象排除
            for (int i = 0; i < _m_lTickCheckInfoTmpList.Count;)
            {
                _IALUILayerBasicNodeMonoInterface tmpMono = _m_lTickCheckInfoTmpList[i];
                bool ifFind = false;

                //数据无效不做处理
                if(null == tmpMono || null == tmpMono.trans)
                {
                    _m_lTickCheckInfoTmpList.RemoveAt(i);
                    continue;
                }

                if (tmpMono == _interf)
                {
                    _m_lTickCheckInfoTmpList.RemoveAt(i);
                    //去重添加到集合中
                    if(null != _recInterfaceList && !_recInterfaceList.Contains(tmpMono))
                        _recInterfaceList.Add(tmpMono);

                    ifFind = true;
                }
                else
                {
                    //遍历父节点看是否在本节点上的，都需要删除
                    Transform tmpParent = tmpMono.trans.parent;
                    while (null != tmpParent)
                    {
                        //如果父节点是需要移除的，则移除本节点
                        if (tmpParent == _interf.trans)
                        {
                            //移除本对象
                            _m_lTickCheckInfoTmpList.RemoveAt(i);
                            //去重添加到集合中
                            if (null != _recInterfaceList && !_recInterfaceList.Contains(tmpMono))
                                _recInterfaceList.Add(tmpMono);

                            ifFind = true;
                            break;
                        }
                        //获取父节点
                        tmpParent = tmpParent.parent;
                    }
                }

                if (!ifFind)
                    i++;
            }

            //检索处理队列，将所有对象及子对象排除
            for (int i = 0; i < _m_lTickCheckInfoList.Count;)
            {
                _IALUILayerBasicNodeMonoInterface tmpMono = _m_lTickCheckInfoList[i]._checkMono;
                bool ifFind = false;

                //数据无效不做处理
                if (null == tmpMono || null == tmpMono.trans)
                {
                    _m_lTickCheckInfoList.RemoveAt(i);
                    continue;
                }

                if (tmpMono == _interf)
                {
                    _m_lTickCheckInfoList.RemoveAt(i);
                    //去重添加到集合中
                    if (null != _recInterfaceList && !_recInterfaceList.Contains(tmpMono))
                        _recInterfaceList.Add(tmpMono);

                    ifFind = true;
                }
                else
                {
                    //遍历父节点看是否在本节点上的，都需要删除
                    Transform tmpParent = tmpMono.trans.parent;
                    while (null != tmpParent)
                    {
                        //如果父节点是需要移除的，则移除本节点
                        if (tmpParent == _interf.trans)
                        {
                            //移除本对象
                            _m_lTickCheckInfoList.RemoveAt(i);
                            //去重添加到集合中
                            if (null != _recInterfaceList && !_recInterfaceList.Contains(tmpMono))
                                _recInterfaceList.Add(tmpMono);

                            ifFind = true;
                            break;
                        }
                        //获取父节点
                        tmpParent = tmpParent.parent;
                    }
                }

                if (!ifFind)
                    i++;
            }
        }

        /// <summary>
        /// 匹配对应的Transform对象是否是一个根节点
        /// 是则返回对应的根节点对象，否则返回null
        /// </summary>
        /// <param name="_tf"></param>
        /// <returns></returns>
        public ALUILayerRootNode fixRootNode(Transform _tf)
        {
            if (null == _tf)
                return null;

            ALUILayerRootNode tmpNode = null;
            for(int i = 0; i < _m_lRootNodeList.Count; i++)
            {
                tmpNode = _m_lRootNodeList[i];
                if (null == tmpNode)
                    continue;

                if(tmpNode.rootTF == _tf)
                    return tmpNode;
            }

            return null;
        }

        /// <summary>
        /// 添加一个UI层级节点
        /// </summary>
        /// <param name="_node"></param>
        public void addUILayerNode(_AALUILayerBasicNode _node)
        {
            if(null == _node || null == _node.transF)
                return;

            //如对应数据已经存在则不处理
            if(_m_dicAllNodeDic.ContainsKey(_node.goInstanceID))
            {
#if UNITY_EDITOR
                ALLog.Error($"Node Has Been in the manager! Trans:{_node.transF}");
#endif
                return;
            }

            //遍历查询是否有已经注册的父节点，如无则报错
            _AALUILayerBasicNode parentNode = null;
            Transform tmpParent = _node.transF.parent;
            while(true)
            {
                if(null == tmpParent)
                    break;

                //获取数据
                int parentInstanceId = tmpParent.GetInstanceID();
                if(_m_dicAllNodeDic.TryGetValue(parentInstanceId, out parentNode))
                {
                    if (!(parentNode is _IALUILayerParentNode))
                    {
                        //获取父节点
                        tmpParent = tmpParent.parent;
                        continue;
                    }

                    //进行后续处理
                    parentNode.addChild(_node);

                    //加入数据集
                    if(!_m_dicAllNodeDic.ContainsKey(_node.goInstanceID))
                    {
                        _m_dicAllNodeDic.Add(_node.goInstanceID, _node);
                    }

                    break;
                }

                //获取父节点
                tmpParent = tmpParent.parent;
            }

            //如果没数据则报错
            if(null == parentNode)
            {
                ALLog.Error("Can not find parent Node for: " + _node.transF.name);
            }
        }

        /// <summary>
        /// 添加一个动态管理层级的UI层级节点
        /// </summary>
        /// <param name="_node"></param>
        public void addUILayerDynamicNode(_AALUILayerBasicNode _node, int _additionLayer)
        {
            if(null == _node || null == _node.transF)
                return;

            //如对应数据已经存在则不处理
            if (_m_dicAllNodeDic.ContainsKey(_node.goInstanceID))
            {
                return;
            }

            //遍历查询是否有已经注册的父节点，如无则报错
            _AALUILayerBasicNode parentNode = null;
            Transform tmpParent = _node.transF.parent;
            while(true)
            {
                if(null == tmpParent)
                    break;

                //获取数据
                int parentInstanceId = tmpParent.GetInstanceID();
                if(_m_dicAllNodeDic.TryGetValue(parentInstanceId, out parentNode))
                {
                    if(!(parentNode is _IALUILayerDynamicNode))
                    {
                        //获取父节点
                        tmpParent = tmpParent.parent;
                        continue;
                    }

                    _IALUILayerDynamicNode dynamicNode = (_IALUILayerDynamicNode)parentNode;
                    //进行后续处理
                    dynamicNode.addDynamicChild(_node, _additionLayer);

                    //加入数据集
                    if(!_m_dicAllNodeDic.ContainsKey(_node.goInstanceID))
                    {
                        _m_dicAllNodeDic.Add(_node.goInstanceID, _node);
                    }

                    break;
                }

                //获取父节点
                tmpParent = tmpParent.parent;
            }

            //如果没数据则报错
            if(null == parentNode)
            {
                ALLog.Error("Can not find parent Node for: " + _node.transF.name);
            }
        }

        /// <summary>
        /// 刷新某个实例节点的层级
        /// </summary>
        /// <param name="_instanceId"></param>
        public void refreshNodeLayer(Transform _trans)
        {
            if (null == _trans)
                return;

            _AALUILayerBasicNode node = null;
            int instanceId = _trans.GetInstanceID();
            //构造移除的队列
            List<_IALUILayerBasicNodeMonoInterface> rmvList = new List<_IALUILayerBasicNodeMonoInterface>();

            if (_m_dicAllNodeDic.TryGetValue(instanceId, out node))
            {
                //unregData.Add(node.nodeObj.monoData);

                //先移除
                node.nodeObj.monoData.unregFromLayer(rmvList);
            }

            _IALUILayerBasicNodeMonoInterface interf = _trans.GetComponent<_IALUILayerBasicNodeMonoInterface>();
            //如果存在数据则放入检查
            if (null != interf)
            {
                //从既有检查队列中移除
                removeCurTickCheckInfo(interf, rmvList);
            }

            //逐个将移除的接口对象添加回处理队列
            foreach (_IALUILayerBasicNodeMonoInterface mono in rmvList)
            {
                addCheckInfo(mono);
            }
        }

        public void refreshNodeLayer(List<Transform> _trans)
        {
            if (null == _trans)
                return;

            // List<_AALUILayerBasicMonoData> unregData = new List<_AALUILayerBasicMonoData>();
            //构造移除的队列
            List<_IALUILayerBasicNodeMonoInterface> rmvList = new List<_IALUILayerBasicNodeMonoInterface>();

            Transform itemTrans = null;
            //逐个先移除，再添加
            for (int i = 0; i < _trans.Count; i++)
            {
                itemTrans = _trans[i];
                if(null == itemTrans)
                    continue;

                _AALUILayerBasicNode node = null;
                int instanceId = itemTrans.GetInstanceID();
                if (_m_dicAllNodeDic.TryGetValue(instanceId, out node))
                {
                    //unregData.Add(node.nodeObj.monoData);

                    //先移除
                    node.nodeObj.monoData.unregFromLayer(rmvList);
                }

                _IALUILayerBasicNodeMonoInterface interf = itemTrans.GetComponent<_IALUILayerBasicNodeMonoInterface>();
                //如果存在数据则放入检查
                if (null != interf)
                {
                    //从既有检查队列中移除
                    removeCurTickCheckInfo(interf, rmvList);
                }
            }

            //逐个将移除的接口对象添加回处理队列
            foreach (_IALUILayerBasicNodeMonoInterface mono in rmvList)
            {
                addCheckInfo(mono);
            }
        }

        /// <summary>
        /// 从管理器中移除某个节点
        /// </summary>
        /// <param name="_node"></param>
        public void rmvLayerNode(int _instanceId, List<_IALUILayerBasicNodeMonoInterface> _recInterfaceList = null)
        {
            _AALUILayerBasicNode node = null;
            if(!_m_dicAllNodeDic.TryGetValue(_instanceId, out node))
                return;

            //UnityEngine.Debug.LogError($"[{Time.frameCount}] remove Node: instanceId:{_instanceId}]");
            //添加到接收结果集，判断是否在独列，不在则添加
            if (null != _recInterfaceList && !_recInterfaceList.Contains(node.nodeObj.nodeInterface))
            {
                _recInterfaceList.Add(node.nodeObj.nodeInterface);
            }

            //移除本节点
            _m_dicAllNodeDic.Remove(_instanceId);
            //移除子节点
            node.clearAllChild(_recInterfaceList);

            //清理对应节点的数据
            if(null != node.parentNode)
                node.parentNode.rmvChild(node);
        }
        public void rmvLayerNode(_AALUILayerBasicNode _node, List<_IALUILayerBasicNodeMonoInterface> _recInterfaceList = null)
        {
            if(null == _node || null == _node.transF)
                return;

            //移除本节点
            int instanceId = _node.goInstanceID;
            //UnityEngine.Debug.LogError($"[{Time.frameCount}] remove Node: {_node.transF.name} - [{_node.transF.GetInstanceID()}]");
            //移除本对象
            if (_m_dicAllNodeDic.Remove(instanceId))
            {
                //添加到接收结果集，判断是否在独列，不在则添加
                if (null != _recInterfaceList && !_recInterfaceList.Contains(_node.nodeObj.nodeInterface))
                {
                    _recInterfaceList.Add(_node.nodeObj.nodeInterface);
                }

                //成功则移除所有子节点对象
                _node.clearAllChild(_recInterfaceList);

                //清理对应节点的数据
                if(null != _node.parentNode)
                    _node.parentNode.rmvChild(_node);
            }
        }

        /// <summary>
        /// 获取对应Go的节点所对应的层级信息
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <returns></returns>
        public _AALUILayerBasicNode getUILayerNode(int _instanceId)
        {
            _AALUILayerBasicNode resObj = null;
            if(_m_dicAllNodeDic.TryGetValue(_instanceId, out resObj))
                return resObj;

            return null;
        }

        /// <summary>
        /// 注册节点
        /// </summary>
        /// <param name="_node"></param>
        protected void _regUILayerNode(_AALUILayerBasicNode _node)
        {
            if(null == _node)
                return;

#if UNITY_EDITOR
            //判断是否有重复值
            if(_m_dicAllNodeDic.ContainsKey(_node.goInstanceID))
            {
                ALLog.Error($"Node Has Been in the manager! Trans:{_node.transF}");
                return;
            }
#endif

            _m_dicAllNodeDic.Add(_node.goInstanceID, _node);
        }
    }
}
