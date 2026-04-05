package NPCrossRankServer.NPGeneralListener.RequestDispather.p001_CrossRankOp;

import NP2CRS_R.p001_CrossRankOp.NP2CRS_R_001_006_DiscardCrossInstance;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.RankErr;
import NPCommon.Log.CommLog;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceInfo;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceMgr;
import NPCrossRankServer.NPGeneralListener.RequestDispather.Write.NPWriter_001_CrossRankOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;


public class NPRequestDealer_001_006_DiscardCrossInstance extends NPRequestDealer<NP2CRS_R_001_006_DiscardCrossInstance>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, NP2CRS_R_001_006_DiscardCrossInstance _msg)
    {
        //查询跨服排行集合信息
        NPCrossInstanceInfo crossInstanceInfo = NPCrossInstanceMgr.getInstance().lookupCrossInstance(_msg.getCrossInstanceId());
        if(null == crossInstanceInfo)
        {
            CommLog.error("Can not find cross Instance:[{}] to remove"
                    , _msg.getCrossInstanceId());
            _commiter.commitFailRes(RankErr.INSTANCE_NO_EXIST.getCode());
            return ;
        }

        //调用移除处理操作
        NPCrossInstanceMgr.getInstance().discardCrossInstance(_msg.getCrossInstanceId());

        _commiter.commitSucRes(NPWriter_001_CrossRankOp.make_006_DiscardCrossInstance());
    }
}
