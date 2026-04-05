using System;
using System.Collections.Generic;

/*********************
 * 多个步骤同时执行的执行对象
 **/
namespace ALPackage
{
    public class ALMultiProcess : _AALProcess
    {
        /// <summary>
        /// 创建一个多过程过程对象
        /// </summary>
        /// <returns></returns>
        public static ALMultiProcess CreateMultiProcess(string _processTag) { return new ALMultiProcess(_processTag); }

        /// <summary>
        /// 执行的任务队列
        /// </summary>
        private List<_AALProcessBasicMonoTask> _m_lDealTaskList;
        //步骤统计对象
        private ALStepCounter _m_scStepCounter;

        protected ALMultiProcess(string _processTag)
            : base(_processTag)
        {
            _m_lDealTaskList = new List<_AALProcessBasicMonoTask>();
            
            _m_scStepCounter = new ALStepCounter();
        }

        /// <summary>
        /// 添加一个执行过程
        /// </summary>
        /// <param name="_process"></param>
        /// <returns></returns>
        public ALMultiProcess addProcess(_AALProcess _process)
        {
            if(null == _process)
                return this;

            if(isRunning)
            {
                UnityEngine.Debug.LogError("add process when process started!");
                return this;
            }

            //加入队列返回
            _m_lDealTaskList.Add(_process);
            //设置父节点
            _process.setParentProcess(this);

            return this;
        }

        /// <summary>
        /// 增加一个过程处理，如果处理过程出现异常则调用失败处理
        /// </summary>
        /// <param name="_processAction"></param>
        /// <param name="_failDelegate"></param>
        /// <param name="_isFailContinue">是否出错了仍然继续</param>
        /// <returns>返回自身，可以继续添加步骤</returns>
        public ALMultiProcess addMultiProcess(Action _processAction, Action _failDelegate = null, bool _isFailContinue = true)
        {
            if(isRunning)
            {
                UnityEngine.Debug.LogError("add process action when process started!");
                return this;
            }

            //创建任务放入队列
            _m_lDealTaskList.Add(new ALProcessStepActionMonoTask(_processAction, processTag, _m_lDealTaskList.Count, _m_scStepCounter.addDoneStepCount
                , _failDelegate, _isFailContinue, _onProcessFailStop));

            return this;
        }
        public ALMultiProcess addMultiProcess(Func<bool> _processAction, Action _failDelegate = null, bool _isFailContinue = true)
        {
            if(isRunning)
            {
                UnityEngine.Debug.LogError("add process when process started!");
                return this;
            }

            //创建任务放入队列
            _m_lDealTaskList.Add(new ALProcessStepFuncMonoTask(_processAction, processTag, _m_lDealTaskList.Count, _m_scStepCounter.addDoneStepCount
                , _failDelegate, _isFailContinue, _onProcessFailStop));

            return this;
        }

        /// <summary>
        /// 获取出来可以在监控对象外围补充输出的信息
        /// </summary>
        protected override string _processMonitorExInfo
        {
            get
            {
                return $"[Multi: {_m_scStepCounter.doneStep}/{_m_scStepCounter.totalStep}]";
            }
        }

        /// <summary>
        /// 进行处理
        /// </summary>
        protected override void _dealProcess(_IALProcessMonitor _monitor)
        {
            //无数据则直接完成
            if(_m_lDealTaskList.Count <= 0)
            {
                _onProcessDone();
                return;
            }

            //加入步骤
            _m_scStepCounter.chgTotalStepCount(_m_lDealTaskList.Count);
            _m_scStepCounter.regAllDoneDelegate(_onProcessDone);
            //逐个直接开始
            for(int i = 0; i < _m_lDealTaskList.Count; i++)
            {
                //判断过程是否步骤类，是则可以设置监控对象
                _m_lDealTaskList[i].setMonitor(_monitor);

                ALMonoTaskMgr.instance.addMonoTask(_m_lDealTaskList[i]);
            }

            _m_lDealTaskList.Clear();
        }

        /// <summary>
        /// 当子处理对象完成时的处理
        /// </summary>
        protected override void _onChildDone()
        {
            _m_scStepCounter.addDoneStepCount();
        }

        /// <summary>
        /// 重置处理
        /// </summary>
        protected override void _onDiscard()
        {
            _m_lDealTaskList.Clear();
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
