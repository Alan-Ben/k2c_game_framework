using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(topon-rewarded：激励广告加载)消息结构体  
    /// </summary>
    public class MJSDK_Topon_2SDK_topon_rewarded
    {
        //广告源Id
        public string placementId;
        //是否为预加载（0：否 1：是）
        public int preLoad = 0;
    }
}