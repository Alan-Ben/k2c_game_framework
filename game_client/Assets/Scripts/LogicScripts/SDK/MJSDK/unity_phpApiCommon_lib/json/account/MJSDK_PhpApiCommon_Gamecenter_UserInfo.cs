using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// GameCenter用户信息 消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_Gamecenter_UserInfo 
    {
        public string publicKeyUrl;//公共key（gamecenter返回参数）
        public string playerID;//玩家ID（gamecenter返回参数
        public string signature;//
        public string gc_timestamp;//时间戳（gamecenter返回参数）
        public string salt;//1加盐（gamecenter返回参数）
    }
}
