using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// SDK->Engine(maxsdk-广告变现)消息结构体
    /// </summary>
    public class MJSDK_AppLovin_2Engine_maxAd : MJSDK_2Engine_Base
    {
        //0: 广告资源加载成功 -- 准备播放 1：播放中2：用户点击3：播放成功
        public int status;
        //当前播放ad广告源
        public string networkName;
        public string msg;

        //广告的收入数额，如果不存在收入数额，则为0。
        public double revenue;
    }
}