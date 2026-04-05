using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// AIHelp 客服组件脚本错误码
    /// </summary>
    public class MJSDK_AIHelpError : MJSDK_Error
    {
        //600001 - AIHelp 配置信息为空（AIHelp_apiKey、AIHelp_domain、AIHelp_appid 为空）
        public const int C_Unity_AIHelp_Error_config_info_null = 80101;
        //SDK 初始化失败
        public const int C_Unity_AIHelp_Error_init_fail = 80102;
        //AIHelp未初始化
        public const int C_Unity_AIHelp_Error_no_init = 80103;
    }
}