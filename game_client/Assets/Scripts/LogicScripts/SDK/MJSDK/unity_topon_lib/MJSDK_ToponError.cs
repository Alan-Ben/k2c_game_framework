using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// topon 组件错误码
    /// </summary>
    public class MJSDK_ToponError : MJSDK_Error
    {
        public const int C_Unity_Topon_Configuration_Null = 70201;//未配置应用信息（topon后台应用编号、秘钥）
        public const int C_Unity_Topon_No_Init = 70202;//SDK未初始化
        public const int C_Unity_Topon_ad_load_fail = 70203;//加载广告失败
        public const int C_Unity_Topon_ad_play_fail = 70204;//播放失败
    }
}