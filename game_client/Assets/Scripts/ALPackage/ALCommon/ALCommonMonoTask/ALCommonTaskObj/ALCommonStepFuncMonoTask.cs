using System;
using System.Collections.Generic;

namespace ALPackage
{
    /// <summary>
    /// 步骤处理类，带入三个回调，分别是执行函数，完成回调，失败回调。
    /// 在执行完成执行函数后，将执行完成回调，在执行函数过程出错则执行失败回调
    /// </summary>
    public class ALCommonStepFuncMonoTask : _IALBaseMonoTask, _IALCommonTaskMonitorInterface
    {
        #region 对外接口
        /// <summary>
        /// 创建一个对应的任务，最后一个参数代表是否失败也执行完成
        /// 切记不要重复添加到任务管理器中
        /// </summary>
        /// <param name="_process"></param>
        /// <param name="_doneDelegate"></param>
        /// <param name="_failDelegate"></param>
        /// <param name="_isFailDoDone"></param>
        /// <returns></returns>
        protected internal static ALCommonStepFuncMonoTask _createTask(Func<bool> _process, Action _doneDelegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container
#endif
            , Action _failDelegate = null, bool _isFailDoDone = false)
        {
            ALCommonStepFuncMonoTask task = ALCommonStepProcessTaskCache.instance.popItem();
            task._setAction(_process, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );

            return task;
        }
        /** 对外开放的任务创建操作函数 */
        public static void addMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addMonoTask(_createTask(_action, _doneDelegate
#if UNITY_EDITOR
                , _container
#endif
                , _failDelegate, _isFailDoDone));
        }
        public static void addMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addMonoTask(_createTask(_action, _doneDelegate
#if UNITY_EDITOR
                , _container
#endif
                , _failDelegate, _isFailDoDone), _delayTime);
        }

        public static void addLaterMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addLaterMonoTask(_createTask(_action, _doneDelegate
#if UNITY_EDITOR
                , _container
#endif
                , _failDelegate, _isFailDoDone));
        }
        public static void addLaterMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addLaterMonoTask(_createTask(_action, _doneDelegate
#if UNITY_EDITOR
                , _container
#endif
                , _failDelegate, _isFailDoDone), _delayTime);
        }

        public static void addFixedMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addFixedMonoTask(_createTask(_action, _doneDelegate
#if UNITY_EDITOR
                , _container
#endif
                , _failDelegate, _isFailDoDone));
        }
        public static void addFixedMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addFixedMonoTask(_createTask(_action, _doneDelegate
#if UNITY_EDITOR
                , _container
#endif
                , _failDelegate, _isFailDoDone), _delayTime);
        }

#if AL_UNITY_GUI
        public static void addGuiMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addGuiMonoTask(_createTask(_action, _doneDelegate
#if UNITY_EDITOR
                , _container
#endif
                , _failDelegate, _isFailDoDone));
        }
        public static void addGuiMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addGuiMonoTask(_createTask(_action, _doneDelegate
#if UNITY_EDITOR
                , _container
#endif
                , _failDelegate, _isFailDoDone), _delayTime);
        }
#endif

        public static void addScaleTimeDelayMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addScaleTimeDelayMonoTask(_createTask(_action, _doneDelegate
#if UNITY_EDITOR
                , _container
#endif
                , _failDelegate, _isFailDoDone), _delayTime);
        }
        public static void addScaleTimeDelayLaterMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addScaleTimeDelayLaterMonoTask(_createTask(_action, _doneDelegate
#if UNITY_EDITOR
                , _container
#endif
                , _failDelegate, _isFailDoDone), _delayTime);
        }
        public static void addScaleTimeDelayFixedMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addScaleTimeDelayFixedMonoTask(_createTask(_action, _doneDelegate
#if UNITY_EDITOR
                , _container
#endif
                , _failDelegate, _isFailDoDone), _delayTime);
        }

        public static void addNextFrameTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addNextFrameTask(_createTask(_action, _doneDelegate
#if UNITY_EDITOR
                , _container
#endif
                , _failDelegate, _isFailDoDone));
        }
        public static void addNextFrameLaterTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addNextFrameLaterTask(_createTask(_action, _doneDelegate
#if UNITY_EDITOR
                , _container
#endif
                , _failDelegate, _isFailDoDone));
        }
        public static void addNextFixedUpdateTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addNextFixedUpdateTask(_createTask(_action, _doneDelegate
#if UNITY_EDITOR
                , _container
#endif
                , _failDelegate, _isFailDoDone));
        }
#if AL_UNITY_GUI
        public static void addGuiNextFrameTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addGuiNextFrameTask(_createTask(_action, _doneDelegate
#if UNITY_EDITOR
                , _container
#endif
                , _failDelegate, _isFailDoDone));
        }
