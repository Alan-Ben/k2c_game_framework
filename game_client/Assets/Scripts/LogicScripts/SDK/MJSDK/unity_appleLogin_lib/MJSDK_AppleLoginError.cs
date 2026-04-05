using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// 苹果登录组件 错误定义常量类
    /// </summary>
    public class MJSDK_AppleLoginError : MJSDK_Error
    {
        //iOS系统低，不支持苹果登录功能
        public const int C_Unity_AppleLogin_Error_Get_UserInfo_No_Supportr = 40101;
        //获取苹果用户失败失败
        public const int C_Unity_AppleLogin_Error_Get_UserInfo_Fail = 40102;
    }
}
