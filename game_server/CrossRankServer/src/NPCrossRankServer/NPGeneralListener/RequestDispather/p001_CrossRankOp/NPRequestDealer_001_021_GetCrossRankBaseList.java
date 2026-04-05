package NPCrossRankServer.NPGeneralListener.RequestDispather.p001_CrossRankOp;

import Common.RankObj.Rank_BaseItem;
import NP2CRS_R.p001_CrossRankOp.NP2CRS_R_001_021_GetCrossRankBaseList;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.RankErr;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceInfo;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceMgr;
import NPCrossRankServer.NPCrossRankMgr.NPCrossRankListMgr;
import NPCrossRankServer.NPCrossRankMgr.NPCrossRankRankList;
import NPCrossRankServer.NPGeneralListener.RequestDispather.Write.NPWriter_001_CrossRankOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.List;


public class NPRequestDealer_001_021_GetCrossRankBaseList extends NPRequestDealer<NP2CRS_R_001_021_GetCrossRankBaseList>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, NP2CRS_R_001_021_GetCrossRankBaseList _msg)
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

        List<Rank_BaseItem> rankBaseList;
        if (_msg.getLimit() <= 0)
        {
            rankBaseList = rankList.makeRankObjBaseList();
        } else
        {
            rankBaseList = rankList.makeRankObjBaseList(_msg.getLimit());
        }

        _commiter.commitSucRes(NPWriter_001_CrossRankOp.make_021_GetCrossRankBaseList(rankBaseList));
    }
}
