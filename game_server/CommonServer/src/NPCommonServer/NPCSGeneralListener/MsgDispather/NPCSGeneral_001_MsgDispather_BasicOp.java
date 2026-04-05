package NPCommonServer.NPCSGeneralListener.MsgDispather;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NP2CS.p001_BasicOp.*;
import NPCommon.Dispather.NPCustomMsgDispatcher.NPCustomMsgDealer;
import NPCommon.Log.CommLog;
import NPCommonServer.CSServerMgr.CSServerMgr;
import NPCommonServer.CrossRankServerHandleMgr.CrossRankServerMgr;
import NPCommonServer.HandleServerMgr.HandleServerMgr;
import NPCommonServer.HandleServerMgr.HandleServerType;
import NPCommonServer.NPCrossGameServerHandleMgr.NPCrossGameServerMgr;
import NPCommonServer.WhiteAccMgr.CSWhiteAccMgr;

public class NPCSGeneral_001_MsgDispather_BasicOp
{
    public static void init(NPCSGeneralMsgDispather _dispather)
    {
        _dispather.regHandler(new NPCustomMsgDealer<NP2CS_001_001_AddRoomServer>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2CS_001_001_AddRoomServer _msg)
            {
                //Plat通知添加新的Room服务器
                CSServerMgr.getInstance().regRS(_msg.getServerTypeId());
            }
        });

        _dispather.regHandler(new NPCustomMsgDealer<NP2CS_001_002_RomoveRoomServer>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2CS_001_002_RomoveRoomServer _msg)
            {
                //Plat服务器通知移除Room服务器
                CSServerMgr.getInstance().unregRS(_msg.getServerTypeId());
            }
        });

        _dispather.regHandler(new NPCustomMsgDealer<NP2CS_001_003_AddCrossRankServer>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2CS_001_003_AddCrossRankServer _msg)
            {
                //Plat通知添加新的CRS服务器
                CrossRankServerMgr.getInstance().regServer(_msg.getServerTypeId());
            }
        });

        _dispather.regHandler(new NPCustomMsgDealer<NP2CS_001_006_AddCrossGameServer>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2CS_001_006_AddCrossGameServer _msg)
            {
                NPCrossGameServerMgr.getInstance().regCrossGameServer(_msg.getServerTypeId());
            }
        });
        _dispather.regHandler(new NPCustomMsgDealer<NP2CS_001_007_RomoveCrossGameServer>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2CS_001_007_RomoveCrossGameServer _msg)
            {
                NPCrossGameServerMgr.getInstance().unregCrossGameServer(_msg.getServerTypeId());
            }
        });
        _dispather.regHandler(new NPCustomMsgDealer<NP2CS_001_008_OnHSOnline>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2CS_001_008_OnHSOnline _msg)
            {
                //发起加载白名单
                CSWhiteAccMgr.getInstance().startLoad();
            }
        });

        _dispather.regHandler(new NPCustomMsgDealer<NP2CS_001_010_AddHandleServer>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2CS_001_010_AddHandleServer _msg)
            {
                HandleServerMgr mgr = HandleServerType.getInstance().getHandleServerMgr(_msg.getServerType());
                if (null == mgr)
                {
                    CommLog.error("NP2CS_001_010_AddHandleServer get handle server mgr failed, serverType:{}", _msg.getServerType());
                    return;
                }

                mgr.regHandleServer(_msg.getServerTypeId());
            }
        });
        _dispather.regHandler(new NPCustomMsgDealer<NP2CS_001_011_RemoveHandleServer>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2CS_001_011_RemoveHandleServer _msg)
            {
                HandleServerMgr mgr = HandleServerType.getInstance().getHandleServerMgr(_msg.getServerType());
                if (null == mgr)
                {
                    CommLog.error("NP2CS_001_011_RemoveHandleServer get handle server mgr failed, serverType:{}", _msg.getServerType());
                    return;
                }

                mgr.unRegHandleServer(_msg.getServerTypeId());
            }
        });
    }
}
