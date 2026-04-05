using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(user-tokenVerify：用户登录令牌校验)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2SDK_user_tokenVerfity : MJSDK_2SDK_Base
    {
        //用户登录令牌
        public string token;
        //用户id
        public string user_id;
    }
}