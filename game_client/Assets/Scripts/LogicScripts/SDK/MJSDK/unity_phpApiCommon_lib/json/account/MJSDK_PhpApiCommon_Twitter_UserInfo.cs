using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// Twitter用户信息 消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_Twitter_UserInfo 
    {
        //twitter授权后 登录令牌
        public string auth_token;
        //twitter授权后 返回令牌秘钥
        public string token_secret;
        //twitter授权后 user_id
        public string user_id;
    }
}