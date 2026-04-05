using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// Unity事件节点
    /// </summary>
    public class MJSDK_Event
    {
        //未知错误
        public const string C_Unity_Event_Err = "90000";
        //开始初始化
        public const string C_Unity_Event_StartInit = "90001";
        //Untiy库与平台组件依赖错误
        public const string C_Unity_Event_Unity_Dependence_Platform_Fail = "90002";
        //UnitySDK开始初始化库
        public const string C_Unity_Event_InitUnityLib = "90003";
        //UnitySDK初始化失败
        public const string C_Unity_Event_InitFail = "90004";
        //UnitySDK初始化成功
        public const string C_Unity_Event_InitSuc = "90005";

    }
}