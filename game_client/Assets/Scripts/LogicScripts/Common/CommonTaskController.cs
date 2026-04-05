using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 一开始只为了hotfix使用
    /// 因为al的task有editor宏，再生成bind时候识别不到，编译会报错，所以包一层
    /// 后续所有task可以用这个
    /// </summary>
    public class CommonTaskController
    {
        /** ALCommonTaskController */
        public static void CommonActionAddMonoTask(Action _delegate)
        {
            ALCommonTaskController.CommonActionAddMonoTask(_delegate);
        }
        
        public static void CommonActionAddMonoTask(Action _delegate, float _delayTime)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.CommonActionAddMonoTask(_delegate, _delayTime);
        }

        public static void CommonActionAddLaterMonoTask(Action _delegate)
        {
            ALCommonTaskController.CommonActionAddLaterMonoTask(_delegate);
        }
        
        public static void CommonActionAddLaterMonoTask(Action _delegate, float _delayTime)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.CommonActionAddLaterMonoTask(_delegate, _delayTime);
        }

        public static void CommonActionAddFixedMonoTask(Action _delegate)
        {
            ALCommonTaskController.CommonActionAddFixedMonoTask(_delegate);
        }
        
        public static void CommonActionAddFixedMonoTask(Action _delegate, float _delayTime)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.CommonActionAddFixedMonoTask(_delegate, _delayTime);
        }
        
        public static void CommonActionAddScaleTimeDelayMonoTask(Action _delegate, float _delayTime)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.CommonActionAddScaleTimeDelayMonoTask(_delegate, _delayTime);
        }
        
        public static void CommonActionAddScaleTimeDelayFixedMonoTask(Action _delegate, float _delayTime)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.CommonActionAddScaleTimeDelayFixedMonoTask(_delegate, _delayTime);
        }

        public static void CommonActionAddNextFrameTask(Action _delegate)
        {
            ALCommonTaskController.CommonActionAddNextFrameTask(_delegate);
        }
        
        public static void CommonActionAddNextFrameLaterTask(Action _delegate)
        {
            ALCommonTaskController.CommonActionAddNextFrameLaterTask(_delegate);
        }
        
        public static void CommonActionAddNextFixedUpdateTask(Action _delegate)
        {
            ALCommonTaskController.CommonActionAddNextFixedUpdateTask(_delegate);
        }
        
        
        /** ALCommonTaskController */
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableLateTickActionAddLaterMonoTask(Action _delegate)
        {
            return ALCommonTaskController.CommonEnableLateTickActionAddLaterMonoTask(_delegate);
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <param name="_delayTime"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableLateTickActionAddLaterMonoTask(Action _delegate, float _delayTime)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            return ALCommonTaskController.CommonEnableLateTickActionAddLaterMonoTask(_delegate, _delayTime);
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableLateTickActionAddNextFrameLaterTask(Action _delegate)
        {
            return ALCommonTaskController.CommonEnableLateTickActionAddNextFrameLaterTask(_delegate);
        }

        #region ALCommonTaskController
        /** ALCommonTaskController */
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddMonoTask(Action _delegate)
        {
            return ALCommonTaskController.CommonEnableTickActionAddMonoTask(_delegate);
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <param name="_delayTime"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddMonoTask(Action _delegate, float _delayTime)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            return ALCommonTaskController.CommonEnableTickActionAddMonoTask(_delegate, _delayTime);
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <param name="_delayTime"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddScaleTimeDelayMonoTask(Action _delegate, float _delayTime)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            return ALCommonTaskController.CommonEnableTickActionAddScaleTimeDelayMonoTask(_delegate, _delayTime);
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddNextFrameTask(Action _delegate)
        {
            return ALCommonTaskController.CommonEnableTickActionAddNextFrameTask(_delegate);
        }
        /** 对外开放的任务创建操作函数终结 */
        #endregion

        #region ALCommonTaskController
        /** ALCommonTaskController */
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableDurationActionAddMonoTask(Action _delegate, float _duration)
        {
            return ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_delegate, _duration);
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <param name="_delayTime"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableDurationActionAddMonoTask(Action _delegate, float _duration, float _delayTime)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            return ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_delegate, _duration, _delayTime);
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <param name="_delayTime"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableDurationActionAddScaleTimeDelayMonoTask(Action _delegate, float _duration, float _delayTime)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            return ALCommonTaskController.CommonEnableDurationActionAddScaleTimeDelayMonoTask(_delegate, _duration, _delayTime);
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableDurationActionAddNextFrameTask(Action _delegate, float _duration)
        {
            return ALCommonTaskController.CommonEnableDurationActionAddNextFrameTask(_delegate, _duration);
        }
        /** 对外开放的任务创建操作函数终结 */
        #endregion

        /** ALCommonTaskController */
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddFixedMonoTask(Action _delegate)
        {
            return ALCommonTaskController.CommonEnableTickActionAddFixedMonoTask(_delegate);
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <param name="_delayTime"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddFixedMonoTask(Action _delegate, float _delayTime)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            return ALCommonTaskController.CommonEnableTickActionAddFixedMonoTask(_delegate, _delayTime);
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <param name="_delayTime"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddScaleTimeDelayFixedMonoTask(Action _delegate, float _delayTime)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            return ALCommonTaskController.CommonEnableTickActionAddScaleTimeDelayFixedMonoTask(_delegate, _delayTime);
        }
        /// <summary>
        /// 创建一个对应的任务，切记不可重复添加到任务管理器中
        /// </summary>
        /// <param name="_delegate"></param>
        /// <returns></returns>
        public static ALCommonEnableTaskController CommonEnableTickActionAddNextFixedUpdateTask(Action _delegate)
        {
            return ALCommonTaskController.CommonEnableTickActionAddNextFixedUpdateTask(_delegate);
        }
        /** 对外开放的任务创建操作函数终结 */
        
        public static void CommonStepActionAddMonoTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.CommonStepActionAddMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime);
        }
        
        public static void CommonStepActionAddLaterMonoTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.CommonStepActionAddLaterMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime);
        }
        
        public static void CommonStepActionAddFixedMonoTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.CommonStepActionAddFixedMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime);
        }

        public static void CommonStepActionAddScaleTimeDelayMonoTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.CommonStepActionAddScaleTimeDelayMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime);
        }
        
        public static void CommonStepActionAddScaleTimeDelayFixedMonoTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.CommonStepActionAddScaleTimeDelayFixedMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime);
        }

        public static void CommonStepActionAddNextFrameTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false)
        {
            ALCommonTaskController.CommonStepActionAddNextFrameTask(_action, _doneDelegate, _failDelegate, _isFailDoDone);
        }
        
        public static void CommonStepActionAddNextFrameLaterTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false)
        {
            ALCommonTaskController.CommonStepActionAddNextFrameLaterTask(_action, _doneDelegate, _failDelegate, _isFailDoDone);
        }
        
        public static void CommonStepActionAddNextFixedUpdateTask(Action _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false)
        {
            ALCommonTaskController.CommonStepActionAddNextFixedUpdateTask(_action, _doneDelegate, _failDelegate, _isFailDoDone);
        }
        
        public static void CommonStepProcessAddMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.CommonStepProcessAddMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime);
        }
        
        public static void CommonStepProcessAddLaterMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.CommonStepProcessAddLaterMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime);
        }
        
        public static void CommonStepProcessAddFixedMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.CommonStepProcessAddFixedMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime);
        }

        public static void CommonStepProcessAddScaleTimeDelayMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.CommonStepProcessAddScaleTimeDelayMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime);
        }
        
        public static void CommonStepProcessAddScaleTimeDelayFixedMonoTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false, float _delayTime = 0f)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.CommonStepProcessAddScaleTimeDelayFixedMonoTask(_action, _doneDelegate, _failDelegate, _isFailDoDone, _delayTime);
        }

        public static void CommonStepProcessAddNextFrameTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false)
        {
            ALCommonTaskController.CommonStepProcessAddNextFrameTask(_action, _doneDelegate, _failDelegate, _isFailDoDone);
        }
        public static void CommonStepProcessAddNextFrameLaterTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false)
        {
            ALCommonTaskController.CommonStepProcessAddNextFrameLaterTask(_action, _doneDelegate, _failDelegate, _isFailDoDone);
        }
        public static void CommonStepProcessAddNextFixedUpdateTask(Func<bool> _action, Action _doneDelegate, Action _failDelegate = null, bool _isFailDoDone = false)
        {
            ALCommonTaskController.CommonStepProcessAddNextFixedUpdateTask(_action, _doneDelegate, _failDelegate, _isFailDoDone);
        }

        /** 对外开放的任务创建操作函数 */
        public static void FrameDelayActionAddMonoTask(int _frameCount, Action _delegate)
        {
            ALCommonTaskController.FrameDelayActionAddMonoTask(_frameCount, _delegate);
        }
        public static void FrameDelayActionAddMonoTask(int _frameCount, Action _delegate, float _delayTime)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.FrameDelayActionAddMonoTask(_frameCount, _delegate, _delayTime);
        }

        public static void FrameDelayActionAddScaleTimeDelayMonoTask(int _frameCount, Action _delegate, float _delayTime)
        {
            if (float.IsNaN(_delayTime))
                _delayTime = 0;
            ALCommonTaskController.FrameDelayActionAddScaleTimeDelayMonoTask(_frameCount, _delegate, _delayTime);
        }

        public static void FrameDelayActionAddNextFrameTask(int _frameCount, Action _delegate)
        {
            ALCommonTaskController.FrameDelayActionAddNextFrameTask(_frameCount, _delegate);
        }
    }
}