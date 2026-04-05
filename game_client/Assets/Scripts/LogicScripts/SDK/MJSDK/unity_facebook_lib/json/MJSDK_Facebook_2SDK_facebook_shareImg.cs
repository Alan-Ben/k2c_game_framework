using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(facebook-shareImg：图片分享)消息结构体
    /// </summary>
    public class MJSDK_Facebook_2SDK_facebook_shareImg : MJSDK_2SDK_Base
    {
        //图片地址（支持文件、base64文件地址）
        public string img_path;
    }
}
