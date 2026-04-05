using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace MJSDK_Package
{
    /// <summary>
    /// 系统基础组件的相关接口类
    /// </summary>
    public class MJSDK_BasicLib
    {

        /// <summary>
        /// mj-componentInfo：获取项目所集成组件信息
        /// </summary>
        /// <param name="_tag"></param>
        /// <param name="_resDelegate"></param>
        public static void mj_componentInfo(Action<MJSDK_Basic_2Engine_mj_componentInfo> _sucDelegate, Action<int, string> _failDelegate)
        {
            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("mj", "componentInfo", "", new MJSDK_CallbackDealer_Model<MJSDK_Basic_2Engine_mj_componentInfo>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// mj_initThirdSDK：初始化第三方SDK(初始化mjsdk、初始化各组件包含的SDK)
        /// </summary>
        /// <param name="_tag"></param>
        /// <param name="_resDelegate"></param>
        public static void mj_initThirdSDK(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("mj", "initSDK", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// mj_sdkTrace：SDK埋点
        /// 警告：此接口禁用在任何SDK回执上调用，防止循环引用。
        /// </summary>
        /// <param name="_resDelegate"></param>
        public static void mj_sdkTrace(string traceStep, string ext)
        {
            MJSDK_2SDK_PHP_SDK_Trace trace = new MJSDK_2SDK_PHP_SDK_Trace();
            trace.step = traceStep;
            trace.extend = ext;
            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("mj", "sdkTrace", JsonUtility.ToJson(trace), new MJSDK_CallbackDealer_Action((string result) => { }, (int errCode, string errMsg) => { }));
        }

        /// <summary>
        /// 根据带入的标记，获取对应的系统信息
        /// </summary>
        /// <param name="_tag"></param>
        /// <param name="_resDelegate"></param>
        public static void sys_info(string _tag, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //必要参数不得为空
            if (string.IsNullOrEmpty(_tag))
            {
                _failDelegate(MJSDK_Error.C_Unity_Param_Miss, "sys_info --> Parameters are missing,_tag is null");
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("tag", _tag);

            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("sys", "info", ht.toJson(), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// 根据带入的标记，获取对应的组件版本号
        /// </summary>
        /// <param name="_tag"></param>
        /// <param name="_resDelegate"></param>
        public static void sys_version(string _componentTag, Action<MJSDK_Version_Model> _sucDelegate, Action<int, string> _failDelegate)
        {
            //必要参数不得为空
            if (string.IsNullOrEmpty(_componentTag))
            {
                _failDelegate(MJSDK_Error.C_Unity_Param_Miss, "sys_version --> Parameters are missing,_componentTag is null");
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("tag", _componentTag);

            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("sys", "version", ht.toJson(), new MJSDK_CallbackDealer_Model<MJSDK_Version_Model>(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// sys-appStore：跳转至商店（苹果跳转App Store，谷歌跳转至谷歌Play）
        /// </summary>
        /// <param name="_app_store_url">应用商店地址</param>
        /// <param name="_resDelegate"></param>
        public static void sys_appStore(string _app_store_url, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //必要参数不得为空
            if (string.IsNullOrEmpty(_app_store_url))
            {
                _failDelegate(MJSDK_Error.C_Unity_Param_Miss, "sys_appStore --> Parameters are missing,app_store_url is null");
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("app_store_url", _app_store_url);

            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("sys", "appStore", ht.toJson(), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// sys-vibrator：振动
        /// </summary>
        /// <param name="ms">毫秒</param>
        /// <param name="_resDelegate"></param>
        public static void sys_vibrator(long _ms, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {

            Hashtable ht = new Hashtable();
            ht.Add("ms", _ms.ToString());

            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("sys", "vibrator", ht.toJson(), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// sys-mjAdfrom: 梦加数据中心 -- 广告渠道标识信息
        /// </summary>
        /// <param name="_resDelegate"></param>
        public static void sys_mjAdfrom(Action<MJSDK_Basic_2Engine_sys_mjAdfrom> _sucDelegate, Action<int, string> _failDelegate)
        {
            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("sys", "mjAdfrom", "", new MJSDK_CallbackDealer_Model<MJSDK_Basic_2Engine_sys_mjAdfrom>(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// sys-appReview：应用内评价
        /// </summary>
        /// <param name="_resDelegate"></param>
        public static void sys_appReview(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("sys", "appReview", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// sys-openUrl：打开链接
        /// </summary>
        /// <param name="_resDelegate"></param>
        public static void sys_openUrl(string _url, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //必要参数不得为空
            if (string.IsNullOrEmpty(_url))
            {
                _failDelegate(MJSDK_Error.C_Unity_Param_Miss, "sys_openUrl --> Parameters are missing,_url is null");
                return;
            }

            Hashtable ht = new Hashtable();
            ht.Add("url", _url);
            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("sys", "openUrl", ht.toJson(), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }

        /// <summary>
        /// sys_appQuit：应用退出
        /// </summary>
        /// <param name="_resDelegate"></param>
        public static void sys_appQuit(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            MJSDK.sendMsgToPhonePlatform("sys", "appQuit", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// sys-share：系统分享
        /// </summary>
        /// <param name="_resDelegate"></param>
        public static void sys_share(MJSDK_Basic_2Engine_sys_share _shareInfo, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            if (_shareInfo == null)
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_Error.C_Unity_Param_Miss, "sys_share --> Parameters are missing,_shareInfo is null");
                }
                return;
            }
            MJSDK.sendMsgToPhonePlatform("sys", "share", JsonUtility.ToJson(_shareInfo), new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// sys-openSettings：打开设备设置界面
        /// </summary>
        /// <param name="_resDelegate"></param>
        public static void sys_openSettings(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            MJSDK.sendMsgToPhonePlatform("sys", "openSettings", "", new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }
    }
}
