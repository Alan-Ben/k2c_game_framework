using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(user-autoLogin：用户自动登录)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2SDK_user_autoLogin : MJSDK_2SDK_Base
    {
        //用户登录令牌
        public string token;
        //广告afid
        public string afid;
        //登录扩传参数，仅进行记录
        public string login_params;
        //国家代码
        public string nation;
    }
}
