using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// Facebook 组件
    /// </summary>
    public class MJSDK_FacebookLib
    {
        //组件标识符  — 库名
        private static string _g_MJSDK_LibName = "MJSDK_Facebook";
        //组件版本号初始化
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 2, 14, 0);
        //初始化判断
        private static bool _g_isInit = false;
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
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 2, 13, 0);
#elif UNITY_ANDROID
            //组件库依赖Android平台库 版本判断
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 5, 0);
#endif
        }

        /// <summary>
        /// 获取Facebook用户信息
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void facebook_userInfo(Action<MJSDK_PhpApiCommon_Facebook_UserInfo> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }
            MJSDK.sendMsgToPhonePlatform("facebook", "userInfo", "", new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_Facebook_UserInfo>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// facebook-purchaseEvent：购买事件上报
        /// facebook_purchaseEvent 主要是针对不是从google/apple上支付的订单需要手动统计（比如网页支付、支付宝、微信）
        /// 应用如果是谷歌支付、苹果支付（就是Facebook能自动统计到支付方式）请不要对接这个口。如果不确定要不要接联系SDK技术进行核实
        /// </summary>
        /// <param name="_2SDK_Facebook_PurchaseEvent"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void facebook_purchaseEvent(MJSDK_2SDK_ThirdParty_purchaseEvent_base _2SDK_Facebook_PurchaseEvent, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (!MJSDK_2SDK_ThirdParty_purchaseEvent_base.checkParam(_2SDK_Facebook_PurchaseEvent))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_FacebookError.C_Unity_Param_Miss, "facebook_purchaseEvent --> Parameters are missing");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("facebook", "purchaseEvent", JsonUtility.ToJson(_2SDK_Facebook_PurchaseEvent), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// facebook-customEvent：自定义事件上报
        /// </summary>
        /// <param name="_2SDK_Facebook_CustomEvent"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void facebook_customEvent(MJSDK_2SDK_ThirdParty_customEvent_base _2SDK_Facebook_CustomEvent, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (!MJSDK_2SDK_ThirdParty_customEvent_base.checkParam(_2SDK_Facebook_CustomEvent))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_FacebookError.C_Unity_Param_Miss, "facebook_customEvent --> Parameters are missing, event_key is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("facebook", "customEvent", JsonUtility.ToJson(_2SDK_Facebook_CustomEvent), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// facebook 图片分享
        /// </summary>
        /// <param name="_2SDK_Facebook_ShareImg"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void facebook_shareImg(MJSDK_Facebook_2SDK_facebook_shareImg _2SDK_Facebook_ShareImg, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (_2SDK_Facebook_ShareImg == null
                || string.IsNullOrEmpty(_2SDK_Facebook_ShareImg.img_path))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_FacebookError.C_Unity_Param_Miss, "facebook_shareImg --> Parameters are missing, img_path is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("facebook", "shareImg", JsonUtility.ToJson(_2SDK_Facebook_ShareImg), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// facebook 链接分享
        /// </summary>
        /// <param name="_2SDK_Facebook_ShareImg"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void facebook_shareLink(MJSDK_Facebook_2SDK_facebook_shareLink _2SDK_Facebook_ShareLink, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (_2SDK_Facebook_ShareLink == null
                || string.IsNullOrEmpty(_2SDK_Facebook_ShareLink.link))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_FacebookError.C_Unity_Param_Miss, "facebook_shareLink --> Parameters are missing, link is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("facebook", "shareLink", JsonUtility.ToJson(_2SDK_Facebook_ShareLink), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
    }
}
