using ALBasicProtocolPack;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;

namespace GOE
{
    //服务端推送公告
    public class NPGSSubDealer_001_009_ResumeUSConnectionRes : NPSubDealer<NPGS2GC.p001_BasicOp.NPGS2GC_001_009_ResumeUSConnectionRes>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPGS2GC.p001_BasicOp.NPGS2GC_001_009_ResumeUSConnectionRes _createProtocolObj()
        {
            return new NPGS2GC.p001_BasicOp.NPGS2GC_001_009_ResumeUSConnectionRes();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, NPGS2GC.p001_BasicOp.NPGS2GC_001_009_ResumeUSConnectionRes _msg)
        {
            if (_msg.getError() != 0)
            {
                ALLog.Sys($"Gs resume USConnectionRes error[{_msg.getError()}]");
                //重新登录
                Game.instance.reloginByDefault();
                return;
            }

            //设置时间，开启心跳包处理，带入序列号0可以跳过序列号判断
            if (_msg.getServerTimeTag() != 0 && _dealer is NPGSClientListener)
                FpsAndPingMgr.instance.dealHeartPackMesg(0, ALCommon.getNowTimeMill(), _msg.getServerTimeTag(), (NPGSClientListener)_dealer, true);
            
            //处理玩家初始化操作
            GamePlayerInit.onGSResume();
        }
    }
}

