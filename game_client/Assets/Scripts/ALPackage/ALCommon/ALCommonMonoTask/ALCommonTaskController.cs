using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 根据序列号控制对应有效变量的控制对象
    /// </summary>
    public struct ALCommonEnableTaskController
    {
        private long _m_lTaskSerialize;

        public ALCommonEnableTaskController(long _serialize)
        {
            _m_lTaskSerialize = _serialize;
        }

        /// <summary>
        /// 设置任务无效
        /// </summary>
        public void setDisable()
        {
            ALCommonTaskController._AALEnableMonoTask task = ALCommonTaskController._AALEnableMonoTask.ALEnableMonoTaskMgr.instance.popTask(_m_lTaskSerialize);
            if(null == task)
                return;

            task.setDisable();
        }
    }

    /// <summary>
    /// 通用任务的控制对象，当每次这个对象处理后，任务将会被设置为空
    /// </summary>
    internal class ALCommonTaskController
    {
        /** ALCommonActionMonoTask */
        public static void CommonActionAddMonoTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonActionMonoTask.addMonoTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonActionAddMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonActionMonoTask.addMonoTask(_delegate, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }

        public static void CommonActionAddLaterMonoTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonActionMonoTask.addLaterMonoTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonActionAddLaterMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonActionMonoTask.addLaterMonoTask(_delegate, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }

        public static void CommonActionAddFixedMonoTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonActionMonoTask.addFixedMonoTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonActionAddFixedMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonActionMonoTask.addFixedMonoTask(_delegate, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }

#if AL_UNITY_GUI
        public static void CommonActionAddGuiMonoTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonActionMonoTask.addGuiMonoTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonActionAddGuiMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonActionMonoTask.addGuiMonoTask(_delegate, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }
#endif

        public static void CommonActionAddScaleTimeDelayMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonActionMonoTask.addScaleTimeDelayMonoTask(_delegate, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonActionAddScaleTimeDelayFixedMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonActionMonoTask.addScaleTimeDelayFixedMonoTask(_delegate, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }

        public static void CommonActionAddNextFrameTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonActionMonoTask.addNextFrameTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonActionAddNextFrameLaterTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonActionMonoTask.addNextFrameLaterTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonActionAddNextFixedUpdateTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonActionMonoTask.addNextFixedUpdateTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
#if AL_UNITY_GUI
        public static void CommonActionAddGuiNextFrameTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonActionMonoTask.addGuiNextFrameTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
#endif
        /** 对外开放的任务创建操作函数终结 */


#if AL_UNITY_GUI
        /** ALCommonEnableGUITickActionMonoTask */
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableGUITickActionAddGuiMonoTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableGUITickActionMonoTask.addGuiMonoTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <param name="_delayTime"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableGUITickActionAddGuiMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableGUITickActionMonoTask.addGuiMonoTask(_delegate, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableGUITickActionAddGuiNextFrameTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableGUITickActionMonoTask.addGuiNextFrameTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
#endif
        /** 对外开放的任务创建操作函数终结 */


        /** ALCommonEnableLateTickActionMonoTask */
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableLateTickActionAddLaterMonoTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableLateTickActionMonoTask.addLaterMonoTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <param name="_delayTime"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableLateTickActionAddLaterMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableLateTickActionMonoTask.addLaterMonoTask(_delegate, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableLateTickActionAddNextFrameLaterTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableLateTickActionMonoTask.addNextFrameLaterTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /** 对外开放的任务创建操作函数终结 */

        #region ALCommonEnableTickActionMonoTask
        /** ALCommonEnableTickActionMonoTask */
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddMonoTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableTickActionMonoTask.addMonoTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <param name="_delayTime"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableTickActionMonoTask.addMonoTask(_delegate, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <param name="_delayTime"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddScaleTimeDelayMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableTickActionMonoTask.addScaleTimeDelayMonoTask(_delegate, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddNextFrameTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableTickActionMonoTask.addNextFrameTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /** 对外开放的任务创建操作函数终结 */
        #endregion

        #region ALCommonTickActionMonoTask
        /** ALCommonEnableTickActionMonoTask */
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonTickActionMonoTask CommonTickActionAddMonoTask(Func<bool> _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonTickActionMonoTask.addMonoTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonTickActionMonoTask CommonEnableTickActionAddNextFrameTask(Func<bool> _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonTickActionMonoTask.addNextFrameTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /** 对外开放的任务创建操作函数终结 */
        #endregion


        #region ALCommonEnableDurationActionMonoTask
        /** ALCommonEnableDurationActionMonoTask */
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableDurationActionAddMonoTask(Action _delegate, float _duration
#if UNITY_EDITOR
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableDurationActionMonoTask.addMonoTask(_delegate, _duration
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <param name="_delayTime"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableDurationActionAddMonoTask(Action _delegate, float _duration, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableDurationActionMonoTask.addMonoTask(_delegate, _duration, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <param name="_delayTime"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableDurationActionAddScaleTimeDelayMonoTask(Action _delegate, float _duration, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableDurationActionMonoTask.addScaleTimeDelayMonoTask(_delegate, _duration, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableDurationActionAddNextFrameTask(Action _delegate, float _duration
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableDurationActionMonoTask.addNextFrameTask(_delegate, _duration
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /** 对外开放的任务创建操作函数终结 */
        #endregion

        /** ALCommonEnableTickActionMonoTask */
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddFixedMonoTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableFixedTickActionMonoTask.addFixedMonoTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <param name="_delayTime"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddFixedMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableFixedTickActionMonoTask.addFixedMonoTask(_delegate, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <param name="_delayTime"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddScaleTimeDelayFixedMonoTask(Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableFixedTickActionMonoTask.addScaleTimeDelayFixedMonoTask(_delegate, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddNextFixedUpdateTask(Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            return ALCommonEnableFixedTickActionMonoTask.addNextFixedUpdateTask(_delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /** 对外开放的任务创建操作函数终结 */
        
        /** ALCommonStepActionMonoTask */
        public static void CommonStepActionAddMonoTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepActionMonoTask.addMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonStepActionAddMonoTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepActionMonoTask.addMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }

        public static void CommonStepActionAddLaterMonoTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepActionMonoTask.addLaterMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonStepActionAddLaterMonoTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepActionMonoTask.addLaterMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }

        public static void CommonStepActionAddFixedMonoTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepActionMonoTask.addFixedMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonStepActionAddFixedMonoTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepActionMonoTask.addFixedMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }

#if AL_UNITY_GUI
        public static void CommonStepActionAddGuiMonoTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepActionMonoTask.addGuiMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonStepActionAddGuiMonoTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepActionMonoTask.addGuiMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }
#endif

        public static void CommonStepActionAddScaleTimeDelayMonoTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepActionMonoTask.addScaleTimeDelayMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonStepActionAddScaleTimeDelayFixedMonoTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepActionMonoTask.addScaleTimeDelayFixedMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }

        public static void CommonStepActionAddNextFrameTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepActionMonoTask.addNextFrameTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonStepActionAddNextFrameLaterTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepActionMonoTask.addNextFrameLaterTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonStepActionAddNextFixedUpdateTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepActionMonoTask.addNextFixedUpdateTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }

#if AL_UNITY_GUI
        public static void CommonStepActionAddGuiNextFrameTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepActionMonoTask.addGuiNextFrameTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }
#endif
        /** 对外开放的任务创建操作函数终结 */

        /** ALCommonStepProcessMonoTask */
        public static void CommonStepProcessAddMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepFuncMonoTask.addMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonStepProcessAddMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepFuncMonoTask.addMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }

        public static void CommonStepProcessAddLaterMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepFuncMonoTask.addLaterMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonStepProcessAddLaterMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepFuncMonoTask.addLaterMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }

        public static void CommonStepProcessAddFixedMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepFuncMonoTask.addFixedMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonStepProcessAddFixedMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepFuncMonoTask.addFixedMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }

#if AL_UNITY_GUI
        public static void CommonStepProcessAddGuiMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepProcessMonoTask.addGuiMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonStepProcessAddGuiMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepProcessMonoTask.addGuiMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }
#endif

        public static void CommonStepProcessAddScaleTimeDelayMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepFuncMonoTask.addScaleTimeDelayMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonStepProcessAddScaleTimeDelayFixedMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepFuncMonoTask.addScaleTimeDelayFixedMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }

        public static void CommonStepProcessAddNextFrameTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepFuncMonoTask.addNextFrameTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonStepProcessAddNextFrameLaterTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepFuncMonoTask.addNextFrameLaterTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void CommonStepProcessAddNextFixedUpdateTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepFuncMonoTask.addNextFixedUpdateTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }

#if AL_UNITY_GUI
        public static void CommonStepProcessAddGuiNextFrameTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALCommonStepProcessMonoTask.addGuiNextFrameTask(_action, _doneDelegate, _failDelegate, _isFailDoDone
#if UNITY_EDITOR
                , _container
#endif
                );
        }
#endif
        /** 对外开放的任务创建操作函数终结 */

        /** 对外开放的任务创建操作函数 */
        public static void FrameDelayActionAddMonoTask(int _frameCount, Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALFrameDelayActionMonoTask.addMonoTask(_frameCount, _delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        public static void FrameDelayActionAddMonoTask(int _frameCount, Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALFrameDelayActionMonoTask.addMonoTask(_frameCount, _delegate, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }

        public static void FrameDelayActionAddScaleTimeDelayMonoTask(int _frameCount, Action _delegate, float _delayTime
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALFrameDelayActionMonoTask.addScaleTimeDelayMonoTask(_frameCount, _delegate, _delayTime
#if UNITY_EDITOR
                , _container
#endif
                );
        }

        public static void FrameDelayActionAddNextFrameTask(int _frameCount, Action _delegate
#if UNITY_EDITOR 
            , ALCommonTaskMonitorContainer _container = null
#endif
            )
        {
            ALFrameDelayActionMonoTask.addNextFrameTask(_frameCount, _delegate
#if UNITY_EDITOR
                , _container
#endif
                );
        }
        /** 对外开放的任务创建操作函数终结 */

        /// <summary>
        /// 可用于enable管理控制的monotask基类
        /// </summary>
        public abstract class _AALEnableMonoTask : _IALBaseMonoTask
        {
            private static long _g_lSerialize = 1;

            private long _m_lSerialize;
            protected bool _m_bIsEnable;

            protected _AALEnableMonoTask()
            {
                _m_lSerialize = 0;
                _m_bIsEnable = true;
            }

            public long serialize { get { return _m_lSerialize; } }

            //设置是否有效
            public void setDisable()
            {
                _m_bIsEnable = false;

                //处理无效操作
                _onDisable();
            }

            /// <summary>
            /// 刷新序列号
            /// </summary>
            protected void _refreshSerialize()
            {
                _m_lSerialize = _g_lSerialize++;
            }
            /// <summary>
            /// 重置信息
            /// </summary>
            protected void _reset()
            {
                _m_bIsEnable = true;

                //处理重置操作
                _onReset();
            }

            //处理函数
            public abstract void deal();

            //无效时的处理函数，一般用于释放资源
            protected abstract void _onDisable();
            protected abstract void _onReset();


            /// <summary>
            /// 所用可用enable管理控制的任务注册管理器
            /// </summary>
            public class ALEnableMonoTaskMgr
            {
                private static ALEnableMonoTaskMgr _g_instance = new ALEnableMonoTaskMgr();
                public static ALEnableMonoTaskMgr instance
                {
                    get
                    {
                        if(null == _g_instance)
                            _g_instance = new ALEnableMonoTaskMgr();

                        return _g_instance;
                    }
                }

                //任务对象
                private Dictionary<long, _AALEnableMonoTask> _m_dicTask;

                protected ALEnableMonoTaskMgr()
                {
                    _m_dicTask = new Dictionary<long, _AALEnableMonoTask>();
                }

                /// <summary>
                /// 注册并执行一个任务
                /// </summary>
                /// <param name="_task"></param>
                public long regTask(_AALEnableMonoTask _task)
                {
                    if(null == _task)
                        return 0;

                    if(_task._m_lSerialize != 0)
                    {
                        ALLog.Error("Reg a Task when the task is reged!");
                        return 0;
                    }

                    //刷新序列号
                    _task._refreshSerialize();
                    long serialize = _task._m_lSerialize;

                    //使用序列号注册
                    _m_dicTask.Add(serialize, _task);

                    return serialize;
                }

                /// <summary>
                /// 将一个任务无效并从队列取出
                /// </summary>
                /// <param name="_serialize"></param>
                public _AALEnableMonoTask popTask(long _serialize)
                {
                    _AALEnableMonoTask task = null;
                    //尝试获取值
                    if(!_m_dicTask.TryGetValue(_serialize, out task))
                        return null;

                    //从集合删除
                    _m_dicTask.Remove(_serialize);
                    //设置任务序列号为0
                    task._m_lSerialize = 0;
                    //返回
                    return task;
                }
            }
        }
    }
}
