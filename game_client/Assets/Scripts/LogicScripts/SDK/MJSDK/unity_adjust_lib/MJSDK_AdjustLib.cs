
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// adjust 组件
    /// </summary>
    public class MJSDK_AdjustLib
    {
        //组件标识符  — 库名
        private static string _g_MJSDK_LibName = "MJSDK_Adjust";
        //组件版本信息
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 1, 4, 0);
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
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 3, 0);
#elif UNITY_ANDROID
            //组件库依赖Android平台库 版本判断
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 1, 0);
#endif
        }

        /// <summary>
        /// adjust-adid：adjust设备ID
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void adjust_adid(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            MJSDK.sendMsgToPhonePlatform("adjust", "adid", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// adjust-purchaseEvent：购买事件上报
        /// </summary>
        /// <param name="_2SDK_ThirdParty_PurchaseEvent_Base"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void adjust_purchaseEvent(MJSDK_Adjust_2SDK_adjust_purchaseEvent _2SDK_ThirdParty_PurchaseEvent_Base, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (_2SDK_ThirdParty_PurchaseEvent_Base == null
                || string.IsNullOrEmpty(_2SDK_ThirdParty_PurchaseEvent_Base.event_key)
                || !MJSDK_2SDK_ThirdParty_purchaseEvent_base.checkParam(_2SDK_ThirdParty_PurchaseEvent_Base))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_AppleLoginError.C_Unity_Param_Miss, "appsflyer_purchaseEvent --> Parameters are missing");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("adjust", "purchaseEvent", JsonUtility.ToJson(_2SDK_ThirdParty_PurchaseEvent_Base), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// adjust-customEvent：自定义事件上报
        /// </summary>
        /// <param name="_2SDK_ThirdParty_CustomEvent_Base"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void adjust_customEvent(MJSDK_Adjust_2SDK_adjust_customEvent _2SDK_ThirdParty_CustomEvent_Base, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_2SDK_ThirdParty_customEvent_base.checkParam(_2SDK_ThirdParty_CustomEvent_Base))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_AppleLoginError.C_Unity_Param_Miss, "appsflyer_customEvent --> Parameters are missing,event_key is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("adjust", "customEvent", JsonUtility.ToJson(_2SDK_ThirdParty_CustomEvent_Base), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
    }
}