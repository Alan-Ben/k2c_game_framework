using System;
using System.Collections.Generic;

namespace ALPackage
{
    /// <summary>
    /// 步骤管理器中，专门给Action进行处理并对过程进行监控的任务对象
    /// </summary>
    public class ALProcessStepActionMonoTask : _AALProcessBasicMonoTask
    {
        //执行回调
        private Action _m_dAction;
        //完成回调
        private Action _m_dDoneAction;
        //失败回调
        private Action _m_dFailAction;
        //是否失败的时候也执行完成函数
        private bool _m_bIsFailDoDone;
        //失败时停止流程的回调
        private Action _m_dFailStopAction;

        //对应步骤标记
        private string _m_sProcessTag;
        //对应步骤中的任务下标
        private int _m_iProcessTaskIdx;
        //步骤开始执行的时间
        private long _m_lProcessStartTimeMS;
        //超时完成时的监控对象，此对象注意回收和开始的时候都需要置空
        //如果通过task的deal调用，则会默认使用这个参数作为内部步骤处理的监控对象
        private _IALProcessMonitor _m_mTimeoutMonitor;

        public ALProcessStepActionMonoTask(Action _action, string _processTag, int _processTaskIdx, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = true, Action _failStopAction = null)
        {
            _m_dAction = _action;
            _m_dDoneAction = _doneDelegate;
            _m_dFailAction = _failDelegate;
            _m_bIsFailDoDone = _isFailDoDone;
            _m_dFailStopAction = _failStopAction;

            _m_sProcessTag = _processTag;
            _m_iProcessTaskIdx = _processTaskIdx;
            _m_lProcessStartTimeMS = 0;

            _m_mTimeoutMonitor = null;
        }

        /// <summary>
        /// 内部逻辑中部分脱节情况下可以在通过本函数设置monitor初始值
        /// 以此在task调用的时候设置步骤监控对象的调用方式
        /// </summary>
        public override void setMonitor(_IALProcessMonitor _monitor)
        {
            _m_mTimeoutMonitor = _monitor;
        }

        public override void deal()
        {
            //设置开始执行时间
            _m_lProcessStartTimeMS = ALCommon.getNowTimeMill();

            //获取监控时长
            long monitMaxTimeMS = 0;
            if (null != _m_mTimeoutMonitor)
            {
                monitMaxTimeMS = _m_mTimeoutMonitor.monitorTimeMS(_m_sProcessTag);

                //最多监控100秒
                if (monitMaxTimeMS > 100000)
                    monitMaxTimeMS = 100000;
            }

            try
            {
                if (null != _m_dAction)
                    _m_dAction();

                //计算超出监控的时间
                long timeoutMS = ALCommon.getNowTimeMill() - _m_lProcessStartTimeMS - monitMaxTimeMS;
                //已经完成且时间差在1秒以内则不进行处理，这里进行时间判断是为了避免因为计算量过大引起的超时没被捕获
                if (timeoutMS > 500)
                {
                    //调用回调
                    if (null != _m_mTimeoutMonitor)
                    {
                        _m_mTimeoutMonitor.onTimeout(ALCommon.getNowTimeMill() - _m_lProcessStartTimeMS, _m_sProcessTag, _m_iProcessTaskIdx.ToString());

                        //如果已经完成需要调用完成的超时处理
                        _m_mTimeoutMonitor.onTimeoutDone(ALCommon.getNowTimeMill() - _m_lProcessStartTimeMS, _m_sProcessTag, _m_iProcessTaskIdx.ToString());
                    }
                }

                //执行完成后执行完成函数，根据结果判断是否正常
                if (null != _m_dDoneAction)
                    _m_dDoneAction();
            }
            catch (Exception _ex)
            {
                //调用回调
                if (null != _m_mTimeoutMonitor)
                {
                    _m_mTimeoutMonitor.onErr(ALCommon.getNowTimeMill() - _m_lProcessStartTimeMS, _m_sProcessTag, _m_iProcessTaskIdx.ToString(), _ex);
                }
                else
                {
                    ALLog.Sys($"Multi Process Action {_m_sProcessTag} Fail! Task Index: [{_m_iProcessTaskIdx}]");
                    UnityEngine.Debug.LogError(_ex.ToString());
                }

                //执行失败函数
                if (null != _m_dFailAction)
                    _m_dFailAction();

                //判断失败是否需要执行成功函数
                if (_m_bIsFailDoDone)
                {
                    //执行成功函数
                    if (null != _m_dDoneAction)
                        _m_dDoneAction();
                }
                else
                {
                    //调用monitor的失败处理函数
                    if (null != _m_dFailStopAction)
                        _m_dFailStopAction();

                    ALLog.Sys("One Step Action Error!");
                }
            }
            finally
            {
                //释放资源
                discard();
            }
        }

        /// <summary>
        /// 释放本任务对象的相关资源或者关联
        /// </summary>
        public void discard()
        {
            _m_dAction = null;
            _m_dDoneAction = null;
            _m_dFailAction = null;
            _m_bIsFailDoDone = true;
            
            _m_mTimeoutMonitor = null;
        }

        public override string ToString()
        {
            if(_m_dAction != null)
            {
                return $"ALProcessStepActionMonoTask:{_m_dAction.Method.ReflectedType.FullName}.{_m_dAction.Method.Name}";
            }
            else
            {
                return $"ALProcessStepActionMonoTask:null";
            }
        }
    }
}
