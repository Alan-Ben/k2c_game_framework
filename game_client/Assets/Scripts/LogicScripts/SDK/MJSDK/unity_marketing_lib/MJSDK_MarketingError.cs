using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// Marketing 组件错误码
    /// </summary>
    public class MJSDK_MarketingError : MJSDK_Error
    {
        // 配置信息为空
        public const int C_None_Config_Param = 60601;
        //Marketing 初始化失败
        public const int C_SDK_Init_Fail = 60602;
        //Marketing 购买事件上报失败
        public const int C_Purchase_Event_Fail = 60603;
        //Marketing 自定义事件上报失败
        public const int C_Custom_Event_Fail = 60604;
    }
}

