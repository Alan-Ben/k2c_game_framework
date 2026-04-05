using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using LitJson;
using ALBasicProtocolPack;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 客户端向服务器进行回调式请求时，回调的超时监控对象
    /// </summary>
    public class NPRequestCallbackMonitorTask : _IALBaseMonoTask
    {
        //管理器的操作序列号，避免同时多个任务处理。管理器可能会开启多个任务，需要序列号进行判断
        private long _m_lOpSerialize;

        public NPRequestCallbackMonitorTask(long _opSerialize)
        {
            _m_lOpSerialize = _opSerialize;
        }

        /***********
         * 处理函数
         */
        public void deal()
        {
            //获取当前时间
            long nowMs = ALCommon.getNowTimeMill();

            //处理超时
            NPRequestCallbackMonitorMgr.instance._dealTimeoutTask(_m_lOpSerialize, nowMs);

            //判断序列号不一致则不继续
            if (_m_lOpSerialize != NPRequestCallbackMonitorMgr.instance.getCallbackOpSerialize())
                return;

            //下一秒处理
            ALMonoTaskMgr.instance.addMonoTask(this, 1.0f);
        }
    }
}
