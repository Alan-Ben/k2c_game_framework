using System;
using System.Collections.Generic;

/*********************
 * 加载步骤的管理对象
 **/
namespace ALPackage
{
    public class ALProcess : _AALProcess
    {
        /// <summary>
        /// 创建一个过程对象
        /// </summary>
        /// <returns></returns>
        public static ALProcess CreateProcess() { return CreateProcess(string.Empty); }
        public static ALProcess CreateProcess(string _processTag) { return new ALProcess(_processTag); }
        /// <summary>
        /// 创建一个多过程过程对象
        /// </summary>
        /// <returns></returns>
        public static ALMultiProcess CreateMultiProcess() { return CreateMultiProcess(string.Empty); }
        public static ALMultiProcess CreateMultiProcess(string _processTag) { return ALMultiProcess.CreateMultiProcess(_processTag); }
        /// <summary>
        /// 创建一个异步的处理对象
        /// </summary>
        /// <returns></returns>
        public static ALAsynProcess CreateAsynProcess() { return CreateAsynProcess(string.Empty); }
        public static ALAsynProcess CreateAsynProcess(string _processTag) { return ALAsynProcess.CreateAsynProcess(_processTag); }
        /// <summary>
        /// 创建一个执行时再创建步骤的处理过程
        /// </summary>
        /// <param name="_process">返回一个步骤对象，当步骤对象设置完成的时候将完成本步骤</param>
        /// <returns></returns>
        public static ALActionProcess CreateActionProcess(Action<ALProcess> _process) { return CreateActionProcess(_process, string.Empty); }
        public static ALActionProcess CreateActionProcess(Action<ALProcess> _process, string _processTag) { return ALActionProcess.CreateFuncProcess(_process, _processTag); }
        /// <summary>
        /// 创建一个步骤处理对象
        /// </summary>
        /// <returns></returns>
        public static ALStepProcess CreateStepProcess() { return CreateStepProcess(string.Empty); }
        public static ALStepProcess CreateStepProcess(string _processTag) { return ALStepProcess.CreateStepProcess(_processTag); }
        /// <summary>
        /// 创建一个使用回调调用完成的处理对象
        /// </summary>
        /// <returns></returns>
        public static ALDelegateProcess CreateDelegateProcess() { return CreateDelegateProcess(string.Empty); }
        public static ALDelegateProcess CreateDelegateProcess(string _processTag) { return ALDelegateProcess.CreateDelegateProcess(_processTag); }

        /// <summary>
        /// 执行的任务队列
        /// </summary>
        private List<_AALProcessBasicMonoTask> _m_lDealTaskList;

        protected ALProcess(string _processTag)
            : base(_processTag)
        {
            _m_lDealTaskList = new List<_AALProcessBasicMonoTask>();
        }

        /// <summary>
        /// 添加一个执行过程
        /// </summary>
        /// <param name="_process"></param>
        /// <returns></returns>
        public ALProcess addProcess(_AALProcess _process)
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
        public ALProcess addAsyncProcess(_AALProcess _process)
        {
            if (null == _process)
                return this;

            if (isRunning)
            {
                UnityEngine.Debug.LogError("add process when process started!");
                return this;
            }

            ALAsynProcess asynProcess = CreateAsynProcess().setProcess(_process);

            //加入队列返回
            _m_lDealTaskList.Add(asynProcess);
            //设置父节点
            asynProcess.setParentProcess(this);

            return this;
        }

