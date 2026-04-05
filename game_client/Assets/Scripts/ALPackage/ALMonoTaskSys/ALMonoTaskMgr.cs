using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace ALPackage
{
    public class ALMonoTaskMgr
    {
        private static ALMonoTaskMgr _g_instance = new ALMonoTaskMgr();
        [NotNull]
        public static ALMonoTaskMgr instance
        {
            get
            {
                if (ReferenceEquals(null, _g_instance))
                    _g_instance = new ALMonoTaskMgr();

                return _g_instance;
            }
        }

        /** 正常任务对象列表 */
        private ALMonoTaskRealtimeSingleTypeMgr _m_tmMonoTaskMgr;
        private ALMonoTaskRealtimeSingleTypeMgr _m_tmLateMonoTaskMgr;
        private ALMonoTaskRealtimeSingleTypeMgr _m_tmFixedMonoTaskMgr;

        /** 根据时间缩放后的时间进行处理的任务管理对象 */
        private ALMonoTaskScaletimeSingleTypeMgr _m_tmScaletimeMonoTaskMgr;
        private ALMonoTaskScaletimeSingleTypeMgr _m_tmScaletimeLaterMonoTaskMgr;
        private ALMonoTaskFixedScaleTimeSingleTpeMgr _m_tmScaletimeFixedMonoTaskMgr;

        /** 需要在Update后放入处理任务队列的所有任务 */
        private List<_IALBaseMonoTask> _m_lNextFrameMonoTaskList;
        private List<_IALBaseMonoTask> _m_lNextFrameLaterMonoTaskList;
        private List<_IALBaseMonoTask> _m_lNextFixedUpdateMonoTaskList;

#if AL_UNITY_GUI
        /** GUI绘制部分执行的任务 */
        private ALMonoTaskRealtimeSingleTypeMgr _m_tmGuiMonoTaskMgr;
        private List<_IALBaseMonoTask> _m_lGuiNextFrameMonoTaskList;
#endif

        /** 
         * 由于获取unity的time时间是本帧开始的时间，与实际的真实时间不一定相符
         * 因此需要先缓存，在下一帧才将延迟时间的任务正式加入队列中
         * 原因是本帧加入任务的时间肯定比下一帧的调用时间提前
         **/
        private List<ALDelayMonoTaskInfo> _m_lCacheDelayTaskInfoList;
        private List<ALDelayMonoTaskInfo> _m_lFrameAddDelayTaskInfoList;

        private List<ALDelayMonoTaskInfo> _m_lCacheFixedDelayTaskInfoList;
        private List<ALDelayMonoTaskInfo> _m_lFrameAddFixedDelayTaskInfoList;
#if AL_UNITY_GUI
        private List<ALDelayMonoTaskInfo> _m_lCacheGuiDelayTaskInfoList;
        private List<ALDelayMonoTaskInfo> _m_lFrameAddGuiDelayTaskInfoList;
#endif

        /** 接受时间缩放处理的任务对象 */
        private List<ALDelayMonoTaskInfo> _m_lCacheDelayScaleTimeTaskInfoList;
        private List<ALDelayMonoTaskInfo> _m_lFrameAddDelayScaleTimeTaskInfoList;

        private List<ALDelayMonoTaskInfo> _m_lCacheFixedDelayScaleTimeTaskInfoList;
        private List<ALDelayMonoTaskInfo> _m_lFrameAddFixedDelayScaleTimeTaskInfoList;
        
        protected ALMonoTaskMgr()
        {
            _m_tmMonoTaskMgr = new ALMonoTaskRealtimeSingleTypeMgr(60, 300);
            _m_tmLateMonoTaskMgr = new ALMonoTaskRealtimeSingleTypeMgr(60, 300);
            _m_tmFixedMonoTaskMgr = new ALMonoTaskRealtimeSingleTypeMgr(60, 300);
            _m_tmScaletimeMonoTaskMgr = new ALMonoTaskScaletimeSingleTypeMgr(60, 300);
            _m_tmScaletimeLaterMonoTaskMgr = new ALMonoTaskScaletimeSingleTypeMgr(60, 300);
            _m_tmScaletimeFixedMonoTaskMgr = new ALMonoTaskFixedScaleTimeSingleTpeMgr(60, 300);

            _m_lNextFrameMonoTaskList = new List<_IALBaseMonoTask>();
            _m_lNextFrameLaterMonoTaskList = new List<_IALBaseMonoTask>();
            _m_lNextFixedUpdateMonoTaskList = new List<_IALBaseMonoTask>();

#if AL_UNITY_GUI
            _m_tmGuiMonoTaskMgr = new ALMonoTaskRealtimeSingleTypeMgr(60, 100);
            _m_lGuiNextFrameMonoTaskList = new List<_IALBaseMonoTask>();
#endif

            _m_lCacheDelayTaskInfoList = new List<ALDelayMonoTaskInfo>();
            _m_lFrameAddDelayTaskInfoList = new List<ALDelayMonoTaskInfo>();

            _m_lCacheFixedDelayTaskInfoList = new List<ALDelayMonoTaskInfo>();
            _m_lFrameAddFixedDelayTaskInfoList = new List<ALDelayMonoTaskInfo>();

#if AL_UNITY_GUI
            _m_lCacheGuiDelayTaskInfoList = new List<ALDelayMonoTaskInfo>();
            _m_lFrameAddGuiDelayTaskInfoList = new List<ALDelayMonoTaskInfo>();
#endif

            _m_lCacheDelayScaleTimeTaskInfoList = new List<ALDelayMonoTaskInfo>();
            _m_lFrameAddDelayScaleTimeTaskInfoList = new List<ALDelayMonoTaskInfo>();

            _m_lCacheFixedDelayScaleTimeTaskInfoList = new List<ALDelayMonoTaskInfo>();
            _m_lFrameAddFixedDelayScaleTimeTaskInfoList = new List<ALDelayMonoTaskInfo>();
        }

        /****************
         * 添加一个需要执行Coroutine的对象
         **/
        public void addMonoTask(_IALBaseMonoTask _task)
        {
            if (ReferenceEquals(null, _task))
                return;

            lock (_m_tmMonoTaskMgr)
            {
                _m_tmMonoTaskMgr.regTask(_task);
            }
        }
        public void addLaterMonoTask(_IALBaseMonoTask _task)
        {
            if (ReferenceEquals(null, _task))
                return;

            lock (_m_tmLateMonoTaskMgr)
            {
                _m_tmLateMonoTaskMgr.regTask(_task);
            }
        }
        public void addFixedMonoTask(_IALBaseMonoTask _task)
        {
            if (ReferenceEquals(null, _task))
                return;

            lock (_m_tmFixedMonoTaskMgr)
            {
                _m_tmFixedMonoTaskMgr.regTask(_task);
            }
        }
        public void addMonoTask(_IALBaseMonoTask _task, float _delayTime)
        {
#if UNITY_EDITOR
            if(_delayTime > 1000000L)
                UnityEngine.Debug.LogError("Add Mono Task Time Too Long: " + _delayTime);
#endif

            if (ReferenceEquals(null, _task))
                return;

            //时间为负数则立即插入队列
            if (_delayTime <= 0)
            {
                addMonoTask(_task);
                return;
            }

            lock (_m_lCacheDelayTaskInfoList)
            {
                //添加到延迟处理队列
                _m_lCacheDelayTaskInfoList.Add(new ALDelayMonoTaskInfo(_task, _delayTime, false));
            }
        }
        public void addLaterMonoTask(_IALBaseMonoTask _task, float _delayTime)
        {
#if UNITY_EDITOR
            if(_delayTime > 1000000L)
                UnityEngine.Debug.LogError("Add Mono Task Time Too Long: " + _delayTime);
#endif

            if(ReferenceEquals(null, _task))
                return;

            //时间为负数则立即插入队列
            if (_delayTime <= 0)
            {
                addLaterMonoTask(_task);
                return;
            }

            lock (_m_lCacheDelayTaskInfoList)
            {
                //添加到延迟处理队列
                _m_lCacheDelayTaskInfoList.Add(new ALDelayMonoTaskInfo(_task, _delayTime, true));
            }
        }
        public void addFixedMonoTask(_IALBaseMonoTask _task, float _delayTime)
        {
#if UNITY_EDITOR
            if(_delayTime > 1000000L)
                UnityEngine.Debug.LogError("Add Fixed Mono Task Time Too Long: " + _delayTime);
#endif

            if (ReferenceEquals(null, _task))
                return;

            //时间为负数则立即插入队列
            if (_delayTime <= 0)
            {
                addFixedMonoTask(_task);
                return;
            }

            lock (_m_lCacheFixedDelayTaskInfoList)
            {
                //添加到延迟处理队列
                _m_lCacheFixedDelayTaskInfoList.Add(new ALDelayMonoTaskInfo(_task, _delayTime, false));
            }
        }

        /****************
         * 添加一个需要执行Coroutine的对象
         **/
        public void addNextFrameTask(_IALBaseMonoTask _task)
        {
            if(ReferenceEquals(null, _task))
                return;

            lock(_m_tmMonoTaskMgr)
            {
                _m_lNextFrameMonoTaskList.Add(_task);
            }
        }
        public void addNextFrameLaterTask(_IALBaseMonoTask _task)
        {
            if(ReferenceEquals(null, _task))
                return;

            lock(_m_tmLateMonoTaskMgr)
            {
                _m_lNextFrameLaterMonoTaskList.Add(_task);
            }
        }
        public void addNextFixedUpdateTask(_IALBaseMonoTask _task)
        {
            if(ReferenceEquals(null, _task))
                return;

            lock(_m_tmFixedMonoTaskMgr)
            {
                _m_lNextFixedUpdateMonoTaskList.Add(_task);
            }
        }

#if AL_UNITY_GUI
        /******************
         * 添加一个需要在GUI部分执行的任务
         **/
        public void addGuiMonoTask(_IALBaseMonoTask _task)
        {
            if (ReferenceEquals(null, _task))
                return;

            lock (_m_tmGuiMonoTaskMgr)
            {
                _m_tmGuiMonoTaskMgr.regTask(_task);
            }
        }
        public void addGuiMonoTask(_IALBaseMonoTask _task, float _delayTime)
        {
#if UNITY_EDITOR
            if(_delayTime > 1000000L)
                UnityEngine.Debug.LogError("Add Mono Task Time Too Long: " + _delayTime);
#endif

            if(ReferenceEquals(null, _task))
                return;

            //时间为负数则立即插入队列
            if (_delayTime <= 0)
            {
                addGuiMonoTask(_task);
                return;
            }

            lock (_m_lCacheGuiDelayTaskInfoList)
            {
                //添加到延迟处理队列
                _m_lCacheGuiDelayTaskInfoList.Add(new ALDelayMonoTaskInfo(_task, _delayTime, false));
            }
        }
        public void addGuiNextFrameTask(_IALBaseMonoTask _task)
        {
            if (ReferenceEquals(null, _task))
                return;

            lock (_m_tmGuiMonoTaskMgr)
            {
                _m_lGuiNextFrameMonoTaskList.Add(_task);
            }
        }
#endif

        /*******************
         * 添加受时间缩放影响的延迟任务
         **/
        public void addScaleTimeDelayMonoTask(_IALBaseMonoTask _task, float _delayTime)
        {
#if UNITY_EDITOR
            if (_delayTime > 1000000L)
                UnityEngine.Debug.LogError("Add Mono Task Time Too Long: " + _delayTime);
#endif

            if (ReferenceEquals(null, _task))
                return;

            //时间为负数则立即插入队列
            if (_delayTime <= 0)
            {
                addMonoTask(_task);
                return;
            }

            lock (_m_lCacheDelayScaleTimeTaskInfoList)
            {
                //添加到延迟处理队列
                _m_lCacheDelayScaleTimeTaskInfoList.Add(new ALDelayMonoTaskInfo(_task, _delayTime, false));
            }
        }

        /*******************
         * 添加受时间缩放影响的延迟Late任务
         **/
        public void addScaleTimeDelayLaterMonoTask(_IALBaseMonoTask _task, float _delayTime)
        {
#if UNITY_EDITOR
            if (_delayTime > 1000000L)
                UnityEngine.Debug.LogError("Add Late Mono Task Time Too Long: " + _delayTime);
#endif

            if (ReferenceEquals(null, _task))
                return;

            //时间为负数则立即插入队列
            if (_delayTime <= 0)
            {
                addLaterMonoTask(_task);
                return;
            }

            lock (_m_lCacheDelayScaleTimeTaskInfoList)
            {
                //添加到延迟处理队列
                _m_lCacheDelayScaleTimeTaskInfoList.Add(new ALDelayMonoTaskInfo(_task, _delayTime, true));
            }
        }

        /*******************
         * 添加受时间缩放影响的延迟FixedUpdate任务
         **/
        public void addScaleTimeDelayFixedMonoTask(_IALBaseMonoTask _task, float _delayTime)
        {
#if UNITY_EDITOR
            if (_delayTime > 1000000L)
                UnityEngine.Debug.LogError("Add Fixed Mono Task Time Too Long: " + _delayTime);
#endif

            if (ReferenceEquals(null, _task))
                return;

            //时间为负数则立即插入队列
            if (_delayTime <= 0)
            {
                addFixedMonoTask(_task);
                return;
            }

            lock (_m_lCacheFixedDelayScaleTimeTaskInfoList)
            {
                //添加到延迟处理队列
                _m_lCacheFixedDelayScaleTimeTaskInfoList.Add(new ALDelayMonoTaskInfo(_task, _delayTime, false));
            }
        }
        
        /****************
         * 将下一帧需要执行的任务放入执行队列
         **/
        public void swapNextFrameTask()
        {
            //取出下一帧执行任务队列
            lock (_m_tmMonoTaskMgr)
            {
#if UNITY_EDITOR
                if(_m_lNextFrameMonoTaskList.Count > 1000)
                {
                    Dictionary<Type, int> classCount = new Dictionary<Type, int>();
                    _IALBaseMonoTask tmpTask = null;
                    for(int i = 0; i < _m_lNextFrameMonoTaskList.Count; i++)
                    {
                        tmpTask = _m_lNextFrameMonoTaskList[i];
                        if(null == tmpTask)
                            continue;

                        Type tt = tmpTask.GetType();
                        if(classCount.ContainsKey(tt))
                        {
                            classCount[tt] = classCount[tt] + 1;
                        }
                        else
                        {
                            classCount[tt] = 1;
                        }
                    }

                    UnityEngine.Debug.LogError($"next frame task too much! more than 1000 - [{_m_lNextFrameMonoTaskList.Count}]");

                    foreach(Type type in classCount.Keys)
                    {
                        UnityEngine.Debug.LogError($"Type[{type.ToString()}] has count: [{classCount[type]}]");
                    }
                }
#endif

                //批量插入
                _m_tmMonoTaskMgr.regTask(_m_lNextFrameMonoTaskList);
                //清空队列
                _m_lNextFrameMonoTaskList.Clear();
            }
            lock(_m_tmLateMonoTaskMgr)
            {
#if UNITY_EDITOR
                if (_m_lNextFrameLaterMonoTaskList.Count > 1000)
                {
                    Dictionary<Type, int> classCount = new Dictionary<Type, int>();
                    _IALBaseMonoTask tmpTask = null;
                    for(int i = 0; i < _m_lNextFrameLaterMonoTaskList.Count; i++)
                    {
                        tmpTask = _m_lNextFrameLaterMonoTaskList[i];
                        if(null == tmpTask)
                            continue;

                        Type tt = tmpTask.GetType();
                        if(classCount.ContainsKey(tt))
                        {
                            classCount[tt] = classCount[tt] + 1;
                        }
                        else
                        {
                            classCount[tt] = 1;
                        }
                    }

                    UnityEngine.Debug.LogError($"next frame later task too much! more than 1000 - [{_m_lNextFrameLaterMonoTaskList.Count}]");

                    foreach(Type type in classCount.Keys)
                    {
                        UnityEngine.Debug.LogError($"Type[{type.ToString()}] has count: [{classCount[type]}]");
                    };
                }
#endif
                //批量插入
                _m_tmLateMonoTaskMgr.regTask(_m_lNextFrameLaterMonoTaskList);
                //清空队列
                _m_lNextFrameLaterMonoTaskList.Clear();
            }

            //将有延迟时间的任务从缓存队列转入下一帧需要加入实际监控的队列
            lock (_m_lCacheDelayTaskInfoList)
            {
                _m_lFrameAddDelayTaskInfoList.AddRange(_m_lCacheDelayTaskInfoList);
                _m_lCacheDelayTaskInfoList.Clear();
            }
            lock (_m_lCacheDelayScaleTimeTaskInfoList)
            {
                _m_lFrameAddDelayScaleTimeTaskInfoList.AddRange(_m_lCacheDelayScaleTimeTaskInfoList);
                _m_lCacheDelayScaleTimeTaskInfoList.Clear();
            }

            //将当前帧注册的任务注册到下一帧处理中
            regDelayTimerMonoTask();
            regDelayScaleTimeTimerMonoTask();
        }

        public void swapNextFixedUpdateTask()
        {
            //取出下一帧执行任务队列
            lock (_m_tmFixedMonoTaskMgr)
            {
#if UNITY_EDITOR
                if(_m_lNextFixedUpdateMonoTaskList.Count > 1000)
                {
                    Dictionary<Type, int> classCount = new Dictionary<Type, int>();
                    _IALBaseMonoTask tmpTask = null;
                    for(int i = 0; i < _m_lNextFixedUpdateMonoTaskList.Count; i++)
                    {
                        tmpTask = _m_lNextFixedUpdateMonoTaskList[i];
                        if(null == tmpTask)
                            continue;

                        Type tt = tmpTask.GetType();
                        if(classCount.ContainsKey(tt))
                        {
                            classCount[tt] = classCount[tt] + 1;
                        }
                        else
                        {
                            classCount[tt] = 1;
                        }
                    }

                    UnityEngine.Debug.LogError($"next fixed update task too much! more than 1000 - [{_m_lNextFixedUpdateMonoTaskList.Count}]");

                    foreach(Type type in classCount.Keys)
                    {
                        UnityEngine.Debug.LogError($"Type[{type.ToString()}] has count: [{classCount[type]}]");
                    }
                }
#endif
                //批量插入
                _m_tmFixedMonoTaskMgr.regTask(_m_lNextFixedUpdateMonoTaskList);
                //清空队列
                _m_lNextFixedUpdateMonoTaskList.Clear();
            }

            //将有延迟时间的任务从缓存队列转入下一帧需要加入实际监控的队列
            lock (_m_lCacheFixedDelayTaskInfoList)
            {
                _m_lFrameAddFixedDelayTaskInfoList.AddRange(_m_lCacheFixedDelayTaskInfoList);
                _m_lCacheFixedDelayTaskInfoList.Clear();
            }
            lock (_m_lCacheFixedDelayScaleTimeTaskInfoList)
            {
                _m_lFrameAddFixedDelayScaleTimeTaskInfoList.AddRange(_m_lCacheFixedDelayScaleTimeTaskInfoList);
                _m_lCacheFixedDelayScaleTimeTaskInfoList.Clear();
            }

            //将当前帧注册的任务注册到下一帧处理中
            regDelayTimerFixedMonoTask();
            regDelayScaleTimeTimerFixedMonoTask();
        }
#if AL_UNITY_GUI
        public void swapGuiNextFrameTask()
        {
            //取出下一帧GUI任务
            lock (_m_tmGuiMonoTaskMgr)
            {
                //批量插入
                _m_tmGuiMonoTaskMgr.regTask(_m_lGuiNextFrameMonoTaskList);
                //清空队列
                _m_lGuiNextFrameMonoTaskList.Clear();
            }

            lock (_m_lCacheGuiDelayTaskInfoList)
            {
                _m_lFrameAddGuiDelayTaskInfoList.AddRange(_m_lCacheGuiDelayTaskInfoList);
                _m_lCacheGuiDelayTaskInfoList.Clear();
            }

            //将当前帧注册的任务注册到下一帧处理中
            regDelayTimerGuiMonoTask();
        }
#endif

        /****************
         * 取出一个需要执行的Coroutine对象
         **/
        public _IALBaseMonoTask popMonoTask()
        {
            lock (_m_tmMonoTaskMgr)
            {
                return _m_tmMonoTaskMgr.popCurrentTask();
            }
        }
        
        public _IALBaseMonoTask popLaterMonoTask()
        {
            lock (_m_tmLateMonoTaskMgr)
            {
                return _m_tmLateMonoTaskMgr.popCurrentTask();
            }
        }
        
        public _IALBaseMonoTask popFixedMonoTask()
        {
            lock (_m_tmFixedMonoTaskMgr)
            {
                return _m_tmFixedMonoTaskMgr.popCurrentTask();
            }
        }
        
        public _IALBaseMonoTask popScaletimeMonoTask()
        {
            lock(_m_tmScaletimeMonoTaskMgr)
            {
                return _m_tmScaletimeMonoTaskMgr.popCurrentTask();
            }
        }
        
        public _IALBaseMonoTask popScaletimeLaterMonoTask()
        {
            lock(_m_tmScaletimeLaterMonoTaskMgr)
            {
                return _m_tmScaletimeLaterMonoTaskMgr.popCurrentTask();
            }
        }
        
        public _IALBaseMonoTask popScaletimeFixedMonoTask()
        {
            lock(_m_tmScaletimeFixedMonoTaskMgr)
            {
                return _m_tmScaletimeFixedMonoTaskMgr.popCurrentTask();
            }
        }

#if AL_UNITY_GUI
        public void resetGuiMonoTask()
        {
            lock (_m_tmGuiMonoTaskMgr)
            {
                _m_tmGuiMonoTaskMgr.resetCurrentTask();
            }
        }
        public _IALBaseMonoTask popGuiMonoTask()
        {
            lock (_m_tmGuiMonoTaskMgr)
            {
                return _m_tmGuiMonoTaskMgr.popCurrentTask();
            }
        }
#endif

        /***************************
         * 处理定时处理的相关任务
         **/
        public void dealTimerMonoTask()
        {
            //进行定时检测
            _m_tmMonoTaskMgr.frameCheckTask();
        }
        public void dealTimerLaterMonoTask()
        {
            //进行定时检测
            _m_tmLateMonoTaskMgr.frameCheckTask();
        }
        public void dealTimerFixedMonoTask()
        {
            //进行定时检测
            _m_tmFixedMonoTaskMgr.frameCheckTask();
        }

#if AL_UNITY_GUI
        public void dealTimerGuiMonoTask()
        {
            //进行定时检测
            _m_tmGuiMonoTaskMgr.frameCheckTask();
        }
#endif
        public void dealScaleTimeTimerMonoTask()
        {
            //进行定时检测
            _m_tmScaletimeMonoTaskMgr.frameCheckTask();
        }
        public void dealScaleTimeTimerLaterMonoTask()
        {
            //进行定时检测
            _m_tmScaletimeLaterMonoTaskMgr.frameCheckTask();
        }
        public void dealScaleTimeTimerFixedMonoTask()
        {
            //进行定时检测
            _m_tmScaletimeFixedMonoTaskMgr.frameCheckTask();
        }

        /// <summary>
        /// 将需要延迟的任务添加到对应队列中
        /// </summary>
        public void regDelayTimerMonoTask()
        {
            //将上一帧添加的延迟任务放入实际任务队列
            for(int i = 0; i < _m_lFrameAddDelayTaskInfoList.Count; i++)
            {
                ALDelayMonoTaskInfo info = _m_lFrameAddDelayTaskInfoList[i];
                if(ReferenceEquals(null, info))
                    continue;

                //放入管理对象
                if(info.isLateTask)
                    _m_tmLateMonoTaskMgr.regTask(info.task, info.delayTime);
                else
                    _m_tmMonoTaskMgr.regTask(info.task, info.delayTime);
            }
            //清空处理了的队列
            _m_lFrameAddDelayTaskInfoList.Clear();
        }
        public void regDelayTimerFixedMonoTask()
        {
            //将上一帧添加的延迟任务放入实际任务队列
            for(int i = 0; i < _m_lFrameAddFixedDelayTaskInfoList.Count; i++)
            {
                ALDelayMonoTaskInfo info = _m_lFrameAddFixedDelayTaskInfoList[i];
                if(ReferenceEquals(null, info))
                    continue;

                //放入管理对象
                _m_tmFixedMonoTaskMgr.regTask(info.task, info.delayTime);
            }
            //清空处理了的队列
            _m_lFrameAddFixedDelayTaskInfoList.Clear();
        }
#if AL_UNITY_GUI
        public void regDelayTimerGuiMonoTask()
        {
            //将上一帧添加的延迟任务放入实际任务队列
            for(int i = 0; i < _m_lFrameAddGuiDelayTaskInfoList.Count; i++)
            {
                ALDelayMonoTaskInfo info = _m_lFrameAddGuiDelayTaskInfoList[i];
                if(ReferenceEquals(null, info))
                    continue;

                //放入管理对象
                _m_tmGuiMonoTaskMgr.regTask(info.task, info.delayTime);
            }
            //清空处理了的队列
            _m_lFrameAddGuiDelayTaskInfoList.Clear();
        }
#endif
        public void regDelayScaleTimeTimerMonoTask()
        {
            //将上一帧添加的延迟任务放入实际任务队列
            for(int i = 0; i < _m_lFrameAddDelayScaleTimeTaskInfoList.Count; i++)
            {
                ALDelayMonoTaskInfo info = _m_lFrameAddDelayScaleTimeTaskInfoList[i];
                if(ReferenceEquals(null, info))
                    continue;

                //放入管理对象
                if (info.isLateTask)
                    _m_tmScaletimeLaterMonoTaskMgr.regTask(info.task, info.delayTime);
                else
                    _m_tmScaletimeMonoTaskMgr.regTask(info.task, info.delayTime);
            }
            //清空处理了的队列
            _m_lFrameAddDelayScaleTimeTaskInfoList.Clear();
        }

        public void regDelayScaleTimeTimerFixedMonoTask()
        {
            //将上一帧添加的延迟任务放入实际任务队列
            for(int i = 0; i < _m_lFrameAddFixedDelayScaleTimeTaskInfoList.Count; i++)
            {
                ALDelayMonoTaskInfo info = _m_lFrameAddFixedDelayScaleTimeTaskInfoList[i];
                if(ReferenceEquals(null, info))
                    continue;

                //放入管理对象
                _m_tmScaletimeFixedMonoTaskMgr.regTask(info.task, info.delayTime);
            }
            //清空处理了的队列
            _m_lFrameAddFixedDelayScaleTimeTaskInfoList.Clear();
        }
    }
}
