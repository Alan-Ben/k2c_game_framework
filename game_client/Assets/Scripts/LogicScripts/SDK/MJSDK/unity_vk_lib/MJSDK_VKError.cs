using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// vk 组件错误码
    /// </summary>
    public class MJSDK_VKError:MJSDK_Error
    {
        //未配置vk应用信息（vk后台应用编号、秘钥）
        public const int C_Unity_vk_Error_config_info_null = 40401;
        //vkSDK未初始化
        public const int C_Unity_vk_Error_No_Init = 40402;
        //设备未安装vk应用
        public const int C_Unity_vk_Error_Device_No_Exist_VKApp = 40403;
        //获取vk用户信息失败
        public const int C_Unity_vk_Error_Get_UserInfo_Fail = 40404;
        //分享失败
        public const int C_Unity_VK_Share_Fail = 40405;
    }
}
