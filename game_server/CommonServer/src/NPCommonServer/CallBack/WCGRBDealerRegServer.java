package NPCommonServer.CallBack;

import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import Common.Common_RSInfo;
import NP2PS_RB.p001_BasicOp.NP2PS_RB_001_001_RetCSRegInfo;
import NPCommonServer.CSServerMgr.CSServerMgr;
import NPCommonServer.CrossRankServerHandleMgr.CrossRankServerMgr;
import NPCommonServer.HandleServerMgr.HandleServerMgr;
import NPCommonServer.HandleServerMgr.HandleServerType;
import NPCommonServer.NPCrossGameServerHandleMgr.NPCrossGameServerMgr;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum;

public class WCGRBDealerRegServer implements _IWCGCallbackDealer
{

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2PS_RB_001_001_RetCSRegInfo();
    }

    @Override
    public void dealFail(int _errCode)
    {
        ALServerLog.Error("Reg Common Server Error! - " + _errCode);
    }

    @Override
    public void dealSuc(_IALProtocolStructure _msg)
    {
        NP2PS_RB_001_001_RetCSRegInfo protocol = (NP2PS_RB_001_001_RetCSRegInfo) _msg;
        if (null == protocol)
            return;

        //初始化所有房间信息
        for (int i = 0; i < protocol.getRsList().size(); i++)
        {
            Common_RSInfo rsInfo = protocol.getRsList().get(i);
            if (null == rsInfo)
                continue;

            CSServerMgr.getInstance().regRS(rsInfo.getServerTypeId());
        }

        //初始化所有CrossGame服务器列表
        for (Integer typeId : protocol.getCrossGameServerTypeIdList())
        {
            NPCrossGameServerMgr.getInstance().regCrossGameServer(typeId);
        }

        //初始化所有CrossRank服务器列表
        for (Integer typeId : protocol.getCrossRankServerTypeIdList())
        {
            CrossRankServerMgr.getInstance().regServer(typeId);
        }

        //初始化所有GameLogic服务器列表
        HandleServerMgr gameLogicHandleServerMgr = HandleServerType.getInstance().getHandleServerMgr(NPEnum.EServerType.GAME_LOGIC.ordinal());
        for (Integer typeId : protocol.getGameLogicServerTypeIdList())
        {
            gameLogicHandleServerMgr.regHandleServer(typeId);
        }
    }
}
