using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// SDK->Engine(net-info：设备网络信息)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2Engine_net_info : MJSDK_2Engine_Base
    {
        //网络类型（Wifi，3G、4G‘5G）
        public string type;
    }
}