#endif
        #endregion

        /** 对外开放的任务创建操作函数终结 */

        //执行回调
        private Func<bool> _m_dAction;
        //完成回调
        private Action _m_dDoneAction;
        //失败回调
        private Action _m_dFailAction;
        //是否失败的时候也执行完成函数
        private bool _m_bIsFailDoDone;
#if UNITY_EDITOR 
        private ALCommonTaskMonitorContainer _m_tmcTaskMonitor;
#endif

        protected ALCommonStepFuncMonoTask()
        {
            _m_dAction = null;
            _m_dDoneAction = null;
            _m_dFailAction = null;
            _m_bIsFailDoDone = false;
#if UNITY_EDITOR
            _m_tmcTaskMonitor = null;
#endif
        }

        protected void _setAction(Func<bool> _process, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
#if UNITY_EDITOR
            if(_process == null)
            {
                UnityEngine.Debug.LogError("ALCommonStepProcessMonoTask 传入Action为空");
            }
#endif
            _m_dAction = _process;
            _m_dDoneAction = _doneDelegate;
            _m_dFailAction = _failDelegate;
            _m_bIsFailDoDone = _isFailDoDone;

#if UNITY_EDITOR
            _m_tmcTaskMonitor = _container;
#endif
        }

        public void deal()
        {
            try
            {
                bool res = true;
                if(null != _m_dAction)
                    res = _m_dAction();

                //执行完成后执行完成函数，根据结果判断是否正常
                if(res)
                {
                    if(null != _m_dDoneAction)
                        _m_dDoneAction();
                }
                else
                {
                    if(null != _m_dFailAction)
                        _m_dFailAction();

                    //判断失败是否需要执行成功函数
                    if(_m_bIsFailDoDone)
                    {
                        //执行成功函数
                        if(null != _m_dDoneAction)
                            _m_dDoneAction();
                    }
                    else
                    {
                        ALLog.Sys("One Multi Process Fail!");
                    }
                }
            }
            catch(Exception _ex)
            {
                UnityEngine.Debug.LogError(_ex.ToString());

                //执行失败函数
                if(null != _m_dFailAction)
                    _m_dFailAction();

                //判断失败是否需要执行成功函数
                if(_m_bIsFailDoDone)
                {
                    //执行成功函数
                    if(null != _m_dDoneAction)
                        _m_dDoneAction();
                }
                else
                {
                    ALLog.Sys("One Multi Process Fail!");
                }
            }
            finally
            {
                _m_dAction = null;
                _m_dDoneAction = null;
                _m_dFailAction = null;
                _m_bIsFailDoDone = false;
            }

#if UNITY_EDITOR
            if (null != _m_tmcTaskMonitor)
                _m_tmcTaskMonitor.rmvMonitor(this);
#endif

            //放回缓存
            ALCommonStepProcessTaskCache.instance.pushBackCacheItem(this);
        }

        protected void _reset()
        {
            _m_dAction = null;
            _m_dDoneAction = null;
            _m_dFailAction = null;
            _m_bIsFailDoDone = false;
#if UNITY_EDITOR
            _m_tmcTaskMonitor = null;
#endif
        }

        /// <summary>
        /// 释放本任务对象的相关资源或者关联
        /// </summary>
        public void discard()
        {
            _m_dAction = null;
            _m_dDoneAction = null;
            _m_dFailAction = null;
            _m_bIsFailDoDone = false;
#if UNITY_EDITOR
            _m_tmcTaskMonitor = null;
#endif
        }

        public override string ToString()
        {
            if(_m_dAction != null)
            {
                return $"ALCommonStepProcessMonoTask:{ALCommon.ToReadableString(_m_dAction)}";
            }
            else
            {
                return $"ALCommonStepProcessMonoTask:null";
            }
        }

        public class ALCommonStepProcessTaskCache : _AALUnsafeThreadCacheController<ALCommonStepFuncMonoTask, ALCommonStepFuncMonoTask>
        {
            private static ALCommonStepProcessTaskCache _g_instance = new ALCommonStepProcessTaskCache();
            public static ALCommonStepProcessTaskCache instance
            {
                get
                {
                    if(null == _g_instance)
                        _g_instance = new ALCommonStepProcessTaskCache();
                    return _g_instance;
                }
            }

            public ALCommonStepProcessTaskCache() : base(32, 128)
            {
                init(new ALCommonStepFuncMonoTask());
            }

            protected override ALCommonStepFuncMonoTask _createItem(ALCommonStepFuncMonoTask _template)
            {
                return new ALCommonStepFuncMonoTask();
            }

            //警告信息文字
            protected override string _warningTxt { get { return "ALCommonStepProcessTaskCache"; } }

            protected override void _discardItem(ALCommonStepFuncMonoTask _item)
            {
                _item.discard();
                return;
            }

            protected override void _onInit(ALCommonStepFuncMonoTask _template)
            {
            }

            protected override void _resetItem(ALCommonStepFuncMonoTask _item)
            {
                _item._reset();
            }
        }
    }
}
