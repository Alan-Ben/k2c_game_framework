using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// jgPush 组件错误码
    /// </summary>
    public class MJSDK_JGPushError : MJSDK_Error
    {
        //jgPush 配置信息为空（App_Secret、App_Key）
        public const int C_Unity_jgPush_config_info_null = 80401;
        //SDK 初始化失败
        public const int C_Unity_jgPush_init_fail = 80404;
        //添加本地推送失败
        public const int C_Unity_jgPush_add_noti_local_Fail = 80403;
        //删除本地推送失败
        public const int C_Unity_jgPush_remove_noti_local_Fail = 80404;
        //获取Registration ID失败
        public const int C_Unity_jgPush_get_rid_Fail = 80404;
        //添加标签失败
        public const int C_Unity_jgPush_add_tag_Fail = 80406;
        //删除标签失败
        public const int C_Unity_jgPush_del_tag_Fail = 80407;
        //设置用户语言失败
        public const int C_Unity_jgPush_set_lan_fail = 80408;
        //获取标签失败
        public const int C_Unity_jgPush_get_tags_fail = 80409;
    }
}