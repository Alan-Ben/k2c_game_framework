using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// firebase组件库
    /// </summary>
    public class MJSDK_FirebaseLib
    {
        //组件标识符  — 库名
        private static string _g_MJSDK_LibName = "MJSDK_Firebase";
        //组件版本信息
        public static MJSDK_Component_Version g_Version = new MJSDK_Component_Version(_g_MJSDK_LibName, 0, 6, 10, 0);
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
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 6, 13, 0);
#elif UNITY_ANDROID
            //组件库依赖Android平台库 版本判断
            _g_isInit = g_Version.judgePlatformDependenceVersionIsEnough(_platform_ComponentInfo.version, 0, 1, 4, 0);
#endif
        }

        #region 事件上报
        /// <summary>
        /// firebase_purchaseEvent：购买事件上报
        /// </summary>
        /// <param name="_2SDK_Firebase_PurchaseEvent"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void firebase_purchaseEvent(MJSDK_2SDK_ThirdParty_purchaseEvent_base _2SDK_Firebase_PurchaseEvent, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (!MJSDK_2SDK_ThirdParty_purchaseEvent_base.checkParam(_2SDK_Firebase_PurchaseEvent))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_FirebaseError.C_Unity_Param_Miss, "firebase_purchaseEvent --> Parameters are missing");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("firebase", "purchaseEvent", JsonUtility.ToJson(_2SDK_Firebase_PurchaseEvent), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// firebase-customEvent：自定义事件上报
        /// </summary>
        /// <param name="_2SDK_Firebase_CustomEvent"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void firebase_customEvent(MJSDK_2SDK_ThirdParty_customEvent_base _2SDK_Firebase_CustomEvent, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (!MJSDK_2SDK_ThirdParty_customEvent_base.checkParam(_2SDK_Firebase_CustomEvent))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_FirebaseError.C_Unity_Param_Miss, "firebase_customEvent --> Parameters are missing,event_key is null");
                }
                return;
            }

            MJSDK.sendMsgToPhonePlatform("firebase", "customEvent", JsonUtility.ToJson(_2SDK_Firebase_CustomEvent), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
        #endregion

        #region 崩溃日志相关
        /// <summary>
        /// firebase-setUserId：设置用户标识
        /// </summary>
        /// <param name="_userId">用户标识</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void firebase_setUserId(string _userId, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (string.IsNullOrEmpty(_userId))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_FirebaseError.C_Unity_Param_Miss, "firebase_setUserId --> Parameters are missing,_userId is null");
                }
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("user_id", _userId);


            MJSDK.sendMsgToPhonePlatform("firebase", "setUserId", ht.toJson(), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// firebase-setCustomKey：添加自定义键
        /// </summary>
        /// <param name="_key_value">自定义键(jsonstr)</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void firebase_setCustomKey(string _key_value, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (string.IsNullOrEmpty(_key_value))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_FirebaseError.C_Unity_Param_Miss, "firebase_setCustomKey --> Parameters are missing,_key_value is null");
                }
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("key_value", _key_value);

            MJSDK.sendMsgToPhonePlatform("firebase", "setCustomKey", ht.toJson(), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// firebase-customLog：自定义日志
        /// </summary>
        /// <param name="_cusLog">自定义日志</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void firebase_customLog(string _cusLog, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (string.IsNullOrEmpty(_cusLog))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_FirebaseError.C_Unity_Param_Miss, "firebase_customLog --> Parameters are missing,_cusLog is null");
                }
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("mes", _cusLog);

            MJSDK.sendMsgToPhonePlatform("firebase", "customLog", ht.toJson(), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// firebase-customExc：自定义异常上报（非严重异常）
        /// </summary>
        /// <param name="_cusLog">自定义异常上报（非严重异常）</param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public static void firebase_customExc(string _mes, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (!MJSDK_Tool_Lib.checkInitState(_g_MJSDK_LibName, _g_isInit, _failDelegate))
            {
                return;
            }

            if (string.IsNullOrEmpty(_mes))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_FirebaseError.C_Unity_Param_Miss, "firebase_customLog --> Parameters are missing,_mes is null");
                }
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("mes", _mes);

            MJSDK.sendMsgToPhonePlatform("firebase", "customExc", ht.toJson(), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
        #endregion
    }
}
