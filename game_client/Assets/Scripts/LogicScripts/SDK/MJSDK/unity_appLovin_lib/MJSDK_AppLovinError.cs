using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// AppLovin组件（maxSDK广告变现）  错误码
    /// </summary>
    public class MJSDK_AppLovinError : MJSDK_Error
    {
        // 未配置应用信息（Applovin后台应用编号、秘钥）
        public const int C_Unity_AppLovin_Error_config_info_null = 70101;
        //SDK 初始化失败
        public const int C_Unity_AppLovin_Error_init_fail = 70102;
        // 广告资源请求失败
        public const int C_Unity_AppLovin_Error_MAXAd_LoadFail = 70103;
        //广告视图播放失败
        public const int C_Unity_AppLovin_Error_MAXAd_DisplayFail = 70104;
        //广告变现SDK未初始化
        public const int C_Unity_AppLovin_Error_MAXAd_No_Init = 70105;
    }
}