using System.Collections.Generic;
using ALPackage;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Object = System.Object;

namespace ALPackage
{
    public enum EALQueueMgrState
    {
        NONE,       //无
        IDLE,       //空闲
        DEALING,    //处理中
        DEALING_CACHE,      //在处理缓存中
    }


    /// <summary>
    /// UI队列管理器
    /// </summary>
    public abstract class _AALQueueMgr<T> where T : _AALQueueBaseNode
    {
        private long _m_lRollBackSwitch = 0;
        private long _m_lNodeChgSerialie = 1;

        private List<T> _m_NodeList = new List<T>();

        /// <summary>
        /// 当前队列管理器所处状态
        /// </summary>
        private EALQueueMgrState _m_eMgrState = EALQueueMgrState.IDLE;
        //当前管理器状态的进入时间，避免出现逻辑问题导致所有node处理无法跳转
        private long _m_lCurMgrStateTimeTagMS = 0;
        //10秒内状态有效，否则回复idle
        private const long _c_lStateTimeEnableMS = 10000;
        /// <summary>
        /// 是否有节点变更发生，如发生将会触发对应的节点变更函数
        /// </summary>
        private bool _m_bIsNodeChg = false;
        /// <summary>
        /// 延迟处理的相关事务转换的任务对象列表
        /// </summary>
        private List<ALCommonActionMonoTask> _m_lLaterDealTask = new List<ALCommonActionMonoTask>();

#if UNITY_EDITOR
        private ALCommonTaskMonitorContainer _m_tmcTaskMonitor = new ALCommonTaskMonitorContainer();
#endif

        //获取最后一个节点
        public T _lastNode { get { return _m_NodeList.Count == 0 ? null : _m_NodeList[_m_NodeList.Count - 1]; } }
        //检测最后一个有效的Node（会自动过滤无效Node。无效定义为：isEnable = true, needAutoRemove = false）
        public T _lastEnableNode 
        { 
            get 
            {
                //队列为空直接返回
                if(_m_NodeList.Count == 0)
                    return null;

                int i = _m_NodeList.Count - 1;
                T tmpNode = _m_NodeList[i];
                while(null != tmpNode)
                {
                    if (tmpNode.isEnable && !tmpNode.NeedAutoRemove)
                        return tmpNode;

                    //累加编号
                    i--;
                    if (i < 0)
                        return null;

                    //获取下一个节点
                    tmpNode = _m_NodeList[i];
                }

                return null;
            } 
        }
        public long nodeChgSerialize { get { return _m_lNodeChgSerialie; } }

        /// <summary>
        /// 尝试获取最后一个主节点
        /// </summary>
        /// <returns></returns>
        public T tryGetLastMainNode()
        {
            if (null == _m_NodeList || _m_NodeList.Count <= 0)
                return null;

            T tmpNode = null;
            //循环处理无效或需要自动退出的节点
            for (int i = _m_NodeList.Count - 1; i >= 0; i--)
            {
                tmpNode = _m_NodeList[i];
                if (null == tmpNode)
                    continue;

                //主节点且需要退出则全退出
                if (tmpNode.IsMainViewNode)
                {
                    return tmpNode;
                }
            }

            //返回空
            return null;
        }

        /// <summary>
        /// 尝试设置处理状态，返回是否设置成功
        /// </summary>
        /// <param name="_state"></param>
        public bool tryDealMgr()
        {
            //检测状态
            _checkMgrState();

            if(_m_eMgrState != EALQueueMgrState.IDLE)
                return false;

            //设置为处理中状态
            _setMgrState(EALQueueMgrState.DEALING);

            return true;
        }

        /// <summary>
        /// 清除所有需要等待恢复idle再处理的node节点，一般在需要打断正常跳转的时候使用本函数
        /// </summary>
        public void clearLaterDealNodeTask()
        {
            //此时直接清理所有待处理node，避免出现异常展示情况
            for(int i = 0; i < _m_lLaterDealTask.Count; i++)
            {
                ALCommonActionMonoTask.ALCommonActionTaskCache.instance.pushBackCacheItem(_m_lLaterDealTask[i]);
            }
            _m_lLaterDealTask.Clear();
        }

        /// <summary>
        /// 根据是否已经退出了主视图，判断是否需要进行视图刷新行为，或直接恢复idle状态
        /// </summary>
        /// <param name="_isQuitMainView"></param>
        protected void _checkAndResumeIdleState(bool _isQuitMainView)
        {
            //此时如果已经退出了主视图，这里需要进行进入视图处理
            if (_isQuitMainView)
            {
                _enterLastNeedEnterNode(_resumeIdleState);
            }
            else
            {
                //设置恢复idle状态
                _resumeIdleState();
            }
        }
        /// <summary>
        /// 在完成操作时统一恢复idle的状态
        /// </summary>
        private void _resumeIdleState()
        {
            //设置恢复idle状态
            _setMgrState(EALQueueMgrState.IDLE);
        }

        /// <summary>
        /// 设置管理器状态
        /// </summary>
        /// <param name="_state"></param>
        private void _setMgrState(EALQueueMgrState _state)
        {
            if(_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]setMgrState: 设置管理器状态.   {_state}");
            }
            _m_eMgrState = _state;
            _m_lCurMgrStateTimeTagMS = ALCommon.getNowTimeMill();

            if(_m_eMgrState == EALQueueMgrState.IDLE)
            {
                //判断是否有需要处理的任务，如有则设置状态并延迟一帧处理检测
                if (_m_lLaterDealTask.Count > 0)
                {
                    _m_eMgrState = EALQueueMgrState.DEALING_CACHE;
                    //延迟处理
                    ALCommonTaskController.CommonActionAddMonoTask(_checkAndDealLaterDealTask);
                }
                else
                {
                    //此时尝试触发node修改的处理函数
                    if(_m_bIsNodeChg)
                    {
                        _m_bIsNodeChg = false;
                        //增加节点修改序列号
                        _m_lNodeChgSerialie++;
                        //调用节点变更处理函数
                        _onNodeChg();
                    }
                }
            }
        }

        /// <summary>
        /// 检测当前管理器状态
        /// </summary>
        protected void _checkMgrState()
        {
            if(_m_eMgrState == EALQueueMgrState.IDLE)
                return;

            long nowTimeMS = ALCommon.getNowTimeMill();
            if(nowTimeMS <= _m_lCurMgrStateTimeTagMS + _c_lStateTimeEnableMS)
                return;

#if UNITY_EDITOR
            ALLog.Error("check Mgr State timeout!");
#endif

            //此时直接清理所有待处理node，避免出现异常展示情况
            for(int i = 0; i < _m_lLaterDealTask.Count; i++)
            {
                ALCommonActionMonoTask.ALCommonActionTaskCache.instance.pushBackCacheItem(_m_lLaterDealTask[i]);
            }
            _m_lLaterDealTask.Clear();

            _m_eMgrState = EALQueueMgrState.IDLE;
        }

        /// <summary>
        /// 检测状态并执行下一个任务状态
        /// </summary>
        protected void _checkAndDealLaterDealTask()
        {
            //检测状态
            _checkMgrState();

            //如果非处理缓存任务状态则不进行处理
            if (_m_eMgrState != EALQueueMgrState.DEALING_CACHE)
                return;

            //此时一定是idle状态取出一个任务进行处理
            if(_m_lLaterDealTask.Count > 0)
            {
                //如果还有任务此时需要设置为缓存处理中，避免在本贞还有其他任务加入
                ALCommonActionMonoTask task = _m_lLaterDealTask[0];
                _m_lLaterDealTask.RemoveAt(0);

                //执行任务
                ALMonoTaskMgr.instance.addMonoTask(task);
            }
            else
            {
                //此时无任务需要重置状态
                _m_eMgrState = EALQueueMgrState.IDLE;
            }
        }

