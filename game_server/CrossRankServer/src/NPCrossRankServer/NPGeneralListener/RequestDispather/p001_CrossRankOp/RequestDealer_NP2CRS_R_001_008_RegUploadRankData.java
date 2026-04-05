package NPCrossRankServer.NPGeneralListener.RequestDispather.p001_CrossRankOp;

import Common.ServerObj.ServerObj_RankObjInfo;
import Common.ServerObj.ServerObj_RankSubObjInfo;
import NP2CRS_R.p001_CrossRankOp.NP2CRS_R_001_008_RegUploadRankData;
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


public class RequestDealer_NP2CRS_R_001_008_RegUploadRankData extends NPRequestDealer<NP2CRS_R_001_008_RegUploadRankData>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, NP2CRS_R_001_008_RegUploadRankData _msg)
    {
        //查询跨服排行集合信息
        NPCrossInstanceInfo crossInstanceInfo = NPCrossInstanceMgr.getInstance().lookupCrossInstance(_msg.getCrossInstanceId());
        if (null == crossInstanceInfo)
        {
            CommLog.error("update rank data to cross rank rankId:[{}] of cross instance:[{}] fail! can not find cross instance", _msg.getRankId(), _msg.getCrossInstanceId());
            _commiter.commitFailRes(RankErr.INSTANCE_NO_EXIST.getCode());
            return;
        }

        //查询对应的排行实例Id，此时如果排行不存在则会创建一个排行
        long rankInstanceId = crossInstanceInfo.ensureRankListId(_msg.getRankId());

        //根据排行Id查询对应的排行实例对象
        NPCrossRankRankList rankList = NPCrossRankListMgr.getInstance().lookup(rankInstanceId);
        if (null == rankList)
        {
            CommLog.error("update rank data to cross rank rankId:[{}] of cross instance:[{}] fail! can not find rank list for:[{}]", _msg.getRankId(), _msg.getCrossInstanceId(), rankInstanceId);
            _commiter.commitFailRes(RankErr.RANK_NO_EXIST.getCode());
            return;
        }

        NPCrossRankContext context = NPCrossRankContext.createNew(ENPGameEvent.RANK_SET_OBJ_SCORE);

        //遍历所有的排行对象信息，设置分数
        for (ServerObj_RankObjInfo rankObjInfo : _msg.getRankList())
        {
            //如果是子对象列表为空，则直接设置分数
            if (rankObjInfo.getSubObjList().isEmpty())
            {
                rankList.setScoreCompareUpdateTime(rankObjInfo.getObjId(),
                        rankObjInfo.getScoreSourceId(), rankObjInfo.getScore(), rankObjInfo.getUpdatedMs(), context);
            } else
            {
                for (ServerObj_RankSubObjInfo subRankObjInfo : rankObjInfo.getSubObjList())
                {
                    rankList.setSubScoreCompareUpdateTime(rankObjInfo.getObjId(), subRankObjInfo.getSubObjId(),
                            subRankObjInfo.getScoreSourceId(), subRankObjInfo.getScore(), subRankObjInfo.getUpdatedMs(), context);
                }
            }
        }

        _commiter.commitSucRes(NPWriter_001_CrossRankOp.make_008_RegUploadRankData());
    }
}
