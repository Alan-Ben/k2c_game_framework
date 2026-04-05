using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// TrackingIO 组件错误码
    /// </summary>
    public class MJSDK_TrackingIOError : MJSDK_Error
    {
        // 配置信息为空（未配置应用信息（TrackingIO后台应用编号、秘钥））
        public const int C_Unity_TrackingIO_Error_ConfigInfo_Null = 60401;
        //TrackingIO 初始化失败
        public const int C_Unity_TrackingIO_Error_no_init = 60402;
        //TrackingIO 购买事件上报失败
        public const int C_Unity_TrackingIO_Error_PurchaseEvent_Report_Fail = 60403;
        //TrackingIO 自定义事件上报失败
        public const int C_Unity_TrackingIO_Error_CustomEvent_Report_Fail = 60404;
    }
}