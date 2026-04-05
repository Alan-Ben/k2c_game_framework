using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(trace - gameStep：游戏事件跟踪)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2SDK_trace_gameStep : MJSDK_2SDK_Base
    {
        //平台自定义id
        public string platform_id;
        //区域ID
        public string area_id;
        //服务器ID
        public string server_id;
        //客户端接入afID的时候启动时会回调
        public string sdkId;
        //json数组
        public MJSDK_SDKEvent_2SDK_step_info step_info;
        //自定义扩展字段(数据库字段名mark1)
        public string extend;
        //自定义扩展字段2
        public string mark2;
        //自定义扩展字段3
        public string mark3;
        //是否支持gpu instancing(0:否;1:是)
        public string support_gpu_instancing = "0";
        //当时登录客户端版本号
        public string login_tag;

    }


    /// <summary>
    /// 游戏跟踪-节点具体信息
    /// </summary>
    [Serializable]
    public class MJSDK_SDKEvent_2SDK_step_info
    {
        //平台用户id:刚开始默认0
        public string uid;
        //角色ID
        public string cid;
        //事件ID
        public int step_id;
        //当前时间戳
        public string op_time;
    }
}