        /// <summary>
        /// 如在处理缓存任务时，在执行前需要重置状态
        /// </summary>
        protected void _preDealCacheDealFunc()
        {
            //如果在处理缓存中，此时需要重置状态，然后续处理正常
            if (_m_eMgrState == EALQueueMgrState.DEALING_CACHE)
                _m_eMgrState = EALQueueMgrState.IDLE;
        }

        /// <summary>
        /// 关闭当前节点，然后进入一个新节点的处理
        /// </summary>
        /// <param name="_newNode">新节点</param>
        /// <param name="_checkLastNode">是否检查当前节点</param>
        public void quitCurAndAddNode(T _newNode, T _checkLastNode = null)
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]quitCurAndAddNode: 关闭当前节点，然后进入一个新节点的处理.   {_newNode}, {_checkLastNode}");
            }
            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            quitCurAndAddNode(_newNode, _checkLastNode);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //执行最后一个节点的退出,并从列表中删除
            T tmpNode = null;
            //获取最后一个节点，当带入的检测节点不符的时候不允许退出
            tmpNode = _lastNode;
            if(null == tmpNode
                || (null != _checkLastNode && _checkLastNode != tmpNode))
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);
                return;
            }

            //根据最后一个节点是否主视图，调整是否退出了主视图的变量
            isQuitMainView = isQuitMainView || tmpNode.IsMainViewNode;

            //移除最后一个
            _removeLastNode();

            //处理关闭退出
            _dealNodeCloseQuit(tmpNode
                , ()=>
                {
                    //设置恢复idle状态，这里需要先处理是因为后面的addnode是一个对外流程接口
                    _checkAndResumeIdleState(isQuitMainView);

                    if (_newNode != null)
                    {
                        //添加下一个节点
                        AddNode(_newNode);
                    }
                });
        }
        /// <summary>
        /// 进入某个节点
        /// </summary>
        /// <param name="_node"></param>
        public void AddNode(T _node)
        {
            if(_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]AddNode: 进入某个节点.   {_node}");
            }
            if (_node == null)
                return;

            //当添加node的时候，如果不允许添加同类型节点的话，最后一个节点是同类型则不可添加
            if(!_node.canAddWhenLastNodeIsTheSameType
                && null != _lastNode && _lastNode.GetType() == _node.GetType())
                return;

            //尝试进入处理状态，如无法进入则等待处理
            if(!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            AddNode(_node);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //判断是否要退出自动退出的节点
            bool isAutoQuitMainView = false;
            if (_node.NeedRemovePreAutoRemove)
            {
                _quitAllAutoRemoveNodes(out isAutoQuitMainView);
            }

            //创建计步器，先留一步确保不会出现提前完成
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);

            //判断新节点是否需要进入前节点，如是则需要寻找上一个主视图节点，并将之后的节点都退出
            if (_node.IsMainViewNode)
            {
                if (isQuitMainView || isAutoQuitMainView)
                {
                    //如果已经退出了某个主视图节点,此时这里不需要做额外处理
                }
                else
                {
                    stepCounter.chgTotalStepCount(1);
                    //临时寻找并退出上一个主视图节点，相关节点还在队列中
                    _tmpQuitLastMainViewNodes(stepCounter.addDoneStepCount);
                }
            }
            else
            {
                //如果新节点不是主视图节点，而已经退出了某个主视图节点,此时需要先刷新所有视图
                if (isQuitMainView || isAutoQuitMainView)
                {
                    stepCounter.chgTotalStepCount(1);
                    _enterLastNeedEnterNode(stepCounter.addDoneStepCount);
                }
            }

            //此时注册全部调用完后的处理
            stepCounter.regAllDoneDelegate(() =>
            {
                //判断是否根节点，且必须是主节点才会退出所有前面的节点
                if(_node.isRootNode && _node.IsMainViewNode)
                {
                    //队列每个节点处理退出操作
                    T tmpNode = null;
                    for(int i = _m_NodeList.Count - 1; i >= 0; i--)
                    {
                        //逐个释放
                        tmpNode = _m_NodeList[i];
                        if (null == tmpNode)
                            continue;

                        //调用关闭函数
                        _dealSysCloseNode(tmpNode);
                    }

                    //清空队列
                    _m_NodeList.Clear();
                }
                //加入队列
                _m_NodeList.Add(_node);

                //处理加入队列的操作
                if(null != _node)
                    _node.onEnterQueue();

                //进入节点
                _dealNodeEnter(_node
                    , _resumeIdleState);
            });

            //累加预留步骤
            stepCounter.addDoneStepCount();
        }

        /// <summary>
        /// 从队列尾部退出对应stageType的所有节点
        /// </summary>
        /// <param name="_stageType"></param>
        /// <param name="_needResumeDefaultNode"></param>
        public void quitStage(int _stageType)
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]quitStage: 从队列尾部退出对应stageType的所有节点.  _stageType: {_stageType}");
            }
            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            quitStage(_stageType);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //创建计步器，先留一步确保不会出现提前完成
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);

            T tmpNode = _lastNode;
            bool needEnter = isQuitMainView;
            bool hasQuit = false;
            //循环处理无效或需要自动退出的节点
            while(null != tmpNode)
            {
                if(!tmpNode.isEnable
                    || tmpNode.NeedAutoRemove
                    || tmpNode.stageType == _stageType)
                {
                    stepCounter.chgTotalStepCount(1);

                    //从队列删除
                    _removeLastNode();

                    //处理关闭退出
                    _dealNodeCloseQuit(tmpNode, stepCounter.addDoneStepCount);

                    //设置需要进入的节点
                    if(!needEnter)
                        needEnter = tmpNode.IsMainViewNode;

                    hasQuit = true;
                }
                else
                {
                    break;
                }

                tmpNode = _lastNode;
            }

            //判断是否有倒退节点
            if(!needEnter)
            {
                //触发引导检查效果，只有在有退出且没进入其他节点的时候才触发
                if(hasQuit)
                {
                    //在没有处理主节点切换时触发的事件函数
                    _onNotMainNodeChg();
                }
                //重置步骤信息
                stepCounter.resetAll();
                //设置恢复idle状态
                _resumeIdleState();

                return;
            }

            //此时注册全部调用完后的处理
            stepCounter.regAllDoneDelegate(() =>
            {
                //进入上一个节点
                _enterLastNeedEnterNode(_resumeIdleState);
            });

            //累加预留步骤
            stepCounter.addDoneStepCount();
        }

        /// <summary>
        /// 退出到允许停留的主节点
        /// </summary>
        public void QuitUntilCanStopMainViewNode(Func<T, bool> _canStopByNode)
        {
            QuitUntilCanStopMainViewNode(_canStopByNode, false);
        }
        public void QuitUntilCanStopMainViewNode(Func<T, bool> _canStopByNode, bool _quitLastNode)
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]QuitUntilCanStopMainViewNode: 退出到允许停留的主节点. ");
            }
            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            QuitUntilCanStopMainViewNode(_canStopByNode, _quitLastNode);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //创建计步器，先留一步确保不会出现提前完成
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);

            T tmpNode;
            bool needEnter = isQuitMainView;
            bool hasFix = false;//是否匹配
            //循环处理无效或需要自动退出的节点
            for(int i = _m_NodeList.Count - 1; i >= 0; i--)
            {
                tmpNode = _m_NodeList[i];
                if(null == tmpNode)
                    continue;

                //主节点且需要退出则全退出
                if(tmpNode.IsMainViewNode)
                {
                    //符合条件则直接退出循环
                    if(_canStopByNode(tmpNode))
                    {
                        //如果不是匹配节点也要推出则此处退出循环
                        if (!_quitLastNode)
                            break;

                        //设置匹配
                        hasFix = true;
                    }

                    T delNode = null;
                    for(int n = _m_NodeList.Count - 1; n >= i; n--)
                    {
                        stepCounter.chgTotalStepCount(1);

                        delNode = _m_NodeList[n];
                        _m_NodeList.RemoveAt(n);

                        //处理关闭退出
                        _dealNodeCloseQuit(delNode, stepCounter.addDoneStepCount);
                    }

                    //设置需要进入的节点
                    needEnter = true;

                    //如果已经匹配则退出
                    if (hasFix)
                        break;
                }
            }

            //判断是否有倒退节点
            if(!needEnter)
            {
                //在没有处理主节点切换时触发的事件函数
                _onNotMainNodeChg();
                //重置步骤信息
                stepCounter.resetAll();
                //设置恢复idle状态
                _resumeIdleState();

                return;
            }

            //此时注册全部调用完后的处理
            stepCounter.regAllDoneDelegate(() =>
            {
                //进入上一个节点
                _enterLastNeedEnterNode(_resumeIdleState);
            });

            //累加预留步骤
            stepCounter.addDoneStepCount();
        }

        /// <summary>
        /// 退出到允许停留的任意节点
        /// </summary>
        public void QuitUntilCanStop(Func<T, bool> _canStopByNode)
        {
            QuitUntilCanStop(_canStopByNode, false);
        }
        public void QuitUntilCanStop(Func<T, bool> _canStopByNode, bool _quitLastNode)
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]QuitUntilCanStop: 退出到允许停留的任意节点. ");
            }
            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            QuitUntilCanStop(_canStopByNode, _quitLastNode);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //创建计步器，先留一步确保不会出现提前完成
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);

            T tmpNode = _lastNode;
            bool needEnter = isQuitMainView;
            bool hasQuit = false;
            bool hasFix = false;//是否匹配
            //循环处理无效或需要自动退出的节点
            while (null != tmpNode)
            {
                //符合条件则直接退出循环
                if (_canStopByNode(tmpNode))
                {
                    //如果不是匹配节点也要推出则此处退出循环
                    if (!_quitLastNode)
                        break;

                    //设置匹配
                    hasFix = true;
                }

                stepCounter.chgTotalStepCount(1);

                //从队列删除
                _removeLastNode();

                //处理关闭退出
                _dealNodeCloseQuit(tmpNode, stepCounter.addDoneStepCount);

                //设置需要进入的节点
                if (!needEnter)
                    needEnter = tmpNode.IsMainViewNode;

                hasQuit = true;

                tmpNode = _lastNode;

                //如果已经匹配则退出
                if (hasFix)
                    break;
            }

            //判断是否有倒退节点
            if(!needEnter)
            {
                //触发引导检查效果，只有在有退出且没进入其他节点的时候才触发
                if(hasQuit)
                {
                    //在没有处理主节点切换时触发的事件函数
                    _onNotMainNodeChg();
                }
                //重置步骤信息
                stepCounter.resetAll();
                //设置恢复idle状态
                _resumeIdleState();

                return;
            }

            //此时注册全部调用完后的处理
            stepCounter.regAllDoneDelegate(() =>
            {
                //进入上一个节点
                _enterLastNeedEnterNode(_resumeIdleState);
            });

            //累加预留步骤
            stepCounter.addDoneStepCount();
        }

        /// <summary>
        /// 单步回退操作的处理
        /// </summary>
        public void DoUIRollBackByEsc()
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]DoUIRollBackByEsc: 单步回退操作的处理. {canRollBack()}");
            }
            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            DoUIRollBackByEsc();
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //回退开关
            if(!canRollBack())
            {
                //设置恢复idle状态
                _resumeIdleState();

                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //执行最后一个节点的退出,并从列表中删除
            bool isDoLastNodeEnter = true;
            T tmpNode = null;

            //获取最后一个节点
            tmpNode = _lastNode;
            if(null == tmpNode)
            {
                //设置为主界面节点处理
                _onError();

                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //不能退出的节点则不处理
            if(!tmpNode.IsCanRollBackQuit) //这种节点判断为不能退出
            {
                //处理无法回退的操作
                tmpNode.onCannotEscBack();

                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //移除最后一个
            _removeLastNode();

            //设置最后一个节点的前一个节点是否需要进入
            isDoLastNodeEnter = isQuitMainView || tmpNode.IsMainViewNode;

            //处理rollback退出
            _dealNodeRollBackQuit(tmpNode
                , ()=>
                {
                    //判断是否需要执行节点的enter
                    if(isDoLastNodeEnter)
                    {
                        _enterLastNeedEnterNode(_resumeIdleState);
                    }
                    else
                    {
                        //在没有处理主节点切换时触发的事件函数
                        _onNotMainNodeChg();

                        //设置恢复idle状态
                        _resumeIdleState();
                    }
                });
        }
        /// <summary>
        /// 检查最后一个节点是否对应对象，是则使用常规esc退出操作
        /// </summary>
        /// <param name="_checkLastNode"></param>
        public void DoUIRollBackByEscByCheckLastObj(T _checkLastNode)
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]DoUIRollBackByEscByCheckLastObj: 检查最后一个节点是否对应对象，是则使用常规esc退出操作.  {_checkLastNode}, {canRollBack()}");
            }
            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            DoUIRollBackByEscByCheckLastObj(_checkLastNode);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //回退开关
            if (!canRollBack())
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //执行最后一个节点的退出,并从列表中删除
            bool isDoLastNodeEnter = true;
            T tmpNode = null;

            //获取最后一个节点
            tmpNode = _lastNode;
            if (null == tmpNode
                || _checkLastNode != tmpNode)
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //不能退出的节点则不处理
            if (!tmpNode.IsCanRollBackQuit) //这种节点判断为不能退出
            {
                //处理无法回退的操作
                tmpNode.onCannotEscBack();
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //移除最后一个
            _removeLastNode();

            //设置最后一个节点的前一个节点是否需要进入
            isDoLastNodeEnter = isQuitMainView || tmpNode.IsMainViewNode;

            //处理rollback退出
            _dealNodeRollBackQuit(tmpNode
                , ()=>
                {
                    //判断是否需要执行节点的enter
                    if(isDoLastNodeEnter)
                    {
                        _enterLastNeedEnterNode(_resumeIdleState);
                    }
                    else
                    {
                        //在没有处理主节点切换时触发的事件函数
                        _onNotMainNodeChg();

                        //设置恢复idle状态
                        _resumeIdleState();
                    }
                });
        }

        /// <summary>
        /// 检查最后一个节点是否对应类型，是则使用常规esc退出操作
        /// </summary>
        /// <param name="_lastNodeType"></param>
        public void DoUIRollBackByEscByCheckLastType(Type _lastNodeType)
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]DoUIRollBackByEscByCheckLastType: 检查最后一个节点是否对应类型，是则使用常规esc退出操作.  {_lastNodeType.Name}, {canRollBack()}");
            }
            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            DoUIRollBackByEscByCheckLastType(_lastNodeType);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //回退开关
            if (!canRollBack())
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //执行最后一个节点的退出,并从列表中删除
            bool isDoLastNodeEnter = true;
            T tmpNode = null;

            //获取最后一个节点
            tmpNode = _lastNode;
            if(null == tmpNode
                || _lastNodeType != tmpNode.GetType())
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //不能退出的节点则不处理
            if(!tmpNode.IsCanRollBackQuit) //这种节点判断为不能退出
            {
                //处理无法回退的操作
                tmpNode.onCannotEscBack();
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //移除最后一个
            _removeLastNode();

            //设置最后一个节点的前一个节点是否需要进入
            isDoLastNodeEnter = isQuitMainView || tmpNode.IsMainViewNode;

            //处理rollback退出
            _dealNodeRollBackQuit(tmpNode
                , ()=>
                {
                    //判断是否需要执行节点的enter
                    if(isDoLastNodeEnter)
                    {
                        _enterLastNeedEnterNode(_resumeIdleState);
                    }
                    else
                    {
                        //在没有处理主节点切换时触发的事件函数
                        _onNotMainNodeChg();

                        //设置恢复idle状态
                        _resumeIdleState();
                    }
                });
        }

        /// <summary>
        /// 查询到最后一个对应类型的节点，退出该节点
        /// </summary>
        /// <param name="_type"></param>
        public void DoUIRollBackByEscByObj(T _nodeObj)
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]DoUIRollBackByEscByType: 查询到最后一个对应类型的节点，退出该节点.  {_nodeObj}, {canRollBack()}");
            }
            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            DoUIRollBackByEscByObj(_nodeObj);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //回退开关
            if (!canRollBack())
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //执行最后一个节点的退出,并从列表中删除
            bool isDoLastNodeEnter = true;
            T tmpNode = null;

            //获取最后一个节点
            bool isLastMain = true;
            //对应节点当做最后节点
            tmpNode = _nodeObj;

            if (null == tmpNode)
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //不能退出的节点则不处理
            if (!tmpNode.IsCanRollBackQuit) //这种节点判断为不能退出
            {
                //处理无法回退的操作
                tmpNode.onCannotEscBack();
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //移除对应节点
            if (!_removeLastNode(tmpNode, out isLastMain))
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);
                return;
            }

            //设置最后一个节点的前一个节点是否需要进入
            isDoLastNodeEnter = isQuitMainView || (tmpNode.IsMainViewNode && isLastMain);

            //处理rollback退出
            _dealNodeRollBackQuit(tmpNode
                , () =>
                {
                    //判断是否需要执行节点的enter
                    if (isDoLastNodeEnter)
                    {
                        _enterLastNeedEnterNode(_resumeIdleState);
                    }
                    else
                    {
                        //在没有处理主节点切换时触发的事件函数
                        _onNotMainNodeChg();

                        //设置恢复idle状态
                        _resumeIdleState();
                    }
                });
        }

        /// <summary>
        /// 查询到最后一个对应类型的节点，退出该节点
        /// </summary>
        /// <param name="_type"></param>
        public void DoUIRollBackByEscByType(Type _type)
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]DoUIRollBackByEscByType: 查询到最后一个对应类型的节点，退出该节点.  {_type.Name}, {canRollBack()}");
            }
            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            DoUIRollBackByEscByType(_type);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //回退开关
            if (!canRollBack())
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //执行最后一个节点的退出,并从列表中删除
            bool isDoLastNodeEnter = true;
            T tmpNode = null;

            //获取最后一个节点
            bool isLastMain = true;
            //获取最后一个对应节点
            tmpNode = _findLastNode(_type, out isLastMain);

            if(null == tmpNode)
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //不能退出的节点则不处理
            if(!tmpNode.IsCanRollBackQuit) //这种节点判断为不能退出
            {
                //处理无法回退的操作
                tmpNode.onCannotEscBack();
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //移除对应节点
            if(!_removeLastNode(tmpNode, out isLastMain))
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);
                return;
            }

            //设置最后一个节点的前一个节点是否需要进入
            isDoLastNodeEnter = isQuitMainView || (tmpNode.IsMainViewNode && isLastMain);

            //处理rollback退出
            _dealNodeRollBackQuit(tmpNode
                , ()=>
                {
                    //判断是否需要执行节点的enter
                    if(isDoLastNodeEnter)
                    {
                        _enterLastNeedEnterNode(_resumeIdleState);
                    }
                    else
                    {
                        //在没有处理主节点切换时触发的事件函数
                        _onNotMainNodeChg();

                        //设置恢复idle状态
                        _resumeIdleState();
                    }
                });
        }

        /// <summary>
        /// 查询到最后一个匹配标记的节点，退出该节点
        /// </summary>
        /// <param name="_type"></param>
        public void DoUIRollBackByEscByTag(string _nodeTag)
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]DoUIRollBackByEscByJudge: 查询到最后一个对应类型的节点，退出该节点. By Judge Func {canRollBack()}");
            }
            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            DoUIRollBackByEscByTag(_nodeTag);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //回退开关
            if (!canRollBack())
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //执行最后一个节点的退出,并从列表中删除
            bool isDoLastNodeEnter = true;
            T tmpNode = null;

            //获取最后一个节点
            bool isLastMain = true;
            //获取最后一个对应节点
            tmpNode = _findLastNode(_nodeTag, out isLastMain);

            if (null == tmpNode)
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //不能退出的节点则不处理
            if (!tmpNode.IsCanRollBackQuit) //这种节点判断为不能退出
            {
                //处理无法回退的操作
                tmpNode.onCannotEscBack();
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //移除对应节点
            T rmvNode = _removeLastNode(_nodeTag, out isLastMain);
            if (null == rmvNode)
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);
                return;
            }

            //设置最后一个节点的前一个节点是否需要进入
            isDoLastNodeEnter = isQuitMainView || (tmpNode.IsMainViewNode && isLastMain);

            //处理rollback退出
            _dealNodeRollBackQuit(tmpNode
                , () =>
                {
                    //判断是否需要执行节点的enter
                    if (isDoLastNodeEnter)
                    {
                        _enterLastNeedEnterNode(_resumeIdleState);
                    }
                    else
                    {
                        //在没有处理主节点切换时触发的事件函数
                        _onNotMainNodeChg();

                        //设置恢复idle状态
                        _resumeIdleState();
                    }
                });
        }

        /// <summary>
        /// 查询到最后一个匹配判断函数的节点，退出该节点
        /// </summary>
        /// <param name="_type"></param>
        public void DoUIRollBackByEscByJudge(Func<T, bool> _judgeBackNode)
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]DoUIRollBackByEscByJudge: 查询到最后一个对应类型的节点，退出该节点. By Judge Func {canRollBack()}");
            }
            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            DoUIRollBackByEscByJudge(_judgeBackNode);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //回退开关
            if (!canRollBack())
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //执行最后一个节点的退出,并从列表中删除
            bool isDoLastNodeEnter = true;
            T tmpNode = null;

            //获取最后一个节点
            bool isLastMain = true;
            //获取最后一个对应节点
            tmpNode = _findLastNode(_judgeBackNode, out isLastMain);

            if (null == tmpNode)
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //不能退出的节点则不处理
            if (!tmpNode.IsCanRollBackQuit) //这种节点判断为不能退出
            {
                //处理无法回退的操作
                tmpNode.onCannotEscBack();
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //移除对应节点
            T rmvNode = _removeLastNode(_judgeBackNode, out isLastMain);
            if (null == rmvNode)
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);
                return;
            }

            //设置最后一个节点的前一个节点是否需要进入
            isDoLastNodeEnter = isQuitMainView || (tmpNode.IsMainViewNode && isLastMain);

            //处理rollback退出
            _dealNodeRollBackQuit(tmpNode
                , () =>
                {
                    //判断是否需要执行节点的enter
                    if (isDoLastNodeEnter)
                    {
                        _enterLastNeedEnterNode(_resumeIdleState);
                    }
                    else
                    {
                        //在没有处理主节点切换时触发的事件函数
                        _onNotMainNodeChg();

                        //设置恢复idle状态
                        _resumeIdleState();
                    }
                });
        }

        /// <summary>
        /// 删除所有无效节点后的回退操作的处理
        /// </summary>
        public void DoUIRollBackAllDisableNode()
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]DoUIRollBackAllDisableNode: 删除所有无效节点后的回退操作的处理. ");
            }
            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            DoUIRollBackAllDisableNode();
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //创建计步器，先留一步确保不会出现提前完成
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);

            T tmpNode = _lastNode;
            bool needEnter = isQuitMainView;
            //循环处理无效或需要自动退出的节点
            while(null != tmpNode)
            {
                if(!tmpNode.isEnable)
                {
                    stepCounter.chgTotalStepCount(1);

                    //从队列删除
                    _removeLastNode();

                    //处理关闭退出
                    _dealNodeRollBackQuit(tmpNode, stepCounter.addDoneStepCount);

                    //设置需要进入的节点
                    if(!needEnter)
                        needEnter = tmpNode.IsMainViewNode;
                }
                else
                {
                    break;
                }

                tmpNode = _lastNode;
            }

            //判断是否有倒退节点
            if(!needEnter)
            {
                //在没有处理主节点切换时触发的事件函数
                _onNotMainNodeChg();
                //重置步骤信息
                stepCounter.resetAll();

                //设置恢复idle状态
                _resumeIdleState();
                return;
            }

            //此时注册全部调用完后的处理
            stepCounter.regAllDoneDelegate(() =>
            {
                //进入上一个节点
                _enterLastNeedEnterNode(_resumeIdleState);
            });

            //累加预留步骤
            stepCounter.addDoneStepCount();
        }

        /************************
     * 关闭最后一个节点
     **/
        public void forceCloseLastNode()
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]forceCloseLastNode: 关闭最后一个节点,忽略回退开关的判断. ");
            }
            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            forceCloseLastNode();
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //执行最后一个节点的退出,并从列表中删除
            bool isDoLastNodeEnter = true;
            T tmpNode = null;

            //获取最后一个节点，当带入的检测节点不符的时候不允许退出
            tmpNode = _lastNode;
            if(null == tmpNode)
            {
                //设置恢复idle状态
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //移除最后一个
            _removeLastNode();

            //设置最后一个节点的前一个节点是否需要进入
            isDoLastNodeEnter = isQuitMainView || tmpNode.IsMainViewNode;

            //处理关闭退出
            _dealNodeCloseQuit(tmpNode
                , ()=>
                {
                    //判断是否需要执行节点的enter
                    if(isDoLastNodeEnter)
                    {
                        _enterLastNeedEnterNode(_resumeIdleState);
                    }
                    else
                    {
                        //在没有处理主节点切换时触发的事件函数
                        _onNotMainNodeChg();

                        //设置恢复idle状态
                        _resumeIdleState();
                    }
                });
        }
        public void forceCloseNode(T _closeNode)
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]forceCloseLastNode: 关闭最后一个节点,忽略回退开关的判断. {_closeNode.GetType()}");
            }
            if (null == _closeNode)
                return;

            //尝试进入处理状态，如无法进入则等待处理
            if(!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            forceCloseNode(_closeNode);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //执行最后一个节点的退出,并从列表中删除
            bool isDoLastNodeEnter = true;
            T tmpNode = null;

            //获取最后一个节点
            bool isLastMain = true;
            if(!_removeLastNode(_closeNode, out isLastMain))
            {
                //此时如果已经退出了主视图，这里需要进行进入视图处理
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //获取最后一个节点，当带入的检测节点不符的时候不允许退出
            tmpNode = _closeNode;
            if(null == tmpNode)
            {
                //此时如果已经退出了主视图，这里需要进行进入视图处理
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //设置最后一个节点的前一个节点是否需要进入
            isDoLastNodeEnter = isQuitMainView || (tmpNode.IsMainViewNode && isLastMain);

            //处理关闭退出
            _dealNodeCloseQuit(tmpNode
                , ()=>
                {
                    //判断是否需要执行节点的enter
                    if(isDoLastNodeEnter)
                    {
                        _enterLastNeedEnterNode(_resumeIdleState);
                    }
                    else
                    {
                        //在没有处理主节点切换时触发的事件函数
                        _onNotMainNodeChg();

                        //设置恢复idle状态
                        _resumeIdleState();
                    }
                });
        }

        /// <summary>
        /// 根据节点标记强制退出最后一个匹配标记的节点
        /// </summary>
        /// <param name="_judgeQuitNode"></param>
        public void forceCloseNodeByTag(string _nodeTag)
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]forceCloseNode: 关闭最后一个节点,忽略回退开关的判断.  By Judge Func ");
            }
            if (string.IsNullOrEmpty(_nodeTag))
                return;

            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            forceCloseNodeByTag(_nodeTag);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //执行最后一个节点的退出,并从列表中删除
            bool isDoLastNodeEnter = true;
            T tmpNode = null;

            //获取最后一个节点
            bool isLastMain = true;
            //获取最后一个节点，当带入的检测节点不符的时候不允许退出
            tmpNode = _removeLastNode(_nodeTag, out isLastMain);
            if (null == tmpNode)
            {
                //此时如果已经退出了主视图，这里需要进行进入视图处理
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //设置最后一个节点的前一个节点是否需要进入
            isDoLastNodeEnter = isQuitMainView || (tmpNode.IsMainViewNode && isLastMain);

            //处理关闭退出
            _dealNodeCloseQuit(tmpNode
                , () =>
                {
                    //判断是否需要执行节点的enter
                    if (isDoLastNodeEnter)
                    {
                        _enterLastNeedEnterNode(_resumeIdleState);
                    }
                    else
                    {
                        //在没有处理主节点切换时触发的事件函数
                        _onNotMainNodeChg();

                        //设置恢复idle状态
                        _resumeIdleState();
                    }
                });
        }

        /// <summary>
        /// 带入判断节点是否需要退出的函数，根据判断返回结果进行处理
        /// </summary>
        /// <param name="_judgeQuitNode"></param>
        public void forceCloseNode(Func<T, bool> _judgeQuitNode)
        {
            if (_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]forceCloseNode: 关闭最后一个节点,忽略回退开关的判断.  By Judge Func ");
            }
            if (null == _judgeQuitNode)
                return;

            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            forceCloseNode(_judgeQuitNode);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //执行最后一个节点的退出,并从列表中删除
            bool isDoLastNodeEnter = true;
            T tmpNode = null;

            //获取最后一个节点
            bool isLastMain = true;
            //获取最后一个节点，当带入的检测节点不符的时候不允许退出
            tmpNode = _removeLastNode(_judgeQuitNode, out isLastMain);
            if (null == tmpNode)
            {
                //此时如果已经退出了主视图，这里需要进行进入视图处理
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //设置最后一个节点的前一个节点是否需要进入
            isDoLastNodeEnter = isQuitMainView || (tmpNode.IsMainViewNode && isLastMain);

            //处理关闭退出
            _dealNodeCloseQuit(tmpNode
                , () =>
                {
                    //判断是否需要执行节点的enter
                    if (isDoLastNodeEnter)
                    {
                        _enterLastNeedEnterNode(_resumeIdleState);
                    }
                    else
                    {
                        //在没有处理主节点切换时触发的事件函数
                        _onNotMainNodeChg();

                        //设置恢复idle状态
                        _resumeIdleState();
                    }
                });
        }

        //强退某个类型的Node
        public void forceCloseNodeByType(Type _forceType)
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]forceCloseNodeByType: 强退某个类型的Node, 忽略回退开关的判断. {_forceType.Name}");
            }
            //尝试进入处理状态，如无法进入则等待处理
            if (!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            forceCloseNodeByType(_forceType);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //执行最后一个节点的退出,并从列表中删除
            bool isDoLastNodeEnter = true;
            T tmpNode = null;

            //获取最后一个节点
            bool isLastMain = true;
            tmpNode = _removeLastNode(_forceType, out isLastMain);

            if(null == tmpNode)
            {
                //此时如果已经退出了主视图，这里需要进行进入视图处理
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //设置最后一个节点的前一个节点是否需要进入
            isDoLastNodeEnter = isQuitMainView || (tmpNode.IsMainViewNode && isLastMain);

            //处理rollback退出
            _dealNodeCloseQuit(tmpNode
                , ()=>
                {
                    //判断是否需要执行节点的enter
                    if(isDoLastNodeEnter)
                    {
                        _enterLastNeedEnterNode(_resumeIdleState);
                    }
                    else
                    {
                        //在没有处理主节点切换时触发的事件函数
                        _onNotMainNodeChg();

                        //设置恢复idle状态
                        _resumeIdleState();
                    }
                });
        }
        public void forceCloseLastNodeByType(Type _lastNodeType)
        {
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Queue]forceCloseNodeByType: 强退最后一个节点类型为参数的Node, 忽略回退开关的判断. {_lastNodeType.Name}");
            }
            if (null == _lastNodeType)
                return;

            //尝试进入处理状态，如无法进入则等待处理
            if(!tryDealMgr())
            {
                //无法获取状态则添加任务等待后续处理
                _m_lLaterDealTask.Add(
                    ALCommonActionMonoTask._createTask(
                        () =>
                        {
                            //如果在处理缓存中，此时需要重置状态，然后续处理正常
                            _preDealCacheDealFunc();

                            forceCloseLastNodeByType(_lastNodeType);
                        }
#if UNITY_EDITOR
                        , _m_tmcTaskMonitor
#endif
                        ));
                return;
            }

            //退出所有需要提前退出的节点
            bool isQuitMainView = false;
            _quitAllNodeOpQuitNode(out isQuitMainView);

            //执行最后一个节点的退出,并从列表中删除
            bool isDoLastNodeEnter = true;
            T tmpNode = null;

            //获取最后一个节点，当带入的检测节点不符的时候不允许退出
            tmpNode = _lastNode;
            if(null == tmpNode
                || _lastNodeType != tmpNode.GetType())
            {
                //此时如果已经退出了主视图，这里需要进行进入视图处理
                _checkAndResumeIdleState(isQuitMainView);

                return;
            }

            //移除最后一个
            _removeLastNode();

            //设置最后一个节点的前一个节点是否需要进入
            isDoLastNodeEnter = isQuitMainView || tmpNode.IsMainViewNode;

            //处理关闭退出
            _dealNodeCloseQuit(tmpNode
                , ()=>
                {
                    //判断是否需要执行节点的enter
                    if(isDoLastNodeEnter)
                    {
                        _enterLastNeedEnterNode(_resumeIdleState);
                    }
                    else
                    {
                        //在没有处理主节点切换时触发的事件函数
                        _onNotMainNodeChg();

                        //设置恢复idle状态
                        _resumeIdleState();
                    }
                });
        }

        /// <summary>
        /// 检索最后一个主视图节点，从这个节点开始到最后的所有主视图节点都执行退出操作，但是所有这些节点都还在队列中，只是进行临时退出
        /// </summary>
        protected void _tmpQuitLastMainViewNodes(Action _dealOnQuitDone)
        {
            if(_m_NodeList.Count <= 0)
            {
                if(null != _dealOnQuitDone)
                    _dealOnQuitDone();

                return;
            }

            //创建计步器，先留一步确保不会出现提前完成
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);

            T tmpNode = null;
            //查询上一个需要进入的点
            for(int i = _m_NodeList.Count - 1; i >= 0; i--)
            {
                tmpNode = _m_NodeList[i];
                if(null == tmpNode)
                    continue;

                if(tmpNode.IsMainViewNode)
                {
                    //往后逐个加入，此处要注意nodelist的数量可能在回调被改动引起循环次数增多的异常
                    //所以队列长度先进行存储
                    int tmpCount = _m_NodeList.Count;
                    for (int n = i; n < tmpCount; n++)
                    {
                        tmpNode = _m_NodeList[n];

                        stepCounter.chgTotalStepCount(1);

                        //退出节点
                        _dealNodeQuit(tmpNode, stepCounter.addDoneStepCount);
                    }

                    //此时注册全部调用完后的处理
                    stepCounter.regAllDoneDelegate(_dealOnQuitDone);
                    //累加预留步骤
                    stepCounter.addDoneStepCount();

                    return;
                }
            }

            //如果上面没有找到头部，则从整个队列头部开始
            //往后逐个执行退出操作，此处要注意nodelist的数量可能在回调被改动引起循环次数增多的异常
            //所以队列长度先进行存储
            int nodeCount = _m_NodeList.Count;
            for (int n = 0; n < nodeCount; n++)
            {
                tmpNode = _m_NodeList[n];

                stepCounter.chgTotalStepCount(1);
                //退出节点
                _dealNodeQuit(tmpNode, stepCounter.addDoneStepCount);
            }

            //此时注册全部调用完后的处理
            stepCounter.regAllDoneDelegate(_dealOnQuitDone);
            //累加预留步骤
            stepCounter.addDoneStepCount();
        }

        /// <summary>
        /// 提前退出所有需要退出的节点
        /// </summary>
        protected void _quitAllNodeOpQuitNode(out bool _isRemoveMain)
        {
            T tmpNode = _lastNode;
            //初始没有删除主视图节点
            _isRemoveMain = false;

            //循环处理无效或需要自动退出的节点
            while (null != tmpNode)
            {
                if(tmpNode.isAllNodeOpQuitNode)
                {
                    //如果还没删除主视图节点需要尝试设置值
                    if (!_isRemoveMain)
                        _isRemoveMain = tmpNode.IsMainViewNode;

                    //从队列删除
                    _removeLastNode();

                    //处理关闭退出
                    _dealNodeCloseQuit(tmpNode, null);
                }
                else
                {
                    break;
                }

                tmpNode = _lastNode;
            }
        }

        /// <summary>
        /// 自动退出所有需要自动退出的节点
        /// </summary>
        protected void _quitAllAutoRemoveNodes(out bool _isRemoveMain)
        {
            //获取上一节点
            T tmpNode = _lastNode;
            //初始没有删除主视图节点
            _isRemoveMain = false;

            //当需要关闭前面需要自动退出的节点时，开始循环递归退出处理
            while (null != tmpNode)
            {
                if(tmpNode.NeedAutoRemove)
                {
                    //如果还没删除主视图节点需要尝试设置值
                    if (!_isRemoveMain)
                        _isRemoveMain = tmpNode.IsMainViewNode;

                    //从队列删除
                    _removeLastNode();

                    //处理关闭退出
                    _dealNodeCloseQuit(tmpNode, null);
                }
                else
                {
                    break;
                }

                tmpNode = _lastNode;
            }
        }

        /// <summary>
        /// 进入最后一个需要进入的节点
        /// </summary>
        protected void _enterLastNeedEnterNode(Action _triggerEnterDone)
        {
            if(_m_NodeList.Count <= 0)
            {
                if(null != _triggerEnterDone)
                    _triggerEnterDone();

                //处理空列表
                AddNode(_getDefaultRootNode());
                return;
            }

            //删除最后一个节点
            //创建计步器，先留一步确保不会出现提前完成
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);

            //获取上一节点
            T tmpNode = _lastNode;
            //循环处理无效或需要自动退出的节点
            while(null != tmpNode)
            {
                if(!tmpNode.isEnable)
                {
                    stepCounter.chgTotalStepCount(1);

                    //从队列删除
                    _removeLastNode();
                    //处理关闭退出
                    _dealNodeCloseQuit(tmpNode, stepCounter.addDoneStepCount);
                }
                else
                {
                    break;
                }

                tmpNode = _lastNode;
            }

            //此时注册全部调用完后的处理
            stepCounter.regAllDoneDelegate(() =>
            {
                T tmpN = null;
                //查询上一个需要进入的点
                for(int i = _m_NodeList.Count - 1; i >= 0; i--)
                {
                    tmpN = _m_NodeList[i];
                    if(null == tmpN)
                        continue;

                    if(tmpN.IsMainViewNode)
                    {
                        //创建步骤并处理
                        ALStepCounter enterStepCounter = new ALStepCounter();
                        enterStepCounter.chgTotalStepCount(_m_NodeList.Count - i);
                        enterStepCounter.regAllDoneDelegate(_triggerEnterDone);

                        //往后逐个加入，此处要注意nodelist的数量可能在回调被改动引起循环次数增多的异常
                        //所以队列长度先进行存储
                        int nodeCount = _m_NodeList.Count;
                        for (int n = i; n < nodeCount; n++)
                        {
                            tmpN = _m_NodeList[n];
                            //进入节点
                            _dealNodeEnter(tmpN, enterStepCounter.addDoneStepCount);
                        }
                        return;
                    }
                }

                //创建步骤并处理
                ALStepCounter enterStepCounter_2 = new ALStepCounter();
                enterStepCounter_2.chgTotalStepCount(_m_NodeList.Count);
                enterStepCounter_2.regAllDoneDelegate(_triggerEnterDone);

                //到头部
                //往后逐个加入
                int curEnd = _m_NodeList.Count;
                for(int n = 0; n < curEnd; n++)
                {
                    tmpN = _m_NodeList[n];
                    //进入节点
                    _dealNodeEnter(tmpN, enterStepCounter_2.addDoneStepCount);
                }
            });

            //累加预留步骤
            stepCounter.addDoneStepCount();
        }

        /// <summary>
        /// 判断是否可以esc回退
        /// </summary>
        /// <returns></returns>
        public bool canRollBack()
        {
            return _m_lRollBackSwitch == 0;
        }
        /// <summary>
        /// 开启回退功能
        /// </summary>
        /// <param name="_sysIndex_64">带入系统索引，最高64</param>
        public void OpenRollBack()
        {
            OpenRollBack(0);
        }
        public void OpenRollBack(int _sysIndex_64)
        {
            if (_sysIndex_64 > 64)
            {
                ALLog.Error("Queue Roll Back SysIndex is Bigger than 64");
                _sysIndex_64 = 64;
            }
            else if(_sysIndex_64 < 0)
            {
                ALLog.Error("Queue Roll Back SysIndex is Smaller than 0");
                _sysIndex_64 = 0;
            }

            _m_lRollBackSwitch = _m_lRollBackSwitch & ~(1L << _sysIndex_64);
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Qeueu] √ OpenRollBack({_sysIndex_64}): canRollBack:{canRollBack()}");
            }
        }
        /// <summary>
        /// 关闭回退功能
        /// </summary>
        /// <param name="_sysIndex_64">带入系统索引，最高64</param>
        public void CloseRollBack()
        {
            CloseRollBack(0);
        }
        public void CloseRollBack(int _sysIndex_64)
        {
            if(_sysIndex_64 > 64)
            {
                ALLog.Error("Queue Roll Back SysIndex is Bigger than 64");
                _sysIndex_64 = 64;
            }
            else if(_sysIndex_64 < 0)
            {
                ALLog.Error("Queue Roll Back SysIndex is Smaller than 0");
                _sysIndex_64 = 0;
            }

            _m_lRollBackSwitch = _m_lRollBackSwitch | (1L << _sysIndex_64);
            if(_AALMonoMain.instance.showDebugOutput)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[Qeueu] × CloseRollBack({_sysIndex_64}): canRollBack:{canRollBack()}");
            }
        }

        public void Clear()
        {
            _m_NodeList.Clear();
        }

        /// <summary>
        /// 获取最后一个对应类型的节点
        /// </summary>
        /// <param name="_lastNodeType"></param>
        /// <returns></returns>
        protected T _findLastNode(Type _lastNodeType, out bool _isLastMain)
        {
            T tmpNode = null;
            bool isLastMain = true;
            for(int i = _m_NodeList.Count - 1; i >= 0; i--)
            {
                tmpNode = _m_NodeList[i];
                if(null == tmpNode)
                    continue;

                if(tmpNode.GetType() == _lastNodeType)
                {
                    _isLastMain = isLastMain;
                    return tmpNode;
                }
                else
                {
                    if(tmpNode.IsMainViewNode)
                        isLastMain = false;
                }
            }

            _isLastMain = isLastMain;
            return null;
        }

        /// <summary>
        /// 获取最后一个对应类型的节点
        /// </summary>
        /// <param name="_lastNodeType"></param>
        /// <returns></returns>
        protected T _findLastNode(string _nodeTag, out bool _isLastMain)
        {
            if (string.IsNullOrEmpty(_nodeTag))
            {
                _isLastMain = false;
                return null;
            }

            T tmpNode = null;
            bool isLastMain = true;
            for (int i = _m_NodeList.Count - 1; i >= 0; i--)
            {
                tmpNode = _m_NodeList[i];
                if (null == tmpNode)
                    continue;

                //比对节点标记
                if (_nodeTag.Equals(tmpNode.nodeTag, StringComparison.Ordinal))
                {
                    _isLastMain = isLastMain;
                    return tmpNode;
                }
                else
                {
                    if (tmpNode.IsMainViewNode)
                        isLastMain = false;
                }
            }

            _isLastMain = isLastMain;
            return null;
        }

        /// <summary>
        /// 获取最后一个对应类型的节点
        /// </summary>
        /// <param name="_lastNodeType"></param>
        /// <returns></returns>
        protected T _findLastNode(Func<T, bool> _judgeNode, out bool _isLastMain)
        {
            if (null == _judgeNode)
            {
                _isLastMain = false;
                return null;
            }

            T tmpNode = null;
            bool isLastMain = true;
            for (int i = _m_NodeList.Count - 1; i >= 0; i--)
            {
                tmpNode = _m_NodeList[i];
                if (null == tmpNode)
                    continue;

                if (_judgeNode(tmpNode))
                {
                    _isLastMain = isLastMain;
                    return tmpNode;
                }
                else
                {
                    if (tmpNode.IsMainViewNode)
                        isLastMain = false;
                }
            }

            _isLastMain = isLastMain;
            return null;
        }

        /// <summary>
        /// 删除最后一个节点，并返回
        /// </summary>
        /// <returns></returns>
        protected T _removeLastNode()
        {
            if(_m_NodeList.Count <= 0)
                return null;

            T lstN = _m_NodeList[_m_NodeList.Count - 1];
            _m_NodeList.RemoveAt(_m_NodeList.Count - 1);

            return lstN;
        }
        /// <summary>
        /// 删除最后一个带入的node节点
        /// </summary>
        /// <param name="_node"></param>
        /// <param name="_isLastMain">返回是否最后一个进入节点，是则需要处理后续进入操作</param>
        /// <returns></returns>
        protected bool _removeLastNode(T _node, out bool _isLastMain)
        {
            T tmpNode = null;
            bool isLastMain = true;
            for(int i = _m_NodeList.Count - 1; i >= 0; i--)
            {
                tmpNode = _m_NodeList[i];
                if(tmpNode == _node)
                {
                    _m_NodeList.RemoveAt(i);
                    _isLastMain = isLastMain;
                    return true;
                }
                else
                {
                    if(tmpNode.IsMainViewNode)
                        isLastMain = false;
                }
            }

            _isLastMain = isLastMain;
            return false;
        }

        /// <summary>
        /// 删除最后一个带入类型的节点
        /// </summary>
        /// <param name="_lastNodeType"></param>
        /// <returns></returns>
        protected T _removeLastNode(Type _lastNodeType, out bool _isLastMain)
        {
            T tmpNode = null;
            bool isLastMain = true;
            for(int i = _m_NodeList.Count - 1; i >= 0; i--)
            {
                tmpNode = _m_NodeList[i];
                if(null == tmpNode)
                    continue;

                if(tmpNode.GetType() == _lastNodeType)
                {
                    _m_NodeList.RemoveAt(i);
                    _isLastMain = isLastMain;
                    return tmpNode;
                }
                else
                {
                    if(tmpNode.IsMainViewNode)
                        isLastMain = false;
                }
            }

            _isLastMain = isLastMain;
            return null;
        }

        /// <summary>
        /// 删除最后一个符合带入判断函数条件的节点
        /// </summary>
        /// <param name="_lastNodeType"></param>
        /// <returns></returns>
        protected T _removeLastNode(string _nodeTag, out bool _isLastMain)
        {
            if (string.IsNullOrEmpty(_nodeTag))
            {
                _isLastMain = false;
                return null;
            }

            T tmpNode = null;
            bool isLastMain = true;
            for (int i = _m_NodeList.Count - 1; i >= 0; i--)
            {
                tmpNode = _m_NodeList[i];
                if (null == tmpNode)
                    continue;

                //比对标记
                if (_nodeTag.Equals(tmpNode.nodeTag, StringComparison.Ordinal))
                {
                    _m_NodeList.RemoveAt(i);
                    _isLastMain = isLastMain;
                    return tmpNode;
                }
                else
                {
                    if (tmpNode.IsMainViewNode)
                        isLastMain = false;
                }
            }

            _isLastMain = isLastMain;
            return null;
        }

        /// <summary>
        /// 删除最后一个符合带入判断函数条件的节点
        /// </summary>
        /// <param name="_lastNodeType"></param>
        /// <returns></returns>
        protected T _removeLastNode(Func<T, bool> _judgeNode, out bool _isLastMain)
        {
            if (null == _judgeNode)
            {
                _isLastMain = false;
                return null;
            }

            T tmpNode = null;
            bool isLastMain = true;
            for (int i = _m_NodeList.Count - 1; i >= 0; i--)
            {
                tmpNode = _m_NodeList[i];
                if (null == tmpNode)
                    continue;

                if (_judgeNode(tmpNode))
                {
                    _m_NodeList.RemoveAt(i);
                    _isLastMain = isLastMain;
                    return tmpNode;
                }
                else
                {
                    if (tmpNode.IsMainViewNode)
                        isLastMain = false;
                }
            }

            _isLastMain = isLastMain;
            return null;
        }

        /// <summary>
        /// 处理单纯的进入操作
        /// </summary>
        /// <param name="_tmpNode"></param>
        private void _dealNodeEnter(T _tmpNode, Action _triggerEnterDone)
        {
            if(null == _tmpNode)
                return;

            //设置节点产生了变更
            _m_bIsNodeChg = true;

            _tmpNode.dealEnterNode(_triggerEnterDone); //处理进入操作
        }

        /// <summary>
        /// 处理单纯的退出操作，此时节点还在队列
        /// </summary>
        /// <param name="_tmpNode"></param>
        private void _dealNodeQuit(T _tmpNode, Action _dealOnQuitDone)
        {
            if(null == _tmpNode)
            {
                if(null != _dealOnQuitDone)
                    _dealOnQuitDone();

                return;
            }

            //设置节点产生了变更
            _m_bIsNodeChg = true;

            _tmpNode.dealQuitNode(_dealOnQuitDone); //处理退出操作
        }

        /// <summary>
        /// 处理回退的退出操作
        /// </summary>
        /// <param name="_tmpNode"></param>
        private void _dealNodeRollBackQuit(T _tmpNode, Action _dealOnQuitDone)
        {
            if(null == _tmpNode)
            {
                if(null != _dealOnQuitDone)
                    _dealOnQuitDone();

                return;
            }

            //设置节点产生了变更
            _m_bIsNodeChg = true;

            _tmpNode.dealQuitNode(
                () =>
                {
                    //处理回退的事件
                    _tmpNode.onRollBackQuit();
                    _tmpNode.onClose();

                    if (null != _dealOnQuitDone)
                        _dealOnQuitDone();
                }); //处理退出操作
        }

        /// <summary>
        /// 处理关闭的退出操作
        /// </summary>
        /// <param name="_tmpNode"></param>
        private void _dealNodeCloseQuit(T _tmpNode, Action _dealOnQuitDone)
        {
            if(null == _tmpNode)
            {
                if(null != _dealOnQuitDone)
                    _dealOnQuitDone();

                return;
            }

            //设置节点产生了变更
            _m_bIsNodeChg = true;

            //退出节点
            _tmpNode.dealQuitNode(
                () =>
                {
                    //关闭节点的退出处理，与rollback互斥的事件
                    _tmpNode.onCloseQuit();
                    _tmpNode.onClose();

                    if (null != _dealOnQuitDone)
                        _dealOnQuitDone();
                });
        }

        /// <summary>
        /// 调用系统关闭节点的操作函数
        /// </summary>
        /// <param name="_tmpNode"></param>
        private void _dealSysCloseNode(T _tmpNode)
        {
            if (null == _tmpNode)
                return;

            _tmpNode.onCloseQuit();
            _tmpNode.onClose();
        }
        
        public override string ToString()
        {
            Dictionary<string, object> baseDic = new Dictionary<string, object>();
            Dictionary<string, object> newDic = new Dictionary<string, object>();
            StringBuilder sb = new StringBuilder();
            StringBuilder nodeNamaList = new StringBuilder();
            sb.Append("----------------------------");
            bool isFirstNode = true;
            foreach (T node in _m_NodeList)
            {
                // 获取Node节点类型
                Type type = node.GetType();
                string nodeShowName = node.NodeShowName;
                sb.Append("【NodeName】:");
                sb.Append(nodeShowName);
                sb.Append("\n");

                if (isFirstNode)
                {
                    nodeNamaList.Append($"【{nodeShowName}】");
                    isFirstNode = false;
                }
                else
                {
                    nodeNamaList.Append(" ----> ");
                    nodeNamaList.Append($"【{nodeShowName}】");
                }
                
                // 获取基类的属性
                PropertyInfo[] basePropertyInfos = type.BaseType.GetProperties();
                foreach (PropertyInfo property in basePropertyInfos)
                {
                    if (property.PropertyType == typeof(bool))
                    {
                        baseDic[property.Name] = property.GetValue(node);
                    }
                }

                // 获取当前类自己定义和重写的的属性
                PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                
                // 找出当前类自己定义的属性
                foreach (PropertyInfo property in propertyInfos)
                {
                    object obj = null;
                    if (!baseDic.TryGetValue(property.Name, out obj) && property.PropertyType == typeof(bool))
                    {
                        newDic[property.Name] = property.GetValue(node);
                    }
                }

                sb.Append("【基类的属性】：\n");
                sb.Append(node.ToString());
                
                sb.Append("【新增的属性】：\n");
                foreach (KeyValuePair<string, object> keyValuePair in newDic)
                {
                    sb.Append($"{keyValuePair.Key} : {keyValuePair.Value}\n");
                }
                sb.Append("----------------------------");
                baseDic.Clear();
                newDic.Clear();
            }

            nodeNamaList.Append('\n');
            sb.Insert(0, nodeNamaList.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// 获取默认根节点
        /// </summary>
        /// <returns></returns>
        protected abstract T _getDefaultRootNode();

        /// <summary>
        /// 在系统出现基础逻辑错误时触发的事件处理
        /// </summary>
        protected abstract void _onError();
        /// <summary>
        /// 在没有处理主节点切换时触发的事件函数，或在部分特殊操作时，如没有进行主视图切换都将执行本操作
        /// </summary>
        protected abstract void _onNotMainNodeChg();
        /// <summary>
        /// 节点的变更函数，在节点无后续处理情况的时候触发的节点变更函数
        /// </summary>
        protected abstract void _onNodeChg();
    }
}