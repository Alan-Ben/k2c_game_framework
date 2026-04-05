using UnityEngine;
using System.Collections;
using System;


namespace MJSDK_Package
{
    /// <summary>
    /// MJSDK的主脚本对象，放在任意顶层对象上即可。请注意不可删除本对象，作为SDK通讯的主体脚本
    /// 此对象在对应的SDK初始化的时候自动创建并处理
    /// </summary>
    public class MJSDK_MainMono : MonoBehaviour
    {
        #region 各平台回调函数处理
        /// <summary>
        /// 发送的消息统一处理之后的回调结果调用函数
        /// 函数的_msg包含格式如下
        /// code: 状态码，0表示成功，其他表示错误码
        /// callbackId: 回调处理对象的注册Id
        /// content: 对应状态的内容，成功则为回调内容，失败则为错误信息
        /// </summary>
        /// <param name="_msg"></param>
        public void ReceivePlatformReceipt(string _msg)
        {
            dealSDKDealMsg(_msg);
            MJSDK_Log.mjsdkLog(_msg, E_MJSDK_BusType.ReceiveMsg);
        }

        /// <summary>
        /// 处理 -- MJSDK处理协议结果回执信息
        /// </summary>
        /// <param name="_msg"></param>
        public static void dealSDKDealMsg(string _msg)
        {
            //开始对返回数据进行解析
            MJSDK_CallbackMsg callbackMsg = null;
            try
            {
                callbackMsg = JsonUtility.FromJson<MJSDK_CallbackMsg>(_msg);
            }
            catch (Exception _e)
            {
                MJSDK_Log.mjsdkLog($"Read SDK callback json error! errorMessage[{_e.Message}] json[{_msg}]", E_MJSDK_BusType.Error_DealSDKMsgErr);
                return;
            }

            if (null == callbackMsg)
            {
                MJSDK_Log.mjsdkLog($"Read SDK callback json error! json[{_msg}]", E_MJSDK_BusType.Error_DealSDKMsgErr);
                return;
            }

            //获取对应回调
            _IMJSDK_CallbackDealer callbackDealer = MJSDK_CallbackMgr.instance.popCallback(callbackMsg.callbackId);
            if (null == callbackDealer)
            {
                //无法获取回调则报错并返回
                MJSDK_Log.mjsdkLog($"Can not find callback[{callbackMsg.callbackId}]", E_MJSDK_BusType.Error_DealSDKMsgErr);
                return;
            }

            //进行回调处理
            if (callbackMsg.code == 0)
            {
                //成功处理，如果没有接口对象则直接调用
                callbackDealer.onSDKDealDone(callbackMsg.content);
            }
            else
            {
                //失败处理，如果没有接口对象则直接调用
                callbackDealer.onSDKDealFail(callbackMsg.code, callbackMsg.content);
            }
        }

        /// <summary>
        /// 在SDK部分主动触发一些错误的时候将会调用本错误触发函数
        /// 带入的_errMsg格式如下
        /// errCode: 错误码
        /// errMsg: 错误的信息
        /// </summary>
        /// <param name="_errMsg"></param>
        public void onMJPhoneError(string _errMsg)
        {
            dealMJSDKError(_errMsg);
            MJSDK_Log.mjsdkLog(_errMsg, E_MJSDK_BusType.Error_GetSDKErr);
        }


        /// <summary>
        /// 处理MJSDK通知的异常信息
        /// </summary>
        /// <param name="_errMsg"></param>
        public static void dealMJSDKError(string _errMsg)
        {
            //开始对返回数据进行解析
            MJSDK_SDKErr errorMsg = null;
            try
            {
                errorMsg = JsonUtility.FromJson<MJSDK_SDKErr>(_errMsg);
            }
            catch (Exception _e)
            {
                MJSDK_Log.mjsdkLog($"Read SDK Error json error! errorMessage[{_e.Message}] json[{_errMsg}]", E_MJSDK_BusType.Error_DealSDKMsgErr);
                return;
            }

            if (MJSDK.isInit)
                MJSDK.logicInterface.onMJSDKError(errorMsg.errCode, errorMsg.errMsg);
            else
                MJSDK_Log.mjsdkLog($"Call SDK func onMJPhoneError when MJSDK is not inited!", E_MJSDK_BusType.Error_DealSDKMsgErr);
        }

        /// <summary>
        /// 在收到由SDK主动触发的消息时的处理函数
        /// 一般此操作提供给各SDK组件进行交互的时候使用的
        /// 此函数触发一般在SDK机制主动触发的时候向引擎发送消息
        /// 消息_msg统一格式如下：
        /// mainOrder: 主消息索引
        /// subOrder: 子消息索引
        /// msg: 消息传递内容
        /// </summary>
        /// <param name="_msg"></param>
        public void onRecMJPhoneMsg(string _msg)
        {
            dealMJSDKMsg(_msg);
            MJSDK_Log.mjsdkLog(_msg, E_MJSDK_BusType.GetSDKApi);
        }


        /// <summary>
        /// 处理MJSDK发起的消息
        /// </summary>
        /// <param name="_msg"></param>
        public static void dealMJSDKMsg(string _msg)
        {
            //解析消息格式
            MJSDK_SDKMsg msg = null;
            try
            {
                msg = JsonUtility.FromJson<MJSDK_SDKMsg>(_msg);
            }
            catch (Exception _e)
            {
                MJSDK_Log.mjsdkLog($"Read SDK Msg json error! errorMessage[{_e.Message}] json[{_msg}]", E_MJSDK_BusType.Error_DealSDKMsgErr);
                return;
            }

            //处理消息，并在失败的时候调用事件函数
            if (!MJSDK_MsgDispather.instance.dealMsg(msg.mainOrder, msg.subOrder, _msg))
            {
                if (MJSDK.isInit)
                    MJSDK.logicInterface.onMJSDKMsg(msg);
                else
                    MJSDK_Log.mjsdkLog($"Call SDK func onRecMJPhoneMsg when MJSDK is not inited!", E_MJSDK_BusType.Error_DealSDKMsgErr);
            }
        }
#endregion
    }
}
