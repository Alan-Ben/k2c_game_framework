using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// MJAcc组件错误声明区域
    /// </summary>
    public class MJSDK_MJAccError : MJSDK_Error
    {
        //【参数表】未配置应用信息（梦加账号中心配置信息为空（App_id、App_Secret））
        public const int C_Unity_MJAcc_Error_config_info_null = 40701;
        //SDK未初始化/初始化失败
        public const int C_Unity_MJAcc_Error_No_Init = 40702;
        //用户关闭梦加账号中心
        public const int C_Unity_MJAcc_Error_Get_UserInfo_Fail = 40703;
    }
}