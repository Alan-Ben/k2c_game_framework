using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// Engine->SDK(trace - gameErr：游戏异常事件上报)消息结构体
    /// </summary>
    public class MJSDK_PhpApiCommon_2SDK_trace_gameErr : MJSDK_2SDK_Base
    {
        //异常级别：debug、warn、info、error
        public string err_level;
        //节点，各项目自行定义
        public string step;
        //异常错误信息
        public string content;
        //备用扩展信息
        public string mark1;
        //当时登录客户端版本号
        public string login_tag;
    }

    public enum E_Err_level
    {
        none = 0,
        debug,   //测试模式
        warn,    //警告模式
        info,    //信息
        error,   //错误
    }
}