using System;
using System.Diagnostics;
using System.Text;
using ALPackage;
using MJSDK_Package;
using GOE;
using UnityEngine;

namespace GOESDK
{
    /// <summary>
    /// SDK接口管理器
    /// </summary>
    public abstract partial class _ASDKMgr
    {
        //通用错误码
        public int COMMON_ERROR = -1;
        //是否初始化
        private bool _m_bIsInit = false;
        //国家
        private string _m_sSysCountry;
        //ip信息
        private MJSDK_PhpApiCommon_2Engine_net_ip _m_ipInfo;
        //AFID
        private string _m_sAppsflyerId;
        //设备ID
        private string _m_sDeviceId;
        //是否获取ip信息完成
        private bool _m_bIsGetIPInfoDone;
        //获取ip信息回调
        private Action<MJSDK_PhpApiCommon_2Engine_net_ip> _m_aOnGetIPInfo;


        /// <summary>是否使用SDK</summary>
        public bool isUseSDK { get { return MainCameraMono.selfInstance != null && MainCameraMono.selfInstance.isUseSDK; } }
        /// <summary>是否初始化SDK</summary>
        public bool isInit { get { return _m_bIsInit; } }

        #region 设备信息

        /// <summary>国家</summary>
        public string sysCountry { get { return _m_sSysCountry != null ? _m_sSysCountry : string.Empty; } }
        /// <summary>设备IP</summary>
        public string ip { get { return _m_ipInfo != null && _m_ipInfo.ip != null ? _m_ipInfo.ip: string.Empty; } }
        /// <summary>当前网络所在国家简写</summary>
        public string netWorkCity { get { return _m_ipInfo != null && _m_ipInfo.city != null ? _m_ipInfo.city: string.Empty; } }
        /// <summary>当前网络所在大陆代号</summary>
        public string netWorkRegion { get { return _m_ipInfo != null && _m_ipInfo.region != null ? _m_ipInfo.region: string.Empty; } }
        /// <summary>AFID</summary>
        public string appsflyerId { get { return _m_sAppsflyerId != null ? _m_sAppsflyerId : string.Empty; } }

        /// <summary>设备ID</summary>
        public string deviceId
        {
            get
            {
                //没用sdk的情况下使用设备唯一标识
                if (!isUseSDK)
                    return SystemInfo.deviceUniqueIdentifier;
                
                return _m_sDeviceId != null ? _m_sDeviceId : string.Empty;
            }
        }

        #endregion



        /// <summary>
        /// 初始化SDK
        /// </summary>
        public void init(Action _onInitDone)
        {
            if (_m_bIsInit || !isUseSDK)
            {
                if (!isUseSDK)
                {
                    //发送埋点-未使用SDK
                    GCommon.sendStepReport(TraceConst.NO_USE_SDK);
                }
                if (_onInitDone != null)
                    _onInitDone();
                return;
            }

            _m_bIsInit = true;
            _m_bIsGetIPInfoDone = false;

            SDKUtil.showSDKDebugLog($"[init] 开始初始化SDK");

            //打印unity sdk版本号
            MJSDK_Unity_Version.printMJSDKUnityVersion();

            //初始化缓存
            SDKLoginSetting.instance.init();

            //统一初始化SDK
            MJSDKDealLogic dealLogic = new MJSDKDealLogic();
            MJSDK.init(dealLogic, (bool _isInit, string _initMsg) =>
            {
                if (_isInit)
                {
                    ALStepCounter stepCounter = new ALStepCounter();
                    stepCounter.chgTotalStepCount(1);
                    //发送埋点-初始化SDK参数完成
                    stepCounter.regAllDoneDelegate(() => 
                    {
                        //发送埋点-初始化SDK成功
                        GCommon.sendStepReport(TraceConst.INIT_SDK_SUC);
                        _onInitDone?.Invoke();
                    });

                    //1 ========开始初始化SDK相关参数=======
                    _initSDKParam();

                    //2 ========初始化AIHelp========
                    GCommon.sendStepReport(TraceConst.INIT_AIHELP_START);
                    aihelp_init(_str=>
                    {
                        GCommon.sendStepReport(TraceConst.INIT_AIHELP_SUC);
                    },(_code, _msg) =>
                    {
                        GCommon.sendStepReport(TraceConst.INIT_AIHELP_FAIL.setMarkParam(_code, _msg));
                    });
                    
                    stepCounter.addDoneStepCount();
                }
                else
                {
                    //发送埋点-初始化SDK失败
                    GCommon.sendStepReport(TraceConst.INIT_SDK_FAIL);
                    SDKUtil.showSDKLogError("MJSDK.init", COMMON_ERROR, _initMsg);
                }
            });
        }

