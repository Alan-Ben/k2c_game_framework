package NPUSServer.NPUserMsgDispather.p017_ActivityOp;

import GC2GS.p017_ActivityOp.GC2GS_017_001_ReqDrawActivityRankReward;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.NPLogDB.CommLogDB;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core.Rank.ActivityRankInfo;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_017_ActivityOp;
import NPUSServer.USLog;
import USLOGDB.Bo.LogActivityRankRewardDrawBO;

public class MsgDealer_GC2GS_017_001_ReqDrawActivityRankReward extends NPUserMsgDealer<GC2GS_017_001_ReqDrawActivityRankReward>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_017_001_ReqDrawActivityRankReward _msg)
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

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.DRAW_ACTIVITY_RANK_REWARD);

        //领取排行榜奖励
        rankInfo.drawRankReward(userData, context, (result) -> 
        {
        	if (!result.isSucc())
            {
                _committer.commitFailRes(result.getCode());
                return;
            }

            //推送奖励弹窗
            userData.sendMsgToGC(context.getCollector().toProto());

            //返回成功
            _committer.commitSucRes(US2GCWriter_017_ActivityOp.make_001_RetDrawActivityRankReward());

            //记录数据日志
            try
            {
                BM bmObj = userData.getUSServer().getBM();

                LogActivityRankRewardDrawBO logBo = new LogActivityRankRewardDrawBO();
                logBo.setCid(bmObj, userData.getCid());
                logBo.setInstanceId(bmObj, _msg.getInstanceId());
                logBo.setRankId(bmObj, _msg.getRankId());
                CommLogDB.log(bmObj, logBo, context);
            } catch (Exception e)
            {
                USLog.error(userData.getUSServer(), "MsgDealer_GC2GS_017_001_ReqDrawActivityRankReward._dealMessage - log failed: exception occurred, cid={}, instanceId={}, rankId={}, error={}",
                           userData.getCid(), _msg.getInstanceId(), _msg.getRankId(), e.getMessage());
            }
        });
    }
}
