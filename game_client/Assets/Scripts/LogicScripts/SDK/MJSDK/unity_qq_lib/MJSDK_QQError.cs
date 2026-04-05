using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// qq 组件错误码
    /// </summary>
    public class MJSDK_QQError : MJSDK_Error
    {
        public const int C_Unity_QQ_Configuration_Null = 40501;//未配置应用信息（QQ后台应用编号、秘钥）
        public const int C_Unity_QQ_No_Init = 40502;//SDK未初始化
        public const int C_Unity_QQ_device_no_install_App = 40503;//设备未安装QQ应用
        public const int C_Unity_QQ_get_userinfo_Fail = 40504;//获取QQ用户信息失败
    }
}