using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// SDK->Engine(user-accountBind：账号绑定)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2Engine_user_accountBind 
    {
        /*用户绑定的帐号类型与昵称格式:*/
        public List<MJSDK_Login_2Engine_account_bind_info> bind;
    }

    /// <summary>
    /// 用户绑定信息
    /// </summary>
    [Serializable]
    public class MJSDK_Login_2Engine_account_bind_info
    {
        //第三方账号类型  类型查看：https://thoughts.teambition.com/share/647e8f5e90a37b003eb87cf4#title=第三方账号类型
        public string type;
        //第三方账号值（一般是用户名）
        public string value;
    }
}