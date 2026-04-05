using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// adjust 组件错误码
    /// </summary>
    public class MJSDK_AdjustError : MJSDK_Error
    {
        // 配置信息为空
        public const int C_Unity_Adjust_Error_ConfigInfo_Null = 60501;
        //adjust 初始化失败
        public const int C_Unity_Adjust_Error_no_init = 60502;
        //adjust 购买事件上报失败
        public const int C_Unity_Adjust_Error_PurchaseEvent_Report_Fail = 60503;
        //adjust 自定义事件上报失败
        public const int C_Unity_Adjust_Error_CustomEvent_Report_Fail = 60504;
    }
}