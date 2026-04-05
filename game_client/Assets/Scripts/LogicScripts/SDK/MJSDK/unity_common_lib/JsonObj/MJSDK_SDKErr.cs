using UnityEngine;
using System.Collections;

namespace MJSDK_Package
{
    /// <summary>
    /// SDK主动触发的错误信息
    /// 格式如下
    /// errCode: 错误码
    /// errMsg: 错误的信息
    /// </summary>
    public class MJSDK_SDKErr
    {
        //错误码
        public int errCode;
        //错误信息内容
        public string errMsg;
    }
}
