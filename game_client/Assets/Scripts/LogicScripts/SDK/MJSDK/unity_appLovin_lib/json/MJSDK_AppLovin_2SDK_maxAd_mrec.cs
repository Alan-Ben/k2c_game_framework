using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(maxAd-mrec：mrec广告)消息结构体
    /// </summary>
    public class MJSDK_AppLovin_2SDK_maxAd_mrec : MJSDK_2SDK_Base
    {
        //mrec广告ID（找运维获取）
        public string adUnitId;
        //当前广告源加载失败情况下可重新尝试加载次数（默认为0）
        public int retryNum;
        //横幅距x轴偏移量（默认为居中）
        public float offx;
        //横幅距y轴偏移量（默认为居中）
        public float offy;
        //横幅宽度（默认为全屏宽度）
        public float width;
        //横幅高度（iphone默认50，ipad默认为90）
        public float height;
    }
}