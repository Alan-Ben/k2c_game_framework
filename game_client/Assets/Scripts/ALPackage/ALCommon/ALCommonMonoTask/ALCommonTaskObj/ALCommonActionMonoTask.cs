using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ALPackage
{
    public class ALCommonActionMonoTask : _IALBaseMonoTask, _IALCommonTaskMonitorInterface
    {
        #region 对外接口
        //创建一个对应的任务
        protected static internal ALCommonActionMonoTask _createTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container 
#endif
            )
        {
            if(null == _delegate)
                return null;

            ALCommonActionMonoTask task = ALCommonActionTaskCache.instance.popItem();

#if UNITY_EDITOR
            //添加到Editor监控
            if (null != _container)
                _container.addMonitor(task);
#endif

            task._setAction(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );

            return task;
        }
        /** 对外开放的任务创建操作函数 */
        public static void addMonoTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addMonoTask(_createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                ));
        }
        public static void addMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addMonoTask(_createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                ), _delayTime);
        }

        public static void addLaterMonoTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addLaterMonoTask(_createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                ));
        }
        public static void addLaterMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addLaterMonoTask(_createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                ), _delayTime);
        }

        public static void addFixedMonoTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addFixedMonoTask(_createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                ));
        }
        public static void addFixedMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addFixedMonoTask(_createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                ), _delayTime);
        }

#if AL_UNITY_GUI
        public static void addGuiMonoTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addGuiMonoTask(_createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                ));
        }
        public static void addGuiMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addGuiMonoTask(_createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                ), _delayTime);
        }
#endif

        public static void addScaleTimeDelayMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addScaleTimeDelayMonoTask(_createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                ), _delayTime);
        }
        public static void addScaleTimeDelayLaterMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addScaleTimeDelayLaterMonoTask(_createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                ), _delayTime);
        }
        public static void addScaleTimeDelayFixedMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addScaleTimeDelayFixedMonoTask(_createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                ), _delayTime);
        }

        public static void addNextFrameTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addNextFrameTask(_createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                ));
        }
        public static void addNextFrameLaterTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addNextFrameLaterTask(_createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                ));
        }
        public static void addNextFixedUpdateTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addNextFixedUpdateTask(_createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                ));
        }
#if AL_UNITY_GUI
        public static void addGuiNextFrameTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addGuiNextFrameTask(_createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                ));
        }
#endif
        #endregion

        /** 对外开放的任务创建操作函数终结 */

        private Action _m_dAction;
#if UNITY_EDITOR 
        private ALCommonTaskMonitorContainer _m_tmcTaskMonitor;
#endif


        protected ALCommonActionMonoTask()
        {
            _m_dAction = null;
#if UNITY_EDITOR
            _m_tmcTaskMonitor = null;
#endif
        }

    protected void _setAction(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container
#endif
            )
        {
#if UNITY_EDITOR
            if(_delegate == null)
            {
                UnityEngine.Debug.LogError("ALCommonActionMonoTask 传入Action为空");
            }
#endif
            _m_dAction = _delegate;

#if UNITY_EDITOR
            _m_tmcTaskMonitor = _container;
#endif
        }

        public void deal()
        {
            if (null != _m_dAction)
                _m_dAction();
            _m_dAction = null;

#if UNITY_EDITOR
            if (null != _m_tmcTaskMonitor)
                _m_tmcTaskMonitor.rmvMonitor(this);
#endif

            //放回缓存
            ALCommonActionTaskCache.instance.pushBackCacheItem(this);
        }

        /// <summary>
        /// 释放本任务对象的相关资源或者关联
        /// </summary>
        public void discard()
        {
            _m_dAction = null;
#if UNITY_EDITOR
            _m_tmcTaskMonitor = null;
#endif
        }

        protected void _reset()
        {
            _m_dAction = null;
#if UNITY_EDITOR
            _m_tmcTaskMonitor = null;
#endif
        }

        public override string ToString()
        {
            if(_m_dAction != null)
            {
                return $"ALCommonActionMonoTask:{ALCommon.ToReadableString(_m_dAction)}";
            }
            else
            {
                return $"ALCommonActionMonoTask:null";
            }
        }

        public class ALCommonActionTaskCache : _AALUnsafeThreadCacheController<ALCommonActionMonoTask, ALCommonActionMonoTask>
        {
            private static ALCommonActionTaskCache _g_instance = new ALCommonActionTaskCache();
            [NotNull]
            public static ALCommonActionTaskCache instance
            {
                get
                {
                    if(null == _g_instance)
                        _g_instance = new ALCommonActionTaskCache();
                    return _g_instance;
                }
            }

            public ALCommonActionTaskCache() : base(64, 256)
            {
                init(new ALCommonActionMonoTask());
            }

            protected override ALCommonActionMonoTask _createItem(ALCommonActionMonoTask _template)
            {
                return new ALCommonActionMonoTask();
            }

            //警告信息文字
            protected override string _warningTxt { get { return "ALCommonActionTaskCache"; } }

            protected override void _discardItem(ALCommonActionMonoTask _item)
            {
                _item.discard();
                return;
            }

            protected override void _onInit(ALCommonActionMonoTask _template)
            {
            }

            protected override void _resetItem(ALCommonActionMonoTask _item)
            {
                _item._reset();
            }
        }
    }
}
