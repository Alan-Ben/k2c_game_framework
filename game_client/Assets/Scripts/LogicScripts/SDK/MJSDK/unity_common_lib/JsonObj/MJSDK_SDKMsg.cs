using UnityEngine;
using System.Collections;

namespace MJSDK_Package
{
    /// <summary>
    /// SDK发送到引擎的消息格式
    /// 格式如下
    /// mainOrder: 消息主索引
    /// subOrder: 消息子索引
    /// msg: 消息具体参数
    /// </summary>
    public class MJSDK_SDKMsg
    {
        //消息主索引
        public string mainOrder;
        //消息子索引
        public string subOrder;
        //消息具体参数
        public string msg;
    }
}
