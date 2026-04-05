package NPCrossRankServer.NPGeneralListener.RequestDispather.p001_CrossRankOp;

import NP2CRS_R.p001_CrossRankOp.NP2CRS_R_001_005_RequestCrossInstance;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceInfo;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceMgr;
import NPCrossRankServer.NPGeneralListener.RequestDispather.Write.NPWriter_001_CrossRankOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;


public class NPRequestDealer_001_005_RequestCrossInstance extends NPRequestDealer<NP2CRS_R_001_005_RequestCrossInstance>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, NP2CRS_R_001_005_RequestCrossInstance _msg)
    {
        //创建跨服排行组信息
        NPCrossInstanceInfo crossInstanceInfo = NPCrossInstanceMgr.getInstance().createCrossInstance();

        _commiter.commitSucRes(NPWriter_001_CrossRankOp.make_005_RequestCrossInstance(crossInstanceInfo.getInstanceId()));
    }
}
