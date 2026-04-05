using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// QQ用户信息 消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_QQ_UserInfo
    {
        //QQ授权后用code换取的access_token
        public string access_token;
        //QQ授权后用code换取的open_id
        public string open_id;
    }
}