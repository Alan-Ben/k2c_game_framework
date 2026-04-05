using UnityEngine;
using System.Collections;

namespace MJSDK_Package
{
    /// <summary>
    /// 发送消息到SDK的格式
    /// 格式如下
    /// mainOrder: 消息主索引
    /// subOrder: 消息子索引
    /// callbackId: 消息回调Id
    /// args: 消息具体参数
    /// </summary>
    public class MJSDK_SendSDKMsg
    {
        //消息主索引
        public string mainOrder;
        //消息子索引
        public string subOrder;
        //消息回调Id，0表示无效
        public long callbackId;
        //消息具体参数
        public string args;
    }
}
