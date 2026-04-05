
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using JetBrains.Annotations;


/// <summary>
/// 限时加载器
/// </summary>
/// <remarks>
/// 一个在主线程跑的加载器，用来拆分加载，加载时长如果超过一个设定值就就会放到下一帧加载
/// </remarks>
public class TimeLimitedLoader
{
    // 每一帧限制的load时长
    private long _m_lTimeLimited;
    // 是否正在跑加载任务
    private bool _m_bIsRunning;
    // 任务列表
    [NotNull] private readonly Queue<_ITimeLimitedLoaderTask> _m_queueTask;
    // 加载任务
    [NotNull] private readonly LoadTask _m_loadTask;

    public TimeLimitedLoader(long _timeLimitedMilliseconds)
    {
        _m_lTimeLimited = _timeLimitedMilliseconds;
        _m_bIsRunning = false;

        _m_queueTask = new Queue<_ITimeLimitedLoaderTask>();
        _m_loadTask = new LoadTask(this);
    }

    /// <summary>
    /// 设置一次加载允许的间隔时间
    /// </summary>
    public void setLimitTime(long _milliseconds)
    {
        _m_lTimeLimited = _milliseconds;
    }

    public void addLoadTask(_ITimeLimitedLoaderTask _task)
    {
        if (_task == null)
            return;

        _m_queueTask.Enqueue(_task);
        _checkAndRunTask();
    }

    private void _checkAndRunTask()
    {
        // 如果已经运行了就不处理了
        if (_m_bIsRunning)
            return;

        _m_bIsRunning = true;
        ALMonoTaskMgr.instance.addMonoTask(_m_loadTask);
    }
    /// <summary>
    /// 当有任务超出了限制的时间数量时
    /// </summary>
    protected virtual void _onTaskBrokeTheLimitation(Type _taskType)
    {

    }

    private class LoadTask : _IALBaseMonoTask
    {
        [NotNull]
        private readonly TimeLimitedLoader _m_loader;

        //当前开始处理的帧数
        private int _m_iFramIndex;
        //总开始时间
        private long _m_lTaskLoadStartTime;
        // 每个load步骤开始时的时间
        private long _m_lStepLoadStartTime;
        // 上一个加载任务
        private Type _m_lastTask;
        private string _m_lastTaskDebugStr;

        public LoadTask([NotNull] TimeLimitedLoader _loader)
        {
            _m_loader = _loader;
            _m_iFramIndex = -1;
        }

        public void deal()
        {
            //判断帧数是否一致，不一致则刷新
            if (_m_iFramIndex != Time.frameCount)
            {
                //刷新帧标记
                _m_iFramIndex = Time.frameCount;

                //刷新时间
                _m_lTaskLoadStartTime = ALCommon.getNowTimeMill();
                _m_lStepLoadStartTime = ALCommon.getNowTimeMill();

                //重置最后一个任务
                _m_lastTask = null;
                _m_lastTaskDebugStr = string.Empty;
            }

            //开始处理
            _dealData();
        }

        /// <summary>
        /// 任务的处理函数，每个加载都在此处理
        /// 通过本帧开始的时间标记判断是否超过单帧处理上限
        /// </summary>
        private void _dealData()
        {
            long nowTimeMS = ALCommon.getNowTimeMill();
            // 判断上一个任务的时间是否过长了
            long stepDuringTime = nowTimeMS - _m_lStepLoadStartTime;
            if (_m_lastTask != null && stepDuringTime > _m_loader._m_lTimeLimited)
            {
                ALLog.Warning($"[{_m_iFramIndex}]TimeLimitedLoader 加载任务【tag : {_m_lastTaskDebugStr}】单次耗时【millisecond : {stepDuringTime}】已经超过了设定的一帧可以接受的耗时【limitTime : {_m_loader._m_lTimeLimited}】");
                _m_loader._onTaskBrokeTheLimitation(_m_lastTask);
            }

            // 如果还有 task 需要处理，就执行
            if (_m_loader._m_queueTask.Count > 0)
            {
                // 看一眼现在的耗时有没有超出设定范围
                if (nowTimeMS - _m_lTaskLoadStartTime > _m_loader._m_lTimeLimited)
                    // 如果超出了，就下一帧再处理
                    ALMonoTaskMgr.instance.addNextFrameTask(this);
                else
                {
                    // 取出当前要处理的任务
                    _ITimeLimitedLoaderTask task = _m_loader._m_queueTask.Dequeue();
                    // 记录下这个任务开始的时间，和这个任务本身
                    _m_lStepLoadStartTime = nowTimeMS;
                    _m_lastTask = task.GetType();
                    _m_lastTaskDebugStr = task.debugTag;
                    try
                    {
                        // 执行任务的加载
                        task.load(_continueCurFrameTask);
                    }
                    catch (Exception ex)
                    {
                        ALLog.Error($"TimeLimitedLoader 加载任务【tag : {_m_lastTaskDebugStr}】执行出错\n{ex.ToString()}");
                        _m_lastTask = null;
                        _m_lastTaskDebugStr = string.Empty;
                        // 上面异常了就手动触发一次，以防卡死
                        _continueCurFrameTask();
                    }
                }
            }
            else
            {
                // 如果都处理完了，就标识为停止
                _m_loader._m_bIsRunning = false;
            }
        }

        /// <summary>
        /// 重新注册本任务
        /// </summary>
        protected void _continueCurFrameTask()
        {
            //直接注册
            ALMonoTaskMgr.instance.addMonoTask(this);
        }
    }
}
