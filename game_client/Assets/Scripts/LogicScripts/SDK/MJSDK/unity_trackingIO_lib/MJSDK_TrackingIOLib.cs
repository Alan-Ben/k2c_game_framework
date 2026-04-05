using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// TrackingIO(热云) 组件
    /// </summary>
    public class MJSDK_TrackingIOLib
    {
        //组件标识符  — 库名
        private static string _g_MJSDK_LibName = "MJSDK_TrackingIO";
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
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 4, 0);
#elif UNITY_ANDROID
            //组件库依赖Android平台库 版本判断
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 1, 0);
#endif
        }

        /// <summary>
        /// trackingIO-purchaseEvent：购买事件上报
        /// </summary>
        /// <param name="_2SDK_TrackingIO_PurchaseEvent"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void trackingIO_purchaseEvent(MJSDK_2SDK_ThirdParty_purchaseEvent_base _2SDK_TrackingIO_PurchaseEvent, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (!MJSDK_2SDK_ThirdParty_purchaseEvent_base.checkParam(_2SDK_TrackingIO_PurchaseEvent))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_TrackingIOError.C_Unity_Param_Miss, "trackingIO_purchaseEvent --> Parameters are missing");
                }
                return;
            }


            MJSDK.sendMsgToPhonePlatform("trackingIO", "purchaseEvent", JsonUtility.ToJson(_2SDK_TrackingIO_PurchaseEvent), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// trackingIO-customEvent：自定义事件上报
        /// </summary>
        /// <param name="_2SDK_TrackingIO_CustomEvent"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void trackingIO_customEvent(MJSDK_2SDK_ThirdParty_customEvent_base _2SDK_TrackingIO_CustomEvent, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (!MJSDK_2SDK_ThirdParty_customEvent_base.checkParam(_2SDK_TrackingIO_CustomEvent))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_TrackingIOError.C_Unity_Param_Miss, "trackingIO_customEvent --> Parameters are missing,event_key is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("trackingIO", "customEvent", JsonUtility.ToJson(_2SDK_TrackingIO_CustomEvent), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
    }
}