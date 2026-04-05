using ALBasicProtocolPack;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;

namespace GOE
{
    //服务端推送公告
    public class NPGSSubDealer_001_004_OnUSEnterDone : NPSubDealer<NPGS2GC.p001_BasicOp.NPGS2GC_001_004_OnUSEnterDone>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override NPGS2GC.p001_BasicOp.NPGS2GC_001_004_OnUSEnterDone _createProtocolObj()
        {
            return new NPGS2GC.p001_BasicOp.NPGS2GC_001_004_OnUSEnterDone();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, NPGS2GC.p001_BasicOp.NPGS2GC_001_004_OnUSEnterDone _msg)
        {
            
#if UNITY_EDITOR
            GameInit_SelectServer.instance.isReceive001_004 = true;
#endif
            
            if (_msg.getErrCode() != 0)
            {
                NPGUIAddSceneCenterTip.instance.showErrorInfo(_msg.getErrCode());
                return;
            }

            //设置时间，开启心跳包处理，带入序列号0可以跳过序列号判断
            if (_msg.getServerTimeTag() != 0 && _dealer is NPGSClientListener)
                FpsAndPingMgr.instance.dealHeartPackMesg(0, ALCommon.getNowTimeMill(), _msg.getServerTimeTag(), (NPGSClientListener)_dealer, true);

            //设置cid
            NPPlayer.instance.playerInfo.setCid(_msg.getCid());
            //发送埋点-服务器发放cid成功
            GCommon.sendStepReport(TraceConst.GET_PLAYER_CID.setMarkParam(_msg.getCid()));
            //触发选服操作流程完毕
            GameInit_SelectServer.instance.onUSEnterDone(_msg.getClientInitSerialize());
            //处理玩家初始化操作
            GamePlayerInit.onUSEntered();
            //处理与服务器id相关CDN
            GameInit_CDN.instance.dealEnterServerCDN();
            //初始化运营公告
            AnnouncementMgr.instance.init();
        }
    }
}

