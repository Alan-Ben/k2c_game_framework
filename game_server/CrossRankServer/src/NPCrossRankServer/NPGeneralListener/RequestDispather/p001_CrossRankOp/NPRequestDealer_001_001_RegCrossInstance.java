package NPCrossRankServer.NPGeneralListener.RequestDispather.p001_CrossRankOp;

import NP2CRS_R.p001_CrossRankOp.NP2CRS_R_001_001_RegCrossInstance;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.RankErr;
import NPCommon.Log.CommLog;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceInfo;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceMgr;
import NPCrossRankServer.NPGeneralListener.RequestDispather.Write.NPWriter_001_CrossRankOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;


public class NPRequestDealer_001_001_RegCrossInstance extends NPRequestDealer<NP2CRS_R_001_001_RegCrossInstance>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, NP2CRS_R_001_001_RegCrossInstance _msg)
    {
        //查询跨服排行集合信息
        NPCrossInstanceInfo crossInstanceInfo = NPCrossInstanceMgr.getInstance().lookupCrossInstance(_msg.getCrossInstanceId());
        if(null == crossInstanceInfo)
        {
            CommLog.error("add joiner:[{}] to cross instance:[{}] fail! can not find cross instance", _msg.getJoinerId(), _msg.getCrossInstanceId());
            _commiter.commitFailRes(RankErr.INSTANCE_NO_EXIST.getCode());
            return ;
        }
        
        //添加参与者
        crossInstanceInfo.addJoiner(_msg.getJoinerId());
        
        _commiter.commitSucRes(NPWriter_001_CrossRankOp.make_001_RegCrossInstance());
    }
}
