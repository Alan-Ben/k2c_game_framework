using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public enum EWCGPingState
    {
        Hight, //高Ping值
        Mid,//中Ping值
        Low,//低Ping值
    }
    
    public class FpsAndPingMgr
    {
        private static FpsAndPingMgr _g_instance = new FpsAndPingMgr();
        public static FpsAndPingMgr instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new FpsAndPingMgr();
                return _g_instance;
            }
        }


        private int m_iHeartSerialize = ALSerializeOpMgr.next();
        public void refreshHeartSerialize() { m_iHeartSerialize = ALSerializeOpMgr.next(); }

        //当前游戏FPS
        private float _m_fFps;
        private int _m_iFpsValue;
        //间隔时间列表的总时间
        private float _m_fTotalFrameTime;
        //每帧的间隔时间
        private List<float> _m_lFrameTimeList;

        //获取的ping值列表
        private long _m_lTotalPing;
        private List<long> _m_lPingList;

        //当前游戏PING值
        private long _m_lPingMS;
        private long _m_lAvgPingMS;
        //最大的游戏ping值
        private long _m_lMaxPingMS;
        private long _m_lMinPingMS;//记录最小ping值
        //服务器时间对应客户端时间
        private float _m_fServer2ClientTime;
        //当前服务器时间
        private long _m_lServerTimeTag;
        //当前Ping值状态
        private EWCGPingState _m_ePingType;
        //当前网络状态
        private NetworkReachability _m_eNetState;

        //低于指定帧率开始记录时间
        private long _m_lLowFrameRateStartRecordTime;
        //是否展示过了低帧率提示弹窗
        private bool _m_bIsShowLowFrameRateTip;

        //fps回调
        private Action<int> _m_dFpsChgDelegate;
        //ping值回调
        private Action<EWCGPingState, long, long> _m_dPingChgDelegate;
        //网络状态回调
        private Action _m_dNetStateChgDelegate;

        private MonoTaskWaitServerHeartPack _m_serverWaitTimeOutTask;

        //检查跨天记录的上个服务器时间
        private long _m_lLastServerTimeTag;

        public EWCGPingState pingType { get { return _m_ePingType; } }
        public float fpsValue { get { return _m_fFps; } }
        public long pingMS { get { return _m_lPingMS; } }
        public long avgPingMS { get { return _m_lAvgPingMS; } }
        public long maxPingMS { get { return _m_lMaxPingMS; } }
        public long serverTimeTagS { get { return _m_fServer2ClientTime == 0 ? 0 : (_m_lServerTimeTag / 1000) + (int)(Time.unscaledTime - _m_fServer2ClientTime); } }
        public long serverTimeTag { get { return _m_fServer2ClientTime == 0 ? 0 : _m_lServerTimeTag + (long)((Time.unscaledTime - _m_fServer2ClientTime) * 1000); } }

        public Action<int> fpsChgDelegate { get { return _m_dFpsChgDelegate; } set { _m_dFpsChgDelegate = value; } }
        public Action<EWCGPingState, long, long> pingChgDelegate { get { return _m_dPingChgDelegate; } set { _m_dPingChgDelegate = value; } }
        public Action NetStateChgDelegate { get { return _m_dNetStateChgDelegate; } set { _m_dNetStateChgDelegate = value; } }

        protected FpsAndPingMgr()
        {
            _m_fFps = 0;
            _m_fTotalFrameTime = 0;
            _m_lFrameTimeList = new List<float>();

            _m_lTotalPing = 0;
            _m_lPingList = new List<long>();

            _m_lPingMS = 0;
            _m_lAvgPingMS = 0;
            _m_lMinPingMS = long.MaxValue;
            _m_fServer2ClientTime = 0;
            _m_lServerTimeTag = 0;

            _m_lLowFrameRateStartRecordTime = 0;
            _m_bIsShowLowFrameRateTip = false;

            _m_eNetState = Application.internetReachability;

            _m_dFpsChgDelegate = default(Action<int>);
            _m_dPingChgDelegate = default(Action<EWCGPingState, long, long>);
            _m_dNetStateChgDelegate = default(Action);

            //开启任务监控fps
            ALMonoTaskMgr.instance.addMonoTask(new WCGMonoTaskCalFps());
            //开启任务进行网络状态检测
            ALMonoTaskMgr.instance.addMonoTask(new WCGMonoTaskCheckNet());
            //开启任务进行跨天检测
            ALMonoTaskMgr.instance.addMonoTask(new MonoTaskCheckIsCrossDay());
        }

        /// <summary>
        /// 处理心跳包返回的数据
        /// </summary>
        /// <param name="_clientHeartSerialize">客户端心跳包的序列号，如非0则需要判断，确保处理的心跳包和当前的逻辑是对应的</param>
        /// <param name="_clientTimeTag">发包时客户端时间戳</param>
        /// <param name="_serverTimeTag">服务器时间戳</param>
        /// <param name="_gsClientListener"></param>
        /// <param name="_forceUpdateServerTime">是否强制更新服务器时间, 若为false, 只有当 当前包ping值小于最小ping值时会更新服务器时间</param>
        public void dealHeartPackMesg(long _clientHeartSerialize, long _clientTimeTag, long _serverTimeTag, NPGSClientListener _gsClientListener, bool _forceUpdateServerTime)
        {
            //确认心跳包是否对应，不对应的逻辑不需要处理
            if (0 != _clientHeartSerialize && m_iHeartSerialize != _clientHeartSerialize)
                return;

            //计算ping值
            _m_lPingMS = (ALCommon.getNowTimeMill() - _clientTimeTag) / 2;
            
            if (_forceUpdateServerTime)//若需要强制更新服务器时间
            {
                //设置服务器时间
                _m_lServerTimeTag = _serverTimeTag;
                //设置当前对应客户端时间
                _m_fServer2ClientTime = Time.unscaledTime;

                _m_lMinPingMS = long.MaxValue;
            }
            else//若不需要强制更新服务器时间
            {
                if (_m_lPingMS < _m_lMinPingMS)//当前ping值小于记录的最小ping值时, 更新服务器时间
                {
                    //设置服务器时间
                    _m_lServerTimeTag = _serverTimeTag;
                    //设置当前对应客户端时间
                    _m_fServer2ClientTime = Time.unscaledTime;
                
                    _m_lMinPingMS = _m_lPingMS;
                }
            }
            
            //增加进入队列
            _m_lTotalPing += _m_lPingMS;
            _m_lPingList.Add(_m_lPingMS);
            //超出长度删除
            while(_m_lPingList.Count > 5)
            {
                _m_lTotalPing -= _m_lPingList[0];
                _m_lPingList.RemoveAt(0);
            }

            //获取最大ping值
            if(_m_lPingMS > _m_lMaxPingMS)
            {
                _m_lMaxPingMS = _m_lPingMS;
            }
            else
            {
                _m_lMaxPingMS = 0;
                //重新获取最大ping值
                for(int i = 0; i < _m_lPingList.Count; i++)
                {
                    if(_m_lPingList[i] > _m_lMaxPingMS)
                        _m_lMaxPingMS = _m_lPingList[i];
                }
            }

            //计算平均ping值
            long newAvgPingMS = _m_lTotalPing / _m_lPingList.Count;

            //判断数据是否变动
            if(newAvgPingMS != _m_lAvgPingMS)
            {
                _m_lAvgPingMS = newAvgPingMS;

                if(_m_lAvgPingMS < 200)
                    _m_ePingType = EWCGPingState.Low;
                else if(_m_lAvgPingMS > 500)
                    _m_ePingType = EWCGPingState.Hight;
                else
                    _m_ePingType = EWCGPingState.Mid;

                //处理回调
                if(null != _m_dPingChgDelegate)
                    _m_dPingChgDelegate(_m_ePingType, _m_lPingMS, _m_lAvgPingMS);
            }

            if(null != _m_serverWaitTimeOutTask)
                _m_serverWaitTimeOutTask.reset();
            _m_serverWaitTimeOutTask = null;
            
            //刷新心跳包操作
            refreshHeartSerialize();
            //延迟处理
            ALMonoTaskMgr.instance.addMonoTask(new WCGMonoTaskSendHeartPack(m_iHeartSerialize, _gsClientListener), 5f);
        }

        /// <summary>
        /// 新增任务等待服务端返回
        /// </summary>
        public void addWaitServerTimeOutTask(int _heartSerialize, NPGSClientListener _gsClientListener)
        {
            if(null != _m_serverWaitTimeOutTask)
                _m_serverWaitTimeOutTask.reset();
            _m_serverWaitTimeOutTask = null;

            _m_serverWaitTimeOutTask = new MonoTaskWaitServerHeartPack(_heartSerialize, _gsClientListener);
            ALMonoTaskMgr.instance.addMonoTask(_m_serverWaitTimeOutTask, 15);
        }

        /****************
         * 每帧处理
         **/
        public void tick()
        {
            //增加每帧时间
            _m_fTotalFrameTime += Time.deltaTime;
            _m_lFrameTimeList.Add(Time.deltaTime);

            //超出长度删除
            while(_m_lFrameTimeList.Count > 100)
            {
                _m_fTotalFrameTime -= _m_lFrameTimeList[0];
                _m_lFrameTimeList.RemoveAt(0);
            }

            //计算帧数
            _m_fFps = 1 / (_m_fTotalFrameTime / _m_lFrameTimeList.Count);
            int newFpsValue = Mathf.RoundToInt(_m_fFps);

            //判断值是否变化
            if(_m_iFpsValue != newFpsValue)
            {
                _m_iFpsValue = newFpsValue;

                //处理回调
                if(null != _m_dFpsChgDelegate)
                    _m_dFpsChgDelegate(_m_iFpsValue);
            }

            //检测低帧率
            _checkLowFrameRate();
        }

        public void CheckNoNet()
        {
            if(Application.internetReachability == _m_eNetState)
                return;

            _m_eNetState = Application.internetReachability;

            //只有在网络变更到无信号状态，才执行回调，有信号时的网络状态交由ping值管理
            if(_m_eNetState == NetworkReachability.NotReachable && _m_dNetStateChgDelegate != null)
                _m_dNetStateChgDelegate();
        }

        /// <summary>
        /// 检查是否跨天
        /// </summary>
        public void checkIsCrossDay()
        {
            if (serverTimeTag <= 0 || _m_lLastServerTimeTag <= 0)
            {
                _m_lLastServerTimeTag = serverTimeTag;
                return;
            }

            //如果跨天了，发送跨天消息
            if (!TimeUtil.serverTimeMsIsInSameDay(serverTimeTag, _m_lLastServerTimeTag))
                WinMsg.SendMsg(WinMsgType.ON_CROSS_DAY);

            _m_lLastServerTimeTag = serverTimeTag;
        }

        /// <summary>
        /// 检测低帧率，持续一段时间内所有帧率都低于指定帧数，弹出提示降低画质
        /// </summary>
        private void _checkLowFrameRate()
        {
            //如果本次登录展示过了弹窗 或者 不是在游戏内 或者 已经是最低档画质了 或者 今日不再提示 则不处理，重置记录时间
            if (_m_bIsShowLowFrameRateTip || 
                !GStageMain.instance.isEntered ||
                GameSetting.instance.gameQuality == ENPGameQuality.VERY_LOW || 
                !AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.CHECK_LOW_FRAME_RATE))
            {
                _m_lLowFrameRateStartRecordTime = 0;
                return;
            }

            //持续时间
            int durationTimeSec = GRefdataCoreMgr.instance.npGeneral.check_low_frame_rate_duration_time_sec;
            //最低帧率
            int limitValue = GRefdataCoreMgr.instance.npGeneral.check_low_frame_rate_value;
            //当前帧率
            int curFps = (int) _m_fFps;
            //没有配置不处理
            if (durationTimeSec == 0 || limitValue == 0)
            {
                _m_lLowFrameRateStartRecordTime = 0;
                return;
            }

            //判断当前帧率是否小于指定帧率
            if (curFps < limitValue)
            {
                //如果没有记录过，设置开始记录时间
                if (_m_lLowFrameRateStartRecordTime == 0)
                    _m_lLowFrameRateStartRecordTime = ALCommon.getNowTimeSec();
                else
                {
                    //如果持续时间大于配置的最大时间，弹出提示
                    if (ALCommon.getNowTimeSec() - _m_lLowFrameRateStartRecordTime >= durationTimeSec)
                    {
                        // //如果在引导中不弹弹窗，但是继续记录时间
                        // if (Game.instance.isInTutorial)
                        //     return;

                        //重置时间
                        _m_lLowFrameRateStartRecordTime = 0;
                        //设置已展示
                        _m_bIsShowLowFrameRateTip = true;

                        //弹窗提示
                        // GGUIWndCheckLowFrameRate.instance.load();
                        // GGUIWndCheckLowFrameRate.instance.regLoadDoneDelegate(()=>
                        // {
                        //     GGUIWndCheckLowFrameRate.instance.showWnd();
                        //     GGUIWndCheckLowFrameRate.instance.setInfo(() =>
                        //     {
                        //         GGUIWndCheckLowFrameRate.instance.hideWnd();
                        //         GGUIWndCheckLowFrameRate.instance.discard();
                        //     });
                        // });

                        //记录埋点-低帧率
                        GCommon.sendStepReport(TraceConst.LOW_FPS.setMarkParam(durationTimeSec, curFps));
                    }
                }
            }
            else
            {
                //帧率没有比配置的低，重置记录
                _m_lLowFrameRateStartRecordTime = 0;
            }
        }


        /// <summary>
        /// 发送心跳包的定时任务
        /// </summary>
        protected class WCGMonoTaskSendHeartPack : _IALBaseMonoTask
        {
            private int _m_iHeartSerialize;
            private NPGSClientListener _m_clGSClientListener;

            public WCGMonoTaskSendHeartPack(int _heartSerialize, NPGSClientListener _gsClientListener)
            {
                _m_iHeartSerialize = _heartSerialize;
                _m_clGSClientListener = _gsClientListener;
            }

            public void deal()
            {
                //序列号不一致不处理
                if(_m_iHeartSerialize != FpsAndPingMgr.instance.m_iHeartSerialize)
                    return;

                //判断客户端是否已连接
                if(NPGSClientListener.instance == _m_clGSClientListener
                    && _m_clGSClientListener != null && !_m_clGSClientListener.isExit && _m_clGSClientListener.getClientStat() == ClientStat.LOGIN)
                {
                    FpsAndPingMgr._g_instance.addWaitServerTimeOutTask(_m_iHeartSerialize, _m_clGSClientListener);
                    NPGSClientListener.directSendMsg(NPGSWriter_001_BasicOp.make_002_HeartPack(ALCommon.getNowTimeMill(), instance.serverTimeTag, _m_iHeartSerialize));
                }
            }
        }

        /// <summary>
        /// 等待心跳包回包的定时任务
        /// </summary>
        protected class MonoTaskWaitServerHeartPack : _IALBaseMonoTask
        {
            private bool _m_activity = true;
            private NPGSClientListener _m_clGSClientListener;
            private int _m_iHeartSerialize;

            public MonoTaskWaitServerHeartPack(int _heartSerialize, NPGSClientListener _gsClientListener)
            {
                _m_clGSClientListener = _gsClientListener;
                _m_iHeartSerialize = _heartSerialize;
            }


            public void deal()
            {
                if(!_m_activity)
                    return;

                //序列号不一致不处理，不一致说明心跳包已经返回
                if(_m_iHeartSerialize != FpsAndPingMgr.instance.m_iHeartSerialize)
                    return;

                if(NPGSClientListener.instance == _m_clGSClientListener && null != _m_clGSClientListener && !_m_clGSClientListener.isExit)
                {
                    _m_clGSClientListener.dealClientLogout();
                }
            }

            public void reset()
            {
                _m_activity = false;
            }
        }

        /****************
         * 计算帧数的任务
         **/
        protected class WCGMonoTaskCalFps : _IALBaseMonoTask
        {
            public void deal()
            {
                FpsAndPingMgr.instance.tick();

                ALMonoTaskMgr.instance.addNextFrameTask(this);
            }
        }

        /****************
         * 检查网络状态的任务
         **/
        protected class WCGMonoTaskCheckNet : _IALBaseMonoTask
        {
            public void deal()
            {
                FpsAndPingMgr.instance.CheckNoNet();

                ALMonoTaskMgr.instance.addMonoTask(this, 5f);
            }
        }

        /// <summary>
        /// 检查是否跨天
        /// </summary>
        protected class MonoTaskCheckIsCrossDay : _IALBaseMonoTask
        {
            public void deal()
            {
                FpsAndPingMgr.instance.checkIsCrossDay();

                ALMonoTaskMgr.instance.addMonoTask(this, 1f);
            }
        }
    }
}