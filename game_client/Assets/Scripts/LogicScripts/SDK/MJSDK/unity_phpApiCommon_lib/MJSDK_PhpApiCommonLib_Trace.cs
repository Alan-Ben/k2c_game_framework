using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// 服务端公共接口支持组件的相关接口类  --- SDK事件跟踪
    /// </summary>
    public class MJSDK_PhpApiCommonLib_Trace
    {
        #region game trace（游戏统计）
        /// <summary>
        /// 游戏事件跟踪-激活上报 （无需初始化UnityMJSDK）
        /// </summary>
        /// <param name="_active_report_info"></param>
        /// <param name="_sucDelegate">请注意：未初始化UnityMJSDK-是无法收到平台成功回执</param>
        /// <param name="_failDelegate">请注意：未初始化UnityMJSDK-是无法收到平台失败回执</param>
        public static void trace_gameActivate(MJSDK_PhpApiCommon_2SDK_trace_gameActivty _active_report_info, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //必要参数不得为空
            if (_active_report_info == null)
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "trace_gameActivate --> Parameters are missing,object is null");
                }
                return;
            }

            //参数
            string paramStr = JsonUtility.ToJson(_active_report_info);

            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("trace", "gameActivate", paramStr
                , new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// 游戏事件跟踪-节点上报 （无需初始化UnityMJSDK）
        /// </summary>
        /// <param name="_step_report_info"></param>
        /// <param name="_sucDelegate">请注意：未初始化UnityMJSDK-是无法收到平台成功回执</param>
        /// <param name="_failDelegate">请注意：未初始化UnityMJSDK-是无法收到平台失败回执</param>
        public static void trace_gameStep(MJSDK_PhpApiCommon_2SDK_trace_gameStep _step_report_info, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //必要参数不得为空
            if (_step_report_info == null
                || string.IsNullOrEmpty(_step_report_info.platform_id)
                || string.IsNullOrEmpty(_step_report_info.area_id))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_JsonErr, "trace_gameStep --> Parameters are missing,platform_id/area_id is null");
                }
                return;
            }

            if (_step_report_info.step_info == null
                || string.IsNullOrEmpty(_step_report_info.step_info.uid)
                || string.IsNullOrEmpty(_step_report_info.step_info.cid)
                || _step_report_info.step_info.step_id <= 0
                || string.IsNullOrEmpty(_step_report_info.step_info.op_time))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_JsonErr, "trace_gameStep --> Parameters are missing,step_info is null or uid/cid/step_id/op_time is null");
                }
                return;
            }

            //support_gpu_instancing 只能填写0或者1
            if (_step_report_info.support_gpu_instancing == "0") { }
            else if (_step_report_info.support_gpu_instancing == "1") { }
            else
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_JsonErr, "trace_gameStep --> Parameters are error,support_gpu_instancing only = 0/1");
                }
                return;
            }
            //参数
            string paramStr = JsonUtility.ToJson(_step_report_info);
            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("trace", "gameStep", paramStr
                , new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// 游戏事件跟踪-节点上报 （无需初始化UnityMJSDK）
        /// </summary>
        /// <param name="_step_report_info"></param>
        /// <param name="_sucDelegate">请注意：未初始化UnityMJSDK-是无法收到平台成功回执</param>
        /// <param name="_failDelegate">请注意：未初始化UnityMJSDK-是无法收到平台失败回执</param>
        public static void trace_gameErr(MJSDK_PhpApiCommon_2SDK_trace_gameErr _err_report_info, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //必要参数判断
            if (_err_report_info == null
                || string.IsNullOrEmpty(_err_report_info.err_level)
                || string.IsNullOrEmpty(_err_report_info.step)
                || string.IsNullOrEmpty(_err_report_info.content)
                || string.IsNullOrEmpty(_err_report_info.login_tag))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "trace_gameErr --> Parameters are missing,content/err_level/step/login_tag is null");
                }
                return;
            }

            //支付类型判断
            E_Err_level err_Level = E_Err_level.none;
            if (!Enum.TryParse<E_Err_level>(_err_report_info.err_level, out err_Level))
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "trace_gameErr --> err_Level error,err_levle exp:debug、warn、info、error");
                }
                return;
            }
            //参数
            string paramStr = JsonUtility.ToJson(_err_report_info);
            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("trace", "gameErr", paramStr
                , new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }



        /// <summary>
        /// trace - gameAd：游戏广告变现数据上报 （无需初始化UnityMJSDK）
        /// </summary>
        /// <param name="_trace_gameAd"></param>
        /// <param name="_sucDelegate">请注意：未初始化UnityMJSDK-是无法收到平台成功回执</param>
        /// <param name="_failDelegate">请注意：未初始化UnityMJSDK-是无法收到平台失败回执</param>
        public static void trace_gameAd(MJSDK_PhpApiCommon_2SDK_trace_gameAd _trace_gameAd, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //必要参数判断
            if (_trace_gameAd == null)
            {
                if (_failDelegate != null)
                {
                    _failDelegate(MJSDK_PhpApiCommonError.C_Unity_Param_Miss, "trace_gameAd --> Parameters are missing");
                }
                return;
            }
            //参数
            string paramStr = JsonUtility.ToJson(_trace_gameAd);
            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("trace", "gameAd", paramStr
                , new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }


        /// <summary>
        /// trace - reReportEnable：游戏埋点失败重报开关
        /// </summary>
        /// <param name="_trace_gameAd"></param>
        /// <param name="_sucDelegate">请注意：未初始化UnityMJSDK-是无法收到平台成功回执</param>
        /// <param name="_failDelegate">请注意：未初始化UnityMJSDK-是无法收到平台失败回执</param>
        public static void trace_reReportEnable(bool _isEnable, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            Hashtable ht = new Hashtable();
            ht.Add("isEnable", _isEnable);
            //发送消息处理，结果处理函数直接带入
            MJSDK.sendMsgToPhonePlatform("trace", "reReportEnable", ht.toJson()
                , new MJSDK_CallbackDealer_Action(_sucDelegate, _failDelegate));
        }

        #endregion
    }
}
