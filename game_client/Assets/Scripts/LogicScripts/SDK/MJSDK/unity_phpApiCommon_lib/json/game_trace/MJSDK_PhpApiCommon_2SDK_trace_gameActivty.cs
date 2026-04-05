using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(trace - gameActivate：游戏激活)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2SDK_trace_gameActivty : MJSDK_2SDK_Base
    {
        //平台自定义id
        public string platform_id;
        //平台用户id:刚开始默认0
        public string uid;
        //角色ID
        public string cid;
        //客户端接入afID的时候启动时会回调
        public string sdkId;
        //自定义扩展字段
        public string extend;
        //当时登录客户端版本号
        public string login_tag;
    }
}