        /// <summary>
        /// 增加一个过程处理，如果处理过程出现异常则调用失败处理
        /// </summary>
        /// <param name="_processAction"></param>
        /// <param name="_failDelegate"></param>
        /// <param name="_isFailContinue">是否出错了仍然继续</param>
        /// <returns>返回自身，可以继续添加步骤</returns>
        public ALProcess addProcess(Action _processAction, Action _failDelegate = null, bool _isFailContinue = false)
        {
            if(isRunning)
            {
                UnityEngine.Debug.LogError("add process action when process started!");
                return this;
            }

            //创建任务放入队列
            _m_lDealTaskList.Add(new ALProcessStepActionMonoTask(_processAction, processTag, _m_lDealTaskList.Count, _dealNextProcess
                , _failDelegate, _isFailContinue, _onProcessFailStop));

            return this;
        }
        public ALProcess addProcess(Action _processAction, string _processTag, Action _failDelegate = null, bool _isFailContinue = false)
        {
            if(isRunning)
            {
                UnityEngine.Debug.LogError("add process action when process started!");
                return this;
            }

            //创建任务放入队列
            _m_lDealTaskList.Add(new ALProcessStepActionMonoTask(_processAction, _processTag, _m_lDealTaskList.Count, _dealNextProcess
                , _failDelegate, _isFailContinue, _onProcessFailStop));

            return this;
        }
        public ALProcess addProcess(Func<bool> _processAction, Action _failDelegate = null, bool _isFailContinue = false)
        {
            if(isRunning)
            {
                UnityEngine.Debug.LogError("add process when process started!");
                return this;
            }

            //创建任务放入队列
            _m_lDealTaskList.Add(new ALProcessStepFuncMonoTask(_processAction, processTag, _m_lDealTaskList.Count, _dealNextProcess
                , _failDelegate, _isFailContinue, _onProcessFailStop));

            return this;
        }

        /// <summary>
        /// 添加一个异步处理函数
        /// </summary>
        /// <param name="_process"></param>
        /// <returns></returns>
        public ALProcess addActionProcess(Action<ALProcess> _process)
        {
            return addActionProcess(_process, string.Empty);
        }
        public ALProcess addActionProcess(Action<ALProcess> _process, string _processTag)
        {
            if (null == _process)
                return this;

            if (isRunning)
            {
                UnityEngine.Debug.LogError("add process when process started!");
                return this;
            }

            ALActionProcess funcProcess = CreateActionProcess(_process, _processTag);
            //加入队列返回
            _m_lDealTaskList.Add(funcProcess);
            //设置父节点
            funcProcess.setParentProcess(this);

            return this;
        }

        /// <summary>
        /// 添加使用回调确认完成的处理
        /// </summary>
        /// <param name="_delegateAction"></param>
        /// <param name="_failDelegate"></param>
        /// <param name="_isFailContinue"></param>
        /// <returns></returns>
        public ALProcess addDelegateProcess(Action<Action> _delegateAction, Action _failDelegate = null, bool _isFailContinue = false)
        {
            return addDelegateProcess(_delegateAction, string.Empty, _failDelegate, _isFailContinue);
        }
        public ALProcess addDelegateProcess(Action<Action> _delegateAction, string _processTag, Action _failDelegate = null, bool _isFailContinue = false)
        {
            if (isRunning)
            {
                UnityEngine.Debug.LogError("add process action when process started!");
                return this;
            }

            addProcess(ALProcess.CreateDelegateProcess(_processTag).setProcess(_delegateAction, _failDelegate, _isFailContinue));

            return this;
        }

        /// <summary>
        /// 一次性加入多个步骤，同时每个步骤完成时会调用对应的回调
        /// 最终所有步骤完成表示大步骤完成
        /// </summary>
        /// <param name="_processAction"></param>
        /// <returns></returns>
        public ALProcess addStepProcess(params Action<Action>[] _stepAction)
        {
            return addStepProcess(string.Empty, _stepAction);
        }
        public ALProcess addStepProcess(string _processTag, params Action<Action>[] _stepAction)
        {
            if (isRunning)
            {
                UnityEngine.Debug.LogError("add process when process started!");
                return this;
            }

            //创建任务放入队列
            ALStepProcess stepP = CreateStepProcess(_processTag);
            for (int i = 0; i < _stepAction.Length; i++)
            {
                if (null == _stepAction[i])
                    continue;

                stepP.addProcess(_stepAction[i]);
            }

            //添加到节点
            addProcess(stepP);

            return this;
        }

