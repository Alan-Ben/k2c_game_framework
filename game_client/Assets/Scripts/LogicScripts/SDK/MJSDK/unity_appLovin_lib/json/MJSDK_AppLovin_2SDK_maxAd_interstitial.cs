using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(maxAd-interstitial：插入式广告)消息结构体
    /// </summary>
    public class MJSDK_AppLovin_2SDK_maxAd_interstitial : MJSDK_2SDK_Base
    {
        //插入式广告ID（找运维获取）
        public string adUnitId;
        //当前广告源加载失败情况下可重新尝试加载次数（默认为0）
        public int retryNum;
    }
}