using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// aihelp 客服脚本库
    /// </summary>
    public class MJSDK_AIHelpLib
    {
        //组件标识符  — 库名
        private static string _g_MJSDK_LibName = "MJSDK_AIHelp";
        //组件版本信息
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 5, 10, 0);
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
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 6, 1, 0);
#elif UNITY_ANDROID
            //组件库依赖Android平台库 版本判断
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 0, 6);
#endif
        }

        /// <summary>
        /// aihelp-init：aihelp初始化
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void aihelp_init(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

#if UNITY_IOS
            if (_sucDelegate != null)
            {
                _sucDelegate("IOS端自动初始化");
            }
            return;
#endif

            MJSDK.sendMsgToPhonePlatform("aihelp", "init", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// aihelp-setUserInfo：设置玩家信息
        /// </summary>
        /// <param name="_2SDK_Aihelp_SetUserInfo"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void aihelp_setUserInfo(MJSDK_AIHelp_2SDK_aihelp_setUserInfo _2SDK_Aihelp_SetUserInfo, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必填参数判空
            if (_2SDK_Aihelp_SetUserInfo == null
                || string.IsNullOrEmpty(_2SDK_Aihelp_SetUserInfo.user_id)
                || string.IsNullOrEmpty(_2SDK_Aihelp_SetUserInfo.user_name)
                || string.IsNullOrEmpty(_2SDK_Aihelp_SetUserInfo.server_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_AIHelpError.C_Unity_Param_Miss, "aihelp_setUserInfo --> Parameters are missing,user_id/user_name/server_id is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("aihelp", "setUserInfo", JsonUtility.ToJson(_2SDK_Aihelp_SetUserInfo), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// aihelp-cleanUserInfo：清除玩家信息
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void aihelp_cleanUserInfo(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            MJSDK.sendMsgToPhonePlatform("aihelp", "cleanUserInfo", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// aihelp-updateLan：更新语言
        /// </summary>
        /// <param name="_lan">//语言标识（不能随便填哦哦 ） 请参考：https://docs.aihelp.net/zh/docs/configuration/language/#%E8%AF%AD%E8%A8%80%E7%A0%81%E5%AF%B9%E7%85%A7%E8%A1%A8</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void aihelp_updateLan(string _lan, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必填参数判空
            if (string.IsNullOrEmpty(_lan))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_AIHelpError.C_Unity_Param_Miss, "aihelp_updateLan --> Parameters are missing,_lan is null");
                }
                return;
            }


            Hashtable ht = new Hashtable();
            ht.Add("lan", MJSDK_MJUtil.convertMJLanType(_lan));

            MJSDK.sendMsgToPhonePlatform("aihelp", "updateLan", ht.toJson(), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// aihelp-showRPA：机器人流程自动化
        /// </summary>
        /// <param name="_2SDK_Aihelp_ShowAllFAQ"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void aihelp_showRPA(MJSDK_AIHelp_2SDK_aihelp_showRPA _2SDK_Aihelp_ShowRPA, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必填参数判空
            if (_2SDK_Aihelp_ShowRPA == null || string.IsNullOrEmpty(_2SDK_Aihelp_ShowRPA.entranceId))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_AIHelpError.C_Unity_Param_Miss, "aihelp_showRPA --> Parameters are missing,_2SDK_Aihelp_ShowAllFAQ is null or entranceId is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("aihelp", "showRPA", JsonUtility.ToJson(_2SDK_Aihelp_ShowRPA), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
    }
}

