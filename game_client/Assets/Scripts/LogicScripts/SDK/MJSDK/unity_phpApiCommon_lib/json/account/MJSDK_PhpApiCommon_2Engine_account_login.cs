using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// SDK->Engine(账号登录回调)消息结构体  
    /// </summary>
    public class MJSDK_PhpApiCommon_2Engine_account_login : MJSDK_2Engine_Base
    {
        //登录帐号的TOKEN，
        public string account_token;
        //帐号类型
        public string account_type;
    }
}

