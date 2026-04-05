using System;
using System.Collections.Generic;

/*********************
 * 加载步骤的管理对象
 **/
namespace ALPackage
{
    public abstract class _AALProcess : _AALProcessBasicMonoTask
    {
        /// <summary>
        /// 过程控制对象，所有子对象自动向父对象同步
        /// 这样父对象终止的时候各子对象也会自动终止
        /// </summary>
        public class ALProcessController
        {
            //目前控制对象是否还有效
            public volatile bool enable;

            //是否已经停止，如果过程已停止，在调用stopProcess的时候会调用节点完成处理
            protected internal volatile bool _isStop;
        }

        /// <summary>
        /// 本加载步骤的标记，用于输出日志或者在异常回调中使用
        /// </summary>
        private string _m_sProcessTag;
        /// <summary>
        /// 父节点的过程控制对象
        /// </summary>
        private _AALProcess _m_apParentProcess;

        //控制对象
        protected ALProcessController _m_cController;

        //是否已经开始执行，开始执行的过程不允许修改
        private bool _m_bIsRunning;
        //是否已完成
        private bool _m_bIsDone;

        //监控对应序列号
        private long _m_lMonitorSerialize;
        //步骤开始执行的时间
        private long _m_lProcessStartTimeMS;
        //是否超过监控时间
        private bool _m_bIsMonitorOutTime;
        //过程对象的监控处理对象
        private _IALProcessMonitor _m_mMonitor;
        
        public _AALProcess(string _tag)
        {
            _m_sProcessTag = _tag;
            _m_apParentProcess = null;

            _m_cController = new ALProcessController();
            _m_cController.enable = true;
            _m_cController._isStop = false;

            _m_bIsRunning = false;
            _m_bIsDone = false;

            _m_lMonitorSerialize = 0;
            _m_lProcessStartTimeMS = 0;
            _m_bIsMonitorOutTime = false;
            _m_mMonitor = null;
        }

        public string processTag { get { return _m_sProcessTag; } }
        public bool isRunning { get { return _m_bIsRunning; } }
        public bool isDone { get { return _m_bIsDone; } }

        public long processStartTimeMS { get { return _m_lProcessStartTimeMS; } }

        public _IALProcessMonitor monitor { get { return _m_mMonitor; } }

        /// <summary>
        /// 强制刷新步骤开始时间，避免错误的监控
        /// </summary>
        protected void _forceRefreshProcessStartTime()
        {
            //生成新序列号
            _m_lMonitorSerialize = ALSerializeOpMgr.next();
            //设置开始执行时间
            _m_lProcessStartTimeMS = ALCommon.getNowTimeMill();
            //设置未超时
            _m_bIsMonitorOutTime = false;
        }

        /// <summary>
        /// 设置步骤标记
        /// </summary>
        /// <param name="_tag"></param>
        public void setTag(string _tag)
        {
            _m_sProcessTag = _tag;
        }

        /// <summary>
        /// 设置父节点
        /// </summary>
        /// <param name="_parentPRocess"></param>
        public void setParentProcess(_AALProcess _parentPRocess)
        {
            //判断是否原来有父对象，如果有这里有可能逻辑错误，报错
            if(null != _m_apParentProcess)
            {
                UnityEngine.Debug.LogError("ALProcess Set Parent while parent already exist!");
            }

            _m_apParentProcess = _parentPRocess;
            //设置控制对象为父节点控制对象
            _m_cController = _m_apParentProcess._m_cController;
        }

        /// <summary>
        /// 进行任务的处理，这个会直接调用执行过程的处理
        /// </summary>
        public override void deal()
        {
            dealProcess(_m_mMonitor);
        }

        /**************
         * 终止整个过程，调用本函数不会马上终止
         * 会在各执行节点上根据状态自行终止
         */
        public void stopProcess()
        {
            if (null == _m_cController)
                return;

            _m_cController.enable = false;

            //判断是否已经终止，是则需要调用_processDone处理
            if (_m_cController._isStop)
                _onProcessDone();
        }

