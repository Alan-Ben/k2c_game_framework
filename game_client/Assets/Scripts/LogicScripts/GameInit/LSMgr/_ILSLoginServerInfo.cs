using UnityEngine;
using System.Collections.Generic;
using System;
using ALPackage;
using ALBasicProtocolPack;

namespace GOE
{
    /// <summary>
    /// 用于登录服务器时带入登录相关信息集合的接口对象
    /// 一般用于存储和处理CDN数据转化的接口
    /// </summary>
    public interface _ILSLoginServerInfo
    {
        //默认登录信息
        string defaultIP { get; }
        int defaultPort { get; }
        //默认登录方式延迟登录时间
        float defaultDelayLoginTime { get; }

        //轮询地址的登录信息
        List<string> loopIPList { get; }
        List<int> loopPortList { get; }
        //轮询登录方式延迟登录时间
        List<float> loopDelayLoginTimeList { get; }
        
        //异步初始化获取数据
        void reqDataDone(Action _action);
    }
}