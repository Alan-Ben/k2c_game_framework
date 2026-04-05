using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// MJ账号中心用户信息 消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_MJAcc_UserInfo
    {
        //MJ授权mj_token
        public string mj_token;
        //MJ授权mj_token_expiry
        public string mj_token_expiry;
        //MJ授权mj_token_sign
        public string mj_token_sign;
        //梦加账号id
        public string user_id;
        //类型
        public string operate_type;
        //邮箱信息
        public string email;
    }
}