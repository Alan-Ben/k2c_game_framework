using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// TikTok 组件错误码
    /// </summary>
    public class MJSDK_TikTokError : MJSDK_Error
    {
        // 配置信息为空
        public const int C_None_Config_Param = 60701;
        //Marketing 初始化失败
        public const int C_SDK_Init_Fail = 60702;
        //Marketing 购买事件上报失败
        public const int C_Purchase_Event_Fail = 60703;
        //Marketing 自定义事件上报失败
        public const int C_Custom_Event_Fail = 60704;
    }
}

