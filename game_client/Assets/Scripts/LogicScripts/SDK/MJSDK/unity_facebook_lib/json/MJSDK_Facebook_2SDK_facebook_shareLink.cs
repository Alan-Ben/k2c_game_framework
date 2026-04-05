using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(facebook-shareLink：链接分享)消息结构体 
    /// </summary>
    public class MJSDK_Facebook_2SDK_facebook_shareLink : MJSDK_2SDK_Base
    {
        //链接地址
        public string link;
        //链接说明
        public string link_show;
    }
}
