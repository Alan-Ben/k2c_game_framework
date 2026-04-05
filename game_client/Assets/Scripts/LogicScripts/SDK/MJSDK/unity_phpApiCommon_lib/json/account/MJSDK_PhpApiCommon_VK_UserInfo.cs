using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// vk用户信息 消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_VK_UserInfo
    {
        //vk授权后用code换取的access_token
        public string access_token;
        //vk授权后用code换取的user_id
        public string user_id;
        //vk授权后用code换取的id_token
        public string id_token;
    }
}