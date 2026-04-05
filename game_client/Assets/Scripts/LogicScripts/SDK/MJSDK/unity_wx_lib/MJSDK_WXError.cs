using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// 微信 组件错误码
    /// </summary>
    public class MJSDK_WXError : MJSDK_Error
    {
        public const int C_Unity_WX_Configuration_Null = 40601;//未配置应用信息（wx后台应用编号、秘钥）
        public const int C_Unity_WX_No_Init = 40602;//SDK未初始化
        public const int C_Unity_WX_device_no_install_vkApp = 40603;//设备未安装wx应用
        public const int C_Unity_WX_get_userinfo_Fail = 40604;//获取wx用户信息失败
    }
}