        /// <summary>
        /// 进行处理
        /// </summary>
        public void dealProcess()
        {
            dealProcess(_m_mMonitor);
        }
        /// <summary>
        /// 进行处理
        /// </summary>
        public void dealProcess(_IALProcessMonitor _monitor)
        {
            if(_m_bIsRunning)
            {
                UnityEngine.Debug.LogError("Deal after started!");
                return;
            }

            //如果已经不继续执行，则直接返回
            if (!_m_cController.enable)
            {
                //此时直接执行过程完成
                _onProcessDone();
                return;
            }

            //设置开始执行
            _m_bIsRunning = true;
            _m_bIsDone = false;
            //生成新序列号
            _m_lMonitorSerialize = ALSerializeOpMgr.next();
            //设置开始执行时间
            _m_lProcessStartTimeMS = ALCommon.getNowTimeMill();
            //设置未超时
            _m_bIsMonitorOutTime = false;
            _m_mMonitor = _monitor;

            //获取监控时长
            long monitMaxTimeMS = 0;
            if(null != _m_mMonitor)
            {
                monitMaxTimeMS = _m_mMonitor.monitorTimeMS(processTag);

                //最多监控100秒
                if (monitMaxTimeMS > 100000)
                    monitMaxTimeMS = 100000;
            }

            //只有在监控时间超过0.1秒的时候才有效
            if (monitMaxTimeMS >= 100)
            {
                long curSerialize = _m_lMonitorSerialize;
                //开启定时任务进行监控
                ALCommonTaskController.CommonActionAddMonoTask(
                    () =>
                    {
                        //序列号如果不一致则直接不处理监控
                        if (curSerialize != _m_lMonitorSerialize)
                            return;

                        //计算超出监控的时间
                        long timeoutMS = ALCommon.getNowTimeMill() - _m_lProcessStartTimeMS - monitMaxTimeMS;
                        //已经完成且时间差在1秒以内则不进行处理，这里进行时间判断是为了避免因为计算量过大引起的超时没被捕获
                        if (isDone && timeoutMS < 500)
                            return;

                        //设置超时
                        _m_bIsMonitorOutTime = true;
                        //调用回调
                        if (null != _m_mMonitor)
                        {
                            //如果已经完成需要调用完成的超时处理
                            if (isDone)
                                _m_mMonitor.onTimeoutDone(ALCommon.getNowTimeMill() - _m_lProcessStartTimeMS, processTag, _processMonitorExInfo);
                            else
                                _m_mMonitor.onTimeout(ALCommon.getNowTimeMill() - _m_lProcessStartTimeMS, processTag, _processMonitorExInfo);
                        }
                    }
                    , monitMaxTimeMS / 1000f);
            }

            try
            {
                //执行任务
                _dealProcess(_m_mMonitor);
            }
            catch(Exception _ex)
            {
                //调用回调
                if (null != _m_mMonitor)
                    _m_mMonitor.onErr(ALCommon.getNowTimeMill() - _m_lProcessStartTimeMS, processTag, _processMonitorExInfo, _ex);

                //调用失败处理函数
                _onProcessFailStop();
            }
        }

        /// <summary>
        /// 重置处理
        /// </summary>
        public void discard()
        {
            _m_apParentProcess = null;

            _m_bIsRunning = false;
            
            _m_bIsMonitorOutTime = false;
            _m_mMonitor = null;

            _onDiscard();
        }

        /// <summary>
        /// 内部逻辑中部分脱节情况下可以在通过本函数设置monitor初始值
        /// 以此在task调用的时候设置步骤监控对象的调用方式
        /// </summary>
        public override void setMonitor(_IALProcessMonitor _monitor)
        {
            _m_mMonitor = _monitor;
        }

        //在本节点处理完成后的处理
        protected void _onProcessDone()
        {
            if(_m_bIsDone)
            {
                UnityEngine.Debug.LogError("done process multi times!");
                return;
            }

            _m_bIsDone = true;

            //此时根据是否超时和监控对象是否有效进行回调调用
            if (_m_bIsMonitorOutTime && null != _m_mMonitor)
            {
                _m_mMonitor.onTimeoutDone(ALCommon.getNowTimeMill() - _m_lProcessStartTimeMS, processTag, _processMonitorExInfo);
            }

            //只有在有效的情况下才会执行成功回调
            if (_m_cController.enable)
            {
                //放回缓存处理
                _onDone();
            }

            //完结执行父节点的下一个处理
            if (null != _m_apParentProcess)
                _m_apParentProcess._onChildDone();
            else if (null != _m_mMonitor)
            {
                //如果是被异常终止，需要调用对应的事件函数
                if (!_m_cController.enable)
                    _m_mMonitor.onRootProecssStop();//在没有父节点的时候，对加农对象执行根节点完成操作

                _m_mMonitor.onRootProecssDone();//在没有父节点的时候，对加农对象执行根节点完成操作
            }

            //执行释放操作
            discard();
        }

        /// <summary>
        /// 在子过程失败停下之后的触发函数
        /// </summary>
        protected void _onProcessFailStop()
        {
            //设置被停止
            _m_cController._isStop = true;

            if (null != monitor)
                monitor.onProcessFailStop(this);
        }

        /// <summary>
        /// 获取出来可以在监控对象外围补充输出的信息
        /// </summary>
        protected abstract string _processMonitorExInfo { get; }

        /// <summary>
        /// 执行任务
        /// </summary>
        protected abstract void _dealProcess(_IALProcessMonitor _monitor);
        /// <summary>
        /// 当子处理对象完成时的处理
        /// </summary>
        protected abstract void _onChildDone();
        /// <summary>
        /// 重置处理
        /// </summary>
        protected abstract void _onDiscard();
        /// <summary>
        /// 完成的处理
        /// </summary>
        protected abstract void _onDone();
    }
}
