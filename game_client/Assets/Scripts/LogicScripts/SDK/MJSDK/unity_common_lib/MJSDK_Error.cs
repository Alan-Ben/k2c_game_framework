using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// SDK系统错误码
    /// </summary>
    public class MJSDK_Error
    {
        //系统消息 - 系统未知错误
        public const int C_Unity_SysErr = 10001;
        //系统消息 - 有组件版本不匹配或不符合要求
        public const int C_Unity_ComponentVersionErr = 10002;
        //系统消息- 平台未注册该协议（可能未集成组件）
        public const int C_Unity_Pro_Unregistered = 10003;

        //消息处理失败 - 其他情况（异常）
        public const int C_Unity_MsgDealFail = 10100;
        //消息处理失败 - 消息接收Json格式错误
        public const int C_Unity_JsonErr = 10101;
        //消息处理失败 - 消息接收参数缺失
        public const int C_Unity_Param_Miss = 10102;

        //依赖错误  - Unity组件库依赖平台组件库不匹配或不符合要求
        public const int C_Unity_Dependence_Platform_ComponentVersionErr = 10303;

        //unitySDK相关错误码
        public const int C_Unity_Auto_Init_Lib_Err = 900001;
        //unitySDK相关异常信息
        public const int C_Unity_Exception_Err = 990001;
    }
}