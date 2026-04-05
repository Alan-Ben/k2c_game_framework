using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(aihelp-setUserInfo：设置玩家信息)消息结构体 
    /// </summary>
    public class MJSDK_AIHelp_2SDK_aihelp_setUserInfo : MJSDK_2SDK_Base
    {
        //用户名
        public string user_name;
        //用户ID
        public string user_id;
        //服务器
        public string server_id;
        //用户标签。。 多个标签之间以「,」分隔，默认为空字符串
        public string userTags;
        //自定义数据（jsonString） 格式：{“key”:“value”, “key”:“value”} ，默认为空字符串
        public string customData;
    }
}
