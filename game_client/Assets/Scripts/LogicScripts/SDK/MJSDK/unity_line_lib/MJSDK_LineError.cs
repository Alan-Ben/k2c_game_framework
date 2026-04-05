using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// line组件错误声明区域
    /// </summary>
    public class MJSDK_LineError:MJSDK_Error
    {
        //参数为空（channel_id）
        public const int C_Unity_line_Error_config_info_null = 40201;
        //SDK未初始化
        public const int C_Unity_line_Error_No_Init = 40202;
        //获取line用户信息失败
        public const int C_Unity_line_Error_Get_UserInfo_Fail = 40203;
        // line用户登出失败
        public const int C_Unity_line_Error_Logout_Fail = 40204;
    }
}
