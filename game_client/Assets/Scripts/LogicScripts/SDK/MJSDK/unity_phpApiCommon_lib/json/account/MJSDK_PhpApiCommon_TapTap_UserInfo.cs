using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    ///TapTap用户信息消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_TapTap_UserInfo 
    {
        //taptap返回的kid参数
        public string kid;
        //taptap返回的mac_key参数
        public string mac_key;
        //taptap返回唯一标识（unionid）
        public string user_id;
    }
}