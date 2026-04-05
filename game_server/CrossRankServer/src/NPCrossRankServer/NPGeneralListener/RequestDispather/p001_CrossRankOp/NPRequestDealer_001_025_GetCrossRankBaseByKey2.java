package NPCrossRankServer.NPGeneralListener.RequestDispather.p001_CrossRankOp;

import NP2CRS_R.p001_CrossRankOp.NP2CRS_R_001_025_GetCrossRankBaseByKey2;
import NPCommon.CommonRank.RankObj;
import NPCommon.CommonRank.RankSubObj;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.RankErr;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceInfo;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceMgr;
import NPCrossRankServer.NPCrossRankMgr.NPCrossRankListMgr;
import NPCrossRankServer.NPCrossRankMgr.NPCrossRankRankList;
import NPCrossRankServer.NPGeneralListener.RequestDispather.Write.NPWriter_001_CrossRankOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;


public class NPRequestDealer_001_025_GetCrossRankBaseByKey2 extends NPRequestDealer<NP2CRS_R_001_025_GetCrossRankBaseByKey2>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, NP2CRS_R_001_025_GetCrossRankBaseByKey2 _msg)
    {
        //查询跨服排行集合信息
        NPCrossInstanceInfo crossInstanceInfo = NPCrossInstanceMgr.getInstance().lookupCrossInstance(_msg.getCrossInstanceId());
        if(null == crossInstanceInfo)
        {
            _commiter.commitFailRes(RankErr.INSTANCE_NO_EXIST.getCode());
            return ;
        }
        
        //查询对应的排行实例Id，此时如果排行不存在则会创建一个排行
        long rankInstanceId = crossInstanceInfo.ensureRankListId(_msg.getRankId());
        
        //根据排行Id查询对应的排行实例对象
        NPCrossRankRankList rankList = NPCrossRankListMgr.getInstance().lookup(rankInstanceId);
        if(null == rankList)
        {
            _commiter.commitFailRes(RankErr.RANK_NO_EXIST.getCode());
            return ;
        }

        RankObj rankObj = rankList.lookup(_msg.getKey());
        if(null == rankObj)
        {
            _commiter.commitFailRes(RankErr.RANK_ITEM_NO_EXIST.getCode());
            return ;
        }
        
        RankSubObj rankSubObj = rankObj.lookup(_msg.getSubKey());
        if(null == rankSubObj)
        {
            _commiter.commitFailRes(RankErr.RANK_NOT_JOIN.getCode());
            return ;
        }
        
        _commiter.commitSucRes(NPWriter_001_CrossRankOp.make_025_GetCrossRankBaseByKey2(rankObj.toBaseProto()));
    }
}
