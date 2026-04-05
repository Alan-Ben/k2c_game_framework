using System;
using System.Collections.Generic;

/// <summary>
/// 多个步骤需要处理的时候通过本处理对象进行的过程控制处理
/// </summary>
namespace ALPackage
{
    public class ALStepProcess : _AALProcess
    {
        /// <summary>
        /// 创建一个多过程过程对象
        /// </summary>
        /// <returns></returns>
        public static ALStepProcess CreateStepProcess(string _processTag) { return new ALStepProcess(_processTag); }

        /// <summary>
        /// 需要执行的操作队列
        /// </summary>
        private List<Action<Action>> _m_lDealActionList;
        //步骤统计对象
        private ALStepCounter _m_scStepCounter;

        protected ALStepProcess(string _processTag)
            : base(_processTag)
        {
            _m_lDealActionList = new List<Action<Action>>();
            
            _m_scStepCounter = new ALStepCounter();
        }

        /// <summary>
        /// 添加一个执行过程
        /// </summary>
        /// <param name="_process"></param>
        /// <returns></returns>
        public ALStepProcess addProcess(Action<Action> _action)
        {
            if(null == _action)
                return this;

            if(isRunning)
            {
                UnityEngine.Debug.LogError("add process when process started!");
                return this;
            }

            //加入队列返回
            _m_lDealActionList.Add(_action);

            return this;
        }

        /// <summary>
        /// 获取出来可以在监控对象外围补充输出的信息
        /// </summary>
        protected override string _processMonitorExInfo
        {
            get
            {
                return $"[Step: {_m_scStepCounter.doneStep}/{_m_scStepCounter.totalStep}]";
            }
        }

        /// <summary>
        /// 进行处理
        /// </summary>
        protected override void _dealProcess(_IALProcessMonitor _monitor)
        {
            //无数据则直接完成
            if(_m_lDealActionList.Count <= 0)
            {
                _onProcessDone();
                return;
            }

            //加入步骤
            _m_scStepCounter.chgTotalStepCount(_m_lDealActionList.Count);
            _m_scStepCounter.regAllDoneDelegate(_onProcessDone);
            //逐个直接开始
            Action<Action> tmpAction = null;
            for (int i = 0; i < _m_lDealActionList.Count; i++)
            {
                tmpAction = _m_lDealActionList[i];
                //数据无效则直接完成一步
                if (null == tmpAction)
                {
                    _m_scStepCounter.addDoneStepCount();
                    continue;
                }

                //处理过程
                try
                {
                    tmpAction(_m_scStepCounter.addDoneStepCount);
                }
                catch (Exception _ex)
                {
                    //调用回调
                    if (null != _monitor)
                    {
                        _monitor.onErr(ALCommon.getNowTimeMill() - processStartTimeMS, processTag, _processMonitorExInfo, _ex);
                    }
                    else
                    {
                        //报错
                        ALLog.Sys($"Step Process {processTag} Fail! {_processMonitorExInfo}");
                        UnityEngine.Debug.LogError(_ex.ToString());
                    }

                    //调用失败处理函数
                    _onProcessFailStop();
                }
            }

            _m_lDealActionList.Clear();
        }

        /// <summary>
        /// 当子处理对象完成时的处理
        /// </summary>
        protected override void _onChildDone()
        {
        }

        /// <summary>
        /// 重置处理
        /// </summary>
        protected override void _onDiscard()
        {
            _m_lDealActionList.Clear();
            _m_scStepCounter.resetAll();
        }

        /// <summary>
        /// 完成的处理，需要放回缓存池
        /// </summary>
        protected override void _onDone()
        {
        }
    }
}
