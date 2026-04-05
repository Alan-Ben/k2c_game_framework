using System;
using System.Collections.Generic;

namespace ALPackage
{
    public class ALFrameDelayActionMonoTask : _IALBaseMonoTask, _IALCommonTaskMonitorInterface
    {
        #region 对外接口
        //创建一个对应的任务
        protected static ALFrameDelayActionMonoTask _createTask(int _frameCount, Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container
#endif
            )
        {
            ALFrameDelayActionMonoTask task = ALFrameDelayActionTaskCache.instance.popItem();

#if UNITY_EDITOR
            //添加到Editor监控
            if (null != _container)
                _container.addMonitor(task);
#endif

            task._setAction(_frameCount, _delegate
#if UNITY_EDITOR
                , _container
#endif
                );

            return task;
        }
        /** 对外开放的任务创建操作函数 */
        public static void addMonoTask(int _frameCount, Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addMonoTask(_createTask(_frameCount, _delegate
#if UNITY_EDITOR
                , _container
#endif
                ));
        }
        public static void addMonoTask(int _frameCount, Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addMonoTask(_createTask(_frameCount, _delegate
#if UNITY_EDITOR
                , _container
#endif
                ), _delayTime);
        }

        public static void addScaleTimeDelayMonoTask(int _frameCount, Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addScaleTimeDelayMonoTask(_createTask(_frameCount, _delegate
#if UNITY_EDITOR
                , _container
#endif
                ), _delayTime);
        }
        public static void addScaleTimeDelayLaterMonoTask(int _frameCount, Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addScaleTimeDelayLaterMonoTask(_createTask(_frameCount, _delegate
#if UNITY_EDITOR
                , _container
#endif
                ), _delayTime);
        }

        public static void addNextFrameTask(int _frameCount, Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALMonoTaskMgr.instance.addNextFrameTask(_createTask(_frameCount, _delegate
#if UNITY_EDITOR
                , _container
#endif
                ));
        }
        #endregion

        /** 对外开放的任务创建操作函数终结 */

        private int _m_iFrameCount;
        private Action _m_dAction;
#if UNITY_EDITOR 
        private ALCommonTaskMonitorContainer _m_tmcTaskMonitor;
#endif

        protected ALFrameDelayActionMonoTask()
        {
            _m_iFrameCount = 0;
            _m_dAction = null;
#if UNITY_EDITOR
            _m_tmcTaskMonitor = null;
#endif
        }

        protected void _setAction(int _frameCount, Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container
#endif
            )
        {
#if UNITY_EDITOR
            if(_delegate == null)
            {
                UnityEngine.Debug.LogError("ALFrameDelayActionMonoTask 传入Action为空");
            }
#endif
            _m_iFrameCount = _frameCount;
            _m_dAction = _delegate;

#if UNITY_EDITOR
            _m_tmcTaskMonitor = _container;
#endif
        }

        public void deal()
        {
            _m_iFrameCount--;

            if(_m_iFrameCount > 0)
            {
                ALMonoTaskMgr.instance.addNextFrameTask(this);
                return;
            }

            if (null != _m_dAction)
                _m_dAction();
            _m_dAction = null;

#if UNITY_EDITOR
            if (null != _m_tmcTaskMonitor)
                _m_tmcTaskMonitor.rmvMonitor(this);
#endif

            //放回缓存
            ALFrameDelayActionTaskCache.instance.pushBackCacheItem(this);
        }

        protected void _reset()
        {
            _m_iFrameCount = 0;
            _m_dAction = null;
#if UNITY_EDITOR
            _m_tmcTaskMonitor = null;
#endif
        }

        /// <summary>
        /// 释放本任务对象的相关资源或者关联
        /// </summary>
        public void discard()
        {
            _m_iFrameCount = 0;
            _m_dAction = null;
#if UNITY_EDITOR
            _m_tmcTaskMonitor = null;
#endif
        }

        public override string ToString()
        {
            if(_m_dAction != null)
            {
                return $"ALFrameDelayActionMonoTask:{ALCommon.ToReadableString(_m_dAction)}";
            }
            else
            {
                return $"ALFrameDelayActionMonoTask:null";
            }
        }

        public class ALFrameDelayActionTaskCache : _AALUnsafeThreadCacheController<ALFrameDelayActionMonoTask, ALFrameDelayActionMonoTask>
        {
            private static ALFrameDelayActionTaskCache _g_instance = new ALFrameDelayActionTaskCache();
            public static ALFrameDelayActionTaskCache instance
            {
                get
                {
                    if(null == _g_instance)
                        _g_instance = new ALFrameDelayActionTaskCache();
                    return _g_instance;
                }
            }

            public ALFrameDelayActionTaskCache() : base(32, 128)
            {
                init(new ALFrameDelayActionMonoTask());
            }

            protected override ALFrameDelayActionMonoTask _createItem(ALFrameDelayActionMonoTask _template)
            {
                return new ALFrameDelayActionMonoTask();
            }

            //警告信息文字
            protected override string _warningTxt { get { return "ALFrameDelayActionTaskCache"; } }

            protected override void _discardItem(ALFrameDelayActionMonoTask _item)
            {
                _item.discard();
                return;
            }

            protected override void _onInit(ALFrameDelayActionMonoTask _template)
            {
            }

            protected override void _resetItem(ALFrameDelayActionMonoTask _item)
            {
                _item._reset();
            }
        }
    }
}
