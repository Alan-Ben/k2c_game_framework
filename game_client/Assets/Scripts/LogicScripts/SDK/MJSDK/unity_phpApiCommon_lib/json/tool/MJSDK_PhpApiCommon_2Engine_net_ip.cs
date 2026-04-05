using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// SDK->Engine(net-ip：获取ip)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2Engine_net_ip : MJSDK_2Engine_Base
    {
        //当前请求的ip
        public string ip;
        //当前请求的所在国家简写
        public string city;
        //当前请求的所在大陆代号
        public string region;
    }
}