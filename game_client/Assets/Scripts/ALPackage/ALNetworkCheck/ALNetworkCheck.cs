using System;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 间隔一段时间，检查一次网络状况，如果网络状况发生变化，则触发事件
    /// </summary>
    public class ALNetworkCheck
    {
        private static ALNetworkCheck _g_instance;

        public static ALNetworkCheck instance
        {
            get
            {
                if(_g_instance == null) {
                    _g_instance = new ALNetworkCheck();
                }
                return _g_instance;
            }
        }

        public delegate void NetworkStateChangeDelegate(NetworkReachability _lastNetworkState, NetworkReachability _networkState);
        
        //业务操作号，每次有新的操作，号加一，task中如果发现业务号变化，就不再执行task
        private int _m_iOpSerialize = 0;
        private NetworkStateChangeDelegate _m_onNetworkStateChange;

        private ALNetworkCheck()
        {
        }

        /// <summary>
        /// 开始检查网络状况，_interval为检查时间间隔（秒），当网络状况发生变化，_callback会被调用
        /// 每次开始是一次全新的响应回调
        /// </summary>
        public void startCheck(float _interval, NetworkStateChangeDelegate _callback)
        {
            _m_iOpSerialize++;
            //重置对应的回调对象
            _m_onNetworkStateChange = default(NetworkStateChangeDelegate);
            ALMonoTaskMgr.instance.addNextFrameTask(new ALNetworkCheckMonoTask(_m_iOpSerialize, _interval));
            regNetworkStateChange(_callback);
        }

        /// <summary>
        /// 注册网络情况变化的事件
        /// </summary>
        public void regNetworkStateChange(NetworkStateChangeDelegate _callback)
        {
            _m_onNetworkStateChange += _callback;
        }

        /// <summary>
        /// 反注册网络情况变化的事件
        /// </summary>
        public void unregNetworkStateChange(NetworkStateChangeDelegate _callback)
        {
            _m_onNetworkStateChange -= _callback;
        }
        
        /// <summary>
        /// 用于监听网络环境变化的任务
        /// </summary>
        public class ALNetworkCheckMonoTask : _IALBaseMonoTask
        {
            private readonly float _m_checkInterval;
            private int _m_taskSerialize;//这个任务的操作号
            private NetworkReachability _m_lastNetworkState;
        
            public ALNetworkCheckMonoTask(int _opSerialize, float _checkInterval)
            {
                _m_taskSerialize = _opSerialize;
                _m_checkInterval = _checkInterval;
                _m_lastNetworkState = Application.internetReachability;//第一次初始化，不广播事件
            }

            /*******************
             * 任务具体的执行函数
             **/
            public void deal()
            {
                //外部的操作变了，任务不再执行
                if (_m_taskSerialize != ALNetworkCheck.instance._m_iOpSerialize)
                    return;
                
                //当网络状况变化，触发事件
                NetworkReachability nowNetworkState = Application.internetReachability;
                if(_m_lastNetworkState != nowNetworkState)
                {
                    if(ALNetworkCheck.instance._m_onNetworkStateChange != null)
                    {
                        ALNetworkCheck.instance._m_onNetworkStateChange(_m_lastNetworkState, nowNetworkState);
                    }
                    _m_lastNetworkState = nowNetworkState;
                }
            
                //将本任务延迟interval后再次执行
                //<=0的话加入下一帧，防止卡死
                if (_m_checkInterval <= 0)
                {
                    ALMonoTaskMgr.instance.addNextFrameTask(this);
                }
                else
                {
                    ALMonoTaskMgr.instance.addMonoTask(this, _m_checkInterval);
                }
            }
        }
    }
}