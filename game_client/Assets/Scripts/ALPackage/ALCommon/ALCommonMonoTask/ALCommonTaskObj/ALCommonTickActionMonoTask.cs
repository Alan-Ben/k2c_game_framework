using System;
using System.Collections.Generic;

namespace ALPackage
{
    public class ALCommonTickActionMonoTask : _IALBaseMonoTask, _IALCommonTaskMonitorInterface
    {
        #region 对外接口
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        protected static ALCommonTickActionMonoTask _createTask(Func<bool> _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container
#endif
            )
        {
            ALCommonTickActionMonoTask task = ALCommonTickActionTaskCache.instance.popItem();

#if UNITY_EDITOR
            //添加到Editor监控
            if (null != _container)
                _container.addMonitor(task);
#endif

            task.setFunc(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );

            return task;
        }
        /** 对外开放的任务创建操作函数 */
        public static ALCommonTickActionMonoTask addMonoTask(Func<bool> _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonTickActionMonoTask task = _createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );

            ALMonoTaskMgr.instance.addMonoTask(task);

            return task;
        }

        public static ALCommonTickActionMonoTask addNextFrameTask(Func<bool> _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonTickActionMonoTask task = _createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );

            ALMonoTaskMgr.instance.addNextFrameTask(task);

            return task;
        }
        #endregion

        /** 对外开放的任务创建操作函数终结 */

        private Func<bool> _m_dFunc;
#if UNITY_EDITOR 
        private ALCommonTaskMonitorContainer _m_tmcTaskMonitor;
#endif

        protected ALCommonTickActionMonoTask()
            : base()
        {
            _m_dFunc = null;
#if UNITY_EDITOR
            _m_tmcTaskMonitor = null;
#endif
        }

        public void setFunc(Func<bool> _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container
#endif
            )
        {
#if UNITY_EDITOR
            if(_delegate == null)
            {
                UnityEngine.Debug.LogError("ALCommonEnableGUITickActionMonoTask 传入Action为空");
            }
#endif
            _m_dFunc = _delegate;

#if UNITY_EDITOR
            _m_tmcTaskMonitor = _container;
#endif
        }

        public void deal()
        {
            //是否下一帧继续
            bool continueNextFrame = true;
            if (null != _m_dFunc)
                continueNextFrame = _m_dFunc();

            //如返回不循环则直接处理销毁操作
            if(!continueNextFrame)
            {
                //此时处理释放
#if UNITY_EDITOR
                if (null != _m_tmcTaskMonitor)
                    _m_tmcTaskMonitor.rmvMonitor(this);
#endif

                //放回缓存
                ALCommonTickActionTaskCache.instance.pushBackCacheItem(this);
                _m_dFunc = null;
                return;
            }

            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }

        /// <summary>
        /// 缓存体系重置的处理
        /// </summary>
        protected void _reset()
        {
            _m_dFunc = null;
#if UNITY_EDITOR
            _m_tmcTaskMonitor = null;
#endif
        }

        /// <summary>
        /// 释放本任务对象的相关资源或者关联
        /// </summary>
        public void discard()
        {
            _m_dFunc = null;
#if UNITY_EDITOR
            _m_tmcTaskMonitor = null;
#endif
        }

        public override string ToString()
        {
            if(_m_dFunc != null)
            {
                return $"ALCommonEnableTickActionMonoTask:{ALCommon.ToReadableString(_m_dFunc)}";
            }
            else
            {
                return $"ALCommonEnableTickActionMonoTask:null";
            }
        }
        
        public class ALCommonTickActionTaskCache : _AALUnsafeThreadCacheController<ALCommonTickActionMonoTask, ALCommonTickActionMonoTask>
        {
            private static ALCommonTickActionTaskCache _g_instance = new ALCommonTickActionTaskCache();
            public static ALCommonTickActionTaskCache instance
            {
                get
                {
                    if(null == _g_instance)
                        _g_instance = new ALCommonTickActionTaskCache();
                    return _g_instance;
                }
            }

            public ALCommonTickActionTaskCache() : base(16, 128)
            {
                init(new ALCommonTickActionMonoTask());
            }

            protected override ALCommonTickActionMonoTask _createItem(ALCommonTickActionMonoTask _template)
            {
                return new ALCommonTickActionMonoTask();
            }

            //警告信息文字
            protected override string _warningTxt { get { return "ALCommonEnableTickActionTaskCache"; } }

            protected override void _discardItem(ALCommonTickActionMonoTask _item)
            {
                _item.discard();
                return;
            }

            protected override void _onInit(ALCommonTickActionMonoTask _template)
            {
            }

            protected override void _resetItem(ALCommonTickActionMonoTask _item)
            {
                _item._reset();
            }
        }
    }
}
