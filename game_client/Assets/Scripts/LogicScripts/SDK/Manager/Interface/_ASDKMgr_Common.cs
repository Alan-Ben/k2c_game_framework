using System;
using GOE;
using MJSDK_Package;
using UnityEngine;

namespace GOESDK
{
    public abstract partial class _ASDKMgr
    {
        #region MJSDK_BasicLib 系统信息

        /// <summary>
        /// 根据带入的类型，获取对应的系统信息
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void sys_info(ESDKSysInfoType _type, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("sys_info", true, _failDelegate))
                return;

            string typeStr = _type.ToString().ToLower();
            MJSDK_BasicLib.sys_info(typeStr, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[sys_info] 获取系统信息成功，type:", typeStr, ",result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("sys_info", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 根据带入的标记，获取对应的组件版本号
        /// </summary>
        /// <param name="_componentTag"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void sys_version(string _componentTag, Action<MJSDK_Version_Model> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("sys_version", true, _failDelegate))
                return;

            MJSDK_BasicLib.sys_version(_componentTag, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[sys_version] 获取对应的组件版本号成功，version:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("sys_version", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// sys-appStore：跳转至商店（苹果跳转App Store，谷歌跳转至谷歌Play）
        /// </summary>
        /// <param name="_app_store_url"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void sys_appStore(string _app_store_url, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("sys_appStore", true, _failDelegate))
                return;

            MJSDK_BasicLib.sys_appStore(_app_store_url, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[sys_appStore] 跳转至商店成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("sys_appStore", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 振动
        /// </summary>
        /// <param name="_ms">毫秒</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void sys_vibrator(long _ms, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("sys_vibrator", true, _failDelegate))
                return;

            MJSDK_BasicLib.sys_vibrator(_ms, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[sys_vibrator] 振动成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("sys_vibrator", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 梦加数据中心 -- 广告渠道标识信息
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void sys_mjAdfrom(Action<MJSDK_Basic_2Engine_sys_mjAdfrom> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("sys_mjAdfrom", true, _failDelegate))
                return;

            MJSDK_BasicLib.sys_mjAdfrom((_str) =>
            {
                SDKUtil.showSDKDebugLog("[sys_mjAdfrom] 获取广告渠道标识信息成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("sys_mjAdfrom", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 打开链接
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void sys_openUrl(string _url, Action<string> _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("sys_openUrl", true, _failDelegate))
                return;

            MJSDK_BasicLib.sys_openUrl(_url, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[sys_openUrl] 打开链接，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("sys_openUrl", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 应用内评价(本接口只支持iOS，Android请使用Google组件中接口)
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void sys_appReview(Action<string> _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("sys_appReview", true, _failDelegate))
                return;

            MJSDK_BasicLib.sys_appReview((_str) =>
            {
                SDKUtil.showSDKDebugLog("[sys_appReview] 应用内评价，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("sys_appReview", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        #endregion


        #region MJSDK_PhpApiCommonLib PHP 服务端公共接口

        /// <summary>
        /// 获取设备网络信息 (网络类型、网络服务商)
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void net_info(System.Action<MJSDK_PhpApiCommon_2Engine_net_info> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("net_info", MJSDK_PhpApi_CommonLib.g_isInit, _failDelegate))
                return;

            MJSDK_PhpApi_CommonLib.net_info((_info) =>
            {
                SDKUtil.showSDKDebugLog("[net_info] 获取设备网络信息成功，type:", _info.type);
                _sucDelegate?.Invoke(_info);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("net_info", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 获取ip（ 如ip、国家、地区）
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void net_ip(System.Action<MJSDK_PhpApiCommon_2Engine_net_ip> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("net_ip", MJSDK_PhpApi_CommonLib.g_isInit, _failDelegate))
                return;

            MJSDK_PhpApi_CommonLib.net_ip((_info) =>
            {
                SDKUtil.showSDKDebugLog("[net_ip] 获取ip（ 如ip、国家、地区），ip:", _info.ip, ",city:", _info.city, ",region:", _info.region);
                _sucDelegate?.Invoke(_info);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("net_ip", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 谷歌云翻译
        /// </summary>
        /// <param name="_2SDK_Translation_Google"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void translation_google(MJSDK_PhpApiCommon_2SDK_translation_google _2SDK_Translation_Google, System.Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("translation_google", MJSDK_PhpApi_CommonLib.g_isInit, _failDelegate))
                return;

            MJSDK_PhpApi_CommonLib.translation_google(_2SDK_Translation_Google, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[translation_google] 谷歌云翻译成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("translation_google", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        #endregion


        #region MJSDK_PhpApiCommonLib_Trace 事件跟踪

        /// <summary>
        /// 游戏事件跟踪-激活上报
        /// </summary>
        /// <param name="_extend"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void trace_gameActivate(string _extend = "", Action _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            if (!isUseSDK)
            {
                SDKUtil.showSDKDebugLog("[trace_gameActivate]", " 未使用SDK");
                if (_failDelegate != null)
                    _failDelegate(SDKMgr.instance.COMMON_ERROR, $"trace_gameActivate 未使用SDK");
                return;
            }

            //构建数据
            MJSDK_PhpApiCommon_2SDK_trace_gameActivty activateInfo = SDKTraceDataUtil.getActivateData(_extend);

            //发送激活埋点
            MJSDK_PhpApiCommonLib_Trace.trace_gameActivate(activateInfo, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[trace_gameActivate] 游戏事件跟踪-激活上报成功，_str:", _str);
                _sucDelegate?.Invoke();
            }, ((_code, _msg) =>
            {
                SDKUtil.showSDKLogError("trace_gameActivate", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            }));
        }

        /// <summary>
        /// 游戏事件跟踪-节点上报
        /// </summary>
        /// <param name="_stepReportInfo"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void trace_gameStep(TraceStepData _stepReportInfo, Action _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            if (!isUseSDK)
            {
                SDKUtil.showSDKDebugLog("[trace_gameStep]", " 未使用SDK");
                if (_failDelegate != null)
                    _failDelegate(SDKMgr.instance.COMMON_ERROR, $"trace_gameStep 未使用SDK");
                return;
            }

            //判断是否开启埋点
            if (!CDNSetting_ClientConfigInfo.instance.isOpenPHPAD)
            {
                SDKUtil.showSDKDebugLog("[trace_gameStep] 游戏事件跟踪-未开启埋点 info:", _stepReportInfo.ToString());
                _sucDelegate?.Invoke();
                return;
            }

            //判断埋点优先级是否需要发送
            if (_stepReportInfo != null && _stepReportInfo.sendLevel > CDNSetting_ClientConfigInfo.instance.phpADLevel)
            {
                SDKUtil.showSDKDebugLog("[trace_gameStep] 游戏事件跟踪-埋点优先级低于配置，phpADLevel:", CDNSetting_ClientConfigInfo.instance.phpADLevel, "info:", _stepReportInfo.ToString());
                _sucDelegate?.Invoke();
                return;
            }

            //构建数据
            MJSDK_PhpApiCommon_2SDK_trace_gameStep stepInfo = SDKTraceDataUtil.getStepData(_stepReportInfo);

            //发送埋点
            MJSDK_PhpApiCommonLib_Trace.trace_gameStep(stepInfo, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[trace_gameStep] 游戏事件跟踪-节点上报成功，id:", _stepReportInfo.ID, ",mark:", _stepReportInfo.mark);
                _sucDelegate?.Invoke();
            }, ((_code, _msg) =>
            {
                _failDelegate?.Invoke(_code, _msg);
            }));
        }

        /// <summary>
        /// 游戏异常事件上报
        /// </summary>
        /// <param name="_content"></param>
        /// <param name="_errLevel"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void trace_gameErr(string _content, E_Err_level _errLevel = E_Err_level.error, Action _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            if (!isUseSDK)
            {
                SDKUtil.showSDKDebugLog("[trace_gameErr]", " 未使用SDK");
                if (_failDelegate != null)
                    _failDelegate(SDKMgr.instance.COMMON_ERROR, $"trace_gameErr 未使用SDK");
                return;
            }

            //构建数据
            MJSDK_PhpApiCommon_2SDK_trace_gameErr errorInfo = SDKTraceDataUtil.getErrorData(_errLevel, _content);

            //发送异常事件
            MJSDK_PhpApiCommonLib_Trace.trace_gameErr(errorInfo, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[trace_gameErr] 游戏异常事件上报成功，_str:", _str);
                _sucDelegate?.Invoke();
            }, ((_code, _msg) =>
            {
                //不输出错误log，避免无限死循环
                //SDKUtil.showSDKLogError("trace_gameErr", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            }));
        }

        /// <summary>
        /// 游戏广告变现数据上报
        /// </summary>
        /// <param name="_eventId"></param>
        /// <param name="_adType"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void trace_gameAd(string _eventId, string _adType, Action _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            if (!isUseSDK)
            {
                SDKUtil.showSDKDebugLog("[trace_gameAd]", " 未使用SDK");
                if (_failDelegate != null)
                    _failDelegate(SDKMgr.instance.COMMON_ERROR, $"trace_gameAd 未使用SDK");
                return;
            }

            //构建数据
            MJSDK_PhpApiCommon_2SDK_trace_gameAd adInfo = SDKTraceDataUtil.getAdData(_eventId, _adType);

            //发送广告变现数据
            MJSDK_PhpApiCommonLib_Trace.trace_gameAd(adInfo, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[trace_gameAd] 游戏广告变现数据上报成功，_str:", _str);
                _sucDelegate?.Invoke();
            }, ((_code, _msg) =>
            {
                SDKUtil.showSDKLogError("trace_gameAd", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            }));
        }

        #endregion


        #region MJSDK_GoogleLib

        /// <summary>
        /// google商店应用内评价
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void google_appReview(Action<string> _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("google_appReview", MJSDK_GoogleLib.g_isInit, _failDelegate))
                return;

            MJSDK_GoogleLib.google_appReview((_str) =>
            {
                SDKUtil.showSDKDebugLog("[google_appReview] google应用内评价成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("google_appReview", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        #endregion


        #region MJSDK_FacebookLib

        /// <summary>
        /// 购买事件上报
        /// </summary>
        /// <param name="_purchaseEvent"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void facebook_purchaseEvent(MJSDK_2SDK_ThirdParty_purchaseEvent_base _purchaseEvent, Action<string> _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("facebook_purchaseEvent", MJSDK_FacebookLib.g_isInit, _failDelegate))
                return;

            MJSDK_FacebookLib.facebook_purchaseEvent(_purchaseEvent, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[facebook_purchaseEvent] 购买事件上报成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("facebook_purchaseEvent", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 自定义事件上报
        /// </summary>
        /// <param name="_eventKey">自定义事件key</param>
        /// <param name="_ext">扩展参数json(string)</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void facebook_customEvent(string _eventKey, string _ext = "", Action<string> _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("facebook_customEvent", MJSDK_FacebookLib.g_isInit, _failDelegate))
                return;

            MJSDK_2SDK_ThirdParty_customEvent_base info = SDKTraceDataUtil.getThirdPartyEventData(_eventKey, _ext);

            MJSDK_FacebookLib.facebook_customEvent(info, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[facebook_customEvent] 自定义事件上报成功 key:", _eventKey, "，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("facebook_customEvent", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 图片分享
        /// </summary>
        /// <param name="_imgPath">图片地址（支持文件、base64文件地址）</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void facebook_shareImg(string _imgPath, Action<string> _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("facebook_shareImg", MJSDK_FacebookLib.g_isInit, _failDelegate))
                return;

            MJSDK_Facebook_2SDK_facebook_shareImg shareImg = new MJSDK_Facebook_2SDK_facebook_shareImg();
            shareImg.img_path = _imgPath;

            MJSDK_FacebookLib.facebook_shareImg(shareImg, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[facebook_shareImg] 购买事件上报成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("facebook_shareImg", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 链接分享
        /// </summary>
        /// <param name="_link">链接地址</param>
        /// <param name="_linkShow">链接说明</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void facebook_shareLink(string _link, string _linkShow, Action<string> _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("facebook_shareLink", MJSDK_FacebookLib.g_isInit, _failDelegate))
                return;

            MJSDK_Facebook_2SDK_facebook_shareLink shareLink = new MJSDK_Facebook_2SDK_facebook_shareLink();
            shareLink.link = _link;
            shareLink.link_show = _linkShow;

            MJSDK_FacebookLib.facebook_shareLink(shareLink, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[facebook_shareLink] 链接分享成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("facebook_shareLink", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        #endregion


        #region MJSDK_FirebaseLib

        /// <summary>
        /// 购买事件上报
        /// </summary>
        /// <param name="_2SDK_Firebase_PurchaseEvent"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void firebase_purchaseEvent(MJSDK_2SDK_ThirdParty_purchaseEvent_base _2SDK_Firebase_PurchaseEvent, Action<string> _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("firebase_purchaseEvent", MJSDK_FirebaseLib.g_isInit, _failDelegate))
                return;

            MJSDK_FirebaseLib.firebase_purchaseEvent(_2SDK_Firebase_PurchaseEvent, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[firebase_purchaseEvent] 购买事件上报成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("firebase_purchaseEvent", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 自定义事件上报
        /// </summary>
        /// <param name="_eventKey"></param>
        /// <param name="_ext"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void firebase_customEvent(string _eventKey, string _ext = "", Action<string> _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("firebase_customEvent", MJSDK_FirebaseLib.g_isInit, _failDelegate))
                return;

            MJSDK_2SDK_ThirdParty_customEvent_base info = SDKTraceDataUtil.getThirdPartyEventData(_eventKey, _ext);

            MJSDK_FirebaseLib.firebase_customEvent(info, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[firebase_customEvent] 自定义事件上报成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("firebase_customEvent", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 设置用户标识
        /// </summary>
        /// <param name="_userId"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void firebase_setUserId(string _userId, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("firebase_setUserId", MJSDK_FirebaseLib.g_isInit, _failDelegate))
                return;

            MJSDK_FirebaseLib.firebase_setUserId(_userId, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[firebase_setUserId] 设置用户标识成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("firebase_setUserId", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 添加自定义键
        /// </summary>
        /// <param name="_key_value"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void firebase_setCustomKey(string _key_value, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("firebase_setCustomKey", MJSDK_FirebaseLib.g_isInit, _failDelegate))
                return;

            MJSDK_FirebaseLib.firebase_setCustomKey(_key_value, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[firebase_setCustomKey] 添加自定义键成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKDebugLog("firebase_setCustomKey ", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 自定义日志
        /// </summary>
        /// <param name="_cusLog"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void firebase_customLog(string _cusLog, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("firebase_customLog", MJSDK_FirebaseLib.g_isInit, _failDelegate))
                return;

            MJSDK_FirebaseLib.firebase_customLog(_cusLog, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[firebase_customLog] 自定义日志成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKDebugLog("firebase_customLog ", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 自定义异常上报（非严重异常）
        /// </summary>
        /// <param name="_mes"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void firebase_customExc(string _mes, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("firebase_customExc", MJSDK_FirebaseLib.g_isInit, _failDelegate))
                return;

            MJSDK_FirebaseLib.firebase_customExc(_mes, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[firebase_customExc] 自定义异常上报（非严重异常）成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKDebugLog("firebase_customExc ", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        #endregion


        #region MJSDK_AppsflyerLib

        /// <summary>
        /// 获取appsflyer uid
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void appsflyer_uid(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("appsflyer_uid", MJSDK_AppsflyerLib.g_isInit, _failDelegate))
                return;

            MJSDK_AppsflyerLib.appsflyer_uid((_str) =>
            {
                SDKUtil.showSDKDebugLog("[appsflyer_uid] 获取appsflyer uid成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("appsflyer_uid", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 购买事件上报
        /// </summary>
        /// <param name="_2SDK_Appsflyer_PurchaseEvent"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void appsflyer_purchaseEvent(MJSDK_2SDK_ThirdParty_purchaseEvent_base _2SDK_Appsflyer_PurchaseEvent, Action<string> _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("appsflyer_purchaseEvent", MJSDK_AppsflyerLib.g_isInit, _failDelegate))
                return;

            MJSDK_AppsflyerLib.appsflyer_purchaseEvent(_2SDK_Appsflyer_PurchaseEvent, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[appsflyer_purchaseEvent] 购买事件上报成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("appsflyer_purchaseEvent", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 自定义事件上报
        /// </summary>
        /// <param name="_eventKey"></param>
        /// <param name="_ext"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void appsflyer_customEvent(string _eventKey, string _ext = "", Action<string> _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("appsflyer_customEvent", MJSDK_AppsflyerLib.g_isInit, _failDelegate))
                return;

            MJSDK_2SDK_ThirdParty_customEvent_base info = SDKTraceDataUtil.getThirdPartyEventData(_eventKey, _ext);

            MJSDK_AppsflyerLib.appsflyer_customEvent(info, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[appsflyer_customEvent] 自定义事件上报成功 key:", _eventKey, "，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("appsflyer_customEvent", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        #endregion


        #region MJSDK_AdjustLib

        /// <summary>
        /// adjust设备ID
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void adjust_adid(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("adjust_adid", MJSDK_AdjustLib.g_isInit, _failDelegate))
                return;

            MJSDK_AdjustLib.adjust_adid((_str) =>
            {
                SDKUtil.showSDKDebugLog("[adjust_adid] 获取adjust_adid成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("adjust_adid", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// adjust购买事件上报
        /// </summary>
        /// <param name="_2SDK_ThirdParty_PurchaseEvent_Base"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void adjust_purchaseEvent(MJSDK_Adjust_2SDK_adjust_purchaseEvent _2SDK_ThirdParty_PurchaseEvent_Base, Action<string> _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("adjust_purchaseEvent", MJSDK_AdjustLib.g_isInit, _failDelegate))
                return;

            MJSDK_AdjustLib.adjust_purchaseEvent(_2SDK_ThirdParty_PurchaseEvent_Base, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[appsflyer_purchaseEvent] 购买事件上报成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("adjust_purchaseEvent", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// adjust自定义事件上报
        /// </summary>
        /// <param name="_eventKey"></param>
        /// <param name="_ext"></param>
        /// <param name="_callbackId"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void adjust_customEvent(string _eventKey, string _ext = "", string _callbackId = null, Action<string> _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("adjust_customEvent", MJSDK_AdjustLib.g_isInit, _failDelegate))
                return;

            MJSDK_Adjust_2SDK_adjust_customEvent info = SDKTraceDataUtil.getAdjustEventData(_eventKey, _ext, _callbackId);

            MJSDK_AdjustLib.adjust_customEvent(info, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[adjust_customEvent] 自定义事件上报成功 key:", _eventKey, "，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("adjust_customEvent", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        #endregion


        #region MJSDK_JGPushLib

        /// <summary>
        /// 添加本地推送
        /// </summary>
        /// <param name="_title">标题</param>
        /// <param name="_subTitle">子标题</param>
        /// <param name="_body">内容</param>
        /// <param name="_userInfo">用户自定义参数(jsonStr)</param>
        /// <param name="_timeInterval">延迟推送（单位为秒），不填写默认为0就是即时推送</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void noti_local(string _title, string _subTitle, string _body, string _userInfo, long _timeInterval, Action<string> _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("noti_local", MJSDK_JGPushLib.g_isInit, _failDelegate))
                return;

            MJSDK_JGPush_2Engine_jgPush_local notiLocalInfo = new MJSDK_JGPush_2Engine_jgPush_local();
            notiLocalInfo.title = _title;
            notiLocalInfo.subTitle = _subTitle;
            notiLocalInfo.body = _body;
            notiLocalInfo.userInfo = _userInfo;
            notiLocalInfo.timeInterval = _timeInterval;

            MJSDK_JGPushLib.jgPush_local(notiLocalInfo, (_result) =>
            {
                SDKUtil.showSDKDebugLog("[noti_local] 设置本地推送成功，_result:", _result, ",title:", _title, ",timeInterval:", _timeInterval);
                _sucDelegate?.Invoke(_result);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("noti_local", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 移除本地推送
        /// </summary>
        /// <param name="_push_id"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void noti_removeLocal(string _push_id = null, Action _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("noti_removeLocal", MJSDK_JGPushLib.g_isInit, _failDelegate))
                return;

            MJSDK_JGPushLib.jgPush_removeLocal(_push_id, (_info) =>
            {
                SDKUtil.showSDKDebugLog("[noti_removeLocal] 移除本地推送成功，pushId:", _push_id);
                _sucDelegate?.Invoke();
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("noti_removeLocal", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        #endregion


        #region MJSDK_WebViewLib

        /// <summary>
        /// 网页展示
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void webView_show(string _url, Action _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("webView_show", MJSDK_WebViewLib.g_isInit, _failDelegate))
            {
                //失败直接使用浏览器打开
                Application.OpenURL(_url);
                return;
            }

            MJSDK_WebView_2SDK_webView_show info = new MJSDK_WebView_2SDK_webView_show();
            info.url = _url;

            MJSDK_WebViewLib.webView_show(info, (_info) =>
            {
                SDKUtil.showSDKDebugLog("[webView_show] 网页展示成功,url:", _url);
                _sucDelegate?.Invoke();
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("webView_show", _code, _msg);
                //失败直接使用浏览器打开
                Application.OpenURL(_url);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 浏览器展示（应用内）
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void webView_browserShow(string _url, Action _sucDelegate = null, Action<int, string> _failDelegate = null)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("webView_browserShow", MJSDK_WebViewLib.g_isInit, _failDelegate))
            {
                //失败直接使用浏览器打开
                Application.OpenURL(_url);
                return;
            }

            MJSDK_WebView_2SDK_webView_show info = new MJSDK_WebView_2SDK_webView_show();
            info.url = _url;

            MJSDK_WebViewLib.webView_browserShow(info, (_info) =>
            {
                SDKUtil.showSDKDebugLog("[webView_browserShow] 网页展示成功,url:", _url);
                _sucDelegate?.Invoke();
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("webView_browserShow", _code, _msg);
                //失败直接使用浏览器打开
                Application.OpenURL(_url);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        #endregion
    }
}
