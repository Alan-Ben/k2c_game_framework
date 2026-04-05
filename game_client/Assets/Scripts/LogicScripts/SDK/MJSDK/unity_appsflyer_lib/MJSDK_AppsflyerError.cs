using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// appsflyer 组件错误码
    /// </summary>
    public class MJSDK_AppsflyerError : MJSDK_Error
    {
        // 配置信息为空（appsflyer_dev_Key/appsflyer_app_id 为空）
        public const int C_Unity_Appsflyer_Error_ConfigInfo_Null = 60301;
        //appsflyer 初始化失败
        public const int C_Unity_Appsflyer_Error_no_init = 60302;
        //appsflyer 购买事件上报失败
        public const int C_Unity_Appsflyer_Error_PurchaseEvent_Report_Fail = 60303;
        //appsflyer 自定义事件上报失败
        public const int C_Unity_Appsflyer_Error_CustomEvent_Report_Fail = 60304;
    }
}

