using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(第三方平台-customEvent：自定义事件上报)消息结构体
    /// </summary>
    [Serializable]
    public class MJSDK_2SDK_ThirdParty_customEvent_base : MJSDK_2SDK_Base
    {
        //自定义事件key
        public string event_key;
        //触发事件时的玩家等级
        public string level;
        //扩展参数json(string)
        public string ext;

        /// <summary>
        /// 参数校验    
        /// </summary>
        public static bool checkParam(MJSDK_2SDK_ThirdParty_customEvent_base param)
        {
            if (param == null
                || string.IsNullOrEmpty(param.event_key))
            {
                return false;
            }
            return true;
        }
    }
}


