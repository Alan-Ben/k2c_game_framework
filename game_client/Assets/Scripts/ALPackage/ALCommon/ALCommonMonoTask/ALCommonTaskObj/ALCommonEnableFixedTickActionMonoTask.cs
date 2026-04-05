using System;
using System.Collections.Generic;

namespace ALPackage
{
    internal class ALCommonEnableFixedTickActionMonoTask : ALCommonTaskController._AALEnableMonoTask, _IALBaseMonoTask, _IALCommonTaskMonitorInterface
    {
        #region 对外接口
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        protected static ALCommonEnableFixedTickActionMonoTask _createTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container
#endif
            )
        {
            ALCommonEnableFixedTickActionMonoTask task = ALCommonEnableFixedTickActionTaskCache.instance.popItem();

#if UNITY_EDITOR
            //添加到Editor监控
            if (null != _container)
                _container.addMonitor(task);
#endif

            task.setAction(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );

            //注册
            ALCommonTaskController._AALEnableMonoTask.ALEnableMonoTaskMgr.instance.regTask(task);

            return task;
        }
        /** 对外开放的任务创建操作函数 */
        public static ALCommonEnableTaskController addFixedMonoTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonEnableFixedTickActionMonoTask task = _createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );

            ALMonoTaskMgr.instance.addFixedMonoTask(task);

            return new ALCommonEnableTaskController(task.serialize);
        }
        public static ALCommonEnableTaskController addFixedMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonEnableFixedTickActionMonoTask task = _createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );

            ALMonoTaskMgr.instance.addFixedMonoTask(task, _delayTime);

            return new ALCommonEnableTaskController(task.serialize);
        }

        public static ALCommonEnableTaskController addNextFixedUpdateTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonEnableFixedTickActionMonoTask task = _createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );

            ALMonoTaskMgr.instance.addNextFixedUpdateTask(task);

            return new ALCommonEnableTaskController(task.serialize);
        }
        
        public static ALCommonEnableTaskController addScaleTimeDelayFixedMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonEnableFixedTickActionMonoTask task = _createTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );

            ALMonoTaskMgr.instance.addScaleTimeDelayFixedMonoTask(task, _delayTime);

            return new ALCommonEnableTaskController(task.serialize);
        }
        #endregion

        /** 对外开放的任务创建操作函数终结 */

        private Action _m_dAction;
#if UNITY_EDITOR 
        private ALCommonTaskMonitorContainer _m_tmcTaskMonitor;
#endif

        protected ALCommonEnableFixedTickActionMonoTask()
            : base()
        {
            _m_dAction = null;
#if UNITY_EDITOR
            _m_tmcTaskMonitor = null;
#endif
        }

        public void setAction(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container
#endif
            )
        {
#if UNITY_EDITOR
            if(_delegate == null)
            {
                UnityEngine.Debug.LogError("ALCommonEnableFixedTickActionMonoTask 传入Action为空");
            }
#endif
            _m_dAction = _delegate;

#if UNITY_EDITOR
            _m_tmcTaskMonitor = _container;
#endif
        }

        public override void deal()
        {
            if(!_m_bIsEnable)
            {
#if UNITY_EDITOR
                if (null != _m_tmcTaskMonitor)
                    _m_tmcTaskMonitor.rmvMonitor(this);
#endif
                
                //注销
                ALCommonTaskController._AALEnableMonoTask.ALEnableMonoTaskMgr.instance.popTask(serialize);
                //放回缓存
                ALCommonEnableFixedTickActionTaskCache.instance.pushBackCacheItem(this);
                _m_dAction = null;
                return;
            }
            
            if(null != _m_dAction)
                _m_dAction();

            //放入下一帧
            ALMonoTaskMgr.instance.addNextFixedUpdateTask(this);
        }
        
        /// <summary>
        /// 重载几个重置接口
        /// </summary>
        protected override void _onDisable()
        {
            _m_dAction = null;
#if UNITY_EDITOR
            if (null != _m_tmcTaskMonitor)
                _m_tmcTaskMonitor.rmvMonitor(this);
            _m_tmcTaskMonitor = null;
#endif
        }
        protected override void _onReset()
        {
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
            //调用基类的reset
            _reset();
        }

        public override string ToString()
        {
            if(_m_dAction != null)
            {
                return $"ALCommonEnableFixedTickActionMonoTask:{ALCommon.ToReadableString(_m_dAction)}";
            }
            else
            {
                return $"ALCommonEnableFixedTickActionMonoTask:null";
            }
        }

        public class ALCommonEnableFixedTickActionTaskCache : _AALUnsafeThreadCacheController<ALCommonEnableFixedTickActionMonoTask, ALCommonEnableFixedTickActionMonoTask>
        {
            private static ALCommonEnableFixedTickActionTaskCache _g_instance = new ALCommonEnableFixedTickActionTaskCache();
            public static ALCommonEnableFixedTickActionTaskCache instance
            {
                get
                {
                    if(null == _g_instance)
                        _g_instance = new ALCommonEnableFixedTickActionTaskCache();
                    return _g_instance;
                }
            }

            public ALCommonEnableFixedTickActionTaskCache() : base(64, 256)
            {
                init(new ALCommonEnableFixedTickActionMonoTask());
            }

            protected override ALCommonEnableFixedTickActionMonoTask _createItem(ALCommonEnableFixedTickActionMonoTask _template)
            {
                return new ALCommonEnableFixedTickActionMonoTask();
            }

            //警告信息文字
            protected override string _warningTxt { get { return "ALCommonEnableTickActionTaskCache"; } }

            protected override void _discardItem(ALCommonEnableFixedTickActionMonoTask _item)
            {
                _item.discard();
                return;
            }

            protected override void _onInit(ALCommonEnableFixedTickActionMonoTask _template)
            {
            }

            protected override void _resetItem(ALCommonEnableFixedTickActionMonoTask _item)
            {
                _item._reset();
            }
        }
    }
}
