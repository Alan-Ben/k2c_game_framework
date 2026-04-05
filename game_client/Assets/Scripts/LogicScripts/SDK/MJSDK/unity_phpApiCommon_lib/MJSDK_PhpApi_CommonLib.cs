using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// 服务端公共接口支持组件的相关接口类
    /// </summary>
    public class MJSDK_PhpApi_CommonLib
    {

        //组件标识符  — 库名
        private static string _g_MJSDK_LibName = "MJSDK_PhpApi_Common";
        //组件版本信息
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 4, 24, 0);
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
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 4, 16, 0);
#elif UNITY_ANDROID
            //组件库依赖Android平台库 版本判断
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 21, 0);
#endif
        }

        /// <summary>
        /// 检查初始化状态
        /// </summary>
        /// <returns></returns>
        public static bool checkInitState(Action<int, string> _failDelegate)
        {
            return MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate);
        }


        #region tool 
        /// <summary>
        /// 获取设备网络信息 (网络类型、网络服务商)
        /// </summary>
        /// <param name="_resDelegate"></param>
        public static void net_info(System.Action<MJSDK_PhpApiCommon_2Engine_net_info> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!checkInitState(_failDelegate))
            {
                return;
            }
            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("net", "info", "", new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_net_info>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// 获取ip（ 如ip、国家、地区）
        /// </summary>
        /// <param name="_resDelegate"></param>
        public static void net_ip(System.Action<MJSDK_PhpApiCommon_2Engine_net_ip> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!checkInitState(_failDelegate))
            {
                return;
            }
            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("net", "ip", "", new MJSDK_CallbackDealer_Model<MJSDK_PhpApiCommon_2Engine_net_ip>(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// translation-google:谷歌云翻译
        /// </summary>
        /// <param name="_2SDK_Translation_Google"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void translation_google(MJSDK_PhpApiCommon_2SDK_translation_google _2SDK_Translation_Google, System.Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!checkInitState(_failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_2SDK_Translation_Google == null
                || string.IsNullOrEmpty(_2SDK_Translation_Google.content)
                || string.IsNullOrEmpty(_2SDK_Translation_Google.target))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "translation_google --> Parameters are missing,content/target is null");
                }
                return;
            }

            //转换为mj标准语言
            _2SDK_Translation_Google.target = MJSDK_MJUtil.convertMJLanType(_2SDK_Translation_Google.target);

            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("translation", "google", JsonUtility.ToJson(_2SDK_Translation_Google), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        #endregion
    }
}
