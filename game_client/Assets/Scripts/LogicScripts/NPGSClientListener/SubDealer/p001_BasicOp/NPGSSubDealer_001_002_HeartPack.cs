using ALBasicProtocolPack;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 心跳包
    /// </summary>
    public class NPGSSubDealer_001_002_HeartPack : NPSubDealer<NPGS2GC.p001_BasicOp.NPGS2GC_001_002_HeartPack>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPGS2GC.p001_BasicOp.NPGS2GC_001_002_HeartPack _createProtocolObj()
        {
            return new NPGS2GC.p001_BasicOp.NPGS2GC_001_002_HeartPack();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, NPGS2GC.p001_BasicOp.NPGS2GC_001_002_HeartPack _msg)
        {
            //保存服务器返回的值并更新Ping
            if(_dealer is NPGSClientListener)
                FpsAndPingMgr.instance.dealHeartPackMesg(_msg.getClientHeartSerialize(), _msg.getClientTimeTag(), _msg.getServerTimeTag(), (NPGSClientListener)_dealer, false);
        }
    }
}

