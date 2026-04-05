using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(webView-show：网页展示）消息结构体
    /// </summary>
    public class MJSDK_WebView_2SDK_webView_show : MJSDK_2SDK_Base
    {
        //标题
        public string title;
        //网页地址
        public string url;
        //网页地址是否需要编码，URL只能使用英文字母、阿拉伯数字和某些标点符号，不能使用其他文字和符号（如中文）。注意：webView-browserShow不需要此字段
        public bool isEncoding = false;
        //实付金额0：竖屏 1：横屏  默认：竖屏
        public int orientation = 0;
        //是否需要导航  默认：是。注意：webView-browserShow不需要此字段
        public bool isNavBar = true;
    }
}