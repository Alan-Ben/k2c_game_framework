package NPUSServer.NPUserMsgDispather.p017_ActivityOp;

import GC2GS.p017_ActivityOp.GC2GS_017_002_ReqActivityRankSettleInfo;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.CommErr;
import NPGameRes.Refs.Rank.RefRank;
import NPUSServer.CommonActivityMgr.Core.Rank.ActivityRankInfo;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_017_ActivityOp;

public class MsgDealer_GC2GS_017_002_ReqActivityRankSettleInfo extends NPUserMsgDealer<GC2GS_017_002_ReqActivityRankSettleInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_017_002_ReqActivityRankSettleInfo _msg)
    {
        NPUSUserData userData = _committer.getUserData();

        //获取活动对象
        _AActivityBase activity = userData.getUSServer().getCommActivityMgr().lookupActivity(_msg.getInstanceId());
        if (activity == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        //检查活动是否冻结
        if (!activity.isRewarding())
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_NOT_IN_FROZEN.getCode());
            return;
        }

        //获取排行榜信息
        ActivityRankInfo rankInfo = activity.lookupRank(_msg.getRankId());
        if (rankInfo == null)
        {
            _committer.commitFailRes(ActivityErr.ACTIVITY_RANK_NOT_FOUND.getCode());
            return;
        }
        //见擦汗排行榜配置
        RefRank rankRef = rankInfo.getRankRef();
        if(null == rankRef)
        {
            _committer.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        //获取玩家结算数据
    	rankInfo.getSettleInfo(userData, 
    			(err, result) -> 
    			{
    				if(!err.isSucc())
    				{
    					_committer.commitFailRes(err.getCode());
    					return;
    				}
    				
    				_committer.commitSucRes(US2GCWriter_017_ActivityOp.make_002_RetActivityRankSettleInfo(result));
    			});

        //领取排行榜奖励
//        ResultOne<Activity_RankSettleInfo> result = rankInfo.getSettleInfo(userData.getCid());
//        if (!result.isSucc())
//        {
//            _committer.commitFailRes(result.getCode());
//            return;
//        }
//
//        //返回成功
//        _committer.commitSucRes(US2GCWriter_017_ActivityOp.make_002_RetActivityRankSettleInfo(result.getData()));
    }
}
