using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// 极光（海外）推送 组件
    /// </summary>
    public class MJSDK_JGPushLib
    {
        //组件标识符  — 库名
        private static string _g_MJSDK_LibName = "MJSDK_JGPush";
        //组件版本信息
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 1, 2, 0);
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

        #region 基础接口
        /// <summary>
        /// jgPush-regid：获取Registration ID
        /// </summary>
        public static void jgPush_regid(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }


            MJSDK.sendMsgToPhonePlatform("jgPush", "regid", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// jgPush-setUserLan：设置用户语言
        /// </summary>
        /// <param name="_lan">语言码见 https://alidocs.dingtalk.com/i/nodes/r1R7q3QmWe7LX5xPUmB71q0bJxkXOEP2</param>
        public static void jgPush_setUserLan(string _lan, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (string.IsNullOrEmpty(_lan))
            {
                _failDelegate(MJSDK_JGPushError.C_Unity_Param_Miss, "jgPush_setUserLan --> Parameters are missing,_lan is null");
                return;
            }

            //MJSDK 标准语言zh-CN转转成jg 简体中文
            if (_lan.Equals("zh-CN") || _lan.Equals("zh_CN"))
            {
                //转成jg 简体中文
                _lan = "zh-Hans";
            }
            //MJSDK 标准语言zh-TW转转成jg 繁体中文
            if (_lan.Equals("zh-TW") || _lan.Equals("zh_TW"))
            {
                //转成jg 繁体中文
                _lan = "zh-Hant";
            }


            MJSDK.sendMsgToPhonePlatform("jgPush", "setUserLan", _lan, new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// jgPush-checkPermission：判定是否有推送权限
        /// </summary>
        public static void jgPush_checkPermission(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            MJSDK.sendMsgToPhonePlatform("jgPush", "checkPermission", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
        #endregion


        #region 本地推送
        /// <summary>
        /// jgPush-local：本地推送
        /// </summary>
        /// <param name="_2SDK_Noti_Local"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void jgPush_local(MJSDK_JGPush_2Engine_jgPush_local _2Engine_JgPush_Local, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (_2Engine_JgPush_Local == null
                || string.IsNullOrEmpty(_2Engine_JgPush_Local.title)
                || string.IsNullOrEmpty(_2Engine_JgPush_Local.body))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_JGPushError.C_Unity_Param_Miss, "jgPush_local --> Parameters are missing, title/body is null");
                }
                return;
            }


            MJSDK.sendMsgToPhonePlatform("jgPush", "local", JsonUtility.ToJson(_2Engine_JgPush_Local), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// jgPush-removeLocal：移除本地推送
        /// </summary>
        /// <param name="_2Engine_Noti_Local"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void jgPush_removeLocal(string _push_id, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            MJSDK.sendMsgToPhonePlatform("jgPush", "removeLocal", _push_id, new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
        #endregion

        #region 远程推送-标签
        /// <summary>
        /// jgPush-addTags：添加标签
        /// </summary>
        public static void jgPush_addTags(MJSDK_JGPush_2Engine_jgPush_tag _jgPush_tag, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_jgPush_tag == null || _jgPush_tag.tags == null || _jgPush_tag.tags.Count == 0)
            {
                _failDelegate(MJSDK_JGPushError.C_Unity_Param_Miss, "jgPush_addTags --> Parameters are missing,tags is null");
                return;
            }

            MJSDK.sendMsgToPhonePlatform("jgPush", "addTags", JsonUtility.ToJson(_jgPush_tag), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// jgPush-deleteTags：删除标签
        /// </summary>
        public static void jgPush_deleteTags(MJSDK_JGPush_2Engine_jgPush_tag _jgPush_tag, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            //必要参数不得为空
            if (_jgPush_tag == null || _jgPush_tag.tags == null || _jgPush_tag.tags.Count == 0)
            {
                _failDelegate(MJSDK_JGPushError.C_Unity_Param_Miss, "jgPush_deleteTags --> Parameters are missing,tags is null");
                return;
            }

            MJSDK.sendMsgToPhonePlatform("jgPush", "deleteTags", JsonUtility.ToJson(_jgPush_tag), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// jgPush-cleanTags：删除所有标签
        /// </summary>
        public static void jgPush_cleanTags(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            MJSDK.sendMsgToPhonePlatform("jgPush", "cleanTags", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// jgPush-getTags：获取标签
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void jgPush_getTags(Action<MJSDK_JGPush_2Engine_jgPush_tag> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            MJSDK.sendMsgToPhonePlatform("jgPush", "getTags", "", new MJSDK_CallbackDealer_Model<MJSDK_JGPush_2Engine_jgPush_tag>(_sucDelegate, _failDelegate));
        }
        #endregion
    }
}