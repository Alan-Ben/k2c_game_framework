using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// SDK->Engine(topon-rewarded：激励广告加载)消息结构体  
    /// </summary>
    public class MJSDK_Topon_2Engine_topon_rewarded : MJSDK_2Engine_Base
    {
        //广告播放状态码
        public string status;
        //广告播放状态码说明
        public string statusName;
        //登录帐号的TOKEN，
        public string adunit_format;
        //帐号类型
        public string network_type;
        //广告类型，
        public string network_placement_id;
        //广告收入
        public string publisher_revenue;
        //广告源ID，
        public string currency;
        //帐号类型
        public string adsource_id;
        //TopOn广告位ID，
        public string adunit_id;
    }
}