using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(noti-local：本地推送）消息结构体
    /// </summary>
    public class MJSDK_MobPush_2SDK_noti_local : MJSDK_2SDK_Base
    {
        //标题
        public string title;
        //子标题
        public string subTitle;
        //内容
        public string body;
        //用户自定义参数(jsonStr)
        public string userInfo;
        //延迟推送（单位为秒），不填写默认为0就是即时推送
        public long timeInterval;
    }
}