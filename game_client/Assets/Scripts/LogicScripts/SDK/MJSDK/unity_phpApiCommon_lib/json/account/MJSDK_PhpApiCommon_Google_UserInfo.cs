using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// google用户信息消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_Google_UserInfo 
    {
        //访问令牌（google返回参数）
        public string accessToken;
        //用户ID（google返回参数）
        public string userID;
    }
}