        /// <summary>
        /// 一次性加入多个并行操作
        /// </summary>
        /// <param name="_processAction"></param>
        /// <returns></returns>
        public ALProcess addMultiProcess(params Action[] _processAction)
        {
            return addMultiProcess(string.Empty, _processAction);
        }
        public ALProcess addMultiProcess(string _processTag, params Action[] _processAction)
        {
            if(isRunning)
            {
                UnityEngine.Debug.LogError("add process when process started!");
                return this;
            }

            //创建任务放入队列
            ALMultiProcess multiP = CreateMultiProcess(_processTag);
            for(int i = 0; i < _processAction.Length; i++)
            {
                if(null == _processAction[i])
                    continue;

                multiP.addMultiProcess(_processAction[i]);
            }

            //添加到节点
            addProcess(multiP);

            return this;
        }

        public ALProcess addMultiProcess(params Func<bool>[] _processAction)
        {
            return addMultiProcess(string.Empty, _processAction);
        }
        public ALProcess addMultiProcess(string _processTag, params Func<bool>[] _processAction)
        {
            if(isRunning)
            {
                UnityEngine.Debug.LogError("add process when process started!");
                return this;
            }

            //创建任务放入队列
            ALMultiProcess multiP = CreateMultiProcess(_processTag);
            for(int i = 0; i < _processAction.Length; i++)
            {
                if(null == _processAction[i])
                    continue;

                multiP.addMultiProcess(_processAction[i]);
            }

            //添加到节点
            addProcess(multiP);

            return this;
        }

        public ALProcess addMultiProcess(params ALProcess[] _process)
        {
            return addMultiProcess(string.Empty, _process);
        }
        public ALProcess addMultiProcess(string _processTag, params ALProcess[] _process)
        {
            if(isRunning)
            {
                UnityEngine.Debug.LogError("add process when process started!");
                return this;
            }

            //创建任务放入队列
            ALMultiProcess multiP = CreateMultiProcess(_processTag);
            for(int i = 0; i < _process.Length; i++)
            {
                if(null == _process[i])
                    continue;

                multiP.addProcess(_process[i]);
            }

            //添加到节点
            addProcess(multiP);

            return this;
        }

        /// <summary>
        /// 重置处理
        /// </summary>
        protected override void _onDiscard()
        {
            _m_lDealTaskList.Clear();
        }

        /// <summary>
        /// 获取出来可以在监控对象外围补充输出的信息
        /// </summary>
        protected override string _processMonitorExInfo
        {
            get
            {
                return $"[ALProcess: [{_m_lDealTaskList.Count}] Task Remain]";
            }
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        protected override void _dealProcess(_IALProcessMonitor _monitor)
        {
            //设置监控对象
            setMonitor(_monitor);

            //处理下一个任务
            _dealNextProcess();
        }
        /// <summary>
        /// 处理下一个任务
        /// </summary>
        protected void _dealNextProcess()
        {
            //如果已经不继续执行，则直接返回
            if (!_m_cController.enable)
            {
                //此时直接执行过程完成
                _onProcessDone();
                return;
            }

            if (null == _m_lDealTaskList || _m_lDealTaskList.Count <= 0)
            {
                //完结
                _onProcessDone();
                return;
            }

            //取出第一个进行处理
            _AALProcessBasicMonoTask processObj = _m_lDealTaskList[0];
            while(null == processObj && _m_lDealTaskList.Count >= 0)
            {
                //移除第一个
                _m_lDealTaskList.RemoveAt(0);
                //重新获取第一个对象
                processObj = _m_lDealTaskList[0];
            }

            //如有数据则删除第一个对象
            if(null == processObj)
            {
                _onProcessDone();
                return;
            }

            //移除数据
            _m_lDealTaskList.RemoveAt(0);

            //强制刷新监控步骤的开始时间
            _forceRefreshProcessStartTime();
            //判断对象是否步骤类，是则需要设置监控对象
            processObj.setMonitor(monitor);

            //进行数据的处理
            ALMonoTaskMgr.instance.addMonoTask(processObj);
        }

        /// <summary>
        /// 当子处理对象完成时的处理
        /// </summary>
        protected override void _onChildDone()
        {
            _dealNextProcess();
        }

        /// <summary>
        /// 完成的处理，需要放回缓存池
        /// </summary>
        protected override void _onDone()
        {
        }
    }
}