        /// <summary>
        /// 注册获取IP信息完成回调
        /// </summary>
        /// <param name="_action"></param>
        public void regGetIPDoneDelegate(Action<MJSDK_PhpApiCommon_2Engine_net_ip> _action)
        {
            if(_action == null)
                return;

            if (!isUseSDK)
            {
                _action(null);
                return;
            }

            if (_m_bIsGetIPInfoDone)
                _action(_m_ipInfo);
            else if(_m_aOnGetIPInfo == null)
                _m_aOnGetIPInfo = _action;
            else
                _m_aOnGetIPInfo += _action;
        }

        /// <summary>
        /// 初始化SDK参数
        /// </summary>
        private void _initSDKParam()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(4);
            //发送埋点-初始化SDK参数完成
            stepCounter.regAllDoneDelegate(() => { GCommon.sendStepReport(TraceConst.INIT_SDK_PARAM_SUC); });

            //1 =======================获取国家=======================
            sys_info(ESDKSysInfoType.COUNTRY, _country =>
            {
                _m_sSysCountry = _country;
                //设置firebase参数
                _setFirebaseCrashlyticsDefaultValue("nation", _m_sSysCountry);
                stepCounter.addDoneStepCount();
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("sys_info_country", _code, _msg);
            });

            
            //2 =======================获取设备网络信息=======================
            Stopwatch timeWatch = new Stopwatch();
            timeWatch.Start();
            net_ip(_ipInfo =>
            {
                _m_ipInfo = _ipInfo;
                _m_bIsGetIPInfoDone = true;
                //设置firebase参数
                _setFirebaseCrashlyticsDefaultValue("ip", _m_ipInfo?.ip);
                
                //发送埋点-获取视频校验上限
                long time = timeWatch.ElapsedMilliseconds;
                timeWatch.Stop();
                //发送埋点-初始化IP信息成功
                GCommon.sendStepReport(TraceConst.INIT_SDK_IP_SUC.setMarkParam(_m_ipInfo?.ip, _m_ipInfo?.city, _m_ipInfo?.region, time));
                //完成回调
                _m_aOnGetIPInfo?.Invoke(_m_ipInfo);
                stepCounter.addDoneStepCount();
            }, (_code, _msg) =>
            {
                _m_bIsGetIPInfoDone = true;
                
                //发送埋点-获取视频校验上限
                long time = timeWatch.ElapsedMilliseconds;
                timeWatch.Stop();

                //发送埋点-初始化IP信息失败
                GCommon.sendStepReport(TraceConst.INIT_SDK_IP_FAIL.setMarkParam(time));
                SDKUtil.showSDKLogError("net_ip", _code, _msg);
            });

            //3 =======================获取AFID=======================
            appsflyer_uid(_afid =>
            {
                _m_sAppsflyerId = _afid;
                //设置firebase参数
                _setFirebaseCrashlyticsDefaultValue("sdkId", _m_sAppsflyerId);
                stepCounter.addDoneStepCount();
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("appsflyer_uid", _code, _msg);
            });

            //4 =======================获取设备号=======================
            sys_info(ESDKSysInfoType.DEVICE_ID, _deviceId =>
            {
                _m_sDeviceId = _deviceId;
                //设置firebase用户标识
                firebase_setUserId(_m_sDeviceId, null, null);
                //设置firebase参数
                _setFirebaseCrashlyticsDefaultValue("udid", _m_sDeviceId);
                stepCounter.addDoneStepCount();
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("sys_info_deviceId", _code, _msg);
            });
        }

        /// <summary>
        /// 设置firebase参数
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_value"></param>
        private void _setFirebaseCrashlyticsDefaultValue(string _key, string _value)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.Append($"\"{_key}\":\"{_value}\"");
            builder.Append("}");
            SDKMgr.instance.firebase_setCustomKey(builder.ToString(), null, null);
        }
    }
}
