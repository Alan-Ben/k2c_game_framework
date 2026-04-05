using MJSDK_Package;

namespace GOESDK
{
    public class MJSDKDealLogic : _IMJSDK_DealLogicInterface
    {        
        /// <summary>
        /// 在SDK主动触发错误的时候，调用的事件函数，带入错误码及错误信息
        /// </summary>
        /// <param name="_errCode"></param>
        /// <param name="_errMsg"></param>
        public void onMJSDKError(int _errCode, string _errMsg)
        {
            SDKUtil.showSDKLogError("onMJSDKError", _errCode, _errMsg);
        }

        /// <summary>
        /// 在SDK主动触发消息通知,调用的事件函数,会带入主协议-子协议以及信息（SDK主动通知Unity）
        /// </summary>
        /// <param name="_msg"></param>
        public void onMJSDKMsg(MJSDK_SDKMsg _msg)
        {
            if (_msg == null)
                return;

            SDKUtil.showSDKDebugLog($"[onMJSDKMsg] mainOrder:{_msg.mainOrder},subOrder:{_msg.subOrder},msg:{_msg.msg}");
        }
    }
}
