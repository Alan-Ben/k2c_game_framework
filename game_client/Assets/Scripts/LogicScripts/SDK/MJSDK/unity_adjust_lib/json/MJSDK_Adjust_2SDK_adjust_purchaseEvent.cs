using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(adjust-purchaseEvent：购买事件上报)消息结构体
    /// </summary>
    public class MJSDK_Adjust_2SDK_adjust_purchaseEvent : MJSDK_2SDK_ThirdParty_purchaseEvent_base
    {
        //自定义事件key
        public string event_key;
        //回传标识符
        public string callbackId;
    }
}