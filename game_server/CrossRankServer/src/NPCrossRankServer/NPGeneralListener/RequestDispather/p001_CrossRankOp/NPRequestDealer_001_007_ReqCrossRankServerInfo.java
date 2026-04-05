package NPCrossRankServer.NPGeneralListener.RequestDispather.p001_CrossRankOp;

import NP2CRS_R.p001_CrossRankOp.NP2CRS_R_001_007_ReqCrossRankServerInfo;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceMgr;
import NPCrossRankServer.NPGeneralListener.RequestDispather.Write.NPWriter_001_CrossRankOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;


public class NPRequestDealer_001_007_ReqCrossRankServerInfo extends NPRequestDealer<NP2CRS_R_001_007_ReqCrossRankServerInfo>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, NP2CRS_R_001_007_ReqCrossRankServerInfo _msg)
    {
        //查询跨服排行集合数量
        _commiter.commitSucRes(NPWriter_001_CrossRankOp.make_007_RetCrossRankServerInfo(NPCrossInstanceMgr.getInstance().getAllInstanceCount()));
    }
}
