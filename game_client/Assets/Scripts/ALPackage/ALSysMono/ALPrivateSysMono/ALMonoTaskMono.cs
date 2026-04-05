using System;
using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using System.Text;
using System.Runtime.InteropServices;
using System.Security;
#endif

namespace ALPackage
{
    public class ALMonoTaskMono : MonoBehaviour
    {
#if UNITY_EDITOR && UNITY_STANDALONE
        [SuppressUnmanagedCodeSecurity]
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long performanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long frequency);
#endif

        /** 本类型脚本是否初始化过的操作，避免重复执行 */
        private static bool _g_bIsMonoStarted = false;
        /** 本脚本实际是否有效，如非重复初始化本脚本则有效 */
        private bool _m_bIsEnable = false;
        /** 用于存储本帧是否已经处理过OnGUI事件 */
        //private bool _m_bDealOnGUI = false;

#if UNITY_EDITOR
        private long _m_lTimeTickPerSec;
        private long _m_lTickStart;
        private long _m_lTickEnd;
        
        private int _m_thisFrameTotalTask = 0;
#endif

        // Use this for initialization
        void Start()
        {
            try
            {
                if (_g_bIsMonoStarted)
                    return;

                //设置已初始化
                _g_bIsMonoStarted = true;
                //设置本脚本有效
                _m_bIsEnable = true;

#if UNITY_EDITOR && UNITY_STANDALONE
                QueryPerformanceFrequency(out _m_lTimeTickPerSec);
#endif
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("ALMonoTaskMono Start has Exception:\n" + e);
                if(_AALMonoMain.instance != null)
                {
                    _AALMonoMain.instance.onUnknowErrorOccurred(e);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            try
            {
#if UNITY_EDITOR
                _m_thisFrameTotalTask = 0;
#endif
                if (!_m_bIsEnable)
                    return;

                //_m_bDealOnGUI = false;

                //取出需要执行的任务
                _IALBaseMonoTask monoTask = ALMonoTaskMgr.instance.popMonoTask();
                while (null != monoTask)
                {
                    //进行处理
                    _dealTask(monoTask);

                    //取出下一个需要执行的Task
                    monoTask = ALMonoTaskMgr.instance.popMonoTask();
                }

                //取出需要执行的可缩放时间任务
                monoTask = ALMonoTaskMgr.instance.popScaletimeMonoTask();
                while (null != monoTask)
                {
                    //进行处理
                    _dealTask(monoTask);

                    //取出下一个需要执行的Task
                    monoTask = ALMonoTaskMgr.instance.popScaletimeMonoTask();
                }

                //处理任务管理器中的定时任务
                ALMonoTaskMgr.instance.dealTimerMonoTask();
                //处理根据时间缩放的延迟任务
                ALMonoTaskMgr.instance.dealScaleTimeTimerMonoTask();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("ALMonoTaskMono Update has Exception:\n" + e);
                if(_AALMonoMain.instance != null)
                {
                    _AALMonoMain.instance.onUnknowErrorOccurred(e);
                }
            }
        }

        //将下帧任务放入队列
        void LateUpdate()
        {
            try
            {
                if (!_m_bIsEnable)
                    return;

                //取出需要执行的任务
                _IALBaseMonoTask monoTask = ALMonoTaskMgr.instance.popLaterMonoTask();
                while (null != monoTask)
                {
                    _dealTask(monoTask);

                    //取出下一个需要执行的Task
                    monoTask = ALMonoTaskMgr.instance.popLaterMonoTask();
                }

                //取出需要执行的可缩放时间任务
                monoTask = ALMonoTaskMgr.instance.popScaletimeLaterMonoTask();
                while (null != monoTask)
                {
                    //进行处理
                    _dealTask(monoTask);

                    //取出下一个需要执行的Task
                    monoTask = ALMonoTaskMgr.instance.popScaletimeLaterMonoTask();
                }

                //处理任务管理器中的定时任务
                ALMonoTaskMgr.instance.dealTimerLaterMonoTask();
                ALMonoTaskMgr.instance.dealScaleTimeTimerLaterMonoTask();

                //将本帧的延迟一帧处理任务转换到下一帧执行
                ALMonoTaskMgr.instance.swapNextFrameTask();

#if AL_CREATURE_SYS
                //处理角色对象的Combine处理操作
                ALCreatureMeshCombineMgr.instance.combineAllMgr();
#endif
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("ALMonoTaskMono LateUpdate has Exception:\n" + e);
                if(_AALMonoMain.instance != null)
                {
                    _AALMonoMain.instance.onUnknowErrorOccurred(e);
                }
            }
        }

        private void FixedUpdate()
        {
            try
            {
                if (!_m_bIsEnable)
                    return;

                //取出需要执行的任务
                _IALBaseMonoTask monoTask = ALMonoTaskMgr.instance.popFixedMonoTask();
                while (null != monoTask)
                {
                    //进行处理
                    _dealTask(monoTask);

                    //取出下一个需要执行的Task
                    monoTask = ALMonoTaskMgr.instance.popFixedMonoTask();
                }

                //取出需要执行的可缩放时间任务
                monoTask = ALMonoTaskMgr.instance.popScaletimeFixedMonoTask();
                while (null != monoTask)
                {
                    //进行处理
                    _dealTask(monoTask);

                    //取出下一个需要执行的Task
                    monoTask = ALMonoTaskMgr.instance.popScaletimeFixedMonoTask();
                }

                //处理任务管理器中的定时任务
                ALMonoTaskMgr.instance.dealTimerFixedMonoTask();
                //处理根据时间缩放的延迟任务
                ALMonoTaskMgr.instance.dealScaleTimeTimerFixedMonoTask();
                //将本帧的延迟一帧处理任务转换到下一帧执行
                ALMonoTaskMgr.instance.swapNextFixedUpdateTask();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("ALMonoTaskMono FixedUpdate has Exception:\n" + e);
                if(_AALMonoMain.instance != null)
                {
                    _AALMonoMain.instance.onUnknowErrorOccurred(e);
                }
            }
        }

#if AL_UNITY_GUI
        //执行GUI部分任务
        void OnGUI()
        {
            try
            {
                if(!_m_bIsEnable)
                    return;

                //取出需要执行的任务
                _IALBaseMonoTask monoTask = ALMonoTaskMgr.instance.popGuiMonoTask();
                while(null != monoTask)
                {
                    _dealTask(monoTask);

                    //取出下一个需要执行的Task
                    monoTask = ALMonoTaskMgr.instance.popGuiMonoTask();
                }
                //重置任务序列状态
                ALMonoTaskMgr.instance.resetGuiMonoTask();

                //处理任务管理器中的定时任务
                ALMonoTaskMgr.instance.dealTimerGuiMonoTask();

                //转移下一帧任务
                ALMonoTaskMgr.instance.swapGuiNextFrameTask();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("ALMonoTaskMono OnGUI has Exception:\n" + e);
                if(_AALMonoMain.instance != null)
                {
                    _AALMonoMain.instance.onUnknowErrorOccurred(e);
                }
            }
        }
#endif

        /****************
         * 处理执行函数的操作
         **/
        protected void _dealTask(_IALBaseMonoTask _monoTask)
        {
            if (null == _monoTask)
                return;

#if UNITY_EDITOR && UNITY_STANDALONE
            _m_thisFrameTotalTask++;
            QueryPerformanceCounter(out _m_lTickStart);
            string taskDebugString = _monoTask.ToString();//需要先记录调试string，否则deal之后可能被清空，就不知道是谁了
#endif
            //进行处理
            _monoTask.deal();

#if UNITY_EDITOR && UNITY_STANDALONE
            QueryPerformanceCounter(out _m_lTickEnd);

            //判断时间长度
            double absoluteTime = (double)(_m_lTickEnd - _m_lTickStart) / (double)_m_lTimeTickPerSec;
            if(absoluteTime > 0.01f && null != _AALMonoMain.instance && _AALMonoMain.instance.isMonitorTaskTime)
            {
                Debug.LogWarning("Mono Task Deal Time too Long! task: " + taskDebugString + " time: " + absoluteTime.ToString());
            }
#endif
        }
    }
}