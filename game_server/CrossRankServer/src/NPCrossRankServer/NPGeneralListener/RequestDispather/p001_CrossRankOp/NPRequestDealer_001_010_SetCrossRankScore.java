package NPCrossRankServer.NPGeneralListener.RequestDispather.p001_CrossRankOp;

import NP2CRS_R.p001_CrossRankOp.NP2CRS_R_001_010_SetCrossRankScore;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.RankErr;
import NPCommon.Log.CommLog;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceInfo;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceMgr;
import NPCrossRankServer.NPCrossRankContext.NPCrossRankContext;
import NPCrossRankServer.NPCrossRankMgr.NPCrossRankListMgr;
import NPCrossRankServer.NPCrossRankMgr.NPCrossRankRankList;
import NPCrossRankServer.NPGeneralListener.RequestDispather.Write.NPWriter_001_CrossRankOp;
import NPEnum.ENPGameEvent;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;


public class NPRequestDealer_001_010_SetCrossRankScore extends NPRequestDealer<NP2CRS_R_001_010_SetCrossRankScore>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, NP2CRS_R_001_010_SetCrossRankScore _msg)
    {
        //查询跨服排行集合信息
        NPCrossInstanceInfo crossInstanceInfo = NPCrossInstanceMgr.getInstance().lookupCrossInstance(_msg.getCrossInstanceId());
        if(null == crossInstanceInfo)
        {
            CommLog.error("set rank score:[{}]-[{}] to cross rank rankid:[{}] of cross instance:[{}] fail! can not find cross instance"
                    , _msg.getObjId(), _msg.getScore(), _msg.getRankId(), _msg.getCrossInstanceId());
            _commiter.commitFailRes(RankErr.INSTANCE_NO_EXIST.getCode());
            return ;
        }
        
        //查询对应的排行实例Id，此时如果排行不存在则会创建一个排行
        long rankInstanceId = crossInstanceInfo.ensureRankListId(_msg.getRankId());
        
        //根据排行Id查询对应的排行实例对象
        NPCrossRankRankList rankList = NPCrossRankListMgr.getInstance().lookup(rankInstanceId);
        if(null == rankList)
        {
            CommLog.error("set rank score:[{}]-[{}] to cross rank rankid:[{}] of cross instance:[{}] fail! can not find rank list for:[{}]"
                    , _msg.getObjId(), _msg.getScore(), _msg.getRankId(), _msg.getCrossInstanceId(), rankInstanceId);
            _commiter.commitFailRes(RankErr.RANK_NO_EXIST.getCode());
            return ;
        }
        
    	NPCrossRankContext context = NPCrossRankContext.createNew(ENPGameEvent.RANK_SET_OBJ_SCORE);
    	//暂时先固定设置greater为 false
        boolean res = rankList.setScoreCompareUpdateTime(_msg.getObjId(), _msg.getScoreSourceId(), _msg.getScore(), _msg.getUpdateTimeMs(), context);
    	if(!res)
    	{
            CommLog.error("set rank score:[{}]-[{}] to cross rank rankid:[{}] of cross instance:[{}] fail! set score op fail for rank list:[{}]"
                    , _msg.getObjId(), _msg.getScore(), _msg.getRankId(), _msg.getCrossInstanceId(), rankInstanceId);
    		_commiter.commitFailRes(RankErr.RANK_OP_FAIL.getCode());
    		return;
    	}
    	
    	_commiter.commitSucRes(NPWriter_001_CrossRankOp.make_010_SetCrossRankScore());
    }
}
