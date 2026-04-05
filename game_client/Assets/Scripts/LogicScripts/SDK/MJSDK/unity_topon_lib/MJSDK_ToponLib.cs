using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// topon 组件
    /// </summary>
    public class MJSDK_ToponLib
    {
        //组件标识符  — 库名
        private static string _g_MJSDK_LibName = "MJSDK_Topon";
        //组件版本信息
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 1, 3, 0);
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
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 0, 0);
#elif UNITY_ANDROID
            //组件库依赖Android平台库 版本判断
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 0, 0);
#endif
        }

        /// <summary>
        /// topon-rewarded：激励广告加载
        /// </summary>
        /// <param name="_2SDK_Twitter_AccountLogin"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void topon_rewarded(MJSDK_Topon_2SDK_topon_rewarded _Topon_Rewarded, System.Action<MJSDK_Topon_2Engine_topon_rewarded> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_Topon_Rewarded == null
                || string.IsNullOrEmpty(_Topon_Rewarded.placementId))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_ToponError.C_Unity_Param_Miss, "topon_rewarded --> Parameters are missing,placementId is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("topon", "rewarded", JsonUtility.ToJson(_Topon_Rewarded), new MJSDK_CallbackDealer_Model<MJSDK_Topon_2Engine_topon_rewarded>(_sucDelegate, _failDelegate));
        }
    }
}