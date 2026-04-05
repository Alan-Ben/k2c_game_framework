using UnityEngine;
using System;
using System.Collections;

namespace MJSDK_Package
{
    /// <summary>
    /// MJSDK处理一些逻辑调用的接口对象
    /// 需要根据不同项目继承并带入初始化
    /// 
    /// 主要目标是根据不同项目的处理框架，可以对回调的处理进行不同的触发
    /// 特别关注的是可能在非主线程情况下调用回调或消息函数的时候是否有可容错的处理方式
    /// 特别备注：ALPackage框架下，请使用MonoTask进行处理
    /// </summary>
    public interface _IMJSDK_DealLogicInterface
    {
        /// <summary>
        /// 在SDK主动触发错误的时候，调用的事件函数，带入错误码及错误信息
        /// </summary>
        /// <param name="_errCode"></param>
        /// <param name="_errMsg"></param>
        void onMJSDKError(int _errCode, string _errMsg);

        /// <summary>
        /// 在SDK主动触发消息通知,调用的事件函数,会带入主协议-子协议以及信息（SDK主动通知Unity）
        /// </summary>
        /// <param name="_msg"></param>
        void onMJSDKMsg(MJSDK_SDKMsg _msg);
    }
}
