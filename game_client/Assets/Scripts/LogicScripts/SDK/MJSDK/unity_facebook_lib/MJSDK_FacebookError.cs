using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// facebook 组件错误码
    /// </summary>
    public class MJSDK_FacebookError : MJSDK_Error
    {
        //未配置应用信息（Facebook后台应用编号、秘钥
        public const int C_Unity_Facebook_Error_config_info_null = 60101;
        //facebookSDK初始化失败
        public const int C_Unity_Facebook_Error_init_Fail = 60102;
        // 设备未安装Facebook应用
        public const int C_Unity_Facebook_Error_Device_Facebook_UnInstall = 60103;
        //acebookSDK 获取facebook用户信息失败
        public const int C_Unity_Facebook_Error_Get_UserInfo_Fail = 60104;
        //Facebook 购买事件上报失败
        public const int C_Unity_Facebook_Error_PurchaseEvent_Report_Fail = 60105;
        //Facebook 自定义事件上报失败
        public const int C_Unity_Facebook_Error_CustomEvent_Report_Fail = 60106;
        //facebookSDK 分享失败
        public const int C_Unity_Facebook_Error_Share_Fail = 60107;

    }
}
