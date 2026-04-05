using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(trace - gameAd：游戏广告变现数据上报)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2SDK_trace_gameAd : MJSDK_2SDK_Base
    {
        //平台自定义ID
        public string platform_id;
        //区域ID
        public string area_id;
        //服务器ID
        public string server_id;
        //事件id
        public string event_id;
        //玩家uid
        public string uid;
        //玩家cid
        public string cid;
        //事件发生时玩家等级
        public string level;
        //事件发生时玩家VIP等级
        public string vip_level;
        //变现广告来源渠道(加载成功才会有该值)，如取不到的时候可以留空
        public string source;
        //广告时间发生位置
        public string ad_type;
        //创角时间戳, 秒级10位
        public string role_ct;
        //注册时客户端的版本
        public string ar_version;
        //注册时的包名
        public string ar_adfrom2;
        //国家
        public string ar_nation;
        //当时登录客户端版本号
        public string login_tag;
    }
}
