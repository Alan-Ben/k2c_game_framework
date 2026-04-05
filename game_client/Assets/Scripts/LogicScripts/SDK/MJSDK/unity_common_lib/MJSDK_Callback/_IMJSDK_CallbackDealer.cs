using UnityEngine;
using System.Collections;

namespace MJSDK_Package
{
    /// <summary>
    /// MJSDK消息发送后的回调处理对象
    /// SDK将根据实际的回调索引，将对应的回调信息发送到本处理对象中
    /// 可以在回调对象中对实际的处理结果进行关联处理
    /// </summary>
    public interface _IMJSDK_CallbackDealer
    {
        /// <summary>
        /// 当消息处理失败的时候，调用的本失败处理
        /// </summary>
        /// <param name="_errCode"></param>
        /// <param name="_content"></param>
        void onSDKDealFail(int _errCode, string _content);

        /// <summary>
        /// 在SDK消息处理成功之后的结果反馈
        /// </summary>
        /// <param name="_content"></param>
        void onSDKDealDone(string _content);
    }
}
