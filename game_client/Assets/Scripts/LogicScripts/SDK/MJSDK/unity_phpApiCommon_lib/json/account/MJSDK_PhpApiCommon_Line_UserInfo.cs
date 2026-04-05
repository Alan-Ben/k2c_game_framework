using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// line用户信息 消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_Line_UserInfo 
    {
        //line授权后用code换取的access_token
        public string access_token;
        //line授权后用code换取的user_id
        public string user_id;
    }
}