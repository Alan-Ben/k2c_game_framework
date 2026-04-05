using UnityEngine;
using System.Collections;

namespace MJSDK_Package
{
    /// <summary>
    /// 协议的具体处理对象基类
    /// </summary>
    public abstract class _AMJSDK_MsgSubDealer
    {
        /// <summary>
        /// 实际处理消息的处理函数
        /// </summary>
        /// <param name="_msg"></param>
        public abstract void dealMsg(string _msg);
    }
}
