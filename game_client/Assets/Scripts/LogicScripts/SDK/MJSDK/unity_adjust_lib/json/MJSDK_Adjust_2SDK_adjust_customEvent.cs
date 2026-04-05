using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(Adjust-customEvent：自定义事件上报)消息结构体
    /// </summary>
    public class MJSDK_Adjust_2SDK_adjust_customEvent : MJSDK_2SDK_ThirdParty_customEvent_base
    {
        //回传标识符
        public string callbackId;
    }
}