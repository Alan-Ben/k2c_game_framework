using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    ///Facebook用户信息消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_Facebook_UserInfo 
    {
        //访问令牌（facebook返回参数）
        public string accessToken;
        //用户ID（facebook返回参数）
        public string userID;
        //如果受限登录 传个有效值 1即可；如果非受限登录，不传或者传个无效值 0即可
        public string limitValidType;
        //facebook返回参数，受限登录
        public string authenticationToken;
    }
}