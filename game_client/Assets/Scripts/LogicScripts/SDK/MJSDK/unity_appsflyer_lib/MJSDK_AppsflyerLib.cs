using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// appsflyer 组件
    /// </summary>
    public class MJSDK_AppsflyerLib
    {
        //组件标识符  — 库名
        private static string _g_MJSDK_LibName = "MJSDK_Appsflyer";
        //组件版本信息
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 6, 9, 0);
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
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 6, 12, 0);
#elif UNITY_ANDROID
            //组件库依赖Android平台库 版本判断
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 6, 0);
#endif
        }

        /// <summary>
        /// 获取appsflyer uid
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void appsflyer_uid(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }
            MJSDK.sendMsgToPhonePlatform("appsflyer", "uid", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// appsflyer-purchaseEvent：购买事件上报
        /// </summary>
        /// <param name="_2SDK_Appsflyer_PurchaseEvent"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void appsflyer_purchaseEvent(MJSDK_2SDK_ThirdParty_purchaseEvent_base _2SDK_Appsflyer_PurchaseEvent, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (!MJSDK_2SDK_ThirdParty_purchaseEvent_base.checkParam(_2SDK_Appsflyer_PurchaseEvent))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_AppsflyerError.C_Unity_Param_Miss, "appsflyer_purchaseEvent --> Parameters are missing");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("appsflyer", "purchaseEvent", JsonUtility.ToJson(_2SDK_Appsflyer_PurchaseEvent), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// appsflyer-customEvent：自定义事件上报
        /// </summary>
        /// <param name="_2SDK_Appsflyer_CustomEvent"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void appsflyer_customEvent(MJSDK_2SDK_ThirdParty_customEvent_base _2SDK_Appsflyer_CustomEvent, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (!MJSDK_2SDK_ThirdParty_customEvent_base.checkParam(_2SDK_Appsflyer_CustomEvent))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_AppsflyerError.C_Unity_Param_Miss, "appsflyer_customEvent --> Parameters are missing,event_key is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("appsflyer", "customEvent", JsonUtility.ToJson(_2SDK_Appsflyer_CustomEvent), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
    }
}