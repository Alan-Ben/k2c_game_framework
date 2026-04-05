using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// twitter错误码
    /// </summary>
    public class MJSDK_TwitterError
    {
        //未配置twitter应用信息（twitter后台应用编号、秘钥）
        public const int C_Unity_twitter_Error_config_info_null = 40301;
        //twitterSDK未初始化
        public const int C_Unity_twitter_Error_No_Init = 40302;
        //获取twitter用户信息失败
        public const int C_Unity_twitter_Error_Get_UserInfo_Fail = 40303;
        //设备未安装twitter应用
        public const int C_Unity_twitter_Error_Device_No_Exist_App = 40304;
    }
}
