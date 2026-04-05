
using UnityEngine;
using System;


namespace MJSDK_Package
{
    /// <summary>
    ///MJSDK_OneStoreLib
    /// </summary>
    public class MJSDK_AdMobLib
    {
        //组件标识符  --- 库名
        private static string _g_MJSDK_LibName = "MJSDK_AdMob";
        //组件版本信息
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 1, 1, 0);
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

#if UNITY_ANDROID
            //组件库依赖Android平台库 版本判断
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 0, 0);
#endif
        }

        /// <summary>
        /// admob_load：广告加载
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static void admob_load(string msg, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            MJSDK.sendMsgToPhonePlatform("admob", "load", msg, new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// admob_rewarded：广告播放
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static void admob_rewarded(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            MJSDK.sendMsgToPhonePlatform("admob", "rewarded", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
    }
}

