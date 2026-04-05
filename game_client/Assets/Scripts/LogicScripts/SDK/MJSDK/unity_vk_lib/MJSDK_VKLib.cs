using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// vk 组件
    /// </summary>
    public class MJSDK_VKLib
    {
        //组件标识符  — 库名
        private static string _g_MJSDK_LibName = "MJSDK_VK";
        //组件版本信息
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 4, 11, 0);
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
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 4, 6, 0);
#elif UNITY_ANDROID
            //组件库依赖Android平台库 版本判断
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 0, 3);
#endif
        }

        /// <summary>
        /// 获取vk用户信息
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        [UnityEngine.Scripting.Preserve]
        public static void vk_userInfo(Action<MJSDK_PhpApiCommon_VK_UserInfo> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }
            MJSDK.sendMsgToPhonePlatform("vk", "userInfo", "", new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_VK_UserInfo>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// vk分享
        /// </summary>
        /// <param name="_2SDK_Vk_Share"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void vk_share(MJSDK_VK_2SDK_vk_share _2SDK_Vk_Share, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (_2SDK_Vk_Share == null || string.IsNullOrEmpty(_2SDK_Vk_Share.text))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_VKError.C_Unity_Param_Miss, "vk_share --> Parameters are missing, text is null");
                }
                return;
            }


            MJSDK.sendMsgToPhonePlatform("vk", "share", JsonUtility.ToJson(_2SDK_Vk_Share), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
    }
}
