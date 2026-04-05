using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// 推送 组件
    /// </summary>
    public class MJSDK_MobPushLib
    {
        //组件标识符  — 库名
        private static string _g_MJSDK_LibName = "MJSDK_MobPush";
        //组件版本信息
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 11, 6, 0);
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
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 11, 4, 0);
#elif UNITY_ANDROID
            //组件库依赖Android平台库 版本判断
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 0, 4);
#endif
        }

        /// <summary>
        /// noti-local：本地推送
        /// </summary>
        /// <param name="_2SDK_Noti_Local"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void noti_local(MJSDK_MobPush_2SDK_noti_local _2SDK_Noti_Local, Action<MJSDK_MobPush_2Engine_noti_local> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (_2SDK_Noti_Local == null || string.IsNullOrEmpty(_2SDK_Noti_Local.title) || string.IsNullOrEmpty(_2SDK_Noti_Local.body))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_MobPushError.C_Unity_Param_Miss, "noti_local --> Parameters are missing, title/body is null");
                }
                return;
            }


            MJSDK.sendMsgToPhonePlatform("noti", "local", JsonUtility.ToJson(_2SDK_Noti_Local), new MJSDK_CallbackDealer_Model<MJSDK_MobPush_2Engine_noti_local>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// noti-removeLocal：移除本地推送
        /// </summary>
        /// <param name="_2Engine_Noti_Local"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void noti_removeLocal(MJSDK_MobPush_2Engine_noti_local _2Engine_Noti_Local, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //如果传空对象，默认进行初始化
            if (_2Engine_Noti_Local == null)
            {
                _2Engine_Noti_Local = new MJSDK_MobPush_2Engine_noti_local();
                _2Engine_Noti_Local.push_id = "";
            }



            MJSDK.sendMsgToPhonePlatform("noti", "removeLocal", JsonUtility.ToJson(_2Engine_Noti_Local), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// noti-rId：获取Registration ID
        /// </summary>
        public static void noti_regid(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }


            MJSDK.sendMsgToPhonePlatform("noti", "regid", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// noti-addTags：添加标签
        /// </summary>
        public static void noti_addTags(string _tags, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (string.IsNullOrEmpty(_tags))
            {
                _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "noti_addTags --> Parameters are missing,_tags is null");
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("tag", _tags);

            MJSDK.sendMsgToPhonePlatform("noti", "addTags", ht.toJson(), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// noti-deleteTags：删除标签
        /// </summary>
        public static void noti_deleteTags(string _tags, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (string.IsNullOrEmpty(_tags))
            {
                _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "noti_deleteTags --> Parameters are missing,_tags is null");
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("tag", _tags);

            MJSDK.sendMsgToPhonePlatform("noti", "deleteTags", ht.toJson(), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
    }
}