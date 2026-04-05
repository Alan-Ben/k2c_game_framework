using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// applovin 组件
    /// </summary>
    public class MJSDK_AppLovinLib
    {

        //组件标识符  — 库名
        private static string _g_MJSDK_LibName = "MJSDK_AppLovin";
        //组件版本信息
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 7, 12, 0);
        //是否初始化
        private static bool _g_isInit;
        //组件库初始化状态获取
        public static bool g_isInit { get { return _g_isInit; } }

        /// <summary>
        /// 初始化函数 (common组件自动调用)
        /// </summary>
        /// <param name="_platform_ComponentInfo"></param>
        [UnityEngine.Scripting.Preserve]
        public static void init(MJSDK_Basic_ComponentInfo _platform_ComponentInfo)
        {
            if (_g_isInit)
            {
                Debug.Log(_g_MJSDK_LibName + " already init");
                return;
            }

            if (_platform_ComponentInfo == null)
            {
                Debug.Log(_g_MJSDK_LibName + " _platform_ComponentInfo empty");
                return;
            }

            //暂无需要处理的部分
            g_Version.printVersion();
#if UNITY_IOS
            //组件库依赖iOS平台库 版本判断
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 8, 4, 0);
#elif UNITY_ANDROID
            //组件库依赖Android平台库 版本判断
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 0, 3);
#endif
        }

        /// <summary>
        /// 插入式广告
        /// </summary>
        /// <param name="_2SDK_MaxAd_Interstitial"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void maxAd_interstitial(MJSDK_AppLovin_2SDK_maxAd_interstitial _2SDK_MaxAd_Interstitial, Action<MJSDK_AppLovin_2Engine_maxAd> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_MaxAd_Interstitial == null
                || string.IsNullOrEmpty(_2SDK_MaxAd_Interstitial.adUnitId))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_AppleLoginError.C_Unity_Param_Miss, "maxAd_interstitial --> Parameters are missing,adUnitId is null");
                }
                return;
            }
            MJSDK.sendMsgToPhonePlatform("maxAd", "interstitial", JsonUtility.ToJson(_2SDK_MaxAd_Interstitial), new MJSDK_CallbackDealer_Model<MJSDK_AppLovin_2Engine_maxAd>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 激励广告
        /// </summary>
        /// <param name="_2SDK_MaxAd_Rewarded"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void maxAd_rewarded(MJSDK_AppLovin_2SDK_maxAd_rewarded _2SDK_MaxAd_Rewarded, Action<MJSDK_AppLovin_2Engine_maxAd> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_MaxAd_Rewarded == null
                || string.IsNullOrEmpty(_2SDK_MaxAd_Rewarded.adUnitId))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_AppleLoginError.C_Unity_Param_Miss, "maxAd_rewarded --> Parameters are missing,adUnitId is null");
                }
                return;
            }
            MJSDK.sendMsgToPhonePlatform("maxAd", "rewarded", JsonUtility.ToJson(_2SDK_MaxAd_Rewarded), new MJSDK_CallbackDealer_Model<MJSDK_AppLovin_2Engine_maxAd>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 横幅广告
        /// </summary>
        /// <param name="_2SDK_MaxAd_Banner"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void maxAd_banner(MJSDK_AppLovin_2SDK_maxAd_banner _2SDK_MaxAd_Banner, Action<MJSDK_AppLovin_2Engine_maxAd> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_MaxAd_Banner == null
                || string.IsNullOrEmpty(_2SDK_MaxAd_Banner.adUnitId))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_AppleLoginError.C_Unity_Param_Miss, "maxAd_banner --> Parameters are missing,adUnitId is null");
                }
                return;
            }
            MJSDK.sendMsgToPhonePlatform("maxAd", "banner", JsonUtility.ToJson(_2SDK_MaxAd_Banner), new MJSDK_CallbackDealer_Model<MJSDK_AppLovin_2Engine_maxAd>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 关闭横幅广告
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void maxAd_closeBanner(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }
            MJSDK.sendMsgToPhonePlatform("maxAd", "closeBanner", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// mrec广告
        /// </summary>
        /// <param name="_2SDK_MaxAd_Mrec"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void maxAd_mrec(MJSDK_AppLovin_2SDK_maxAd_mrec _2SDK_MaxAd_Mrec, Action<MJSDK_AppLovin_2Engine_maxAd> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_MaxAd_Mrec == null
                || string.IsNullOrEmpty(_2SDK_MaxAd_Mrec.adUnitId))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_AppleLoginError.C_Unity_Param_Miss, "maxAd_mrec --> Parameters are missing,adUnitId is null");
                }
                return;
            }
            MJSDK.sendMsgToPhonePlatform("maxAd", "mrec", JsonUtility.ToJson(_2SDK_MaxAd_Mrec), new MJSDK_CallbackDealer_Model<MJSDK_AppLovin_2Engine_maxAd>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 关闭mrec广告
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void maxAd_closeMREC(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }
            MJSDK.sendMsgToPhonePlatform("maxAd", "closeMREC", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// 网络中介调试器
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void maxAd_mediationDebugger(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }
            MJSDK.sendMsgToPhonePlatform("maxAd", "mediationDebugger", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
    }
}