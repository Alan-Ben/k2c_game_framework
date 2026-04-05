using UnityEngine;
using System.Collections;

namespace MJSDK_Package
{
    /// <summary>
    /// 发送消息到SDK后，SDK处理完成返回的回调消息内容
    /// 格式如下
    /// code: 状态码，0表示成功，其他表示错误码
    /// callbackId: 回调处理对象的注册Id
    /// content: 对应状态的内容，成功则为回调内容，失败则为错误信息
    /// </summary>
    public class MJSDK_CallbackMsg
    {
        //状态码，0为成功，其他为失败
        public int code;
        //回调处理的序列号
        public long callbackId;
        //对应的内容，成功则为处理结果内容，失败则为失败信息
        public string content;
    }
}
