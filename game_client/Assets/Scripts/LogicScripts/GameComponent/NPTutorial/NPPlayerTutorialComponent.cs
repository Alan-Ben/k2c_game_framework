using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 玩家的引导数据管理组件
    /// </summary>
    public class NPPlayerTutorialComponent : _ANPRemarkInfoComponent
    {
        private const long c_DefaultTutorialId = 1000;
        
        //最后一个完成的强制引导
        private long _m_lLastFinishForceTutorialId;
        private List<long> _m_lFinishIdList;//已经完成对引导的数据

        //根据edge跳转的跳转节点队列
        private List<NPTutorialNode> _m_lEdgeNodeList;//所有节点列表

        //当前正在执行的引导，同时可以缓存之前需要执行的引导，在进行路径引导之前会设置这个值。在触发的时候出力
        private NPTutorialRef _m_trCurTutorial;

        //非强制引导的数据存储队列
        private List<NPTutorialRef> _m_lUnforcedRefObjList;
        //最后一个完成的强制引导
        private NPTutorialRef _m_trLastDoneForceTutorial;

        //引导触发的索引序列号
        private long _m_lNodeChgTriggerTutorialSerialize;
        //是否当前强制引导全部完成了
        private bool _m_bIsAllForceTutorialDone;


        public NPPlayerTutorialComponent(NPPlayerComponentMgr _compMgr)
            : base(_compMgr, ENPClientDataType.TUTORIAL)
        {
            _m_lLastFinishForceTutorialId = 0;
            _m_lFinishIdList = new List<long>();

            _m_lEdgeNodeList = new List<NPTutorialNode>();
            _m_trCurTutorial = null;
            _m_lNodeChgTriggerTutorialSerialize = -1;

            _m_lUnforcedRefObjList = new List<NPTutorialRef>();
            _m_bIsAllForceTutorialDone = false;
        }

        public override bool isMustInit { get { return true; } }

        protected static ENPPlayerCompType[] _g_DependComp = null;
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.TUTORIAL; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }
        public static NPPlayerTutorialComponent instance { get { return NPPlayer.instance.tutorialComp; } }

        public long nodeChgTriggerTutorialSerialize { get { return _m_lNodeChgTriggerTutorialSerialize; } }
        public bool isAllForceTutorialDone { get { return _m_bIsAllForceTutorialDone; } }

        /// <summary>
        ///  获取当前引导
        /// </summary>
        public NPTutorialRef curTutorial { get { return _m_trCurTutorial; } }
        public long curTutorialId { get { return _m_trCurTutorial != null ? _m_trCurTutorial.id : 0; } }
        public long lastDoneForceTutorialId { get { return _m_trLastDoneForceTutorial != null ? _m_trLastDoneForceTutorial.id : 0; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            SimpleTutorialController.instance.init();
            
            WinMsg.RegisterMsgAct(WinMsgType.TRIGGER_TUTORIAL, _onTriggerTutorial);//触发
            WinMsg.RegisterMsgAct(WinMsgType.TRIGGER_TUTORIAL_START, _onTriggerTutorialStart);//触发引导开始的消息
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerTutorialComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            SimpleTutorialController.instance.discard();

            
            clear();

            WinMsg.UnregisterMsgAct(WinMsgType.TRIGGER_TUTORIAL, _onTriggerTutorial);//触发
            WinMsg.UnregisterMsgAct(WinMsgType.TRIGGER_TUTORIAL_START, _onTriggerTutorialStart);//退出当前引导
        }

        /// <summary>
        /// 初始化，从静态数据表=>nodeList
        /// </summary>
        private void _init()
        {
            if(_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"[Tutorial]_init");

            //初始化数据
            _m_trLastDoneForceTutorial = GRefdataCoreMgr.instance.tutorialRefCore.getRef(_m_lLastFinishForceTutorialId);

            //初始化使用的引导静态表
            _initTutorial();

            //获取当前是否所有强制引导都完成了
            _m_bIsAllForceTutorialDone = _checkAllFouceDone();

            //清空当前引导数据以及边数据
            _m_trCurTutorial = null;
            _m_lEdgeNodeList.Clear();
            NPTutorialEdgeRef tempEdge = null;

            //添加所有边的节点，如果节点不在边上，那么也没有意义
            for(int i = 0; i < GRefdataCoreMgr.instance.tutorialEdgeRefCore.refList.Count; ++i)
            {
                tempEdge = GRefdataCoreMgr.instance.tutorialEdgeRefCore.refList[i];

                //尝试获取或添加开头节点
                NPTutorialNode startNode = _findNode(tempEdge.start_node);
                if(startNode == null)
                {
                    startNode = new NPTutorialNode(tempEdge.start_node);
                    _m_lEdgeNodeList.Add(startNode);
                }

                //尝试获取或添加结尾节点
                NPTutorialNode endNode = _findNode(tempEdge.end_node);
                if(endNode == null)
                {
                    endNode = new NPTutorialNode(tempEdge.end_node);
                    _m_lEdgeNodeList.Add(endNode);
                }

                startNode.EdgeList.Add(new NPTutorialEdge(tempEdge, endNode));
            }

            //如果所有强制任务完成则发送一条消息
            if(_m_bIsAllForceTutorialDone)
                WinMsg.SendMsg(WinMsgType.ON_TUTORIAL_ALL_FORCE_DONE);
        }

        /// <summary>
        /// 初始化使用的引导静态表
        /// </summary>
        private void _initTutorial()
        {
            GRefdataCoreMgr.instance.tutorialRefCore.dealAllRef(
                (NPTutorialRef _refObj) =>
                {
                    if(null == _refObj
                    || _refObj.is_disable   //是否有效
                    || !_refObj.is_unforce) //是否非强制引导
                    return;

                    _m_lUnforcedRefObjList.Add(_refObj);
                });
        }

        /// <summary>
        /// 清空引导部分数据
        /// </summary>
        public void clear()
        {
            if(_m_lEdgeNodeList != null)
                _m_lEdgeNodeList.Clear();

            _m_lLastFinishForceTutorialId = 0;
            _m_lFinishIdList.Clear();

            _m_lUnforcedRefObjList.Clear();

            _m_trCurTutorial = null;
            NPGTutorialController.instance.quitCurTutorial();

            _m_trLastDoneForceTutorial = null;

            _m_lNodeChgTriggerTutorialSerialize = -1;
        }

        #region 数据保存处理部分
        /// <summary>
        /// 构造数据写入存储部分
        /// </summary>
        /// <returns></returns>
        protected override byte[] _makeData()
        {
            RemarkData.TutorialData data = new RemarkData.TutorialData();

            //设置相关数据
            data.setLastFinishForceTutorial(_m_lLastFinishForceTutorialId);
            for(int i = 0; i < _m_lFinishIdList.Count; ++i)
            {
                data.addFinishTutorialId(_m_lFinishIdList[i]);
            }

            return data.makePackage();
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="_data"></param>
        protected override void _readData(byte[] _data)
        {
            _m_lFinishIdList.Clear();
            //若数据为空，不读取
            if(_data != null && _data.Length > 0)
            {

                RemarkData.TutorialData data = new RemarkData.TutorialData();
                data.readPackage(_data);

                _m_lLastFinishForceTutorialId = data.getLastFinishForceTutorial();
                _m_lFinishIdList.AddRange(data.getFinishTutorialId());
            }

            //调用初始化函数
            _init();
        }

        protected override void _resetRemarkInfo()
        {
            RemarkData.TutorialData data = new RemarkData.TutorialData();
            _saveData();
            _readData(data.makePackage());
        }

        /// <summary>
        /// 保存完成引导的数据
        /// </summary>
        /// <param name="_id"></param>
        protected void _setTutorialDone(NPTutorialRef _tutorialRef)
        {
            if(null == _tutorialRef)
                return;

            if(isTutorialDone(_tutorialRef.id))
                return;

            //设置数据
            _m_lFinishIdList.Add(_tutorialRef.id);

            //判断引导是否强制引导，是则调整最后一个完成的强制引导数据
            if(!_tutorialRef.is_unforce)
            {
                _m_lLastFinishForceTutorialId = _tutorialRef.id;
                _m_trLastDoneForceTutorial = _tutorialRef;
            }

            //发送消息刷新自定义加载prefab
            GCommon.reloadCustomLoadPrefab();

            //必然发送消息保存
            _saveData();
        }

        /// <summary>
        /// 设置多个引导完成
        /// </summary>
        /// <param name="_ids"></param>
        protected void _setTutorialDone(List<long> _ids)
        {
            for(int i = 0; i < _ids.Count; ++i)
            {
                long id = _ids[i];

                NPTutorialRef tutorialRef = GRefdataCoreMgr.instance.tutorialRefCore.getRef(id);
                if(null == tutorialRef)
                    continue;

                if(isTutorialDone(tutorialRef.id))
                    continue;

                //设置数据
                _m_lFinishIdList.Add(tutorialRef.id);

                //判断引导是否强制引导，是则调整最后一个完成的强制引导数据
                if(!tutorialRef.is_unforce)
                {
                    _m_lLastFinishForceTutorialId = tutorialRef.id;
                    _m_trLastDoneForceTutorial = tutorialRef;
                }
            }

            //发送消息刷新自定义加载prefab
            GCommon.reloadCustomLoadPrefab();

            _saveData();
        }

        /// <summary>
        /// 是否对应的引导Id是完成的
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public bool isTutorialDone(long _id)
        {
            if(_m_lFinishIdList.IndexOf(_id) < 0)
                return false;

            return true;
        }

        /// <summary>
        /// 重置所有的引导相关数据
        /// </summary>
        protected void _resetAllTutorial()
        {
            if(_m_lFinishIdList.Count == 0)
                return;

            _m_lFinishIdList.Clear();

            //发送消息刷新自定义加载prefab
            GCommon.reloadCustomLoadPrefab();

            _saveData();
        }

        /// <summary>
        /// 重置所有的引导相关数据
        /// </summary>
        protected void _resetAllForceTutorial(bool _isUnforce = false)
        {
            for(int i = 0; i < _m_lFinishIdList.Count; )
            {
                NPTutorialRef tutorialRef = GRefdataCoreMgr.instance.tutorialRefCore.getRef(_m_lFinishIdList[i]);
                if(null == tutorialRef)
                {
                    _m_lFinishIdList.RemoveAt(i);
                    continue;
                }

                if(tutorialRef.is_unforce == _isUnforce)
                {
                    _m_lFinishIdList.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            //发送消息刷新自定义加载prefab
            GCommon.reloadCustomLoadPrefab();

            _saveData();
        }
        #endregion

        /// <summary>
        /// 设置引导完成
        /// </summary>
        public void setTutorialListDone(List<long> _idList)
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【{Time.frameCount}】[Tutorial]setTutorialDone, idlist:   {_idList.ToStringList()}");
            }

            if(null == _idList)
                return;

            //保存数据
            _setTutorialDone(_idList);
            _m_trCurTutorial = null;

            _m_bIsAllForceTutorialDone = _checkAllFouceDone();
            
            //下一帧触发判断引导
            ALCommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                _onTriggerTutorial();
            });

            //如果所有强制任务完成则发送一条单独消息
            if(_m_bIsAllForceTutorialDone)
                WinMsg.SendMsg(WinMsgType.ON_TUTORIAL_ALL_FORCE_DONE);
        }

        /// <summary>
        /// 设置引导完成
        /// </summary>
        public void setTutorialDone(long _id)
        {
            NPTutorialRef refObj = GRefdataCoreMgr.instance.tutorialRefCore.getRef(_id);
            if(refObj == null)
            {
                Debug.LogError("Error, 当前要设置完成的引导不存在, tutorialId:  " + _id);
                ALCommonTaskController.CommonActionAddNextFrameTask(() =>
                {
                    _onTriggerTutorial();
                });
                return;
            }

            setTutorialDone(refObj);
        }
        public void setTutorialDone(List<long> _idList)
        {
            for(int i = 0; i < _idList.Count; i++)
            {
                setTutorialDone(_idList[i]);
            }
        }
        public void setTutorialDone(NPTutorialRef _refObj)
        {
            if(_refObj == null)
            {
                Debug.LogError("Error, 当前要设置完成的引导不存在");
                ALCommonTaskController.CommonActionAddNextFrameTask(() =>
                {
                    _onTriggerTutorial();
                });
                return;
            }

            //保存数据
            _setTutorialDone(_refObj);
            
            //发送引导完成的消息
            WinMsg.SendMsg(WinMsgType.SET_TUTORIAL_DONE, _refObj.id);
            
            //判断设置对象是否当前执行对象。是则设置完成
            if(_m_trCurTutorial == _refObj)
                _m_trCurTutorial = null;

            //如果是强制引导则重新判断是否所有强制引导都完成了
            if(!_refObj.is_unforce)
                _m_bIsAllForceTutorialDone = _checkAllFouceDone();

            ALCommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                //尝试弹出提示，之后再开启引导
                NPUINoticeMgr.instance.dealTryPopNotice(_onTriggerTutorial);
            });

            //如果所有强制任务完成则发送一条消息
            if(_m_bIsAllForceTutorialDone)
                WinMsg.SendMsg(WinMsgType.ON_TUTORIAL_ALL_FORCE_DONE);
        }

        /// <summary>
        /// 检测是否所有强制引导都完成了
        /// </summary>
        /// <returns></returns>
        private bool _checkAllFouceDone()
        {
            if(null == _m_trLastDoneForceTutorial)
                return false;

            //如果找不到下一个强引导则返回true
            if(null == _findCanDoForceTutorial(_m_trLastDoneForceTutorial))
                return true;

            return false;
        }


        /// <summary>
        ///   查找第一个可执行的的引导
        ///   优先取强制引导，再取非强制的
        ///   a. 强制引导是按策划配置的顺序，一个一个执行下取，
        ///   b. 非强制引导的查找，是取查找所有引导里能在当前触发的引导
        /// </summary>
        private NPTutorialRef _findFirstCanDoTutorial()
        {
            //如果没有完成过的最终引导，则返回默认初始引导
            if(null == _m_trLastDoneForceTutorial)
            {
                //如果没有最后完成步骤则表示没有处理过引导，此时使用默认第一个引导处理
                if(isTutorialDone(c_DefaultTutorialId))
                    _m_trLastDoneForceTutorial = GRefdataCoreMgr.instance.tutorialRefCore.getRef(c_DefaultTutorialId);
                else
                {
                    return GRefdataCoreMgr.instance.tutorialRefCore.getRef(c_DefaultTutorialId);
                }
            }

            NPTutorialRef canDoTutorial = null;
            //先查找强制引导
            canDoTutorial = _findCanDoForceTutorial(_m_trLastDoneForceTutorial);

            if (null != canDoTutorial)
            {
                //找到的引导如果要自动完成，则自动完成重新找
                if (null != canDoTutorial.auto_complete_condition && !canDoTutorial.auto_complete_condition.isEmpty && canDoTutorial.auto_complete_condition.IsEnable(null))
                {
                    _setTutorialDone(canDoTutorial);
                    return _findFirstCanDoTutorial();
                }
                return canDoTutorial;
            }

            //没有可执行的强制引导，才去查找非强制引导
            canDoTutorial = _findCanDoUnforcedTutorial();

            return canDoTutorial;
        }

        /// <summary>
        /// 查找当前强制引导, 当前执行到的引导
        ///  强制引导是按策划配置的顺序，一个一个执行下取，
        /// </summary>
        /// <returns></returns>
        private NPTutorialRef _findCanDoForceTutorial(NPTutorialRef _lastDoneTutorial)
        {
            if(null == _lastDoneTutorial)
                return null;

            NPTutorialRef tmpRef = null;
            for(int i = 0; i < _lastDoneTutorial.nextTutorialList.Count; i++)
            {
                tmpRef = _lastDoneTutorial.nextTutorialList[i];
                //无效数据，弱引导都不返回
                if(null == tmpRef || tmpRef.is_disable || tmpRef.is_unforce)
                    continue;

                //判断是否完成，如已经完成则不处理
                if(isTutorialDone(tmpRef.id))
                    continue;

                //如果未完成，则判断条件是否满足
                if(!tmpRef.pre_condition.IsEnable(null))
                    continue;

                //如都通过则返回本引导作为当前引导
                return tmpRef;
            }

            return null;
        }

        /// <summary>
        /// 查找当前非强制引导里，当前可触发的引导(第一个)
        /// </summary>
        /// <returns></returns>
        private NPTutorialRef _findCanDoUnforcedTutorial()
        {
            string now = QueueMgr.instance._lastNode?.nodeTag;
            //当前位置不是一个有效的引导节点视图
            if(String.IsNullOrEmpty(now))
            {
                return null;
            }

            NPTutorialRef tmpRef = null;
            for(int i = 0; i < _m_lUnforcedRefObjList.Count; i++)
            {
                tmpRef = _m_lUnforcedRefObjList[i];
                if(null == tmpRef)
                    continue;

                //失效的引导
                if(tmpRef.is_disable)
                    continue;
                //优先判断已经完成的引导
                if(isTutorialDone(tmpRef.id))
                    continue;
                //判断节点是否一致
                if(!string.Equals(tmpRef.end_node, now, StringComparison.OrdinalIgnoreCase))
                    continue;
                //前置条件不满足
                if(!tmpRef.pre_condition.IsEnable(null))
                    continue;

                return tmpRef;
            }
            return null;
        }


        /// <summary>
        /// 查询对应的节点
        /// </summary>
        /// <param name="_nodeUIView"></param>
        private NPTutorialNode _findNode(string _nodeUIView)
        {
            NPTutorialNode tempNode = null;
            for(int i = 0; i < _m_lEdgeNodeList.Count; ++i)
            {
                tempNode = _m_lEdgeNodeList[i];
                if(tempNode.NodeUIView == _nodeUIView)
                {
                    return tempNode;
                }
            }
            return null;
        }


        /// <summary>
        /// 传入起点和终点，查询查询对应路径列表
        /// </summary>
        /// <param name="_start">查询的起始视图</param>
        /// <param name="_end">查询的终点视图</param>
        /// <returns></returns>
        private List<NPTutorialEdge> _findPath(string _start, string _end)
        {
            //遍历所有点设置权重
            for(int i = 0; i < _m_lEdgeNodeList.Count; i++)
            {
                _m_lEdgeNodeList[i].resetCalInfo();
            }

            List<NPTutorialNode> needFindNodeList = new List<NPTutorialNode>();
            NPTutorialNode tmpNode = null;

            //加入初始点
            tmpNode = _findNode(_start);
            if(null == tmpNode)
                return null;
            tmpNode.setLen(null, 0);
            needFindNodeList.Add(tmpNode);

            int idx = 0;
            NPTutorialNode finalNode = null;
            while(idx < needFindNodeList.Count)
            {
                //使用当前位置节点计算
                tmpNode = needFindNodeList[idx];
                if(null == tmpNode)
                    continue;

                //遍历目标线段处理
                NPTutorialEdge tmpEdge = null;
                for(int i = 0; i < tmpNode.EdgeList.Count; i++)
                {
                    tmpEdge = tmpNode.EdgeList[i];
                    if(null == tmpEdge)
                        continue;

                    //判断边是否有效
                    if(tmpEdge.targetNode.curLen >= 0)
                        continue;

                    //设置长度
                    tmpEdge.targetNode.setLen(tmpNode, tmpNode.curLen + 1);

                    //判断是否到达目标点
                    if(tmpEdge.EdgeRef.end_node == _end)
                    {
                        //此时到达终点
                        finalNode = tmpNode;
                        break;
                    }

                    //添加到队列
                    needFindNodeList.Add(tmpEdge.targetNode);
                }

                if(null != finalNode)
                    break;

                idx++;
            }

            //判断是否有结果
            if(null == finalNode)
                return null;

            //创建结果
            List<NPTutorialEdge> resList = new List<NPTutorialEdge>();
            //查询结尾节点
            tmpNode = finalNode;
            //查询的目标节点
            string tmpTargetNodeUIView = _end;
            //逆向查询
            while(tmpNode != null && (tmpNode.curLen >= 0 || null == tmpNode.tmpSrcNode))
            {
                //查询当前线段信息
                resList.Insert(0, tmpNode.findEdge(tmpTargetNodeUIView));

                //查询上一段信息
                tmpTargetNodeUIView = tmpNode.NodeUIView;
                tmpNode = tmpNode.tmpSrcNode;
            }
            return resList;
        }

        //触发引导
        private void _onTriggerTutorial()
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【{Time.frameCount}】[Tutorial]_onTriggerTutorial");
            }

            //在引导中不重复触发
            if(Game.instance.isInTutorial)
                return;

            //当前已经有引导在进行, 需要再判断一下是否满足条件，满足条件则继续执行
            if(_m_trCurTutorial != null)
            {
                if(_AALMonoMain.instance.showDebugOutput)
                {
                    Debug.Log($"【{Time.frameCount}】[Tutorial] _onTriggerTutorial 当前已经有引导在进行，忽略此次触发, 继续执行。 tutorialId: {_m_trCurTutorial.id}");
                }

                //判断是否满足条件，如不满足条件则不执行引导
                if (!_m_trCurTutorial.pre_condition.IsEnable(null))
                {
                    _m_trCurTutorial = null;
                    //这里不return需要继续执行
                }
                else
                {
                    _doTutorial(_m_trCurTutorial);
                    //此处需要打断执行过程，直接return
                    return;
                }
            }

            //取未完成的引导
            _m_trCurTutorial = _findFirstCanDoTutorial();
            if(null != _m_trCurTutorial)
                _doTutorial(_m_trCurTutorial);
        }

        /// <summary>
        /// 处理对应的引导，判断当前位置节点与触发节点，如不在一个节点下，将判断是否存在路径切换。
        /// 可以切换将会逐个路径进行处理
        /// </summary>
        /// <param name="_tutorialRef"></param>
        private void _doTutorial(NPTutorialRef _tutorialRef)
        {
            if(_tutorialRef == null)
                return;

            if (_tutorialRef.id == c_DefaultTutorialId)
            {
                //发送埋点-首次触发引导1000
                GCommon.sendStepReport(TraceConst.FIRST_TRIGGER_TUTORIAL);
            }
            
            if(_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【{Time.frameCount}】[Tutorial]_doTutorial,  tutorialId:   {_tutorialRef.id}");
            }

            //当前位置的节点
            string now = QueueMgr.instance._lastNode?.nodeTag;
            //当前位置不是一个有效的引导节点视图
            if(now == String.Empty)
            {
                if(_AALMonoMain.instance.showDebugOutput)
                {
                    Debug.Log($"【{Time.frameCount}】[Tutorial]Error!!!,  cur ENodeUIView:   {now}");
                }
                return;
            }

            //当前位置就在触发引导的位置，直接触发引导
            if(string.Equals(_tutorialRef.end_node, now, StringComparison.OrdinalIgnoreCase))
            {
                //强制重置简单引导
                SimpleTutorialController.instance.forceResetGuide();
                //开始强制引导
                NPGTutorialController.instance.showTutorial(UIResPathAssistant.getAssetInfo(_tutorialRef.tutorial_asset_id),
                    (_showSucc) =>
                    {
                        // 若展示失败，强制设置引导完成，避免玩家无法继续触发其他引导
                        if(!_showSucc)
                            setTutorialDone(_tutorialRef);
                    });
                return;
            }

            //和tony确认过,非强制引导不查找有向边
            if (_tutorialRef.is_unforce)
                return;

            //强制引导继续查找是否存在有向边，查找最短的路径，
            //不再触发引导的位置，查找最短路径，一步一步走向目标位置
            List<NPTutorialEdge> path = _findPath(now, _tutorialRef.end_node);
            if(null == path || path.Count == 0)
            {
                if(_AALMonoMain.instance.showDebugOutput)
                {
                    Debug.Log($"【{Time.frameCount}】[Tutorial]Error!!!,  Can not find path from:    {now}  to  { _tutorialRef.end_node}, curTutorialId:  {_tutorialRef.id}");
                }
                return;
            }

            if(_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【{Time.frameCount}】[Tutorial] 当前不在触发引导的位置,查找到最短路径,。{path[0].EdgeRef.res_path_id}");
            }
            
            NPGTutorialController.instance.showTutorial(UIResPathAssistant.getAssetInfo(path[0].EdgeRef.res_path_id), null);
        }

        /// <summary>
        /// 触发引导开始的处理函数，记录当前的node节点变动序列号
        /// </summary>
        private void _onTriggerTutorialStart()
        {
            _m_lNodeChgTriggerTutorialSerialize = QueueMgr.instance.nodeChgSerialize;
        }

        #region cheat_作弊命令处理
        /// <summary>
        /// 强制设置当前引导完成
        /// </summary>
        public void forceSetCurTutorialDone()
        {
            //当前不在引导则不处理
            if (null == _m_trCurTutorial)
                return;

            //设置引导完成
            setTutorialDone(_m_trCurTutorial);
            _m_trCurTutorial = null;

            //强退,保证引导窗口都关闭
            NPGTutorialController.instance.quitCurTutorial();
        }

        /// <summary>
        /// 强制设置所有引导完成
        /// </summary>
        public void forceSetAllDone()
        {
            // 遍历所有引导数据设置完成
            GRefdataCoreMgr.instance.tutorialRefCore.dealAllRef(
                (NPTutorialRef _refObj) =>
                {
                    if(null == _refObj || _refObj.is_disable)
                        return;

                    setTutorialDone(_refObj);
                });

            _m_trCurTutorial = null;

            //强退,保证引导窗口都关闭
            NPGTutorialController.instance.quitCurTutorial();
        }

        /// <summary>
        /// 强制所有引导完成
        /// </summary>
        public void forceSetAllForceDone()
        {
            List<long> forceIdList = new List<long>();
            // 遍历所有引导数据设置完成
            GRefdataCoreMgr.instance.tutorialRefCore.dealAllRef(
                (NPTutorialRef _refObj) =>
                {
                    if(null == _refObj || _refObj.is_disable || _refObj.is_unforce)
                        return;

                    forceIdList.Add(_refObj.id);
                });

            //设置所有完成
            setTutorialListDone(forceIdList);

            _m_trCurTutorial = null;

            //强退,保证引导窗口都关闭
            NPGTutorialController.instance.quitCurTutorial();
        }

        //重置所有已完成的引导
        public void resetAllForce()
        {
            _onResetAllForce();

            //重新触发引导
            if(!Game.instance.isInTutorial)
                _onTriggerTutorial();
        }

        //强制引导从某个id开始
        public void forceForceTutorialStartWith(long _tutorialId)
        {
            //先验证下输入的ID是否有效
            NPTutorialRef tutorialRef = GRefdataCoreMgr.instance.tutorialRefCore.getRef(_tutorialId);
            if(tutorialRef == null || tutorialRef.is_disable)
            {
#if UNITY_EDITOR
                Debug.LogError("Error! 引导ID无效.  _tutorialId： " + _tutorialId);
#endif
                return;
            }

            //设置引导对象
            _m_trCurTutorial = tutorialRef;
            //直接处理引导
            _doTutorial(_m_trCurTutorial);
        }

        public void forceSetAllUnforcedDone()
        {
            List<long> tmpIdList = _m_lUnforcedRefObjList.ConvertAll((_refObj) => _refObj.id);
            _setTutorialDone(tmpIdList);
            _m_trCurTutorial = null;

            NPGTutorialController.instance.quitCurTutorial();

        }

        //重置所有已完成的引导
        public void resetAllUnlforcedForce()
        {
            _onResetAllUnForced();

            //重新触发引导
            if(!Game.instance.isInTutorial)
                _onTriggerTutorial();
        }

        //重置所有强制引导
        private void _onResetAllForce()
        {
            //重置所有引导数据
            _resetAllForceTutorial();

            _m_bIsAllForceTutorialDone = false;
            _m_trCurTutorial = null;

            _m_lLastFinishForceTutorialId = 0;
            _m_trLastDoneForceTutorial = null;

            NPGTutorialController.instance.quitCurTutorial();
        }


        //退出引导
        private void _onQuitCurrentTutorial()
        {
            if(_m_trCurTutorial == null)
                return;

            if(!Game.instance.isInTutorial)
                return;

            _m_trCurTutorial = null;

            NPGTutorialController.instance.quitCurTutorial();
        }

        //重置所有引导
        private void _onResetAllUnForced()
        {
            //重置所有引导数据
            _resetAllForceTutorial(true);

            _m_trCurTutorial = null;

            NPGTutorialController.instance.quitCurTutorial();
        }

