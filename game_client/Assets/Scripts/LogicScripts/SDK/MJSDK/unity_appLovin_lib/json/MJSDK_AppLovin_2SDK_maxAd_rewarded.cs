using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(maxAd-rewarded：激励广告)消息结构体
    /// </summary>
    public class MJSDK_AppLovin_2SDK_maxAd_rewarded : MJSDK_2SDK_Base
    {
        //激励广告ID（找运维获取）
        public string adUnitId;
        //当前广告源加载失败情况下可重新尝试加载次数（默认为0）
        public int retryNum;
        //是否为预加载。默认为false。因为maxsdk广告在加载广告时间过长，建议在空闲时候启动预加载（启动一次即可，后续SDK会在每次播放成功/失败后自动开启加载功能）。
        public bool preLoad = false;
    }
}