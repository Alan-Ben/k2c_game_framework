using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(vk-share：vk分享）消息结构体
    /// </summary>
    public class MJSDK_VK_2SDK_vk_share : MJSDK_2SDK_Base
    {
        //文本分享
        public string text;
        //链接分享
        public string link_url;
        //链接说明
        public string link_show;
        //图片分享（图片地址（支持文件、base64文件地址））
        public string img_path;
    }
}