#if UNITY_EDITOR
        //重置强制引导从某一步开始
        public void resetForceTutorialId(long _startId)
        {
            _m_lLastFinishForceTutorialId = _startId;
            _m_trLastDoneForceTutorial = GRefdataCoreMgr.instance.tutorialRefCore.getRef(_startId);
            _m_bIsAllForceTutorialDone = false;
            _m_trCurTutorial = null;
            
            for(int i = _m_lFinishIdList.Count - 1; i >= 0; i--)
            {
                NPTutorialRef tutorialRef = GRefdataCoreMgr.instance.tutorialRefCore.getRef(_m_lFinishIdList[i]);
                if(null == tutorialRef)
                {
                    _m_lFinishIdList.RemoveAt(i);
                }

                //强制引导，并且id大于参数id，都删掉，editor下骇客处理
                if (tutorialRef != null && !tutorialRef.is_unforce && tutorialRef.id > _startId)
                {
                    _m_lFinishIdList.RemoveAt(i);
                }
            }
            
            //必然发送消息保存
            _saveData();
        }  
        
        //重置非强制引导
        public void resetUnforcedTutorialId(long _startId)
        {
            NPTutorialRef tutorialRef = GRefdataCoreMgr.instance.tutorialRefCore.getRef(_startId);
            if(tutorialRef == null || tutorialRef.is_disable || !tutorialRef.is_unforce)
            {
                return;
            }
            
            if (_m_lFinishIdList != null) 
                _m_lFinishIdList.Remove(_startId);
            //必然发送消息保存
            _saveData();
        }
#endif
        
        #endregion
    }
}

