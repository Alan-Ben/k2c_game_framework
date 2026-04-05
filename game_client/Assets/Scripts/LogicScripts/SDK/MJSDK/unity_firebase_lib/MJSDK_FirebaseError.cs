using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// firbase 组件错误码
    /// </summary>
    public class MJSDK_FirebaseError : MJSDK_Error
    {
        // 配置信息为空（GoogleService-Info.plist 未添加进入）
        public const int C_Unity_Firebase_Error_ConfigInfo_Null = 60201;
        //firebase 未初始化
        public const int C_Unity_Firebase_Error_no_init = 60202;
        //firebase 购买事件上报失败
        public const int C_Unity_Firebase_Error_PurchaseEvent_Report_Fail = 60203;
        //firebase 自定义事件上报失败
        public const int C_Unity_Firebase_Error_CustomEvent_Report_Fail = 60204;
    }
}
