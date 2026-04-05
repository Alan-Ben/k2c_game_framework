using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// mobpush 组件错误码
    /// </summary>
    public class MJSDK_MobPushError : MJSDK_Error
    {
        //mobpush 配置信息为空（App_Secret、App_Key）
        public const int C_Unity_mobpush_config_info_null = 80201;
        //SDK 初始化失败
        public const int C_Unity_mobpush_init_fail = 80202;
        //添加本地推送失败
        public const int C_Unity_mobpush_add_noti_local_Fail = 80203;
        //删除本地推送失败
        public const int C_Unity_mobpush_remove_noti_local_Fail = 80204;
        //获取Registration ID失败
        public const int C_Unity_mobpush_get_rid_Fail = 80204;
        //添加标签失败
        public const int C_Unity_mobpush_add_tag_Fail = 80206;
        //删除标签失败
        public const int C_Unity_mobpush_del_tag_Fail = 80207;
    }
}