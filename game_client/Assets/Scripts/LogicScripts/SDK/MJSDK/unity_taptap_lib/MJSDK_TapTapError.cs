using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// TapTap 组件错误码
    /// </summary>
    public class MJSDK_TapTapError : MJSDK_Error
    {
        //未配置应用信息
        public const int C_None_Config_Param = 50801;
        //登陆失败
        public const int C_Get_User_Info_Fail = 50802;
        // 登陆取消
        public const int C_Get_User_Info_Cancel = 50803;

    }
}
