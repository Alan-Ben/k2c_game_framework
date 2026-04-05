using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(user-accountBind：用户绑定  第三方账号)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2SDK_user_accountBind : MJSDK_2SDK_Base
    {
        //账号登录令牌
        public string account_token;
        //用户登录令牌
        public string token;
    }
}
