using System;
using System.Collections.Generic;

namespace ALPackage
{
    internal class ALCommonEnableDurationActionMonoTask : ALCommonTaskController._AALEnableMonoTask, _IALBaseMonoTask, _IALCommonTaskMonitorInterface
    {
        #region 对外接口
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        protected static ALCommonEnableDurationActionMonoTask _createTask(Action _delegate, float _duration
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container
#endif
            )
        {
            ALCommonEnableDurationActionMonoTask task = ALCommonEnableDurationActionTaskCache.instance.popItem();

#if UNITY_EDITOR
            //添加到Editor监控
            if (null != _container)
                _container.addMonitor(task);
#endif

            task.setAction(_delegate, _duration
#if UNITY_EDITOR
                , _container
#endif
                );

            //注册
            ALCommonTaskController._AALEnableMonoTask.ALEnableMonoTaskMgr.instance.regTask(task);

            return task;
        }
        /** 对外开放的任务创建操作函数 */
        public static ALCommonEnableTaskController addMonoTask(Action _delegate, float _duration
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonEnableDurationActionMonoTask task = _createTask(_delegate, _duration
#if UNITY_EDITOR
                , _container
#endif
                );

            ALMonoTaskMgr.instance.addMonoTask(task);

            return new ALCommonEnableTaskController(task.serialize);
        }
        public static ALCommonEnableTaskController addMonoTask(Action _delegate, float _duration, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonEnableDurationActionMonoTask task = _createTask(_delegate, _duration
#if UNITY_EDITOR
                , _container
#endif
                );

            ALMonoTaskMgr.instance.addMonoTask(task, _delayTime);

            return new ALCommonEnableTaskController(task.serialize);
        }

        public static ALCommonEnableTaskController addScaleTimeDelayMonoTask(Action _delegate, float _duration, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonEnableDurationActionMonoTask task = _createTask(_delegate, _duration
#if UNITY_EDITOR
                , _container
#endif
                );

            ALMonoTaskMgr.instance.addScaleTimeDelayMonoTask(task, _delayTime);

            return new ALCommonEnableTaskController(task.serialize);
        }
        public static ALCommonEnableTaskController addScaleTimeDelayLaterMonoTask(Action _delegate, float _duration, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonEnableDurationActionMonoTask task = _createTask(_delegate, _duration
#if UNITY_EDITOR
                , _container
#endif
                );

            ALMonoTaskMgr.instance.addScaleTimeDelayLaterMonoTask(task, _delayTime);

            return new ALCommonEnableTaskController(task.serialize);
        }

        public static ALCommonEnableTaskController addNextFrameTask(Action _delegate, float _duration
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonEnableDurationActionMonoTask task = _createTask(_delegate, _duration
#if UNITY_EDITOR
                , _container
#endif
                );

            ALMonoTaskMgr.instance.addNextFrameTask(task);

            return new ALCommonEnableTaskController(task.serialize);
        }
        #endregion

        /** 对外开放的任务创建操作函数终结 */

        private Action _m_dAction;
        //间隔时间
        private float _m_fDuration;
#if UNITY_EDITOR 
        private ALCommonTaskMonitorContainer _m_tmcTaskMonitor;
#endif

        protected ALCommonEnableDurationActionMonoTask()
            : base()
        {
            _m_dAction = null;
#if UNITY_EDITOR
            _m_tmcTaskMonitor = null;
#endif
        }

        public void setAction(Action _delegate, float _duration
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
            _m_dAction = _delegate;
            _m_fDuration = _duration;

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
                ALCommonEnableDurationActionTaskCache.instance.pushBackCacheItem(this);
                _m_dAction = null;
                return;
            }

            if (null != _m_dAction)
                _m_dAction();

            //放入下一帧
            ALMonoTaskMgr.instance.addMonoTask(this, _m_fDuration);
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
                return $"ALCommonEnableDurationActionMonoTask:{ALCommon.ToReadableString(_m_dAction)}";
            }
            else
            {
                return $"ALCommonEnableDurationActionMonoTask:null";
            }
        }
        
        public class ALCommonEnableDurationActionTaskCache : _AALUnsafeThreadCacheController<ALCommonEnableDurationActionMonoTask, ALCommonEnableDurationActionMonoTask>
        {
            private static ALCommonEnableDurationActionTaskCache _g_instance = new ALCommonEnableDurationActionTaskCache();
            public static ALCommonEnableDurationActionTaskCache instance
            {
                get
                {
                    if(null == _g_instance)
                        _g_instance = new ALCommonEnableDurationActionTaskCache();
                    return _g_instance;
                }
            }

            public ALCommonEnableDurationActionTaskCache() : base(64, 256)
            {
                init(new ALCommonEnableDurationActionMonoTask());
            }

            protected override ALCommonEnableDurationActionMonoTask _createItem(ALCommonEnableDurationActionMonoTask _template)
            {
                return new ALCommonEnableDurationActionMonoTask();
            }

            //警告信息文字
            protected override string _warningTxt { get { return "ALCommonEnableDurationActionTaskCache"; } }

            protected override void _discardItem(ALCommonEnableDurationActionMonoTask _item)
            {
                _item.discard();
                return;
            }

            protected override void _onInit(ALCommonEnableDurationActionMonoTask _template)
            {
            }

            protected override void _resetItem(ALCommonEnableDurationActionMonoTask _item)
            {
                _item._reset();
            }
        }
    }
}
